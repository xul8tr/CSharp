using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericFinomsagok
{
    class GenericTipusWithEnum<T>
    {
        public int val = 5;
        static GenericTipusWithEnum()
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Type member legyen ENUM !!!");
        }
    }

    enum szinek {feher, fekete};

    class Program
    {
        static void Main(string[] args)
        {
            GenericTipusWithEnum<int> gtwe =null;

            try
            {
                gtwe = new GenericTipusWithEnum<int>();
            }
            catch
            {
            }

            Console.WriteLine(gtwe.val);
        }
    }
}
