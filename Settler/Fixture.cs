using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Settler
{
     public interface IFixture {
        Object New();
    }
    public class Fixture<T> : IFixture
    {
        private readonly Type klass;

        public Fixture() {
            klass = typeof(T);
        }

        public T New()
        {
            if (klass == null)
                throw new NullReferenceException();
            T temp;
            if(!klass.IsPrimitive && klass != typeof(string) && !klass.IsArray)
            {
                var ctor = klass.GetConstructors()[0];
                foreach(var param in ctor.GetParameters()){

                }
                foreach (FieldInfo field in klass.GetFields(BindingFlags.Instance | BindingFlags.Public))
                {
                    object obj = AutoFixture.For(field.DeclaringType).New();
                    field.SetValue(temp, obj);
                }
                temp = Activator.CreateInstance<T>();
                return temp;
            }
            return Fill();
           
        }

        Object IFixture.New() {
            return this.New();
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

        public T Fill()
        {
            
           
                T temp = Activator.CreateInstance<T>();
                temp.GetType().GetProperties()[0].SetValue(temp, Randomize.GetRandomInteger());
                return temp;

        }

        
    }
}
