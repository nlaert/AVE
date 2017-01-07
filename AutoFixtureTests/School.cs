using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoFixture.Test
{
    class School
    {
        public string Name { get; set;  }
        public string Location{ get; set; }
        public override bool Equals(object obj)
        {
            School s = obj as School;
            return s.Name == this.Name && s.Location == this.Location;
        }
    }
}
