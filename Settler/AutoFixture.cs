﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settler
{
    public class AutoFixture
    {
        public static Fixture<T> For<T>()
        {
            return new Fixture<T>();
        }

        public static IFixture For(Type klass)
        {
            return (IFixture) typeof(AutoFixture)
                .GetMethod("For", Type.EmptyTypes)
                .MakeGenericMethod(klass)
                .Invoke(null, new object[0]);
        }
        public static IFixture Member(Type klass)
        {
            return (IFixture)typeof(AutoFixture)
                .GetMethod("Member", Type.EmptyTypes)
                .MakeGenericMethod(klass)
                .Invoke(null, new object[0]);
        }
    }
}
