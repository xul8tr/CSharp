using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfDrones
{
    class Position
    {
        public Position(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        private float _x;

        public float X
        {
            get { return _x; }
            set
            {
                if (value >= 0)
                {
                    _x = value;
                }
                else
                {
                    throw new ArgumentException("Hibás Pozicio.X érték. X nem lehet negatív!");
                }
            }
        }
        
        private float _y;

        public float Y
        {
            get { return _y; }
            set
            {
                if (value >= 0)
                {
                    _y = value;
                }
                else
                {
                    throw new ArgumentException("Hibás Pozicio.Y érték. Y nem lehet negatív!");
                }
            }
        }

        public static double Tavolsag(Position pozicio1, Position pozicio2)
        {
            return Math.Sqrt(Math.Pow(pozicio1.X - pozicio2.X, 2d) + Math.Pow(pozicio1.Y - pozicio2.Y, 2d));
        }
    }
}
