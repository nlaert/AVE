using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFixture.Test
{
    class Student
    {
        public Student() {
        
        }
        public Student(int p1, string p2, School p3, DateTime BirthDate)
        {
            this.Nr = p1;
            this.Name = p2;
            this.School = p3;
            this.BirthDate = BirthDate;
        }

        public Student(int p1, string p2, School p3)
        {
            this.Nr = p1;
            this.Name = p2;
            this.School = p3;
        }
        public int Nr { get; set; }
        public string Name { get; set; }
        public School School { get; set; }
        public DateTime BirthDate { get; set; }
        public override string ToString()
        {
            return String.Format("({0}) {1} {2} {3}",School,  Nr, Name, BirthDate);
        }
        public override bool Equals(object obj)
        {
            Student s = obj as Student;
            return s.BirthDate == this.BirthDate && s.Name == this.Name && s.Nr == this.Nr && s.School == this.School;
        }
    }
}
