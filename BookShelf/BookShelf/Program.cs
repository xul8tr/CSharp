using System;
using System.Collections.Generic;
using System.Text;

namespace BookShelf
{
    class Program
    {
        const char delimiter = ';';


        private enum Genere
        {
            mese,
            regény,
            történelem,
            szakkönyv,
            külföldi
        }

        // Ez az osztály reprezentál egy könyvet. Mivel csak adatokat tárol, így autoproperty-kkel oldottam meg a címet, a kiadás évét, az árat és a műfajt.
        // forrás: http://csharp.net-tutorials.com/csharp-3.0/automatic-properties/ 
        // A szerző propertynél az adatokat nagybetűsen tárolom, hogy később a keresésnél ne lehessen kis/nagybetű eltérés
        // Az osztály private, így nem kell felkészíteni külön adatérvényesítésre. Elvárt, hogy érvényes adatokkal 
        // hívják meg
        private class Book
        {
            private string author;

            public string Author
            {
                get { return author; }
                set { author = value.ToUpperInvariant(); }
            }

            public string Title { get; set; }            
            public int DateOfIssue { get; set; }
            public int Price { get; set; }
            public Genere Genere { get; set; }
        }

        // Ez az osztály reprezentálja a polcot. Mivel csak 1 db polcunk lehet, a constructor private és egy 
        // statikus, vagyis a Shelve típuson (és nem a példányokon) létező Instance függvénnyel készítem el 
        // az egyetlen érvényes polc példányt, amit egy lokális változóban (fieldben) tárolok. Ezt Singleton-nak hívják
        // forrás: https://msdn.microsoft.com/en-us/library/ff650316.aspx
        private class Shelf
        {
            private static Shelf instance;
            private IList<Book> booksOnShelfe;

            // private Constructor
            private Shelf() { }

            // Ez a lista tárolja a könyveket
            private IList<Book> BooksOnShelfe
            {
                get
                {
                    // lazy initialization
                    // forrás: https://msdn.microsoft.com/en-us/library/dd997286%28v=vs.110%29.aspx
                    if (booksOnShelfe == null)
                    {
                        booksOnShelfe = new List<Book>();
                    }

                    return booksOnShelfe;
                }
            }

            // Ez tárolja a pédányt: mindig csak egy polc lehet
            public static Shelf Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new Shelf();
                    }

