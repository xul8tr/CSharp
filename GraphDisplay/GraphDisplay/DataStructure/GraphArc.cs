using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ELTE.Algorithms.GraphDisplay.DataStructure
{
    /// <summary>
    /// Gráf éle
    /// </summary>
    public class GraphArc
    {
        /// <summary>
        /// Forrás csúcs
        /// </summary>
        public GraphNode Source;
        /// <summary>
        /// Célcsúcs
        /// </summary>
        public GraphNode Target;
        /// <summary>
        /// Él súlya
        /// </summary>
        public Int32 Weight;

        public GraphArc(GraphNode source, GraphNode target, Int32 weight)
        {
            Source = source;
            Target = target;
            Weight = weight;
        }
        /// <summary>
        /// Közel van-e egy pont az élhez
        /// </summary>
        /// <param name="point">A vizsgált pont</param>
        /// <returns></returns>
        public Boolean Near(Point point)
        {
            // használjuk az egyenes egyenletét, kiszámoljuk a pont távolságát az egyenestől
            Double distance;
            if (Source.Location.X == Target.Location.X)
                distance = Math.Abs(Source.Location.X - point.X);
            else
            {
                Double a = Convert.ToDouble(Target.Location.Y - Source.Location.Y) / (Target.Location.X - Source.Location.X);
                Double b = -a * Source.Location.X + Source.Location.Y;
                distance = Math.Abs((point.Y - a * point.X - b) / Math.Sqrt(1 + a * a));
            }
            // közel van, ha az egyestől 10 pontos sugárban van, és még a szakaszon belül is van
            return (distance < 10 &&
                    ((point.X >= Source.Location.X && point.X <= Target.Location.X) ||
                     (point.X >= Target.Location.X && point.X <= Source.Location.X)) &&
                    ((point.Y >= Source.Location.Y && point.Y <= Target.Location.Y) ||
                     (point.Y >= Target.Location.Y && point.Y <= Source.Location.Y))
                   );
        }
        /// <summary>
        /// Él kirajzolása
        /// </summary>
        /// <param name="gr">Rajzobjektum</param>
        /// <param name="color">Él színe</param>
        /// <param name="font">Szövegtípus</param>
        public void Draw(Graphics gr, Color color, Font font)
        {
            Pen linePen = new Pen(color, 2);
            Brush triangleBrush = new SolidBrush(color);
            SizeF stringSize = gr.MeasureString(Weight.ToString(), font);

            // külön ki kell számolni a nyíl háromszögét úgy, hogy a három pontját adjuk meg
            Point tri1 = Point.Empty, tri2 = Point.Empty, tri3 = Point.Empty;
            Double d, k;

            if (Source.Location.X != Target.Location.X)
                d = Math.Abs(Convert.ToDouble(Source.Location.Y - Target.Location.Y) / Convert.ToDouble(Source.Location.X - Target.Location.X));
            else
                d = 1;

            // figyelembe vesszük, hogy a háromszögnek 15 távolságra kell lennie a céltól, hogy a csúcs rajzán kívül legyen
            if (d != 0)
                k = Math.Sqrt(225 / (Math.Pow(d, 2) + 1));
            else
                k = 15;

            tri1.X = Convert.ToInt32(Target.Location.X + k * Math.Sign(Source.Location.X - Target.Location.X));
            tri1.Y = Convert.ToInt32(Target.Location.Y + d * k * Math.Sign(Source.Location.Y - Target.Location.Y));

            tri2.X = Convert.ToInt32(Target.Location.X + 1.75 * k * Math.Sign(Source.Location.X - Target.Location.X) + 0.75 * d * k * Math.Sign(Source.Location.Y - Target.Location.Y) / 2);
            tri2.Y = Convert.ToInt32(Target.Location.Y + 1.75 * d * k * Math.Sign(Source.Location.Y - Target.Location.Y) - 0.75 * k * Math.Sign(Source.Location.X - Target.Location.X) / 2);

            tri3.X = Convert.ToInt32(Target.Location.X + 1.75 * k * Math.Sign(Source.Location.X - Target.Location.X) - 0.75 * d * k * Math.Sign(Source.Location.Y - Target.Location.Y) / 2);
            tri3.Y = Convert.ToInt32(Target.Location.Y + 1.75 * d * k * Math.Sign(Source.Location.Y - Target.Location.Y) + 0.75 * k * Math.Sign(Source.Location.X - Target.Location.X) / 2);

            // rajzolunk egy összezárt háromszöget a pontok között
            Point[] points = { tri1, tri2, tri3, tri1 };
            GraphicsPath path = new GraphicsPath();
            path.AddLines(points);

            gr.DrawLine(linePen, Source.Location, Target.Location); // vonal rajzolása
            gr.FillPath(triangleBrush, path); // háromszög rajzolása
            gr.DrawString(Weight.ToString(), font, Brushes.Black, (Source.Location.X + Target.Location.X) / 2 - stringSize.Width / 2, (Source.Location.Y + Target.Location.Y) / 2 - stringSize.Height); // élsúly rajzolása
        }
        public override string ToString()
        {
            return "from " + Source + ", to " + Target;
        }
    }
}
