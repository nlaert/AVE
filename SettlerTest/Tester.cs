using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settler.Test
{
    class Tester
    {
        public static void Main()
        {
            SettlerEmit.SettlerEmitter emitter = new SettlerEmit.SettlerEmitter();
            emitter.CreateAssembly(typeof(Student));
        }
        
    }
}
