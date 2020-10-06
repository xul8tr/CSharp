using System;
using System.Drawing;

namespace ELTE.Algorithms.GraphDisplay.DataStructure
{
    /// <summary>
    /// Gráf csúcsa
    /// </summary>
    public class GraphNode
    {
        public static GraphNode Empty = new GraphNode(-1,new Point(1,1));
        /// <summary>
        /// Csúcs száma
        /// </summary>
        public int Number;
        /// <summary>
        /// CSúcs pozíciója a képernyőn
        /// </summary>
        public Point Location;

        public GraphNode(int number, Point location)
        {
            Number = number; 
            Location = location;
        }
        /// <summary>
        /// Közel van-e egy pont a csúcshoz
        /// </summary>
        /// <param name="point">A vizsgált pont</param>
        /// <returns></returns>
        public Boolean Near(Point point)
        {
            // kiszámoljuk a pontok távolságát, és akkro van közel, ha az kisebb, mint a sugár
            return (Math.Sqrt(Math.Pow(Location.X - point.X, 2) + Math.Pow(Location.Y - point.Y, 2)) < 15);
        }
        /// <summary>
        /// Csúcs kirajzolása
        /// </summary>
        /// <param name="gr">Rajzobjektum</param>
        /// <param name="color">Körvonal színe</param>
        /// <param name="font">Szövegtípus</param>
        public void Draw(Graphics gr, Color color, Font font)
        {
            Pen nodePen = new Pen(color, 2); // vonalrajzoló toll
            SizeF stringSize = gr.MeasureString(Number.ToString(), font); // szövegméret lekérdezése, hogy pontosan középre tudjuk írni

            gr.FillEllipse(Brushes.White, Location.X - 15, Location.Y - 15, 30, 30); // fehér háttér
            gr.DrawEllipse(nodePen, Location.X - 15, Location.Y - 15, 30, 30); // körvonal
            gr.DrawString(Number.ToString(), font, Brushes.Black, Location.X - stringSize.Width / 2, Location.Y - stringSize.Height / 2); // szöveg
        }

        public override string ToString()
        {
            return Number.ToString();
        }
    }
}
