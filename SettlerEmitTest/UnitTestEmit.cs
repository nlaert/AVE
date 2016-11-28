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
    }
}
