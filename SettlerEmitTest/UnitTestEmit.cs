using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SettlerEmit;

namespace SettlerEmit.Test
{
    [TestClass]
    public class UnitTestEmit
    {
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

        [TestMethod]
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
        }
    }
}
