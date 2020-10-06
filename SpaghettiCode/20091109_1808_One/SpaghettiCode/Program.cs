using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Diagnostics;
using System.Threading;
using System.IO;
namespace SpaghettiCode
{
    class Program
    {
        static void Main(string[] args)
        {
        start:
            ConsoleKeyInfo i = new ConsoleKeyInfo();
            int beginchar = 0;
            int firstchar = beginchar + 1;
            int stringlength = 0;
            int step = 1;
            List<char> charlist = new List<char>();
            string holderstring = String.Empty;
            List<string> stringlist = new List<string>();
            StringBuilder stringBuilder = new StringBuilder(holderstring);
            string flattenstring = string.Empty;
            string questiontext = string.Empty;
            List<char> newcharlist = new List<char>();
            StringBuilder newstringBuilder = new StringBuilder(holderstring);
            List<string> newstringlist = new List<string>();
            switch (args.Length)
            {
                case 0:
                    i = GetKey(charlist, ConsoleKey.Enter);
                    foreach (char c in charlist)
                    {
                        stringBuilder.Append(c);
                    }
                    holderstring = stringBuilder.ToString(0, stringBuilder.Length);
                    holderstring = GetFileText(holderstring);
                    stringlist.Add(holderstring);
                    WriteChar(beginchar, stringlist, step, 500);
                    i = GetKey(charlist, ConsoleKey.Enter);
                    break;
                case 1:
                    flattenstring = args.GetValue(0) as string;
                    flattenstring = GetFileText(flattenstring);
                    if (flattenstring != null)
                    {
                        foreach (char c in flattenstring)
                        {
                            charlist.Add(c);
                        }
                        foreach (char c in charlist)
                        {
                            stringBuilder.Append(c);
                        }
                        holderstring = stringBuilder.ToString(0, stringBuilder.Length);
                        stringlist.Add(holderstring);
                        WriteChar(beginchar, stringlist, step, 500);
                    }
                    i = GetKey(charlist, ConsoleKey.Enter);
                    break;
                default:
                    foreach (string s in args)
                    {
                        flattenstring += s + " ";
                    }
                    foreach (char c in flattenstring)
                    {
                        charlist.Add(c);
                    }
                    foreach (char c in charlist)
                    {
                        stringBuilder.Append(c);
                    }
                    holderstring = stringBuilder.ToString(0, stringBuilder.Length);
                    stringlist.Add(holderstring);
                    WriteChar(beginchar, stringlist, step, 500);
                    i = GetKey(charlist, ConsoleKey.Enter);
                    break;
            }
            ResourceManager myResMgr = new ResourceManager(typeof(Program));
            try
            {
                questiontext = myResMgr.GetString("Question");
            }
            catch (MissingManifestResourceException e)
            {
                questiontext = "Reverse?";
                Debug.Assert(false, "Resource is missing. Deatils: " + e.ToString());
            }
            if (questiontext != null)
            {
                foreach (char c in questiontext)
                {
                    newcharlist.Add(c);
                }
                foreach (char c in newcharlist)
                {
                    newstringBuilder.Append(c);
                }
                holderstring = newstringBuilder.ToString(0, newstringBuilder.Length);
                newstringlist.Add(holderstring);
                WriteChar(beginchar, newstringlist, step, 100);
                i = GetKey(charlist, ConsoleKey.Y, ConsoleKey.N);
                switch (i.Key)
                {
                    case ConsoleKey.Y:
                        WriteChar(beginchar, stringlist, -step, 100);
                        i = GetKey(charlist, ConsoleKey.Enter);
                        break;
                    case ConsoleKey.N:
                        newcharlist.Clear();
                        newstringlist.Clear();
                        newstringBuilder = new StringBuilder();
                        try
                        {
                            questiontext = myResMgr.GetString("Question2");
                        }
                        catch (MissingManifestResourceException e)
                        {
                            questiontext = "Restart?";
                            Debug.Assert(false, "Resource is missing. Deatils: " + e.ToString());
                        }
                        if (questiontext != null)
                        {
                            foreach (char c in questiontext)
                            {
                                newcharlist.Add(c);
                            }
                            foreach (char c in newcharlist)
                            {
                                newstringBuilder.Append(c);
                            }
                            holderstring = newstringBuilder.ToString(0, newstringBuilder.Length);
                            newstringlist.Add(holderstring);
                            WriteChar(beginchar, newstringlist, step, 100);
                            i = GetKey(charlist, ConsoleKey.Y, ConsoleKey.N);
                            switch (i.Key)
                            {
                                case ConsoleKey.Y:
                                    newcharlist.Clear();
                                    newstringlist.Clear();
                                    newstringBuilder = new StringBuilder();
                                    try
                                    {
                                        questiontext = myResMgr.GetString("Question3");
                                    }
                                    catch (MissingManifestResourceException e)
                                    {
                                        questiontext = "Clean args?";
                                        Debug.Assert(false, "Resource is missing. Deatils: " + e.ToString());
                                    }
                                    if (questiontext != null)
                                    {
                                        foreach (char c in questiontext)
                                        {
                                            newcharlist.Add(c);
                                        }
                                        foreach (char c in newcharlist)
                                        {
                                            newstringBuilder.Append(c);
                                        }
                                        holderstring = newstringBuilder.ToString(0, newstringBuilder.Length);
                                        newstringlist.Add(holderstring);
                                        Console.Clear();
                                        WriteChar(beginchar, newstringlist, step, 100);
                                        i = GetKey(charlist, ConsoleKey.Y, ConsoleKey.N);
                                        switch (i.Key)
                                        {
                                            case ConsoleKey.Y:
                                                args = new string[] { };
                                                break;
                                            case ConsoleKey.N:
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    goto start;
                                    break;
                                case ConsoleKey.N:
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private static string GetFileText(string flattenstring)
        {
            if (flattenstring != null)
            {
                if (File.Exists(flattenstring))
                {
                    try
                    {
                        StreamReader myreader;
                        myreader = new StreamReader(flattenstring);
                        flattenstring = myreader.ReadToEnd();
                    }
                    catch (IOException)
                    {
                        flattenstring = "An I/O exception occured!";
                    }
                    catch (OutOfMemoryException)
                    {
                        flattenstring = "An out of memory exception occured!";
                    }
                }
            }
            return flattenstring;
        }

        private static ConsoleKeyInfo GetKey(List<char> charlist, ConsoleKey consoleKey)
        {
            return GetKey(charlist, consoleKey, consoleKey);
        }
        private static ConsoleKeyInfo GetKey(List<char> charlist, ConsoleKey consoleKey, ConsoleKey consoleKey2)
        {
            //Console.Clear();
            ConsoleKeyInfo i = new ConsoleKeyInfo();

            while (i.Key != consoleKey && i.Key != consoleKey2)
            {
                i = Console.ReadKey();
                if (i.Modifiers != ConsoleModifiers.Alt || i.Modifiers != ConsoleModifiers.Control)
                {
                    if (i.Key != consoleKey && i.Key != consoleKey2)
                    {
                        charlist.Add(i.KeyChar);
                    }
                }
            }
            return i;
        }

        private static void WriteChar(int beginchar, List<string> stringlist, int step, int sleep)
        {
            int stringlength;
            Console.Clear();
            foreach (string s in stringlist)
            {
                stringlength = s.Length;
                for (int j = step > 0 ? beginchar : stringlength - 1; ((j < stringlength - 1) && step > 0) || ((j >= beginchar) && step <= 0); j = j + step)
                {
                    if (!(j.ToString().Equals((step > 0 ? stringlength : beginchar).ToString("d"))))
                    {
                        Console.Write(s.ElementAt(j));
                    }
                    Thread.Sleep(sleep);
                }
                Console.WriteLine(s.ElementAt(step > 0 ? stringlength - 1 : beginchar));
            }
        }
    }
}