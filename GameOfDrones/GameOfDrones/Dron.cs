using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfDrones
{
    internal abstract class Dron
    {
        public Dron(ushort azonosito, string elnevezes, Position pozicio)
        {
            Azonosito = azonosito;
            Elnevezes = elnevezes;
            Pozicio = pozicio;
            EletbenVan = true;
        }

        private ushort _azonosito;

        public ushort Azonosito
        {
            get { return _azonosito; }
            private set 
            {
                if (value >= 0)
                {
                    _azonosito = value;
                }
                else
                {
                    throw new Exception("A drón azonosítója nem lehet negatív szám.");
                }
            }
        }

        private string _elnevezes;

        public string Elnevezes
        {
            get { return _elnevezes; }
            private set 
            {
                if (value != null && value.Length > 2)
                {
                    _elnevezes = value;
                }
                else
                {
                    throw new DronException(this, "A drón neve legalább 3 karkter hosszú kell legyen."); 
                }
            }
        }

        private Position _Pozicio;

        public Position Pozicio
        {
            get { return _Pozicio; }
            private set { _Pozicio = value; }
        }

        private bool eletbenVan;

        public bool EletbenVan
        {
            get { return eletbenVan; }
            set { eletbenVan = value; }
        }

        virtual public void Repul(float x, float y)
        {
            if (EletbenVan)
            {
                Pozicio = new Position(x, y);
                Jelentes(string.Format("Elrepültem a(z) {0:F3}, {1:F3} koordinátákra.", Pozicio.X, Pozicio.Y));
            }
            else
            {
                throw new DronException(this, "Ez a madárka szárnyaszegett, nem tud már repülni. [Lelőtték :( ]"); 
            }
        }

        virtual public void Jelentes (string uzenet, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine("Drón {0} jelenti: {1}", Azonosito, uzenet);
        }
    }
}
