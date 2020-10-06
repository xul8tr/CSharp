using System;
using System.Collections.Generic;
using ELTE.Algorithms.GraphDisplay.DataStructure;
using System.Drawing;
using System.Text;

namespace ELTE.Algorithms.GraphDisplay.Algorithm
{
    /// <summary>
    /// Szélességi bejárás
    /// </summary>
    public class DijkstraAlgorithm : GraphAlgorithm
    {
        private Queue<GraphNode> _NodeQueue; // sorban tároljuk az elért csúcsokat
        private Dictionary<GraphNode, string> _ShortestPath = new Dictionary<GraphNode, string>();
        private Dictionary<GraphNode, GraphNode> _PreviousNode = new Dictionary<GraphNode, GraphNode>();
        private Dictionary<GraphNode, GraphArc> _ShortestPathWithArc = new Dictionary<GraphNode, GraphArc>();
        public override String Status
        {
            get 
            {
                StringBuilder sb = new StringBuilder("Started");
                if (_ShortestPathWithArc.Count>0)
                {
                    sb = new StringBuilder("Dijkstra sais: Go ");
                    foreach (KeyValuePair<GraphNode, GraphArc> pair in _ShortestPathWithArc)
                    {
                        sb.Append(pair.Value.ToString());
                        sb.Append(", ");
                    }
                    sb.Append(" for the shortest way.");
                }
                if (_NodeQueue.Count==0)
                {
                    if (_Graph.PreviousNode != null) //nem a teljes gráfra kerestünk bejárást, hanem 2 pont között
                    {
                        if ( _ShortestPathWithArc.ContainsKey(_Graph.SelectedNode))//van út a két node között
                        {
                            sb = new StringBuilder();

                            GraphArc trace =_ShortestPathWithArc[_Graph.SelectedNode];
                            sb.Insert(0, trace);
                            sb.Insert(trace.ToString().Length, " ");
                            while (trace.Source != _Graph.PreviousNode)
                            {
                                trace = _ShortestPathWithArc[trace.Source];
                                sb.Insert(0, trace);
                                sb.Insert(trace.ToString().Length, " ");
                            }
                            sb.Insert(0, " take ");
                            sb.Insert(0, _Graph.SelectedNode.ToString());
                            sb.Insert(0, " to node ");
                            sb.Insert(0, _Graph.PreviousNode.ToString());
                            sb.Insert(0, "For the shortest way from node");
                            sb.Append("Cost: ");
                            int costPrevious;
                            int costSelected;
                            if ((int.TryParse(_ShortestPath[_Graph.SelectedNode], out costSelected)) && (int.TryParse(_ShortestPath[_Graph.PreviousNode], out costPrevious)))
                            {
                                sb.Append(costSelected - costPrevious);
                            }
                        }
                        else//nincs út a két node között
                        {
                            sb = new StringBuilder("There is no way from node ");
                            sb.Append(_Graph.PreviousNode.ToString());
                            sb.Append(" to node ");
                            sb.Append(_Graph.SelectedNode.ToString());
                        }
                    }
                }
                return sb.ToString();
            }
        }
        public override Boolean Running
        {
            get
            {
                return (_NodeQueue.Count > 0);
            } // akkor fut, ha van elem a sorban
        }

        public DijkstraAlgorithm(Graph graph)
            : base(graph)
        {
            _NodeQueue = new Queue<GraphNode>();
        }

        public override void Start()
        {
            _NodeQueue.Clear();
            _FinishedNodes.Clear();
            if (_Graph.PreviousNode != null)
                _NodeQueue.Enqueue(_Graph.PreviousNode); // ilyenkor két csúcs van kiválasztva. a kiválasztott csúcstól indul az algoritmus, tehát azt tesszük be először a sorba
            else if (_Graph.SelectedNode != null)
                _NodeQueue.Enqueue(_Graph.SelectedNode); // a kiválasztott csúcstól indul az algoritmus, tehát azt tesszük be először a sorba
            InitializeDictionarys();
        }

        private void InitializeDictionarys()
        {
            foreach (GraphNode node in _Graph.AdjList.Keys)
            {
                if (_NodeQueue.Count > 0)
                {
                    if (node != _NodeQueue.Peek())
                    {
                        _ShortestPath.Add(node, "inf");
                    }
                    else
                    {
                        _ShortestPath.Add(node, "0");
                    }
                }
            }
        }

