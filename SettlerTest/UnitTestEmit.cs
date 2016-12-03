using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settler.Test
{
    [TestClass]
    class UnitTestEmit
    {
        public static void Main()
        {
            SettlerEmit.SettlerEmitter emitter = new SettlerEmit.SettlerEmitter();
            emitter.CreateAssembly(typeof(Student));
        }

        [TestMethod]
        public void TestNew()
        {
            Fixture<Student> fix = AutoFixture.For<Student>();
            Student s = fix.New();
            Assert.IsNotNull(s);
            Assert.IsNotNull(s.Name);
            Assert.AreNotEqual(0, s.Nr);
            Console.WriteLine(s);
        }
       /* [TestMethod]
        public void TestFill()
        {
            Fixture<Student> fix = AutoFixture.For<Student>();
            Student[] res = fix.Fill(7);
            foreach (Student s in res)
            {
                Assert.IsNotNull(s);
                Assert.IsNotNull(s.Name);
                Assert.AreNotEqual(0, s.Nr);
            }
        } */
    }
}
