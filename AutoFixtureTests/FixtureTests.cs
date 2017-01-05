using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Test;

namespace AutoFixture.Tests
{
    [TestClass()]
    public class FixtureTests
    {
        [TestMethod()]
        public void NewTest()
        {
            Random rand = new Random();
            DateTime dt = new DateTime(1970, 1, 1);
            
            Fixture<Student> fix = AutoFixture
              .For<Student>()
              .Member("BirthDate", () => dt.AddMonths(1));

            Student s = fix.New();

            Assert.IsNotNull(s);
            Assert.IsNotNull(s.Name);
            Assert.AreNotEqual(0, s.Nr);
            Assert.AreEqual(s.BirthDate, new DateTime(1970, 2, 1));
            Console.WriteLine(s);

        }

        [TestMethod()]
        public void FillTest()
        {
            School school = new School();
            school.Location = "Chelas";
            school.Name = "ISEL";

            Fixture<Student> fix = AutoFixture
              .For<Student>()
              .Member("School", () =>
            {
                School sc = new School();
                sc.Location = "Chelas";
                sc.Name = "ISEL";
                return sc;
            });

            Student s = fix.New();

            Assert.IsNotNull(s);
            Assert.AreEqual(s.School, school);

        }
    }
}