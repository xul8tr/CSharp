using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfDrones
{
    class Program
    {
        enum Akcio
        {
            Repul,
            Fotoz,
            Bombaz,
            Lo
        }

        static void Main(string[] args)
        {   
            FileKezelo fileKezelo = new FileKezelo("Drones.txt");
            DronGyar dronGyar = new DronGyar(fileKezelo);
            VeletlenszeruHarc(dronGyar);
            Eredmeny(dronGyar);
            Console.ReadLine();
        }

        private static void Eredmeny(DronGyar dronGyar)
        {
            int harciDronDb=0, felderitoDronDb=0, fotoUvDb=0, fotoLathatoDb=0, fotoIrDb=0, fotoHokepDb=0;
            uint maradtBombaDb = 0, maradtBombaCsakEloDb = 0;
            foreach (Dron dron in dronGyar.Dronok)
            {
                if (dron is HarciDron)
                {
                    HarciDron harciDron = dron as HarciDron;

                    if (dron.EletbenVan)
                    {
                        harciDronDb++;
                        maradtBombaCsakEloDb += harciDron.BombakSzama;
                    }
                                        
                    maradtBombaDb += harciDron.BombakSzama;
                    fotoHokepDb += harciDron.FotokSzama;
                }
                else
                {
                    if (dron.EletbenVan)
                    {
                        felderitoDronDb++;
                    }

                    FelderitoDron felderitoDron = dron as FelderitoDron;

                    fotoHokepDb += FotokSzamaAFenytartomanyban(felderitoDron, Fenytartomany.hokep);
                    fotoIrDb += FotokSzamaAFenytartomanyban(felderitoDron, Fenytartomany.infravoros);
                    fotoLathatoDb += FotokSzamaAFenytartomanyban(felderitoDron, Fenytartomany.lathato_feny);
                    fotoUvDb += FotokSzamaAFenytartomanyban(felderitoDron, Fenytartomany.ultraibolya);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine();
            }

            Console.ResetColor();
            Console.WriteLine("Életben maradt {0} felderítő és {1} harci drón.", felderitoDronDb, harciDronDb);
            Console.WriteLine("Maradt összesen {0} bomba a kilőtt drónokon lévőkkel együtt és {1} bomba csak az életben lévőket figyelembe véve.", maradtBombaDb, maradtBombaCsakEloDb);
            Console.WriteLine("A készített fotók száma az egyes fénytartományokban a következő:");
            Console.WriteLine("__________________________________________________________");
            Console.WriteLine("{0,-7}|{1,-12}|{2,-14}|{3,-13}|{4,8}", Fenytartomany.hokep, Fenytartomany.infravoros, Fenytartomany.lathato_feny, Fenytartomany.ultraibolya, "Összes");
            Console.WriteLine("{0,-7}|{1,-12}|{2,-14}|{3,-13}|{4,8}", fotoHokepDb, fotoIrDb, fotoLathatoDb, fotoUvDb, fotoHokepDb + fotoIrDb + fotoLathatoDb + fotoUvDb);
            Console.WriteLine("__________________________________________________________");
        }

        private static int FotokSzamaAFenytartomanyban(FelderitoDron felderitoDron, Fenytartomany fenytartomany)
        {
            int fotoDb = 0;
            if (felderitoDron.FenytartomanyokbanMennyiFotoKeszult.ContainsKey(fenytartomany))
            {
                fotoDb += felderitoDron.FenytartomanyokbanMennyiFotoKeszult[fenytartomany];
            }

            return fotoDb;
        }

        private static void VeletlenszeruHarc(DronGyar dronGyar)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            int dronokSzama = dronGyar.Dronok.Count;
            for (int i = 0; i < 5000; i++)
            {
                int kovetkezoDron = rnd.Next(0, dronokSzama);
                Dron dron = dronGyar.Dronok[kovetkezoDron];
                Akcio kovetkezoAkcio = (Akcio)rnd.Next(0, 4);
                if (dron.EletbenVan)
                {
                    dron.Jelentes(string.Format("Ez az én köröm és {0}ni fogok!", kovetkezoAkcio));
                    try
                    {
                        switch (kovetkezoAkcio)
                        {
                            case Akcio.Repul:
                                float a, b, c, d;
                                a = rnd.Next(100, 1000);
                                b = rnd.Next(100, 1000);
                                c = rnd.Next(100, 1000);
                                d = rnd.Next(100, 1000);
                                float ujX = a / b;
                                float ujY = c / d;
                                dron.Repul(ujX, ujY);
                                break;
                            case Akcio.Fotoz:
                                //mindkét gyerek drón típus megvalósítja az IMegfigyelő interfészt
                                Fenytartomany fenytartomany = (Fenytartomany)rnd.Next(0, 5);
                                (dron as IMegfigyelo).Fotoz(fenytartomany);
                                break;
                            case Akcio.Bombaz:
                                if (dron is IHarcos)
                                {
                                    (dron as IHarcos).Bombaz();
                                }
                                else
                                {
                                    if (dron.EletbenVan)
                                    {
                                        dron.Jelentes("Nem vagyok IHarcos, nem bombázok. Peace!", ConsoleColor.White);
                                    }
                                    else
                                    {
                                        dron.Jelentes("Ha élnék nem bombáznék, mert nem vagyok IHarcos, de lelőttek!", ConsoleColor.Red);
                                    }
                                }
                                break;
                            case Akcio.Lo:
                                if (dron is IHarcos)
                                {
                                    int masikDron = rnd.Next(0, dronokSzama);
                                    Dron masik = dronGyar.Dronok[masikDron];
                                    if (masik.EletbenVan)
                                    {
                                        masik.Jelentes("Van egy kis problémám! Mi az?");
                                        (dron as IHarcos).Lo(masik);
                                    }
                                    else
                                    {
                                        dron.Jelentes(string.Format("Ráfordultam {0} drónra, de már lelőtték!", masik.Azonosito), ConsoleColor.Red);
                                    }
                                }
                                else
                                {
                                    if (dron.EletbenVan)
                                    {
                                        dron.Jelentes("Nem vagyok IHarcos, nem lövöldözök!", ConsoleColor.White);
                                    }
                                    else
                                    {
                                        dron.Jelentes("Ha élnék nem lövöldöznék, mert nem vagyok IHarcos, ráadásul még le is lőttek!", ConsoleColor.Red);
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    catch (DronException dronException)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(dronException.Message);
                    }
                    catch (Exception exception)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Valami komolyabb hiba történt: {0}", exception.Message);
                    }
                }
                else
                {
                    dron.Jelentes(string.Format("Ez az én köröm lett volna, ha még élnék és {0}ni kellett volna!", kovetkezoAkcio), ConsoleColor.DarkGreen);
                }
                //System.Threading.Thread.Sleep(150);
            }            
        }
    }
}
