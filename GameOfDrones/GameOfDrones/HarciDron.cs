using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfDrones
{
    class HarciDron: Dron, IMegfigyelo, IHarcos
    {
        public HarciDron(ushort azonosito, string elnevezes, Position pozicio, uint bombakSzama, float lotav)
            : base(azonosito, elnevezes, pozicio)
        {
            BombakSzama = bombakSzama;
            Lotav = lotav;
        }

        private int _fotokSzama;

        public int FotokSzama
        {
            get { return _fotokSzama; }
            private set { _fotokSzama = value; }
        }

        private uint _bombakSzama;

        public uint BombakSzama
        {
            get { return _bombakSzama; }
            private set 
            {
                if (value >= 0)
                {
                    _bombakSzama = value;
                }
                else
                {
                    throw new DronException(this, "A bombák száma nem lehet negatív");
                }

            }
        }

        private float _lotav;

        public float Lotav
        {
            get { return _lotav; }
            private set 
            {
                if (value >= 0)
                {
                    _lotav = value;
                }
                else
                {
                    throw new DronException(this, "A lőtáv értéke nem lehet negatív.");
                }
            }
        }

        
        public void Bombaz()
        {
            if (EletbenVan)
            {
                if (BombakSzama == 0)
                {
                    throw new DronException(this, "Nincs több bomba.");
                }
                else
                {
                    BombakSzama--;
                    this.Jelentes(string.Format("Bomba kioldása megtörtént. Maradt {0} bomba.", BombakSzama), ConsoleColor.Magenta);
                }
            }
            else
            {
                throw new DronException(this, "Ez a drón megsemmisült, nem tud már bombázni. [Lelőtték :( ]"); 
            }
        }

        public void Lo(Dron MasikDron)
        {
            if (EletbenVan)
            {
                if (MasikDron == this)
                {
                    throw new DronException(this, "Saját magamra nem tudok lőni.");
                }
                if (MasikDron.EletbenVan)
                {
                    if (Position.Tavolsag(this.Pozicio, MasikDron.Pozicio) <= Lotav)
                    {
                        MasikDron.Jelentes("Most már semmi! [Lelőtték]");
                        MasikDron.EletbenVan = false;
                        this.Jelentes(string.Format("Lelőttem drón {0}-t.", MasikDron.Azonosito), ConsoleColor.Green);
                    }
                    else
                    {
                        throw new DronException(this, string.Format("Drón {0} túl messze van, nem találtam el.", MasikDron.Azonosito));
                    }
                }
                else
                {
                    throw new DronException(this, string.Format("Drón {0}-t már lelőtték, nem tudok rálőni.", MasikDron.Azonosito));
                }
            }
            else
            {
                throw new DronException(this, "Ez a drón megsemmisült, nem tud már lövöldözni. [Ironikus, őt lőtték le :) ]");
            }
        }

        public void Fotoz(Fenytartomany ft)
        {
            if (EletbenVan)
            {
                if (ft != Fenytartomany.hokep)
                {
                    throw new DronException(this, "Ez a drón csak hőkép készítését támogatja.");
                }
                else
                {
                    FotokSzama++;
                }
            }
            else
            {
                throw new DronException(this, "A drón megsemmisült, nem tud már fotózni sehogyan se. [Lelőtték :( ]");
            }
        }
    }
}
