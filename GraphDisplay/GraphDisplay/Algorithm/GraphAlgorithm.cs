using System;
using System.Collections.Generic;
using ELTE.Algorithms.GraphDisplay.DataStructure;
using System.Drawing;


namespace ELTE.Algorithms.GraphDisplay.Algorithm
{
    /// <summary>
    /// Gráf algoritmus osztálya
    /// </summary>
    public abstract class GraphAlgorithm
    {
        protected Graph _Graph;
        protected List<GraphNode> _FinishedNodes; // bejárt csúcsok listája

        /// <summary>
        /// Aktuális státusz szöveges lekérdezése
        /// </summary>
        public abstract String Status { get; }
        /// <summary>
        /// Fut-e az algoritmus
        /// </summary>
        public abstract Boolean Running { get; }

        public GraphAlgorithm(Graph graph)
        {
            _Graph = graph;
            _FinishedNodes = new List<GraphNode>();
        }

        /// <summary>
        /// Elindítás
        /// </summary>
        public abstract void Start();
        /// <summary>
        /// Léptetés
        /// </summary>
        public abstract void Step();
        /// <summary>
        /// Teljes futtatás
        /// </summary>
        public void Run()
        {
            Start();
            while (Running)
                Step();
        }
        /// <summary>
        /// Algoritmus kirajzolása
        /// </summary>
        /// <param name="gr">Rajzoló objektum</param>
        public abstract void Draw(Graphics gr);
    }
}
