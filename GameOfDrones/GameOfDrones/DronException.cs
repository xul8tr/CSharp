using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfDrones
{
    internal class DronException: Exception
    {
        public DronException(Dron dron)
            : base(string.Format("Drón: {0} meghibásodott.", dron.Azonosito))
        {
        }

        public DronException(Dron dron, string hiba)
            : base(string.Format("Drón: {0} hibát jelez: {1}", dron.Azonosito, hiba))
        {
        }
    }
}
