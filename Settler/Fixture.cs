using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Settler
{
    public interface IFixture
    {
        Object New();
    }
    public class Fixture<T> : IFixture
    {
        private readonly Type klass;

        public Fixture()
        {
            klass = typeof(T);
        }

        public T New()
        {
            if (klass == null)
                throw new NullReferenceException();
            T temp;
            if (!klass.IsPrimitive && klass != typeof(string) && !klass.IsArray)
            {
                temp = (T)getInstance(klass);
                foreach (PropertyInfo prop in klass.GetProperties())
                {
                    IFixture debug = AutoFixture.For(prop.PropertyType);
                    object obj = debug.New();
                    prop.SetValue(temp, obj);
                }

                return temp;
            }
            if (klass.IsArray)
            {
                Fill(Randomize.GetRandomInteger());
            }
            return (T)FillPrimitive();

        }

        Object IFixture.New()
        {
            return this.New();
        }

        public Fixture<T> Member(string name, params object[] pool)
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

        public object FillPrimitive()
        {
            if (klass == typeof(string))
                return Randomize.GetRandomString();
            else
                return Randomize.GetRandomInteger();
        }

        public static object getInstance(Type t)
        {
            ConstructorInfo ci = getSmallestConstructor(t);
            ParameterInfo[] pi = ci.GetParameters();
            object[] parameters = new object[pi.Length];

            for (int i = 0; i < pi.Length; i++)
            {
                ParameterInfo parameterInfo = pi[i];
                ConstructorInfo cType = getSmallestConstructor(parameterInfo.ParameterType);
                if (cType == null || cType.GetParameters().Length == 0) parameters[i] = Activator.CreateInstance(parameterInfo.ParameterType);
                else { parameters[i] = getInstance(parameterInfo.ParameterType); }
            }
            return ci.Invoke(parameters);
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
    }
}