                    return instance;
                }
            }

            // Végigszalad a polcon lévő könyveken és visszaadja az összesített árukat
            public int SumPrice
            {
                get
                {
                    int price = 0;
                    foreach (Book alreadyShelvedBook in BooksOnShelfe)
                    {
                        price += alreadyShelvedBook.Price;
                    }

                    return price;
                }
            }

            // Lekérdezi a szerzőket. Csak azokat adja vissza, amik egyediek.
            public IList<string> ListOfAuthors
            {                
                get
                {
                    IList<string> listOfAuthors = new List<string>();
                    foreach (Book alreadyShelvedBook in BooksOnShelfe)
                    {
                        if (!listOfAuthors.Contains(alreadyShelvedBook.Author))
                        {
                            listOfAuthors.Add(alreadyShelvedBook.Author);
                        }                        
                    }

                    return listOfAuthors;
                }
            }

            // Feltesz egy könyvet a polcra
            public void AddBook(Book book)
            {               
                if (book != null)
                {
                    bool alreadyOnshelve = false;

                    // Itt megnézzük, hogy a könyv még nincs-e hozzáadva a polchoz.
                    foreach (Book alreadyShelvedBook in BooksOnShelfe)
                    {
                        alreadyOnshelve = String.Equals(alreadyShelvedBook.Author, book.Author, StringComparison.InvariantCultureIgnoreCase);
                        alreadyOnshelve = alreadyOnshelve && String.Equals(alreadyShelvedBook.Title, book.Title, StringComparison.InvariantCultureIgnoreCase);
                        alreadyOnshelve = alreadyOnshelve && (alreadyShelvedBook.Price == book.Price);
                        alreadyOnshelve = alreadyOnshelve && (alreadyShelvedBook.DateOfIssue == book.DateOfIssue);
                        alreadyOnshelve = alreadyOnshelve && (alreadyShelvedBook.Genere == book.Genere);
                        if (alreadyOnshelve)
                        {
                            break;
                        }
                    }

                    if (!alreadyOnshelve)
                    {
                        BooksOnShelfe.Add(book);
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("{0} - {1}, kiadás éve: {2}, ár: {3}, műfaj: {4} már polcon van!", new Object[] { book.Author, book.Title, book.DateOfIssue, book.Genere.ToString() }));
                    }
                }
                else
                {
                    throw new ArgumentException("A megadott könyv 'üres'");
                }
            }          

            public IList<Book> FindBooksFromAuthor (string author)
            {
                IList<Book> booksFromAuthor = new List<Book>();
                foreach (Book alreadyShelvedBook in BooksOnShelfe)
                {
                    if (String.Equals(alreadyShelvedBook.Author, author, StringComparison.InvariantCultureIgnoreCase))
                    {
                        booksFromAuthor.Add(alreadyShelvedBook);
                    }
                }

                return booksFromAuthor;
            }
        }

        // Főprogram
        static void Main(string[] args)
        {
            if (args == null || args.Length != 1)
            {
                DisplayUsage();
            }
            else
            {
                if (args[0].ToLowerInvariant() == "/h" || args[0].ToLowerInvariant() == "/?")
                {
                    DisplayUsage();
                }
                else
                {
                    try
                    {
                        // File bolvasása
                        IList<string> lines = ReadFile(args[0]);
                        // Adatok validálása, könyv létrehozás és polcra tétele
                        ValidateBooksAndFillShelf(lines);
                        // Az összár lekérdezése és kiíratása
                        Console.Write("A polcon lévő könyvek összértéke: {0}\n", Shelf.Instance.SumPrice);
                        Console.WriteLine("\n\nA folytatáshoz nyomjon meg a szóköz billentyűt!\n");
                        Console.ReadKey(true);
                        // Szorgalmi feladat megvalósítása
                        HandleAuthorsQuery();
                    }
                    catch (Exception e)
                    {
                        Console.Clear();
                        Console.WriteLine("Hiba történt: {0}", e.Message);
                    }
                }
            }
        }

        private static void HandleAuthorsQuery()
        {
            IList<String> authorList = Shelf.Instance.ListOfAuthors;
            while (true)
            {
                DisplayListOfAuthors(authorList);
                Console.WriteLine("\nVálasszon egy írót és adja meg a hozzá tartotzó szorszámot, majd nyomjon enter-t!");
                string numString = Console.ReadLine();
                int numInt;
                // Ha nem sikerül intesíteni a stringet, akkor kiírja a hibát, egyébként kilistázza a választott író könyveit.
                if (!int.TryParse(numString, out numInt))
                {
                    Console.WriteLine("Hibás számot adott meg!");
                }
                else
                {
                    if (numInt >= authorList.Count)
                    {
                        Console.WriteLine("Hibás számot adott meg!");
                    }
                    else
                    {
                        IList<Book> bookList = Shelf.Instance.FindBooksFromAuthor(authorList[numInt]);
                        foreach (Book book in bookList)
                        {
                            Console.WriteLine(book.Title);
                        }
                    }
                }

                Console.WriteLine("Nyomjon meg egy billentyűt! 'v'-re a program befejeződik");
                ConsoleKeyInfo cki = Console.ReadKey(true);
                if (cki.KeyChar == 'v' || cki.KeyChar == 'V')
                {
                    break;
                }
            }
        }

        private static void DisplayListOfAuthors(IList<String> authorList)
        {
            // A szerzők számozásához
            int i = 0;
            foreach (string author in authorList)
            {
                string textToWrite = string.Concat(i, " - ", author);
                Console.WriteLine(textToWrite);
                i++;
            }
        }

        private static void ValidateBooksAndFillShelf(IList<string> lines)
        {
            string[] pieces;
            foreach (string line in lines)
            {
                // Minden sor egy könyvet képvisel                
                // Az adatok sorrendje a következő: 
                // Author;Title;DateOfIssue;Price;Genere
                // Az adatok a fájlban a fenti delimiter konstansban megadott karakterrel vannak elválasztva, 
                // vagyis a Split metódust ezzel hívom fel.
                // Azt is meg lehet tenni, hogy valami komplex elválasztót használjunk (hiszen a ; szerepelhet
                // akár a könyv címében is). Ha nem csak egy karaktert akarunk használni, akkor a Split metódusban
                // "" jelek közé tesszük az elválasztót, hiszen már nem egy karakterről, hanem egy stringről van szó
                // pl.: line.Split("###");
                // Ilyenkor a fenti delimiter konstans típusát is meg kell változtatni char-ról stringre, pl.:
                // const string delimiter = "###";
                pieces = line.Split(delimiter);

                // Létrehozom a könyvet, validálom az adatokat és a valid adatokat betöltöm a könyvbe
                Book myBook = ValidatePiecesAndCreateBook(pieces);

                // Felteszem a könyvet a polcra
                Shelf.Instance.AddBook(myBook);
            }
        }

        private static Book ValidatePiecesAndCreateBook(string[] pieces)
        {
            Book myBook = new Book();
            // Az adatok érvényesítése
            // Szűrjük ki az üres sorokat
            if (pieces==null)
            {
                throw new ArgumentNullException ("pieces","Valószínűleg van egy üres sor a bemeneti fájlban.");
            }

            // Minden könyvhöz 5 db tulajdonság tartozik ebben a sorrendben: Author;Title;DateOfIssue;Price;Genere
            if (pieces.Length != 5)
            {
                throw new ArgumentException(string.Format("A könyvnek túl kevés a tulajdonsága: {0}", GetCurrentLineTextBack(pieces)));
            }

            // Most jön az adatok típusokba töltése és ellenőrzése
            // Author;Title;DateOfIssue;Price;Genere
            // Az első kettő egyszerű
            string author = pieces[0];

            if (string.IsNullOrEmpty(author) || author.Length < 3 || author.Length > 50)
            {
                throw new ArgumentException(string.Format("{0} nem valós szerző ebben a sorban: {1}. Min. 3 betű, max. 50 betű.", author, GetCurrentLineTextBack(pieces)));
            }

            string title = pieces[1];

            if (string.IsNullOrEmpty(title) || title.Length < 3 || title.Length > 50)
            {
                throw new ArgumentException(string.Format("{0} nem valós cím ebben a sorban: {1}. Min. 3 betű, max. 50 betű.", title, GetCurrentLineTextBack(pieces)));
            }
            
            int dateOfIssue;
            bool success = int.TryParse(pieces[2], out dateOfIssue);
            if (!success || dateOfIssue < 1910 || dateOfIssue > DateTime.Now.Year)
            {
                throw new ArgumentException(string.Format("{0} nem valós kiadási év ebben a sorban: {1}. Min. 1910, max. az aktuális év.", pieces[2], GetCurrentLineTextBack(pieces)));
            }

            int price;
            success = int.TryParse(pieces[3], out price);
            if (!success || price%5 != 0 || price>15000)
            {
                throw new ArgumentException(string.Format("{0} nem valós ár ebben a sorban: {1}. Max. 15e Ft, 5-tel osztható.", pieces[3], GetCurrentLineTextBack(pieces)));
            }
            Genere genere;

            // Az enumhoz a 3-as .netben nincs tryparse
            try
            {
                genere = (Genere)Enum.Parse(typeof(Genere), pieces[4], true);
            }
            catch
            {                 
                throw new ArgumentException(string.Format("{0} nem valós műfaj ebben a sorban: {1}", pieces[4], GetCurrentLineTextBack(pieces)));
            }

            // Létrehozom a könyvet az adatokból

            myBook.Author = author;
            myBook.Title = title;
            myBook.DateOfIssue = dateOfIssue;
            myBook.Price = price;
            myBook.Genere = genere;
            return myBook;
        }

        private static string GetCurrentLineTextBack(string[] pieces)
        {
            string retVal;
            // Mivel várhatóan folyamatosan bővítjük ezt a szöveget a foreach-ben, így a végrehajtási idő javítása érdekében
            // jobb StringBuilder-t használni
            // forrás: http://stackoverflow.com/questions/73883/string-vs-stringbuilder
            StringBuilder failedLineContent = new StringBuilder();
            foreach (string piece in pieces)
            {
                failedLineContent.Append(piece);
                failedLineContent.Append(delimiter);
            }
            
            retVal = failedLineContent.ToString();
            retVal.TrimEnd(delimiter);
            return retVal;
        }

        // Ez a metódus beolvassa a paraméterként kapott útvonalról a delimiter konstanssal elválasztott tulajdonságokkal bíró könyveket
        // Minden sor a fájlban új könyvet reprezentál.
        // A beolvasott sorokat stringként adja vissza egy listában
        // forrás: https://msdn.microsoft.com/en-us/library/aa287535%28v=vs.71%29.aspx
        private static IList<string> ReadFile(string path)
        {
            string line;
            IList<string> lines= new List<string>();

            if (System.IO.File.Exists(path))
            {
                // fájl beolvasása
                System.IO.StreamReader file = new System.IO.StreamReader(path);
                while ((line = file.ReadLine()) != null)
                {
                    lines.Add(line);
                }

                file.Close();
            }
            else
            {
                throw new ArgumentException(string.Format("A fájl: {0} nem található", path));
            }

            return lines;
        }

        private static void DisplayUsage()
        {
            Console.Write("Használat: Paraméterként adja meg a könyveket tartalmazó fájlt!\nA fájlban egy sorba egy könyvet vegyen fel.\nEgy könyvhöz az alábbi adatokat adja meg: író{0}cím{0}kiadási év{0}Price{0}Genere\n", delimiter);
            Console.Write("Az alábbi megszorítások érvényesek:\n\tíró: min. 3 betű, max. 50 betű\n\tcím: min. 3 betű, max. 50 betű\n\tkiadási év, egész szám, min. 1910, max. az aktuális év\n\tár, egész szám, max. 15e Ft, 5-tel osztható\n\t");
            Console.Write("Műfaj: mese, regény, történelem, szakkönyv, külföldi");
        }
    }
}
