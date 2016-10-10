using System;
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
                .GetMethod("For")
                .MakeGenericMethod(klass)
                .Invoke(null, new object[0]);
        }
    }
}
