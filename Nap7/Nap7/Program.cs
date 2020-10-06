using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nap7
{
    class Program
    {
        public delegate DateTime AsyncMethodDelegate(int ido, string nev);

        static DateTime Metodus(int ido, string nev)
        {
            Console.WriteLine("Start Nev: {0}: {1}", nev, DateTime.Now.ToLongTimeString());
            Thread.Sleep(ido);
            Console.WriteLine("End Nev: {0}", nev);
            return DateTime.Now;
        }


        static void Main(string[] args)
        {
            AsyncMethodDelegate am = Metodus;

            IAsyncResult ia = am.BeginInvoke(2000, "methodus", null, null);
            Console.WriteLine("Elindult a metódus");
            Console.WriteLine("IsCompleted: {0}", ia.IsCompleted);

            ////polling
            //while (!ia.IsCompleted)
            //{
            //    Console.WriteLine("Varakozok...");
            //    Thread.Sleep(200);
            //}

            ////blokkolas
            //DateTime vege = am.EndInvoke(ia);
            //Console.WriteLine("Invoke befejeződött: {0}",vege.ToLongTimeString());

            //wait
            //while (!ia.AsyncWaitHandle.WaitOne(300))
            //{
            //    Console.WriteLine("Varakozok...");
            //}

            //// Többszörös példa
            //IAsyncResult ia1 = am.BeginInvoke(2000, "methodus-1", null, null);
            //IAsyncResult ia2 = am.BeginInvoke(2500, "methodus-2", null, null);
            //IAsyncResult ia3 = am.BeginInvoke(1500, "methodus-3", null, null);

            //WaitHandle[] handles = new WaitHandle[]{
            //    ia1.AsyncWaitHandle,ia2.AsyncWaitHandle,ia3.AsyncWaitHandle
            //};

            //WaitHandle.WaitAll(handles);
            //Console.WriteLine("Befejeződtek");
            //Console.WriteLine(am.EndInvoke(ia1));
            //Console.WriteLine(am.EndInvoke(ia2));
            //Console.WriteLine(am.EndInvoke(ia3));

            //callback
            am.BeginInvoke(3000, "callbask pelda", FeldolgozoCallback, am);
            
            Console.ReadLine();
        }

        public static void FeldolgozoCallback(IAsyncResult result)
        {
            AsyncMethodDelegate am = (AsyncMethodDelegate)result.AsyncState;
            Console.WriteLine("Feldolgozas megtortent: {0}", am.EndInvoke(result));
        }
    }
}
