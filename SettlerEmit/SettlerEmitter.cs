using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SettlerEmit
{
    public class SettlerEmitter
    {
        public static string AssemblyNamePrefix = "SettlerFor";
        public static string AssemblyFileExtension = ".dll";

        ILGenerator il;
        MethodInfo callNew;
        MethodInfo callGetRandomString;
        MethodInfo callGetRandomInteger;
        MethodInfo callFixtureFor;
        MethodInfo callFixtureNew;
        LocalBuilder localString;
        LocalBuilder paramObject;
        LocalBuilder lengthLocal;

        public IFixture CreateAssembly(Type type)
        {
            string typeName = type.Name;
            string ASM_NAME = AssemblyNamePrefix + typeName;
            string MOD_NAME = ASM_NAME;
            string TYP_NAME = ASM_NAME;
            string DLL_NAME = ASM_NAME + AssemblyFileExtension;
           
            AssemblyBuilder asmBuilder = CreateAsm(ASM_NAME);
            ModuleBuilder modBuilder = asmBuilder.DefineDynamicModule(MOD_NAME, DLL_NAME);

            TypeBuilder typeBuilder = modBuilder.DefineType(TYP_NAME,
                TypeAttributes.Public,
                typeof(object),
                new Type[] { typeof(IFixture) });

            MethodBuilder NewMethodBuilder = typeBuilder.DefineMethod(
                "New",
                MethodAttributes.Public |
                MethodAttributes.Virtual |
                MethodAttributes.ReuseSlot,
                typeof(object),
                new Type[] {});

            MethodBuilder FillMethodBuilder = typeBuilder.DefineMethod(
                "Fill",
                MethodAttributes.Public |
                MethodAttributes.Virtual |
                MethodAttributes.ReuseSlot,
                typeof(object),
                new Type[] { typeof(int) });

             
            //new Type[] { typeof(object[])}
           
            MethodBuilder MemberMethodBuilder = typeBuilder.DefineMethod(
                "Member",
                MethodAttributes.Public |
                MethodAttributes.Virtual |
                MethodAttributes.ReuseSlot,
                typeof(object), new Type [] { typeof(object[]) });
            
            ImplementNewMethod(NewMethodBuilder, type);
            ImplementFillMethod(FillMethodBuilder, type);
            ImplementMemberMethod(MemberMethodBuilder, type);
            Type FixtureType = typeBuilder.CreateType();
            asmBuilder.Save(DLL_NAME);
            return (IFixture)Activator.CreateInstance(FixtureType);
        }
        /*  only need to call the normal Member method */
        private void ImplementMemberMethod(MethodBuilder MemberMethodBuilder, Type type)
        {
            MethodInfo member = typeof(AutoFixture).GetMethod("Member");
            ILGenerator il = MemberMethodBuilder.GetILGenerator();
            il.Emit(OpCodes.Call, member);

        } 

        private void ImplementFillMethod(MethodBuilder fillMethodBuilder, Type type)
        {
            
            il = fillMethodBuilder.GetILGenerator();
            
            var loopBody = il.DefineLabel();
            var end = il.DefineLabel();
            callFixtureNew = typeof(IFixture).GetMethod("New");
            //paramObject = il.DeclareLocal(type);// declare T aka type
            LocalBuilder returnArray = il.DeclareLocal(typeof(object[]));
            LocalBuilder idx = il.DeclareLocal(typeof(int)); //declare place to store index 
            il.Emit(OpCodes.Ldarg_1); // ld int
            il.Emit(OpCodes.Newarr, typeof(object)); //creates array with size [ldarg_1]
            il.Emit(OpCodes.Stloc, returnArray);
            il.Emit(OpCodes.Ldarg_1); // ld int
            il.Emit(OpCodes.Ldc_I4, 0); //load 0
            il.Emit(OpCodes.Beq_S, end);  //exit if int = 0
            il.Emit(OpCodes.Ldarg_1); // ld int
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Sub); // int--
            il.Emit(OpCodes.Stloc_1); //[idx], int
            il.MarkLabel(loopBody);
            il.Emit(OpCodes.Ldloc,returnArray);
            il.Emit(OpCodes.Ldloc_1); //loads idx
            il.Emit(OpCodes.Ldarg_0); // this
            il.Emit(OpCodes.Call, callFixtureNew);
            il.Emit(OpCodes.Stelem, type); //[idx]= New(); Array upsideDown
            il.Emit(OpCodes.Ldloc_1); //loads idx
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Sub); //idx--
            il.Emit(OpCodes.Stloc_1); //[idx]
            il.Emit(OpCodes.Ldloc_1); // idx
            il.Emit(OpCodes.Ldc_I4, -1);
            il.Emit(OpCodes.Beq, end);
            il.Emit(OpCodes.Br,loopBody);

            il.MarkLabel(end);
            il.Emit(OpCodes.Ldloc_0); //array
            il.Emit(OpCodes.Castclass, type);
            il.Emit(OpCodes.Ret);

        }

        private void ImplementNewMethod(MethodBuilder newMethodBuilder, Type type)
        {
            il = newMethodBuilder.GetILGenerator();
            paramObject = il.DeclareLocal(type);//Declare Student
            
            callGetRandomString = typeof(Randomize).GetMethod("GetRandomString");
            callGetRandomInteger = typeof(Randomize).GetMethod("GetRandomInteger");
            callFixtureFor = typeof(AutoFixture).GetMethod("For", new Type[] { typeof(Type) });
            callFixtureNew = typeof(IFixture).GetMethod("New");

            CallCtor(il, type);

            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);

        }

        private void CallCtor(ILGenerator il, Type type)
        {
            ConstructorInfo ctor = getSmallestConstructor(type);
            if (ctor == null)
                return;
            if (ctor.GetParameters().Length == 0)
            {
                il.Emit(OpCodes.Newobj, ctor);  //inicializa Student
                il.Emit(OpCodes.Stloc_0);
                callRandomize(il,type);
            }
            else
            {
                foreach (ParameterInfo param in ctor.GetParameters())
                {
                    if (param.ParameterType == typeof(string))
                        il.Emit(OpCodes.Call, callGetRandomString);
                    else if (param.ParameterType.IsValueType)
                        il.Emit(OpCodes.Call, callGetRandomInteger);
                    else
                    {
                       
                        CallNewAutoFixture(param.ParameterType);
                        
                        //CallCtor(il, param.ParameterType);
                    }
                       
                }
                il.Emit(OpCodes.Newobj, ctor);
                il.Emit(OpCodes.Stloc_0);
            }
        }

        private void CallNewAutoFixture(Type type)
        {
            //Fazer push do tipo para a stack
            // Chamar o AutoFixture.For(..)
            // Chamar o New()
            il.Emit(OpCodes.Ldtoken, type);
            il.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle", new Type[1] { typeof(RuntimeTypeHandle) }));
            il.Emit(OpCodes.Call, callFixtureFor);
            il.Emit(OpCodes.Call, callFixtureNew);
            il.Emit(OpCodes.Castclass, type);

        }

        private void callRandomize(ILGenerator il, Type type)
        {
                PropertyInfo[] pi = type.GetProperties();
                foreach (PropertyInfo p in pi)
                {
                    il.Emit(OpCodes.Ldloc_0); //load Student
                    if (p.PropertyType == typeof(string))
                        il.Emit(OpCodes.Call, callGetRandomString);
                    else if (p.PropertyType.IsValueType)
                    {
                        il.Emit(OpCodes.Call, callGetRandomInteger);
                    }
                    MethodInfo setValue = p.GetSetMethod();
                    il.Emit(OpCodes.Call, setValue);
                }
        }

        private static ConstructorInfo getSmallestConstructor(Type classType)
        {
            ConstructorInfo[] constructors = classType.GetConstructors();
            if (constructors.Length == 0) return null;
            ConstructorInfo smallestCi = constructors[0];
            int smaller = constructors[0].GetParameters().Length;
            foreach (ConstructorInfo ci in constructors)
            {
                int dimension = ci.GetParameters().Length;
                if (dimension <= smaller)
                {
                    smallestCi = ci;
                    smaller = dimension;
                }
            }
            return smallestCi;
        }

       







        private static AssemblyBuilder CreateAsm(string name)
        {
            AssemblyName aName = new AssemblyName(name);
            AssemblyBuilder ab =
                AppDomain.CurrentDomain.DefineDynamicAssembly(
                    aName,
                    AssemblyBuilderAccess.RunAndSave);
            return ab;
        }
    }
}
