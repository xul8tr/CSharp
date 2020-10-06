using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace Szofordito
{
    /// <summary>
    /// A főprogram a feladathoz
    /// </summary>
    class Program
    {
        /// <summary>
        /// A program itt indul
        /// </summary>
        /// <param name="args">Ez tartalmazza a parancssori adatokat</param>
        static void Main(string[] args)
        {
            //Ebbe kerülnek a szavak, amiket meg kell fordítani
            Stack mainstack = new Stack();
            //Ellenőrizem hogy van-e parancssori paraméter
            if (args.Length != 0)
            {
                //Ha a parancssori paraméter egy valós fájlnév,... 
                if (File.Exists(args[0]))
                {
                    //...akkor betolvastatom abból a tartalmat és beteszem a mainstackbe
                    mainstack = StackFromFile(args[0]);
                }
                else
                {
                    //egyébként az összes szöveget beletöltöm a stackbe
                    foreach (string s in args)
                    {
                        mainstack.Push(s);
                    }
                }
            }
            //ha nincsenek parancssori paraméterek
            else
            {
                //ablak letörlése és adatbekérés
                Console.Clear();
                Console.Write("Kerem adjon meg tetszoleges szavakat, amiknek a sorrendjet a program megforditja: ");
                //a szavak fogja tartalmazni ömlesztve az adatokat
                string szavak = Console.ReadLine();
                //a szavak tömbbe betöltöm a szétdarabolt "szavak"-at
                string[] szavaktomb = szavak.Split(new char[] { ' ', ',', ';' });
                //ha legalább egy elem van
                if (szavaktomb.Length != 0)
                {
                    foreach (string s in szavaktomb)
                    {
                        //beteszem a stackbe
                        mainstack.Push(s);
                    }
                }
                else
                {
                    //egyébként hibaüzenet
                    Console.WriteLine("On nem adott meg szavakat. A program most kilep.");
                }
            }
            //majd a szavak kiiratása következik
            while (mainstack.Count > 0)
            {
                //mivel a stack Last In First Out, ezért a sorrend megfordítása implicit megtörténik
                Console.Write(mainstack.Pop().ToString() + " ");
            }
            Console.Read();
        }
        /// <summary>
        /// Betölt egy létező fájlt és a tartalmát darabolva beteszi egy stackbe
        /// </summary>
        /// <param name="filename">Útvonal a fájlra</param>
        /// <returns>Visszaad egy stack-et a fájl szavaival</returns>
        static Stack StackFromFile(string filename)
        {
            //ez lesz a visszatérési érték
            Stack stack = new Stack();
            //ebbe kerül ömlesztve a fájl tartalma
            string readstring = string.Empty;
            //ha létezik a fájl
            if (File.Exists(filename))
            {
                //létrehozok egy streamreader-t
                StreamReader streamreader = new StreamReader(filename);
                //és a végéig beolvasom a fájlt
                readstring = streamreader.ReadToEnd();
            }
            //az ömlesztett adatokat ezután feldarabolom és beteszem egy tömbbe
            string[] stringarray = readstring.Split(new char[] { ' ', ',', ';' });
            foreach (string s in stringarray)
            {
                //majd egyesével bekerülnek a stackbe
                stack.Push(s);
            }
            return stack;
        }
    }
}
