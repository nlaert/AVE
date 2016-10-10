using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;

namespace Settler.Test
{
    [TestClass]
    public class UnitTestSettler
    {
        [TestMethod]
        public void TestNew()
        {
            Fixture<Student> fix = AutoFixture.For<Student>();
            Student s1 = fix.New();
            Student s2 = fix.New();
            Console.WriteLine(s1); // Populate Student's fields randomly
            Console.WriteLine(s2); // Populate Student's fields randomly
        }

        [TestMethod]
        public void Test()
        {
            Fixture<Student> fix = AutoFixture
                .For<Student>()
                .Member("Name", "Jose Calhau", "Maria Papoila", "Augusto Seabra") // Field or property with the name Name
                .Member("Nr", 8713, 2312, 23123, 131, 54534);                     // Field or property with the name Nr
            Student s = fix.New(); // The properties Name and Nr have one of above values
        }

        [TestMethod]
        public void TestFill()
        {
            Fixture<Student> fix = AutoFixture.For<Student>();
            Student[] res = fix.Fill(7);
        }
    }
}
