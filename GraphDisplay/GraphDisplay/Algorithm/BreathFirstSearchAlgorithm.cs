using System;
using System.Collections.Generic;
using ELTE.Algorithms.GraphDisplay.DataStructure;
using System.Drawing;

namespace ELTE.Algorithms.GraphDisplay.Algorithm
{
    /// <summary>
    /// Szélességi bejárás
    /// </summary>
    public class BreathFirstSearchAlgorithm : GraphAlgorithm
    {
        private Queue<GraphNode> _NodeQueue; // sorban tároljuk az elért csúcsokat

        public override String Status
        {
            get 
            {
                // a státusznál kiírjuk a sorban, illetve a már bejártak listájában található elemeket
                String status = "Queue: ";
                foreach (GraphNode node in _NodeQueue)
                {
                    status += node.Number.ToString() + ", ";
                }
                if (_FinishedNodes.Count > 0)
                {
                    status += " Finished: ";
                    foreach (GraphNode node in _FinishedNodes)
                    {
                        status += node.Number.ToString() + ", ";
                    }
                }
                return status.Substring(0, status.Length - 2);
            }
        }
        public override Boolean Running
        {
            get { return (_NodeQueue.Count > 0); } // akkor fut, ha van elem a sorban
        }

        public BreathFirstSearchAlgorithm(Graph graph)
            : base(graph)
        {
            _NodeQueue = new Queue<GraphNode>();
        }

        public override void Start()
        {
            _NodeQueue.Clear();
            _FinishedNodes.Clear();

            if (_Graph.SelectedNode != null)
                _NodeQueue.Enqueue(_Graph.SelectedNode); // a kiválasztott csúcstól indul az algoritmus, tehát azt tesszük be először a sorba
        }

        public override void Step()
        {
            if (_NodeQueue.Count > 0)
            {
                GraphNode currentNode = _NodeQueue.Dequeue(); // kivesszük a sor első elemét
                foreach (GraphArc arc in _Graph.AdjList[currentNode])
                    // megnézzük egy csúcs összes szomszédját
                    if (!_FinishedNodes.Contains(arc.Target) && !_NodeQueue.Contains(arc.Target))
                        // ha még nincs benne sem a sorban, sem a listában, akkro felvesszük a sorba
                        _NodeQueue.Enqueue(arc.Target);

                _FinishedNodes.Add(currentNode); // felvesszük a csúcsot a bejártak közé
            }
        }

        public override void Draw(Graphics gr)
        {
            Font font = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold);

            foreach (GraphNode node in _Graph.AdjList.Keys)
                foreach (GraphArc arc in _Graph.AdjList[node])
                {
                    if (_NodeQueue.Peek() == arc.Source)
                        arc.Draw(gr, Color.Red, font);
                    else if (_FinishedNodes.Contains(arc.Source) && (_FinishedNodes.Contains(arc.Target) || _NodeQueue.Contains(arc.Target)))
                        arc.Draw(gr, Color.Blue, font);
                    else
                        arc.Draw(gr, Color.Black, font);
                }

            if (_NodeQueue.Count > 0)
            {
                foreach (GraphNode node in _Graph.AdjList.Keys)
                {
                    if (_NodeQueue.Peek() == node)
                        node.Draw(gr, Color.Red, font);
                    else if (_NodeQueue.Contains(node))
                        node.Draw(gr, Color.Green, font);
                    else if (_FinishedNodes.Contains(node))
                        node.Draw(gr, Color.Blue, font);
                    else
                        node.Draw(gr, Color.Black, font);
                }
            }
        }
    }
}
