using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

/// <summary>
/// Ezek kellenek az ef classhoz
/// </summary>

using ExcelApplication = Microsoft.Office.Interop.Excel.Application;
using ExcelWorkbook = Microsoft.Office.Interop.Excel.Workbook;
using ExcelWorksheet = Microsoft.Office.Interop.Excel.Worksheet;
using ExcelRange = Microsoft.Office.Interop.Excel.Range;

///<summary>
///Ezek kellenek az olf classhoz
///</summary>

using System.Reflection;
using System.Collections;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Jelenléti.net
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()

        {
            try
            {
                Application.EnableVisualStyles();
                Application.Run(new Starter());
            }
            catch (Exception exp)
            {
                MessageBox.Show("Kivétel történt: " + exp.Message + ".\nKivétel ezen a ponton rendszerhibára utal.\nKivétel helye: " + exp.StackTrace.ToString(), "Jelenléti", MessageBoxButtons.OK, MessageBoxIcon.Error);
                k.Logging("Kivétel történt: " + exp.Message + ".\nKivétel ezen a ponton rendszerhibára utal.\nKivétel helye: " + exp.StackTrace.ToString(), "sys");
            }
        }
        

    }
				
    public class olf    //outlook funkciók
    {
        public static Outlook.Items Itemek(Outlook.MAPIFolder oInbox)
        {
            try
            {
                Outlook.Items oItems = oInbox.Items;
                return oItems;
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + "\nHely: Elsolevel()"));
            }

        }
        public static Outlook.MailItem Elsolevel(Outlook.Items oItems)
        {
            try
            {                
                Outlook.MailItem oMsg = (Outlook.MailItem)oItems.GetFirst();
                return oMsg;
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + "\nHely: Elsolevel()"));
            }

        }
        public static Outlook.MailItem Kovetkezolevel(Outlook.Items oItems)
        {
            try
            {
                Outlook.MailItem oMsg = (Outlook.MailItem)oItems.GetNext();
                return oMsg;
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + "\nHely: Elsolevel()"));
            }

        }
        public static Outlook.MAPIFolder Inbox()
        {
            try
            {
                Outlook.Application oApp = new Microsoft.Office.Interop.Outlook.Application();
                Outlook.NameSpace oNS = oApp.GetNamespace("mapi");
                oNS.Logon("Outlook", Missing.Value, Missing.Value, Missing.Value);
                Outlook.MAPIFolder oInbox = oNS.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderInbox);
                oApp = null;
                oNS = null;
                return oInbox;                
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + "\nHely: Elsolevel()"));
            }

        }
    }

    public class ff     //file funciók
    {

        public static void AdottSorIras(string szoveg, string utvonal, int sor, bool beszuras)        //Az adott sorba ír a fájlba.        
            //nagyon gagyi megoldás, de egyszerű :-)
        {
            string olvasott, tempfile = @"C:\temp.tmp";
            int i, hossz;

            try
            {
                if (k.Letezik(tempfile, "F")) File.Delete(tempfile);
                if (k.Letezik(utvonal, "F"))
                {
                    hossz = Hanysor(utvonal);
                    if (sor > hossz)
                    {
                        olvasott = File.ReadAllText(utvonal);
                        SorIras(olvasott, tempfile, "A");
                        for (i = hossz + 1; i < sor; i++) SorIras("", tempfile, "A");
                        SorIras(szoveg, tempfile, "A");
                    }
                    else
                    {
                        for (i = 1; i < sor && i <= hossz; i++)
                        {
                            olvasott = SorOlvas(utvonal, i);
                            SorIras(olvasott, tempfile, "A");
                        }
                        SorIras(szoveg, tempfile, "A");
                        if (!beszuras) i++;
                        for (; i <= hossz; i++)
                        {
                            olvasott = SorOlvas(utvonal, i);
                            SorIras(olvasott, tempfile, "A");

                        }
                    }

                    if (k.Letezik(utvonal + ".bak", "F")) File.Delete(utvonal + ".bak");
                    File.Move(utvonal, utvonal + ".bak");
                    File.Move(tempfile, utvonal);
                }
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + "\nHely: AdottSorIras()"));
            }

        }
        public static void SorIras(string szoveg, string utvonal, string mod)        //egy sort ír a fájlba. Hozzáfűz, felülír módok

        {
            switch (mod)
            {
                case "O":       //nincs break
                case "o":       //nincs break
                case "F":       //nincs break
                case "felulir":       //nincs break
                case "f":       //nincs break
                case "Felulir":
                    try
                    {
                        using (StreamWriter sw = File.CreateText(@utvonal))
                        {
                            sw.WriteLine(szoveg);

                        }
                    }
                    catch (Exception exp)
                    {
                        throw (new Exception(exp.Message + "\nHely: SorIras()-Felulir mód"));
                    }
                    break;
                case "A":       //nincs break
                case "a":       //nincs break
                case "h":       //nincs break
                case "H":       //nincs break
                case "Append":       //nincs break
                case "append":       //nincs break
                    try
                    {
                        using (StreamWriter sw = File.AppendText(@utvonal))
                        {
                            sw.WriteLine(szoveg);

                        }
                    }
                    catch (Exception exp)
                    {
                        throw (new Exception(exp.Message + "\nHely: SorIras()-Hozzafuz mód"));
                    }
                    break;
                default:
                    k.Uzi("Hibás paraméter fájl megnyitásakor: " + mod + " nem értelmezett!\nHely: SorIras()", "oo", "!");
                    break;
                    
            }
        }

        
        public static string SorOlvas(string utvonal, int sor)      //beolvassa és visszaadja a sor-ban megadott
                                                                    //sor-ban található stringet. Ha fájlon kívülre kerül
                                                                    //kivételt okoz!
        {
            int i;
            string line;
            if (sor<0) sor=1;
            if (k.Letezik(utvonal, "fajl"))
            {
                FileStream f = new FileStream(utvonal, FileMode.Open);
                try
                {
                    StreamReader t = new StreamReader(f);
                    for (i = 1; i < sor && t.ReadLine() != null; i++) ;
                    if ((line = t.ReadLine()) != null)
                    {
                        f.Close();
                        return line;
                    }
                    else
                    {
                        f.Close();
                        throw (new Exception("Olvasási kísérlet a fájl vége utáni tartományból! Hely: SorOlvas()"));
                    }
                    
                }
                catch(Exception exp)
                {
                    throw (new Exception (exp.Message +"\nHely: SorOlvas()"));
                }
                finally
                {
                    f.Close();
                }
                
            }
            else
            {
                throw (new FileNotFoundException(utvonal + ": A fájl nem található"));
            }

        }
        public static int Hanysor(string utvonal)
        {
            int i=0;
            
            if (k.Letezik(utvonal, "fajl"))
            {
                FileStream f = new FileStream(utvonal, FileMode.Open);
                try
                {
                    StreamReader t = new StreamReader(f);
                    for (i = 0; t.ReadLine() != null; i++) ;
                    f.Close();
                    t = null;
                    return i;

                }
                catch (Exception exp)
                {
                    throw (new Exception(exp.Message + "\nHely: SorOlvas()"));
                }
                finally
                {
                    f.Close();
                }

            }
            else
            {
               
                throw (new FileNotFoundException(utvonal + ": A fájl nem található"));
                
            }

        }
    }
    public class ef     //eben lesznek az excel függvények :-)
    {
        
        public static ExcelApplication Exapp()      //létrehoz egy excel alkalmzást
        {
            try
            {
                ExcelApplication excel = new ExcelApplication();
                return excel;                
            }
            catch (Exception exc)
            {
                throw (new Exception(exc.Message + "\nKivétel új Excel alkalmazás indításakor", exc));
            }
        }
        public static ExcelWorkbook Exwb(ExcelApplication excel)        //létrehoz egy excel munkafüzetet
        {
            try
            {
                ExcelWorkbook Workbook = excel.Workbooks.Add(Missing.Value);
                return Workbook;
            }
            catch (Exception exc)
            {
                throw (new Exception(exc.Message + "\nKivétel új munkafüzet létrehozásakor", exc));
            }

        }
        public static void Sel(ExcelApplication excel,string tol, string ig) //ez kijelöl egy cellát vagy tartományt
        {
            try
            {
                if (ig != " ") ((ExcelWorksheet)excel.ActiveSheet).get_Range(tol, ig).Select();
                else ((ExcelWorksheet)excel.ActiveSheet).get_Range(tol, Missing.Value).Select();
            }
            catch (Exception exc)
            {
                throw (new Exception(exc.Message + "\nKivétel tartomány kijelölésekor", exc));
            }

        }

        public static void Sel(ExcelWorkbook eWb, string tol, string ig) //ez kijelöl egy cellát vagy tartományt
        {
            try
            {
                if (ig != " ") ((ExcelWorksheet)eWb.ActiveSheet).get_Range(tol, ig).Select();
                else ((ExcelWorksheet)eWb.ActiveSheet).get_Range(tol, Missing.Value).Select();
            }
            catch (Exception exc)
            {
                throw (new Exception(exc.Message + "\nKivétel tartomány kijelölésekor", exc));
            }

        }

        public static ExcelRange Range(ExcelApplication excel, string tol, string ig)    //egy tartomány objektumot ad vissza
        {
            try
            {
                if (ig != " ") return ((ExcelWorksheet)excel.ActiveSheet).get_Range(tol, ig);
                else return ((ExcelWorksheet)excel.ActiveSheet).get_Range(tol, Missing.Value);
            }
            catch (Exception exc)
            {
                throw (new Exception(exc.Message + "\nKivétel tartomány létrehozásakor", exc));
            }
            
        }
        public static ExcelRange GetColRange(ExcelWorkbook eWb,int column)
        {
            try
            {
                return ((ExcelRange)((ExcelWorksheet)eWb.ActiveSheet).Cells[1,column]).EntireColumn;
            }
            catch (Exception exc)
            {
                throw (new Exception(exc.Message + "\nHely: GetColRange()", exc));
            }
        }
        public static string cella(ExcelWorkbook eWb, int sor, int oszlop)
        {
            try
            {
                ExcelWorksheet eWs;
                string returnvalue = " ";
                object obj;
                eWs = ((ExcelWorksheet)eWb.ActiveSheet);
                obj = ((ExcelRange)eWs.Cells[sor, oszlop]).Text;//.ToString();
                returnvalue = obj.ToString();
                eWs = null;
                obj = null;
                return returnvalue;
            }
            catch (Exception exc)
            {
                throw (new Exception(exc.Message + "\nHely: cella() ewb olvas", exc));
            }
        }
        public static string cella(ExcelWorksheet eWs, int sor, int oszlop)
        {
            try
            {               
                string returnvalue = " ";
                object obj;                
                obj = ((ExcelRange)eWs.Cells[sor, oszlop]).Text;//.ToString();
                returnvalue = obj.ToString();
                obj = null;
                return returnvalue;
            }
            catch (Exception exc)
            {
                throw (new Exception(exc.Message + "\nHely: cella() ews olvas", exc));
            }
        }
        public static void cella(ExcelWorkbook eWb, int sor, int oszlop, string szoveg)
        {
            try
            {
                ExcelWorksheet eWs;
                eWs = ((ExcelWorksheet)eWb.ActiveSheet);
                ((ExcelRange)eWs.Cells[sor, oszlop]).Value2 = szoveg;
                eWs = null;
            }
            catch (Exception exc)
            {
                throw (new Exception(exc.Message + "\nHely: void cella() ewb ír", exc));
            }
        }

        public static void cella(ExcelWorksheet eWs, int sor, int oszlop, string szoveg)
        {
            try
            {                
                ((ExcelRange)eWs.Cells[sor, oszlop]).Value2 = szoveg;               
            }
            catch (Exception exc)
            {
                throw (new Exception(exc.Message + "\nHely: void cella() ews ír", exc));
            }
        }
        public static ExcelRange cellarange(ExcelWorksheet eWs, int sor, int oszlop)
        {
            try
            {
                return ((ExcelRange)eWs.Cells[sor, oszlop]);
            }
            catch (Exception exc)
            {
                throw (new Exception(exc.Message + "\nHely: void cella() ews ír", exc));
            }
        }
        public static int listavege(ExcelWorkbook eWb, int oszlop)
        {
            try
            {
                int n = 0;
                do
                {
                    n++;
                }
                while (ef.cella(eWb, n, oszlop) != "");
                return n;
            }
            catch (Exception exc)
            {
                throw (new Exception(exc.Message + "\nHely: listavege()", exc));
            }
        }

        public static void Ment(ExcelApplication excel, string utvonal)     //elmenti a munkafüzetet az adott néven
        {
            try
            {
                ((ExcelWorksheet)excel.ActiveSheet).SaveAs(@utvonal, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value);
            }
            catch (Exception exc)
            {
                throw (new Exception(exc.Message + "\nKivétel Excel munkalap mentésekor", exc));
            }
        }
        public static ExcelWorkbook Megnyit(string utvonal, ExcelApplication exapp)
        {
            try
            {
                if (k.Letezik(utvonal,"F") )return exapp.Workbooks.Open(utvonal, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                else throw (new FileNotFoundException(utvonal));
            }
            catch (Exception exc)
            {
                throw (new Exception(exc.Message + "\nKivétel Excel munkalap megnyitásakor", exc));
            }
        }
        public static void Kilep(ExcelApplication eApp)
        {
            try
            {
                eApp.Quit();
            }
            catch (Exception exc)
            {
                throw (new Exception(exc.Message + "\nKivétel Excel alkalmazás bezárásakor", exc));
            }
        }


    }
    public class k
    {

        public static string mail2nev, logpath, tema, mentpath, feldpath, unnepath, utbej, szabipath, munkapath;
        public static bool jelexc;
        public static int tavszaboszl = -1, szabjan=-1;
        public static string[] Napok={"i","i","i","i","i","i","i","i","i","i","i","i","i","i","i","i","i","i","i","i","i","i","i","i","i","i","i","i","i","i","i","i"};
        /// <summary>        
        /// mail2nev: hivatalosan egy mail2nev.xls-re mutat. Ez a nevek adatbázisa.
        /// logpath: \-re végződő útvonal ahová a logfileokat kell menteni
        /// tema: megmutatja milyen témájú levelekkel foglalkozzon a program
        /// mentpath: ez az útvonala a jelenléti lapnak
        /// feldpath: ez az útvonala a feldolg.txt-nek
        /// unnepath: a hónapban található nem hétvége eső ünnepeket tartalmazza
        /// utbej: az utolsó bejelentkezés dátuma
        /// szabipath: szabikat mutató excelsheet
        /// munkapath: a hónapban a hétvégére eső munkanapokat tartalmazó excelsheet
        /// jelexc: a jelenléti egy excelsheetben jön csatolva
        /// tavszaboszl: ha meg van nyitva a szabipathos szabifájl és megvan a tavalyi szabi oszlopa, akkor ez tárolja azt
        /// szabjan: ha meg van nyitva a szabipathos szabifájl és megvan a január oszlopa ez azt tárolja
        /// </summary>
        
        public const int paramereterszama = 10;
        public const int ExcNapXElt=1;
        public const int ExcElsoSorYElt=0;
        public const int MaxExcelSor = 500;
        /// <summary>
        /// parameterekszama: mennyi érték van a settings.set-e írva
        /// ExcNapXElt: mennyi sor marad ki a jelenlétiben elseje előtt (pl.: a nevek miatt értéke minimum 1)
        /// ExcElsoSorYElt: mint fent, de a teljes első sor helyzetére hatással van.
        /// MaxExcelSor: mivel nem elég egy cellát ""-re vizsgálni ez határozza meg, hogy hanyadik sorig olvassa
        ///              a táblát, ha nem találj a keresett értéket.
        /// </summary>

        public struct ExcelMegnyitva
        {
            public bool m;
            public ExcelApplication ea;
            public ExcelWorkbook ewb;
            public ExcelWorksheet ews;
            public ExcelMegnyitva(bool reset)
            {
                m = false;
                ea = null;
                ewb = null;
                ews = null;
            }
        }
        public static ExcelMegnyitva EMm2n = new ExcelMegnyitva(true), EMunn = new ExcelMegnyitva(true), EMszabi = new ExcelMegnyitva(true), EMmunka = new ExcelMegnyitva(true), EMjelen = new ExcelMegnyitva(true);
        public struct Felado
        {
            public string nev;
            public string emailnev;
            public string tipus; //Auto. dolg; Edison; Szerz; Fizikai;
            public bool tulora;
            public Felado(bool reset)
            {
                nev = "";
                emailnev = "";
                tipus = "";
                tulora = false;
            }
        }
        public struct ExcelStruct
        {
            public ExcelApplication ExApp;
            public ExcelWorkbook ExWb;
            public ExcelWorksheet ExWs;
            public int num1;       //Ezek a mezok adatok viasszaadasara vannak, kitoltesuk opcionalis
            public int num2;
            public int num3;
            public int num4;
            public string szov1;
            public string szov2;
            public ExcelStruct(bool reset)
            {
                ExApp = null;
                ExWb = null;
                ExWs = null;
                num1 = -1;
                num2 = -1;
                num3 = -1;
                num4 = -1;
                szov1 = "";
                szov2 = "";
            }
        }
  
        public static string[] Tulorasok()
        {
            try
            {
                string c;
                string[] nevlista = new string[k.MaxExcelSor];
                int j=0;
                if (!k.EMm2n.m)
                {
                    k.EMm2n.ea = new ExcelApplication();
                    k.EMm2n.ewb = ef.Megnyit(k.mail2nev, k.EMm2n.ea);
                    k.EMm2n.m = true;
                }
                c=ef.cella(k.EMm2n.ewb,1,4);
                //k.EMm2n.ea.Visible = true;
                for (int i = 1; c != ""; i++)
                {
                    c=ef.cella(k.EMm2n.ewb,i,4);
                    if (c == "Igen" || c == "igen")
                    {
                        nevlista[j] = ef.cella(k.EMm2n.ewb, i, 2);
                        j++;
                    }
                }
                return nevlista;
            }
            catch (Exception exp)
            {                
                throw (new Exception(exp.Message + " \nHely:Tulorasok()"));
            }
        }
        public class Feldolgozottak
        {
            public class Nevek
            {
                public static void HozzaAd(string nev)
                {
                    try
                    {
                        if (!k.Feldolgozottak.Nevek.Letezik(nev))
                        {
                            if (k.Letezik(k.feldpath + "feldolg.txt", "F"))
                            {
                                ff.SorIras(nev, k.feldpath + "feldolg.txt", "A");
                            }
                            else
                            {
                                ff.SorIras(nev, k.feldpath + "feldolg.txt", "O");
                            }
                        }
                        else
                        {
                            throw (new Exception("A feldolgozottak fájlhoz nem lehetett hozáadni a '" + nev + "' nevet, mert már szerepel!"));
                        }
                    }
                    catch (Exception exp)
                    {
                        k.Felado locfelado = new Felado(true);
                        throw (new Exception(exp.Message + " \nHely:Feldolgozottak.Nevek.HozzaAd()"));
                    }
                }
                public static bool Letezik(string nev)
                {
                    try
                    {
                        if (k.Letezik(k.feldpath + "feldolg.txt", "F"))
                        {
                            int sorok = ff.Hanysor(k.feldpath + "feldolg.txt");
                            for (int i = 1; i <= sorok; i++)
                            {
                                string OlvNev = ff.SorOlvas(k.feldpath + "feldolg.txt", i);
                                if (OlvNev == nev) return true;
                            }
                        }
                        return false;
                    }
                    catch (Exception exp)
                    {
                        k.Felado locfelado = new Felado(true);
                        throw (new Exception(exp.Message + " \nHely:Feldolgozottak.Nevek.Letezik()"));
                    }
                }
                public static string[] Lista()
                {
                    try
                    {
                        if (k.Letezik(k.feldpath + "feldolg.txt", "F"))
                        {
                            int sorok = ff.Hanysor(k.feldpath + "feldolg.txt");
                            string[] nevek = new string[sorok];
                            for (int i = 0; i < sorok; i++)
                            {
                                nevek[i] = ff.SorOlvas(k.feldpath + "feldolg.txt", i + 1);
                            }

                            return nevek;
                        }
                        else
                        {
                            string[] nevek = new string[0];
                            return nevek;
                        }
                    }
                    catch (Exception exp)
                    {
                        k.Felado locfelado = new Felado(true);
                        throw (new Exception(exp.Message + " \nHely:Feldolgozottak.Nevek.Lista()"));
                    }
                }
            }
            public class fajl
            {
                public static void Torles()
                {
                    try
                    {
                        if (k.Letezik(k.feldpath + "feldolg.txt", "F"))
                        {
                            if (k.Letezik(k.feldpath + "feldolg.bak", "F")) System.IO.File.Delete(k.feldpath + "feldolg.bak");  //ha létezik előző feldolg.txt tartalékmentés letörli
                            System.IO.File.Copy(k.feldpath + "feldolg.txt", k.feldpath + "feldolg.bak");
                            System.IO.File.Delete(k.feldpath + "feldolg.txt");
                        }
                    }
                    catch (Exception exp)
                    {
                        k.Felado locfelado = new Felado(true);
                        throw (new Exception(exp.Message + " \nHely:Feldolgozottak.fajl.Torles()"));
                    }

                }
                public static void Mentes(string[] nevek)
                {
                    try
                    {
                        if (k.Letezik(k.feldpath + "feldolg.txt", "F"))
                        {
                            if (k.Letezik(k.feldpath + "feldolg.bak", "F")) System.IO.File.Delete(k.feldpath + "feldolg.bak");  //ha létezik előző feldolg.txt tartalékmentés letörli
                            System.IO.File.Copy(k.feldpath + "feldolg.txt", k.feldpath + "feldolg.bak");                            
                        }
                        for (int i = 0; i < nevek.Length; i++)
                        {
                            if (i == 0) ff.SorIras(nevek[i], k.feldpath + "feldolg.txt", "O");
                            ff.SorIras(nevek[i], k.feldpath + "feldolg.txt", "A");
                        }

                    }
                    catch (Exception exp)
                    {
                        k.Felado locfelado = new Felado(true);
                        throw (new Exception(exp.Message + " \nHely:Feldolgozottak.fajl.Torles()"));
                    }
                }
            }
        }
        public static bool Feldolgozas(string felado, string path, Form form)
        {
            int deb = 0;
            try
            {
                deb++; //1
                form.Text = felado + " jelentésének feldolgozása";
                if (k.Feldolgozottak.Nevek.Letezik(felado))
                {
                    form.Text = "Jelenléti";
                    return false;
                }

                deb++; //2
                k.Felado locfelado;                         //ebben vannak a mail2neves paraméterek
                ExcelStruct eStr = new ExcelStruct(true);   //ebben vannak a Jelenlétis paraméterek, a num1 mutatja a sort, ahol a nevet megtalalta
                ExcelStruct eStr2 = new ExcelStruct(true);  //ebben vannak a szabifájlos paraméterek, a num1 mutatja a sort, ahol a nevet megtalalta,
                //num2 mutatja az oszlopot, ahol a tavalyi szabik száma van és num3 a január oszlopát.                
                string[] napok;
                locfelado = k.GetUserParams(felado);
                deb++; //3
                if (locfelado.nev != "")
                {
                    eStr = GetJelenletiExcel(locfelado);
                    if (eStr.ExApp == null)
                    {
                        k.Uzi("A feladó: " + felado + " jelentése nem került feldolgozásra, mert a jelenléti sheet inicializásása nem sikerült!", "oo", "*");
                        form.Text = "Jelenléti";
                        return false;
                    }

                }
                else
                {
                    k.Uzi("A feladó: " + felado + " jelentése nem került feldolgozásra, mert nem sikerült "+felado+" paramétereit betölteni!", "oo", "*");
                    form.Text = "Jelenléti";
                    return false;
                }
                if (k.EMjelen.m) k.EMjelen.ewb.Save();
                deb++; //4
                eStr2 = GetSzabiExcel(locfelado);                //ebben vannak a szabis paraméterek
                deb++; //5
                if ((eStr2.num1 == -1) || (eStr2.num2 == -1) || (eStr2.num3 == -1))
                {
                    k.Uzi("A feladó: " + felado + " jelentése nem került feldolgozásra, mert a szabadságos sheeten nem található meg!", "oo", "*");
                    form.Text = "Jelenléti";
                    return false;
                }
                deb++; //6
                // k.Uzi("A feladó: " + felado + " adatai:\n" + "Excelsheet: " + path + "\nEmailnév: " + locfelado.emailnev + "\nJelenlétibe kerülő név: "+locfelado.nev+ "\nTípus: " + locfelado.tipus + "\nTúlóra: " + locfelado.tulora + "\nJelenlétiben a " + eStr.num1 + ". sorban található a " + eStr.ExWs.Name + " lapon\nA szabis excelben a " + eStr2.num1 + "sorban taláható a " + eStr2.ExWs.Name + " lapon a " + eStr2.num2 + " oszlopban.\nÉrtéke: " + ef.cella(eStr2.ExWs, eStr2.num1, eStr2.num2) + "\nJanuár oszlopa: " + eStr2.num3, "oo", "i");
                ///
                ///Most megvannak a fenti adatok, a Temp.xls-ből ki kell szedni, hogy mikor volt szabin és be kell rakni a jelenléti megfelelő sorába               
                ///
                if (k.Napok[1] == "i") NapokGenerator(); //ha nem kellett létrehozni a shetet ez tölti ki a napok tömbjét
                deb++; //7
                napok = CreateArray(path);    //létrehoz egy tömböt, amit gyakorlatilag be lehetne tenni a jelenlétibe.
                deb++; //8
                if (napok[0] == "i")
                {
                    k.Uzi("A feladó: " + felado + " jelentése nem került feldolgozásra, mert nem sikerült kitölteni a napok mátrixot (napok[0]='i')!", "oo", "*");
                    form.Text = "Jelenléti";
                    return false;
                }
                deb++; //9
                JelenletiKitolto(eStr, eStr2, locfelado, napok);  // ez összedolgozza tavalyi szabival az idei hiányzásokat.
                deb++; //10
                eStr.ExWb.Save();
                deb++; //11
                k.Feldolgozottak.Nevek.HozzaAd(felado);
                deb++; //12
                eStr.ExWs = null;
                eStr.ExWb = null;
                eStr.ExApp = null;
                eStr2.ExWs = null;
                eStr2.ExWb = null;
                eStr2.ExApp = null;
                form.Text = "Jelenléti";
                return true;

            }
            catch (Exception exp)
            {
                k.Felado locfelado = new Felado(true);
                throw (new Exception(exp.Message + " \nHely:Feldolgozas() deb: " + deb));
            }
            
        }
        public static void JelenletiKitolto(ExcelStruct eStr, ExcelStruct eStr2, Felado felado, string[] napok)
        {
            ///eStr:    //ebben vannak a Jelenlétis paraméterek, a num1 mutatja a sort, ahol a nevet megtalalta
            ///eStr2:   //ebben vannak a szabifájlos paraméterek, a num1 mutatja a sort, ahol a nevet megtalalta,
            ///         //num2 mutatja az oszlopot, ahol a tavalyi szabik száma van és num3 a január oszlopát.                
            ///felado:  //ebben vannak a mail2neves paraméterek
            ///napok:   //ebben a tömbben van, hogy az adott user az adott napon mit jelentett le tárolva
            ///         //és összedolgozva a globális "kalendárral", vagyis csak azok a napok D, amikor nincs 
            ///         //hétvége, azok a napok Ü, amikor ünnapnap van (függetlenül attól, hogy esetleg akkor szabin
            ///         //volt az illető, hasonlóan Sz, Apsz, stb...
            try
            {
                //eStr2.ExApp.Visible = true;
                //eStr.ExApp.Visible = true;
                int napokszama = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                string logstring = DateTime.Now.ToString("yyyy.MM.dd") + "Felado: " + felado.nev + ", jelenletibe írva:\n\t";
                string logstrori = logstring;
                string megjegyzes = "";
                string megjori = megjegyzes;
                int dolg = 0, fizsz = 0, gysz = 0, fizu = 0, eufiz = 0, apsz = 0, tfizo = 0;
                int tavalyiszabi = (Convert.ToInt32(ef.cella(eStr2.ExWs, eStr2.num1, eStr2.num2)));
                int szabadsagok = 0;
                int i;
                for (i = 1; i <= napokszama; i++)
                {
                    switch (napok[i])
                    {
                        case "D":
                            ef.cella(eStr.ExWs, eStr.num1, i + k.ExcNapXElt, "D");
                            dolg++;
                            break;
                        case "B":
                            ef.cella(eStr.ExWs, eStr.num1, i + k.ExcNapXElt, "B");
                            eufiz++;
                            logstring = logstring + i.ToString() + ". Betegség\n\t";
                            break;
                        case "Apsz":
                            ef.cella(eStr.ExWs, eStr.num1, i + k.ExcNapXElt, "Apsz");
                            apsz++;
                            logstring = logstring + i.ToString() + ". Apasági\n\t";
                            megjegyzes = megjegyzes + DateTime.Now.ToString("MM.") + i.ToString("##") + ". Apsz\n";
                            szabadsagok--;
                            break;
                        case "Gysz":
                            ef.cella(eStr.ExWs, eStr.num1, i + k.ExcNapXElt, "Gysz");
                            gysz++;
                            logstring = logstring + i.ToString() + ". Gysz\n\t";
                            megjegyzes = megjegyzes + DateTime.Now.ToString("MM.") + i.ToString("##") + ". Gysz\n";
                            szabadsagok--;
                            break;
                        case "Sz":
                            if (tavalyiszabi > 0)
                            {
                                megjegyzes = megjegyzes + DateTime.Now.ToString("MM.") + i.ToString() + ". tavalyi\n";
                                logstring = logstring + DateTime.Now.ToString("MM.") + i.ToString("##") + ". tavalyi\n\t";
                                tavalyiszabi--;
                                ef.cella(eStr.ExWs, eStr.num1, i + k.ExcNapXElt, "D");
                                dolg++;
                                szabadsagok--;
                            }
                            else
                            {
                                ef.cella(eStr.ExWs, eStr.num1, i + k.ExcNapXElt, "Sz");
                                fizsz++;
                                megjegyzes = megjegyzes + DateTime.Now.ToString("MM.") + i.ToString("##") + ". szabadság\n";
                                szabadsagok--;
                            }
                            break;
                        case "Ü":
                            ef.cella(eStr.ExWs, eStr.num1, i + k.ExcNapXElt, "Ü");
                            ef.cellarange(eStr.ExWs, eStr.num1, i + k.ExcNapXElt).Font.Bold = true;
                            ef.cellarange(eStr.ExWs, eStr.num1, i + k.ExcNapXElt).Font.ColorIndex = 3;
                            fizu++;
                            break;
                        case "":
                            ef.cella(eStr.ExWs, eStr.num1, i + k.ExcNapXElt, " ");
                            break;
                        default:
                            break;
                    }
                }
                ef.cella(eStr2.ExWs, eStr2.num1, eStr2.num3 + DateTime.Now.Month - 1, szabadsagok.ToString());
                if (megjegyzes != megjori) ef.cellarange(eStr2.ExWs, eStr2.num1, eStr2.num3 + DateTime.Now.Month - 1).AddComment(megjegyzes);
                if (logstring != logstrori) k.Logging(logstring, "app");
                tfizo = dolg + fizsz + fizu + eufiz;
                i++;
                ef.cella(eStr.ExWs, eStr.num1, i, (tfizo*8).ToString());
                ef.cella(eStr.ExWs, eStr.num1, i + 1, (dolg*8).ToString());
                ef.cella(eStr.ExWs, eStr.num1, i + 2, (fizsz*8).ToString());
                ef.cella(eStr.ExWs, eStr.num1, i + 3, (gysz*8).ToString());
                ef.cella(eStr.ExWs, eStr.num1, i + 4, (fizu*8).ToString());
                ef.cella(eStr.ExWs, eStr.num1, i + 5, (eufiz*8).ToString());
                ef.cella(eStr.ExWs, eStr.num1, i + 6, (apsz*8).ToString());
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:Jelenletikitolto()"));
            }
        }

        public static void NapokGenerator()
        {
            try
            {
                DateTime dt;
                string datum;
                int i = 1;
                bool kitoltendo;
                IFormatProvider culture = new System.Globalization.CultureInfo("hu-HU", true);
                int napokszama = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                kitoltendo = (k.Napok[1] == "i");
                for (i = 1; i <= napokszama; i++)
                {
                    if (kitoltendo) k.Napok[i] = "m";
                    datum = DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + "." + i.ToString();
                    dt = DateTime.Parse(datum, culture);
                    if ((dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday) && !Munkanap(i))
                    {
                        if (kitoltendo) k.Napok[i] = "sz";
                    }
                    if (Unnepnap(i))
                    {
                        if (kitoltendo) k.Napok[i] = "u";
                    }
                }
            }
            catch (Exception exp)
            {             
                throw (new Exception(exp.Message + " \nHely:NapokGenerator()"));
            }

        }

        public static string[] CreateArray(string utvonal)
        {
            ExcelApplication eApp;
            ExcelWorkbook eWb;
            try
            {
                int napokszama = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                string[] tomb = new string[33];
                for (int i = 0; i < 33; i++)
                {
                    tomb[i] = "i";
                }
                string tol, ig, ok;
                eApp = ef.Exapp();
                eWb = ef.Megnyit(utvonal, eApp);
                if (!(ef.cella(eWb, 1, 1) == "Dolgoztam végig"))
                {
                    for (int i = 1; (tol = ef.cella(eWb, i, 1)) != ""; i++)
                    {
                        ig = ef.cella(eWb, i, 2);
                        ok = ef.cella(eWb, i, 3);
                        for (int j = Convert.ToInt32(tol); j <= Convert.ToInt32(ig); j++)
                        {

                            switch (k.Napok[j])
                            {
                                case "i":
                                    k.Uzi("A napok tömbje nincs kitöltve!", "oo", "*");
                                    tomb[0] = "i";
                                    return tomb;
                                case "sz":
                                    tomb[j] = "";
                                    break;
                                case "u":
                                    tomb[j] = "Ü";
                                    break;
                                case "m":
                                    tomb[j] = ok;
                                    break;
                            }
                        }
                    }
                }
                
                for (int l = 1; l <= napokszama; l++)
                {
                    if (tomb[l] == "i")
                    {
                        switch (k.Napok[l])
                        {
                            case "i":
                                k.Uzi("A napok tömbje nincs kitöltve!", "oo", "*");
                                tomb[0] = "i";
                                return tomb;
                            case "sz":
                                tomb[l] = "";
                                break;
                            case "u":
                                tomb[l] = "Ü";
                                break;
                            case "m":
                                tomb[l] = "D";
                                break;
                        }
                    }
                }
                eWb.Close(false, null, null);
                eApp.Quit();
                tomb[0] = "D";
                return tomb;
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:GetSzabiExcel()"));
            }
            finally
            {                
                eWb = null;
                eApp = null;
            }
        }
        public static ExcelStruct GetSzabiExcel(Felado felado)
        {
            ExcelStruct eStr=new ExcelStruct(true);
            ExcelApplication eApp;
            ExcelWorkbook eWb;
            ExcelWorksheet eWs;
            int szabisor = -1, tavalyioszlop = -1, janoszlop = -1;            
            try
            {
                if (!k.EMszabi.m)
                {
                    k.EMszabi.ea = ef.Exapp();
                    k.EMszabi.ewb = ef.Megnyit(k.szabipath, k.EMszabi.ea);
                    k.EMszabi.m = true;
                }
                eApp = k.EMszabi.ea;
                eWb = k.EMszabi.ewb;
                eStr.ExApp=eApp;
                eStr.ExWb=eWb;
                eWs = GetSzabiSheet(eWb, "szabadsag");
                if (eWs==null)
                {
                    return eStr;
                }
                else
                {
                    eStr.ExWs=eWs;
                }
                szabisor = GetSzabiSor(eWb, eWs, felado);
                if (szabisor == -1)
                {
                    k.Uzi("A felado: " + felado.nev + " nem található a szabadságos listában!\n", "oo", "*");
                    return eStr;
                }
                if (k.tavszaboszl == -1)
                {
                    tavalyioszlop = TavSzOszlop(eWb, eWs);
                    if (tavalyioszlop != -1) k.tavszaboszl = tavalyioszlop;
                }
                else
                {
                    tavalyioszlop = k.tavszaboszl;
                }
                if (k.szabjan == -1)
                {
                    janoszlop = TJanOszlop(eWb, eWs);
                    if (janoszlop != -1) k.szabjan = janoszlop;
                }
                else
                {
                    janoszlop = k.szabjan;
                }
                eStr.num1 = szabisor;
                eStr.num2 = tavalyioszlop;
                eStr.num3 = janoszlop;                
                return eStr;

            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:GetSzabiExcel()"));
            }
            finally
            {
                eWs = null;                
                eWb=null;
                eApp = null;
            }            
        }

        public static int TJanOszlop(ExcelWorkbook eWb, ExcelWorksheet eWs)
        {
            try
            {
                int i = 0, j;
                string cellval;
                Felado felado = new Felado(true);
                eWs.Activate();
                felado.nev = "Név";
                j = GetSzabiSor(eWb, eWs, felado);

                do
                {
                    i++;
                    cellval = ef.cella(eWb, j, i);
                }
                while (cellval != "" && cellval != "1. hó");
                if (cellval == "1. hó") return i;
                else return -1;
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:TavSzOszlop()"));
            }
            
        }

        public static int TavSzOszlop(ExcelWorkbook eWb, ExcelWorksheet eWs)
        {
            try
            {
                int i=0,j;
                string cellval;
                Felado felado=new Felado(true);
                eWs.Activate();
                felado.nev = "Név";
                j= GetSzabiSor(eWb,eWs,felado);

                if (j < 0) return j;
                do
                {
                    i++;
                    cellval=ef.cella(eWb,j,i);
                }
                while (cellval!="" && cellval!="tavalyi");
                if (cellval == "tavalyi") return i;
                else return -1;
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:TavSzOszlop()"));
            }
        }

        public static int GetSzabiSor(ExcelWorkbook eWb,ExcelWorksheet eWs, Felado felado)
        {
            try
            {
                int i = 0;
                string cellval;
                eWs.Activate();
                //eWs.Application.Visible = true;
                do
                {
                    i++;
                    cellval = ef.cella(eWb, i, 1);
                }
                while (((cellval != "") || i<k.MaxExcelSor) && cellval != felado.nev);
                if (cellval == felado.nev) return i;
                else return -1;
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:GetSzabiSor()"));
            }
        }

        public static ExcelWorksheet GetSzabiSheet(ExcelWorkbook eWb, string lapnev)
        {
            int i = 0;
            string nev;
            //bool mehet;
            try
            {
                if (eWb != null)
                {
                    do
                    {
                        i++;
                        nev = ((ExcelWorksheet)eWb.Worksheets[i]).Name;
                        //mehet = true;
                    }
                    while ((i < eWb.Worksheets.Count) && (nev != lapnev));
                    if (nev == lapnev) return ((ExcelWorksheet)eWb.Worksheets[i]);
                }
                return null;
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:GetSzabiSheet()"));
            }
        }

        public static ExcelStruct GetJelenletiExcel(Felado felado)
        {
            int i = 0;
            bool abort = false;
            try
            {
                if (!k.Letezik(k.mentpath + "jelenleti.xls", "F"))
                {
                    do
                    {
                        if (i > 0)
                            if (k.Uzi("Az előző kísérlet a jelenléti.xls létrehozására nem volt sikeres.\nMegpróbálja újra?", "yn", "*") == DialogResult.No) abort = true;
                        CreateJelenleti();
                        i++;
                    }
                    while ((!k.Letezik(k.mentpath + "jelenleti.xls", "F")) && !abort);
                    if (abort)
                    {
                        ExcelStruct eStr = new ExcelStruct(true);
                        return eStr;
                    }
                }
                ExcelApplication ExApp;
                ExcelWorkbook ExWb;
                ExcelWorksheet ExWs;
                if (!k.EMjelen.m)
                {
                    k.EMjelen.ea = ef.Exapp();
                    k.EMjelen.ewb = ef.Megnyit(k.mentpath + "jelenleti.xls", k.EMjelen.ea);
                    k.EMjelen.m = true;
                }
                ExApp=k.EMjelen.ea;
                ExWb = k.EMjelen.ewb;
                i = 0;
                bool found = false;
                do
                {
                    i++;
                    found = (((ExcelWorksheet)ExWb.Worksheets[i]).Name == felado.tipus);
                }
                while ((i < ExWb.Worksheets.Count) && !found);
                if (!found)
                {
                    ExWs = CreateSheet(felado, ExWb);
                }
                else
                {
                    ExWs=((ExcelWorksheet)ExWb.Worksheets[i]);
                }                    
                ((ExcelWorksheet)ExWs).Activate(); 
                int hossz = ef.listavege(ExWb, 1);
                i = 0;
                found = false;
                do
                {
                    i++;
                    found = (ef.cella(ExWb, i, 1) == felado.nev);
                }
                while ((i < hossz) && !found);
                if (found)
                {
                    ExcelStruct ExStr = new ExcelStruct();
                    ExStr.ExApp = ExApp;
                    ExStr.ExWb = ExWb;
                    ExStr.ExWs = ExWs;
                    ExStr.num1 = i;
                    ExApp = null;
                    ExWb = null;
                    ExWs = null;
                    return ExStr;
                }
                else
                {
                    ExcelStruct ExStr = AddtoJelenleti(ExApp, ExWb, ExWs, felado);
                    ExApp = null;
                    ExWb = null;
                    ExWs = null;
                    return ExStr;
                }               
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:GetJelenletiExcel()"));
            }
        }

        public static ExcelStruct AddtoJelenleti(ExcelApplication eApp, ExcelWorkbook eWb, ExcelWorksheet eWs, Felado felado)
        {
            try
            {
                ((ExcelWorksheet)eWs).Activate();
                int i = ef.listavege(eWb, 1);
                ef.cella(eWb, i, 1, felado.nev);
                ef.GetColRange(eWb, 1).EntireColumn.AutoFit();
                ExcelStruct eStr = new ExcelStruct(true);
                eStr.ExApp = eApp;
                eStr.ExWb = eWb;
                eStr.ExWs = eWs;
                eStr.num1 = i;
                return eStr;
            }
            catch (Exception exc)
            {
                throw (new Exception(exc.Message + "\nHely: Addtojelenleti()", exc));
            }
        }

        public static void CreateJelenleti()
        {
            ExcelApplication ExApp;
            ExcelWorkbook ExWb;
            try
            {
                if (!k.EMjelen.m)
                {
                    k.EMjelen.ea = ef.Exapp();
                    k.EMjelen.ewb = k.EMjelen.ea.Workbooks.Add(Missing.Value);
                    k.EMjelen.m = true;
                }
                ExApp = k.EMjelen.ea;
                ExWb = k.EMjelen.ewb;                
                ef.Ment(ExApp, k.mentpath + "jelenleti.xls");
                ExWb = null;
                ExApp = null;
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:CreateJelenleti()"));
            }
            
                
            
        }

        public static ExcelWorksheet CreateSheet(Felado felado, ExcelWorkbook eWb)
        {
            try
            {                
                ExcelWorksheet eWs = (ExcelWorksheet)eWb.Sheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                ExcelRange eR;
                DateTime dt;
                string datum;
                string[] nevek={"T.fiz.ó",	"Dolg.",	"Fiz.sz.",	"Gysz.",	"Fiz.ü.",	"Eü.fiz.",	"Apsz"};
                int i=1;
                bool kitoltendo;
                IFormatProvider culture = new System.Globalization.CultureInfo("hu-HU", true);
                //eWb.Application.Visible = true;
                eWs.Name = felado.tipus;
                int napokszama = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                ef.cella(eWb, 1+k.ExcElsoSorYElt , 1,"Név");
                kitoltendo = (k.Napok[1] == "i");
                for (i = 1; i <= napokszama ; i++)
                {
                    if (kitoltendo) k.Napok[i] = "m";
                    ef.cella(eWb, 1 + k.ExcElsoSorYElt, i + k.ExcNapXElt, i.ToString());
                    datum = DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + "." + i.ToString();
                    dt = DateTime.Parse(datum,culture);
                    eR = ef.GetColRange(eWb, i + k.ExcNapXElt);
                    eR.EntireColumn.ColumnWidth = 2.57;
                    if ((dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday) && !Munkanap(i))
                    {
                        eR.Interior.ColorIndex = 15;
                        if (kitoltendo) k.Napok[i] = "sz";
                    }
                    if (Unnepnap(i))
                    {
                        //eR.Interior.ColorIndex = 15;
                        if (kitoltendo) k.Napok[i] = "u";
                    }

                }
                i=napokszama;
                foreach (string nev in nevek)
                {
                    i++;
                    ef.cella(eWb, 1 + k.ExcElsoSorYElt, i + k.ExcNapXElt, nev);
                    eR = ef.GetColRange(eWb, i + k.ExcNapXElt);
                    eR.EntireColumn.ColumnWidth = 6.29;
                }               
                
                //eWb.Application.Visible = true;
                datum =  DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + ".1";                    
                dt = DateTime.Parse(datum, culture);
                ef.Sel(eWb, "A2", " ");                               
                eWb.Application.ActiveWindow.FreezePanes = true;
                ((ExcelWorksheet)eWb.ActiveSheet).PageSetup.PrintTitleRows = "$1:$1";
                ((ExcelWorksheet)eWb.ActiveSheet).PageSetup.PrintTitleColumns = "";
                ((ExcelWorksheet)eWb.ActiveSheet).PageSetup.PrintArea = "";//"$A$1:$AM$142";
                ((ExcelWorksheet)eWb.ActiveSheet).PageSetup.LeftHeader= "&\"Arial,Italic\"&12Autólámpa fejlesztés";
                ((ExcelWorksheet)eWb.ActiveSheet).PageSetup.CenterHeader = "&\"Arial,Bold\"&14JELENLÉTI ÍV&\"Arial,Regular\"&10\n"+dt.ToString("yyyy.MMMM",culture);
                ((ExcelWorksheet)eWb.ActiveSheet).PageSetup.RightHeader = "&D&T";
                ((ExcelWorksheet)eWb.ActiveSheet).PageSetup.CenterFooter = "Virágh Ila" + "\n"+ "2628";
                ((ExcelWorksheet)eWb.ActiveSheet).PageSetup.LeftMargin = eWb.Application.InchesToPoints(0.275590551181102);
                ((ExcelWorksheet)eWb.ActiveSheet).PageSetup.RightMargin = eWb.Application.InchesToPoints(0.433070866141732);
                ((ExcelWorksheet)eWb.ActiveSheet).PageSetup.TopMargin = eWb.Application.InchesToPoints(0.590551181102362);
                ((ExcelWorksheet)eWb.ActiveSheet).PageSetup.BottomMargin = eWb.Application.InchesToPoints(0.433070866141732);
                ((ExcelWorksheet)eWb.ActiveSheet).PageSetup.HeaderMargin = eWb.Application.InchesToPoints(0.196850393700787);
                ((ExcelWorksheet)eWb.ActiveSheet).PageSetup.FooterMargin = eWb.Application.InchesToPoints(0.196850393700787);
                ((ExcelWorksheet)eWb.ActiveSheet).PageSetup.PrintHeadings = false;
                ((ExcelWorksheet)eWb.ActiveSheet).PageSetup.PrintGridlines = true;
                ((ExcelWorksheet)eWb.ActiveSheet).PageSetup.CenterHorizontally = true;
                ((ExcelWorksheet)eWb.ActiveSheet).PageSetup.CenterVertically = true;
                ((ExcelWorksheet)eWb.ActiveSheet).PageSetup.Orientation  = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
                ((ExcelWorksheet)eWb.ActiveSheet).PageSetup.Draft = false;
                ((ExcelWorksheet)eWb.ActiveSheet).PageSetup.PaperSize =Microsoft.Office.Interop.Excel.XlPaperSize.xlPaperA4;
                eR = null;                
                culture = null;
                return eWs;
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + " \nHely:CreateSheet()"));
            }

        }

        public static k.Felado GetUserParams(string felado)
        {
            k.Felado locfelado= new Felado(true);
            try
            {                
                locfelado = GetParams(felado);
                if (locfelado.nev == "")
                {
                    if (k.Uzi("A felado " + felado + " nem található.\nKívánja a listába felvenni?", "yn", "?") == DialogResult.Yes) locfelado = UjnevHozzaadasa(felado);
                }
            }
            catch (Exception exp)
            {                
                throw (new Exception(exp.Message + " \nHely:GetUserParams"));
            }
            return locfelado;
        }
        public static  k.Felado GetParams(string felado)
        {

            Microsoft.Office.Interop.Excel.Workbook eWb;
            Microsoft.Office.Interop.Excel.Application exapp;

            try
            {
                if (!k.EMm2n.m)
                {
                    k.EMm2n.ea = new ExcelApplication();
                    k.EMm2n.ewb = ef.Megnyit(k.mail2nev, k.EMm2n.ea);
                    k.EMm2n.m = true;
                }
                exapp = k.EMm2n.ea;
                eWb = k.EMm2n.ewb;
            }
            catch (Exception exp)
            {
                k.Felado locfelado = new Felado(true);
                throw (new Exception(exp.Message + " \nHely:GetParams mail2nev megnyitásakor"));
            }
            try
            {
                k.Felado locfelado = new Felado(true);
                int n = 0;
                bool found = false;
                string fa;
                do
                {
                    n++;
                    fa=ef.cella(eWb, n, 1);
                    if (fa == felado) found = true;
                }
                while ((fa!= "") && !found);
                if (found)
                {
                    locfelado.nev = ef.cella(eWb, n, 2);
                    locfelado.emailnev = ef.cella(eWb, n, 1);
                    locfelado.tipus = ef.cella(eWb, n, 3);
                    locfelado.tulora = (ef.cella(eWb, n, 4) == "igen" || ef.cella(eWb, n, 4) == "Igen");
                }

                return locfelado;
            }
            catch (Exception exp)
            {
                k.Felado locfelado = new Felado(true);
                throw (new Exception(exp.Message + " \nHely:GetParams cella olvasásakor"));
            }
            finally
            {
                eWb = null;
                exapp = null;                
            }
        }
        public static k.Felado UjnevHozzaadasa(string nev)
        {
            Form nevhozzaadasa = new Nevhozzadasa(nev);
            k.Felado locfelado = new k.Felado(true);
            nevhozzaadasa.ShowDialog();
            if (nevhozzaadasa.DialogResult  == DialogResult.OK)
            {

                locfelado = k.GetParams(nev);
                if (locfelado.nev == "")
                {
                    if (k.Uzi("A feladó " + nev + " hozzáadása nem sikerült.\nKívánja a újra megpróbálni?", "yn", "?") == DialogResult.Yes) locfelado = UjnevHozzaadasa(nev);
                    else return locfelado;
                }
                else
                {
                    k.Uzi("A feladó " + nev + " felvétele sikerült!", "oo", "i");
                }
            }
            else
            {
                k.Uzi("A feladó " + nev + " felvétele megszakítva!", "oo", "*");
            }
            return locfelado;
        }
        public static bool Unnepnap(int nap)
        {
            try
            {
                ExcelApplication eApp;                
                ExcelWorkbook eWb;
                if (!k.EMunn.m)
                {
                    k.EMunn.ea = new ExcelApplication();
                    k.EMunn.ewb = ef.Megnyit(k.unnepath, k.EMunn.ea);
                    k.EMunn.m = true;
                }
                eApp = k.EMunn.ea;
                eWb = k.EMunn.ewb;
                int i = 0;
                bool found = false;
                string cellaertek;
                do
                {
                    i++;
                    cellaertek = ef.cella(eWb, i, 1);
                    found = (cellaertek == nap.ToString());

                }
                while ((cellaertek != "") && !found);
                eWb = null;                
                eApp = null;                
                return found;
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + "\nHely: Unnepnap()"));
            }
                
        }
        public static bool Munkanap(int nap)
        {
            try
            {
                ExcelApplication eApp;
                ExcelWorkbook eWb;
                if (!k.EMmunka.m)
                {
                    k.EMmunka.ea = new ExcelApplication();
                    k.EMmunka.ewb = ef.Megnyit(k.munkapath, k.EMmunka.ea);
                    k.EMmunka.m = true;
                }
                eApp = k.EMmunka.ea;
                eWb = k.EMmunka.ewb; 
                int i = 0;
                bool found = false;
                string cellaertek;
                do
                {
                    i++;
                    cellaertek = ef.cella(eWb, i, 1);
                    found = (cellaertek == nap.ToString());

                }
                while ((cellaertek != "") && !found);
                eWb = null;
                eApp = null;
                return found;
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + "\nHely: Munkanap()"));
            }

        }
        public static bool Init()
        {
            bool tovabb=false;
            string utvonal = Application.StartupPath + "\\settings.set";
            try
            {
                
                do
                    if (ff.Hanysor(utvonal) < paramereterszama)
                    {
                        if (k.Uzi("A beállítások fájl a program egy régebbi verziójához készült, vagy sérült\nÚj fájl léterehozásához használja kérem az \"Általános beállitások\" menüpontot!", "oc", "!") == DialogResult.Cancel) tovabb=true;
                        else
                        {
                            Form beallitasok = new Beallitasok();
                            beallitasok.ShowDialog();
                        }
                    }
                while ((ff.Hanysor(utvonal) < paramereterszama) && !tovabb);
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + "\nHely: Init()-Beallitasok() hivasa"));
            }
            try
            {
                @mail2nev = ff.SorOlvas(utvonal, 1);
                @logpath = ff.SorOlvas(utvonal, 2);
                @tema = ff.SorOlvas(utvonal, 3);
                @feldpath = ff.SorOlvas(utvonal, 4);
                @unnepath = ff.SorOlvas(utvonal, 5);
                utbej = ff.SorOlvas(utvonal, 6);
                jelexc = ff.SorOlvas(utvonal, 7) == "True";
                @szabipath=ff.SorOlvas(utvonal,8);
                @mentpath = ff.SorOlvas(utvonal, 9);
                @munkapath = ff.SorOlvas(utvonal, 10);

            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message + "\nHely: Init()-Változók beállítása"));
            }
            return true;            
        }

        public static bool Letezik(string utvonal, string tipus)
        {
            switch (tipus)
            {
                case "fajl":        //nincs break
                case "fájl":        //nincs break
                case "f":           //nincs break
                case "F":
                    try
                    {
                        return System.IO.File.Exists(utvonal);
                    }
                    catch (Exception exp)
                    {
                        throw (new Exception(exp.Message + "\nHely: Letezik()-Fájl"));
                        
                    }
                case "mappa":       //nincs break
                case "m":        //nincs break
                case "M":        //nincs break
                case "d":        //nincs break
                case "D":
                    try
                    {
                        return System.IO.Directory.Exists(utvonal);
                    }
                    catch (Exception exp)
                    {
                        throw (new Exception(exp.Message + "\nHely: Letezik()-Mappa"));
                        
                    }

                default:
                    Uzi("Hibás paraméter vizsgálandó tipus kiválasztásakor!\n" + tipus+ "\nHely: Letezik()","oo","x");
                    return false;
            }
        }
        public static DialogResult Uzi(string szoveg, string gomb, string ikon)
        {
            bool log=false ;
            MessageBoxButtons but;
            MessageBoxIcon ico;
            try
            {
                
                switch (gomb)
                {
                    case "oo":        //nincs break
                    case "OO":        //nincs break
                    case "OK":        //nincs break
                    case "ok":        //nincs break
                    case "O":         //nincs break
                    case "o":
                        but = MessageBoxButtons.OK;     //Csak Ok gomb lesz a MessageBoxon
                        break;
                    case "yn":        //nincs break          
                    case "YN":
                        but = MessageBoxButtons.YesNo;  //Yes és No gomb lesz a MessageBoxon
                        break;
                    case "oc":      //nincs break
                    case "OC":
                        but = MessageBoxButtons.OKCancel;   //Ok és Cancel gomb lesz a MessageBoxon
                        break;
                    default:
                        szoveg = "Hibás paraméter a gomb kiválasztásakor!\n" + gomb;
                        but = MessageBoxButtons.AbortRetryIgnore;
                        break;
                }
                switch (ikon)
                {
                    case "!":
                        ico = MessageBoxIcon.Exclamation;
                        break;
                    case "i":       //nincs break
                    case "I":
                        ico = MessageBoxIcon.Information;
                        break;
                    case "?":
                        ico = MessageBoxIcon.Question;
                        break;
                    case "x":        //nincs break
                    case "*":        //nincs break
                    case "X":
                        ico = MessageBoxIcon.Error;
                        log = true;
                        break;
                    default:
                        szoveg = "Hibás paraméter az ikon kiválasztásakor!\n" + ikon;
                        ico = MessageBoxIcon.None;
                        break;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message + " hiba a párbeszédpanel paraméterek kiválasztásakor", "Jelenléti", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return DialogResult.No;
            }
            try
            {
                if (log) Logging(szoveg, "sys");
                return MessageBox.Show(szoveg, "Jelenléti.net", but, ico);                
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message + " hiba a párbeszédpanel megjelenítésekor", "Jelenléti", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return DialogResult.No;
            }
        }
        public static string Endslash(string szoveg)
        {
            try
            {
                int hossz;
                const string backslash = "\\";
                hossz = szoveg.Length;
                if (hossz > 0)
                {
                    if (szoveg.EndsWith(backslash)) return szoveg;
                    else return szoveg + "\\";
                }
                else return "c:\\";
            }
            catch (Exception exc)
            {
                throw (new Exception(exc.Message + "\nHely: Endslash()", exc));
            }
        }
        public static void Logging(string szoveg, string tipus)
        {
			szoveg = DateTime.Now.ToString("yyyy.MM.dd. HH.mm.ss") + "\n" + szoveg + "\n";
            switch (tipus)
            {
                case "hiba":        //nincs break
                case "h":           //nincs break
                case "error":       //nincs break
                case "e":           //nincs break
                case "sys":         //nincs break
                case "Sys":         //nincs break
                case "s":           //nincs break
                case "S":
                    if (!k.Letezik(k.logpath + "syslog.log","F")) ff.SorIras(szoveg, k.logpath + "syslog.log", "O");
                    else ff.SorIras(szoveg, k.logpath + "syslog.log", "A");
                    break;
                case "app":         //nincs break
                case "a":           //nincs break
                case "log":         //nincs break
                case "l":
                    if (!k.Letezik(k.logpath + "applog.log","F")) ff.SorIras(szoveg, k.logpath + "applog.log", "O");
                    else ff.SorIras(szoveg, k.logpath + "applog.log", "A");
                    break;
            }            
        }
    }
}