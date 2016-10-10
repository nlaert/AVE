using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settler
{
    public class Fixture<T>
    {
        private readonly Type klass;

        public Fixture() {
            klass = typeof(T);
        }

        public T New()
        {
            throw new NotImplementedException();
        }

        public Fixture<T> Member(string name, params object [] pool)
        {
            return this;
        }

        public T[] Fill(int size)
        {
            T[] res = new T[size];
            for (int i = 0; i < size; i++)
            {
                res[i] = New();
            }
                return res;
        }
    }
}
