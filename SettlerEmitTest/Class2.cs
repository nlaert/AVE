using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlerEmit.Test
{
    public class SettlerForStudent : IFixture
    {
        public  object New()
        {
            return new Student(Randomize.GetRandomInteger(), Randomize.GetRandomString(), (School)AutoFixture.For(typeof(School)).New());
        }

        public  object Fill(int num)
        {
            Console.WriteLine("hello");
            Student result = null;;
            return result;
        }
    }
}