        public override void Step()
        {
            if (_NodeQueue.Count > 0)
            {
                GraphNode currentNode = _NodeQueue.Dequeue(); // kivesszük a sor első elemét
                if (!_FinishedNodes.Contains(currentNode)) //ha még nem dolgoztuk fel a csúcsot
                {
                    List<GraphArc> templist = new List<GraphArc>(); //mivel a legkisebb súlyú élet kell választani először, szükség lesz egy másolatra szomszédsági tömbről az adott csúcshoz
                    foreach (GraphArc arc in _Graph.AdjList[currentNode]) //lemásoljuk a listát
                    {
                        templist.Add(arc);
                    }
                    while (templist.Count > 0) //végigmegyünk a templisten
                    {
                        GraphArc minWeightArc = new GraphArc(GraphNode.Empty, GraphNode.Empty, int.MaxValue);
                        foreach (GraphArc arc in templist)
                        {
                            if (arc.Weight < minWeightArc.Weight) //megkeressük a legkisebb súlyú élt
                            {
                                minWeightArc = arc;
                            }
                        }
                        int pathToCurrentNodeWeight;
                        int.TryParse(_ShortestPath[currentNode], out pathToCurrentNodeWeight);
                        if (_ShortestPath[minWeightArc.Target] == "inf") //ha a cél csúcshoz nincs még út, ez biztosan javító él
                        {
                            _ShortestPath[minWeightArc.Target] =
                                (minWeightArc.Weight + pathToCurrentNodeWeight).ToString(); //a legrövidebb utakat tartalmazó tömbjöz hozzáadjuk a csúcsot és a hozzá tartozó súlyt
                            _PreviousNode.Add(minWeightArc.Target, currentNode); //eltároljuk az előző csúcsot
                            _ShortestPathWithArc.Add(minWeightArc.Target, minWeightArc); //eltároljuk az ide vezető utat
                        }
                        else // már van ide egy út
                        {
                            int pathToTargetNodeWeight; 
                            int.TryParse(_ShortestPath[minWeightArc.Target], out pathToTargetNodeWeight);
                            if (pathToTargetNodeWeight > minWeightArc.Weight + pathToCurrentNodeWeight) //ha ez az út hosszabb, mint a mostani
                            {
                                _ShortestPath[minWeightArc.Target] =
                                    (minWeightArc.Weight + pathToCurrentNodeWeight).ToString(); // beállítjuk a mostani csúcsig vezető út súlyát + az él súlyát
                                if (_PreviousNode.ContainsKey(minWeightArc.Target))
                                {
                                    _PreviousNode.Remove(minWeightArc.Target);
                                }
                                _PreviousNode.Add(minWeightArc.Target, currentNode);//eltároljuk az előző csúcsot
                                if (_ShortestPathWithArc.ContainsKey(minWeightArc.Target))
                                {
                                    _ShortestPathWithArc.Remove(minWeightArc.Target);
                                }
                                _ShortestPathWithArc.Add(minWeightArc.Target, minWeightArc);//eltároljuk az ide vezető utat
                            }
                        }
                        if (!_NodeQueue.Contains(minWeightArc.Target))
                        {
                            _NodeQueue.Enqueue(minWeightArc.Target); //ha még nincs bent a cél csúcs a vizsgálandók között, felvesszük
                        }
                        templist.Remove(minWeightArc); //a jelenlegi legröviebb élt kidobjuk  a templistből, hogy a következő legrövidebbel lehessen foglalkozni
                    }
                    if (!_FinishedNodes.Contains(currentNode))
                    {
                        _FinishedNodes.Add(currentNode); // felvesszük a csúcsot a bejártak közé
                    }
                }
            }
        }

        public override void Draw(Graphics gr)
        {
            Font font = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold);

            foreach (GraphNode node in _Graph.AdjList.Keys)
                foreach (GraphArc arc in _Graph.AdjList[node])
                {
                    if ((_NodeQueue.Peek() == arc.Source) && !_FinishedNodes.Contains(_NodeQueue.Peek()))
                        arc.Draw(gr, Color.Red, font);
                    else if (_ShortestPathWithArc.ContainsValue(arc))
                        arc.Draw(gr, Color.Green, font);
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
                    {
                        node.Draw(gr, Color.Red, font);
                        gr.DrawString(_ShortestPath[node], font,Brushes.Red,node.Location.X,node.Location.Y+15);
                    }
                    else if (_NodeQueue.Contains(node))
                    {
                        node.Draw(gr, Color.Green, font);
                        gr.DrawString(_ShortestPath[node], font, Brushes.Green, node.Location.X, node.Location.Y + 15);
                    }
                    else if (_FinishedNodes.Contains(node))
                    {
                        node.Draw(gr, Color.Blue, font);
                        gr.DrawString(_ShortestPath[node], font, Brushes.Blue, node.Location.X, node.Location.Y + 15);
                    }
                    else
                    {
                        node.Draw(gr, Color.Black, font);
                        gr.DrawString(_ShortestPath[node], font, Brushes.Black
                            , node.Location.X, node.Location.Y + 15);
                    }
                }
            }
        }
    }
}
