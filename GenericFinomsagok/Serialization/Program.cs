using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace Serialization
{
    [Serializable]
    class teszt
    {
        public int a { get; set; }
        public string b { get; set; }
        public bool c { get; set; }

    }

    

    class Program
    {
        static string sorosit(teszt test)
        {
            string s;
            IFormatter f = new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
            f.Serialize(
        }

        static void Main(string[] args)
        {
        }
    }
}
