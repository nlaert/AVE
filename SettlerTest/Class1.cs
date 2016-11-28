
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settler.Test
{
    class Class1
    {
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
    }
}
