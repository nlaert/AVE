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
        MethodInfo callToJson;
        MethodInfo strBuilderAppend;
        MethodInfo strBuilderToString;
        LocalBuilder localStringBuilder;
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

            AssemblyBuilder asmBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(ASM_NAME), AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder modBuilder = asmBuilder.DefineDynamicModule(MOD_NAME, DLL_NAME);
            TypeBuilder typeBuilder = modBuilder.DefineType(TYP_NAME);
            typeBuilder.AddInterfaceImplementation(typeof(IFixture));

            MethodBuilder FixtureMethodBuilder = typeBuilder.DefineMethod(
                "Fixture",
                MethodAttributes.Public |
                MethodAttributes.Virtual |
                MethodAttributes.ReuseSlot,
                typeof(string),
                new Type[1] { typeof(object) });

            ImplementFixtureMethod(FixtureMethodBuilder, type);

            Type FixtureType = typeBuilder.CreateType();
            asmBuilder.Save(DLL_NAME);


            return (IFixture)Activator.CreateInstance(FixtureType);
        }

        private void ImplementFixtureMethod(MethodBuilder fixtureMethodBuilder, Type type)
        {
            il = fixtureMethodBuilder.GetILGenerator();
            ctor = typeof(StringBuilder).GetConstructor(new Type[] { });

            callGetRandomString = typeof(Randomize).GetMethod("GetRandomString");
            //callGetRandomInteger = typeof(Randomize).GetMethod("GetRandomInteger");
            //callNew = typeof(Fixture).GetMethod("New");

            paramObject = il.DeclareLocal(typeof(object));//0
            localString = il.DeclareLocal(typeof(string));//1
            lengthLocal = il.DeclareLocal(typeof(int));//2
            string hello = "hello";
            il.Emit(OpCodes.Ldstr, hello);
            MethodInfo method = typeof(System.Console).GetMethod("WriteLine",
                BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(object) }, null);
            il.Emit(OpCodes.Call, method);

            //if (type.IsArray)
            //{
            //    GetArrayValues();
            //}
            //else if (!type.IsPrimitive && type != typeof(string))
            //{
            //    int i = 0;
            //    var properties = type.GetProperties();
            //    AppendToStringBuilder("{");
            //    foreach (var p in properties)
            //    {
            //        Type propType = p.PropertyType;

            //        MethodInfo propertyGetMethod = p.GetGetMethod();

            //        AppendToStringBuilder("\"" + p.Name + "\": ");
            //        il.Emit(OpCodes.Ldloc_2);
            //        il.Emit(OpCodes.Call, propertyGetMethod);//retorna o valor



            //        if (propType.IsPrimitive || propType == typeof(string))
            //        {
            //            if (propType.IsValueType)
            //            {
            //                il.Emit(OpCodes.Box, propType);

            //            }
            //            else
            //            {
            //                lengthLocal = il.DeclareLocal(propType);
            //                il.Emit(OpCodes.Castclass, typeof(object));
            //                // il.Emit(OpCodes.Stloc_3);
            //                // il.Emit(OpCodes.Ldloc_3);
            //            }

            //            il.Emit(OpCodes.Call, callGetPrimitiveValue);
            //        }
            //        else
            //        {
            //            il.Emit(OpCodes.Call, callToJson);
            //        }
            //        il.Emit(OpCodes.Stloc_1);
            //        AppendToStringBuilder();
            //        if (i < properties.Length - 1)
            //            AppendToStringBuilder(",");
            //        i++;

            //    }
            //    AppendToStringBuilder("}");
            //}
            //else
            //{
            //    EmitPrimitiveValue(type);
            //}

            //il.Emit(OpCodes.Ldloc_0);
            //il.Emit(OpCodes.Call, strBuilderToString);
            //il.Emit(OpCodes.Stloc_1);
            //il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Ret);

        }

        private void AppendToStringBuilder()
        {
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Call, strBuilderAppend);
            il.Emit(OpCodes.Pop);
        }

        private void AppendToStringBuilder(string str)
        {
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ldstr, str);
            il.Emit(OpCodes.Call, strBuilderAppend);
            il.Emit(OpCodes.Pop);
        }

        private void EmitPrimitiveValue(Type type)
        {
            il.Emit(OpCodes.Ldloc_2);

            if (type.IsValueType)
            {
                il.Emit(OpCodes.Box, typeof(object));

            }
            else
            {
                lengthLocal = il.DeclareLocal(type);
                il.Emit(OpCodes.Castclass, typeof(object));
                // il.Emit(OpCodes.Stloc_3);
                //  il.Emit(OpCodes.Ldloc_3);
            }
            //il.Emit(OpCodes.Call, callGetPrimitiveValue);
            il.Emit(OpCodes.Stloc_1);
            AppendToStringBuilder();
        }






        private void GetArrayValues()
        {
            // StringBuilder JSON = new StringBuilder("[");
            // AppendToStringBuilder("[");

            il.Emit(OpCodes.Ldloc_2);
            //MethodInfo callArrayToJson = typeof(EmitterHelper).GetMethod("ArrayToJson");
            //il.Emit(OpCodes.Call, callArrayToJson);
            il.Emit(OpCodes.Stloc_1);
            AppendToStringBuilder();
            //  AppendToStringBuilder("]");

        }
    }
}
