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
            SettlerEmit.SettlerEmitter emitter = new SettlerEmit.SettlerEmitter();
            IFixture fix= emitter.CreateAssembly(typeof(Student));

           // Fixture<Student> fix = AutoFixture.For<Student>();
            Student s = (Student)fix.New();
            Assert.IsNotNull(s);
            Assert.IsNotNull(s.Name);
            Assert.AreNotEqual(0, s.Nr);
            Console.WriteLine(s);
        }

        [TestMethod]
        public void TestFill()
        {
            SettlerEmit.SettlerEmitter emitter = new SettlerEmit.SettlerEmitter();
            IFixture fix = emitter.CreateAssembly(typeof(Student));
            Student[] res = (Student[])fix.Fill(7);
            foreach (Student s in res)
            {
                Assert.IsNotNull(s);
                Assert.IsNotNull(s.Name);
                Assert.AreNotEqual(0, s.Nr);
            }
        }
        [TestMethod]
        public void TestMember()
        {
            String[] expectedNames = { "Jose Calhau", "Maria Papoila", "Augusto Seabra" };
            Object[] expectedNrs = { 8713, 2312, 23123, 131, 54534 };
            Fixture<Student> fix = AutoFixture
                .For<Student>()
                .Member("Name", expectedNames) // Field or property with the name Name
                .Member("Nr", expectedNrs);   // Field or property with the name Nr
            Student s = fix.New(); // The properties Name and Nr have one of above values
            CollectionAssert.Contains(expectedNames, s.Name);
            CollectionAssert.Contains(expectedNrs, s.Nr);
        }
    }
}
