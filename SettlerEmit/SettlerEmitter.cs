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
        ConstructorInfo ctor;
        MethodInfo callNew;
        MethodInfo callGetRandomString;
        MethodInfo callGetRandomInteger;


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
                new Type[] { typeof(int)});

            ImplementNewMethod(NewMethodBuilder, type);
            ImplementFillMethod(FillMethodBuilder, type);

            Type FixtureType = typeBuilder.CreateType();
            asmBuilder.Save(DLL_NAME);


            return (IFixture)Activator.CreateInstance(FixtureType);
        }

        private void ImplementFillMethod(MethodBuilder fillMethodBuilder, Type type)
        {

            il = fillMethodBuilder.GetILGenerator();
            paramObject = il.DeclareLocal(type);//0


            string hello = "hello";
            il.Emit(OpCodes.Ldstr, hello);
            MethodInfo method = typeof(System.Console).GetMethod("WriteLine",
                BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(object) }, null);
            il.Emit(OpCodes.Call, method);

            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);

        }

        private void ImplementNewMethod(MethodBuilder newMethodBuilder, Type type)
        {
            
            il = newMethodBuilder.GetILGenerator();
            paramObject = il.DeclareLocal(type);//Declare Student

            
            PropertyInfo[] pi = type.GetProperties();
            ctor = getSmallestConstructor(type);
            
            callGetRandomString = typeof(Randomize).GetMethod("GetRandomString");
            callGetRandomInteger = typeof(Randomize).GetMethod("GetRandomInteger");

            il.Emit(OpCodes.Newobj, ctor);  //inicializa Student
            il.Emit(OpCodes.Stloc_0);

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

            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);

    

 

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
