using Settler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoFixture
{
    public interface IFixture
    {
        Object New();
        Object Fill(int size);
    }
    public class Fixture<T> : IFixture
    {
        private readonly Type klass;
        private Object SingletonObject;
        private bool IsSingleton;
        private Dictionary<String, Object> Map = new Dictionary<string, object>();
        private List<string> IgnoredProperties = new List<string>();
       
        public Fixture()
        {
            klass = typeof(T);
            IsSingleton = false;
        }

        public T New()
        {
            if (klass == null)
                throw new NullReferenceException();
            if (IsSingleton && SingletonObject != null)
                return (T)SingletonObject;
            T temp;
            if (!klass.IsPrimitive && klass != typeof(string) && !klass.IsArray)
            {
                temp = (T)getInstance(klass);
                foreach (PropertyInfo prop in klass.GetProperties())
                {
                    if (!IgnoredProperties.Contains(prop.Name))
                    {

                        Object value;
                        if (Map.TryGetValue(prop.Name, out value))
                        {
                            if (typeof(IFixture).IsAssignableFrom(value.GetType()))
                            {
                                var x = value.GetType().GetMethod("New").Invoke(value, null);
                                prop.SetValue(temp, x);
                            }
                            else if(typeof(MemberObjectFunc).IsAssignableFrom(value.GetType())) {
                                Object funcValue = ((MemberObjectFunc)value).Invoke();
                                prop.SetValue(temp, funcValue);
                            }
                            else
                                prop.SetValue(temp, value);
                        }
                        else
                            prop.SetValue(temp, AutoFixture.For(prop.PropertyType).New());
                    }
                }

                foreach (FieldInfo field in klass.GetFields())
                {
                    if (!IgnoredProperties.Contains(field.Name))
                    {
                        Object value;
                        if (Map.TryGetValue(field.Name, out value))
                            field.SetValue(temp, value);
                        else
                            field.SetValue(temp, AutoFixture.For(field.FieldType).New());
                    }
                }
                SingletonObject = temp;
                return temp;
            }
            if (klass.IsArray)
            {
                return (T)AutoFixture.For(klass.GetElementType()).Fill(Randomize.GetRandomInteger());
            }
            return (T)FillPrimitive();

        }

        Object IFixture.New()
        {
            return this.New();
        }

        Object IFixture.Fill(int size)
        {
            return this.Fill(size);
        }

        public Fixture<T> Singleton()
        {
            IsSingleton = true;
            return this;
        }

        public delegate Object MemberObjectFunc();
        
            //need to use delegates in this i think
        public Fixture<T> Member(string name, MemberObjectFunc someFunction)
        {
            
           Object value = someFunction.Invoke() ;

            PropertyInfo p1 = klass.GetProperty(name);
           
                   
            if (p1 != null && !p1.PropertyType.IsAssignableFrom(value.GetType()))
                    throw new InvalidCastException();

            FieldInfo f1 = klass.GetField(name);
            if (f1 != null && !f1.FieldType.IsAssignableFrom(value.GetType()))
                    throw new InvalidCastException();

            Map.Add(name, someFunction);
            return this;
        }

        public Fixture<T> Member(string name, params object[] pool)
        {
            IEnumerable<PropertyInfo> pi = klass.GetProperties().Where(p => p.Name.Equals(name));
            if (pi.Count() == 0)
                throw new InvalidOperationException();
            var index = Randomize.GetRandomInteger(pool.Length);
            Map.Add(name, pool[index]);


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

        public Fixture<T> Ignore(string ToBeIgnored)
        {
            if (!IgnoredProperties.Contains(ToBeIgnored))
                IgnoredProperties.Add(ToBeIgnored);
            return this;
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
                if (cType == null || cType.GetParameters().Length == 0)
                    parameters[i] = Activator.CreateInstance(parameterInfo.ParameterType);
                else
                {
                    parameters[i] = getInstance(parameterInfo.ParameterType);
                }
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
