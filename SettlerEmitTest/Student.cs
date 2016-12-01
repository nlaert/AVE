using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettlerEmit.Test
{
    public class Student
    {
        public Student() {
            
        }
        public Student(int p1, string p2, School p3)
        {
            this.Nr = p1;
            this.Name = p2;
            //this.School = p3;
        }
        public int Nr { get; set; }
        public string Name { get; set; }
        //public School School { get; set; }
        public override string ToString()
        {
            return "test";// String.Format("({0}) {1} {2}", School, Nr, Name);
        }
    }
}
