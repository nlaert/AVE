﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SettlerEmit
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
        private Dictionary<String, object> Map = new Dictionary<string, object>();
        public Fixture()
        {
            klass = typeof(T);
            IsSingleton = false;
        }

        public T New()
        {
            if (Map.ContainsKey(klass.Name))
                return (T)Map[klass.Name];
            if (LoadAssemblies())
                return (T)Map[klass.Name];
            IFixture fixture = new SettlerEmitter().CreateAssembly(klass);
            T newObj = (T)fixture.New();
            Map.Add(klass.Name, newObj);
            return newObj;
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

        public Fixture<T> Member(string name, params object[] pool)
        {
            IEnumerable<PropertyInfo> pi = klass.GetProperties().Where(p => p.Name.Equals(name));
            if (pi.Count() == 0)
                throw new InvalidOperationException();
            var index = Randomize.GetRandomInteger(pool.Length);
            /*
            if (typeof(IFixture).IsAssignableFrom(pool[index].GetType()))
            {
                var x = pool[index].GetType().GetMethod("New").Invoke(pool[index], null);
                Map.Add(name, x);
                //falta ir buscar o valor do singleton
            }
            else

                */
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

        private bool LoadAssemblies()
        {
            Assembly asm;
            T newObj;
            string typeName = klass.Name;

            try
            {
                asm = Assembly.LoadFrom(SettlerEmitter.AssemblyNamePrefix + typeName + SettlerEmitter.AssemblyFileExtension);
                newObj = (T)asm.CreateInstance(SettlerEmitter.AssemblyNamePrefix + typeName);
            }
            catch (Exception)
            {
                return false;
            }
            Map.Add(typeName, newObj);
            return true;
        }
    }
}