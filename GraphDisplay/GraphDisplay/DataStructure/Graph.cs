using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ELTE.Algorithms.GraphDisplay.DataStructure
{
    /// <summary>
    /// Gráf adatszerkezet
    /// </summary>
    public class Graph
    {
        private Int32 _NextNumber; // következő csúcsszám
        private Dictionary<GraphNode, List<GraphArc>> _Adj; 
        // szomszédsági lista, amit kulcs-érték párokkal hozunk létre, a kulcs a csúcs
        private GraphNode _SelectedNode; // kiválasztott csúcs
        private GraphNode _PreviousNode; // kiválasztott csúcs
        private GraphArc _SelectedArc; // kiválasztott él

        /// <summary>
        /// Előző csúcs
        /// </summary>
        public GraphNode PreviousNode
        {
            get
            {
                return _PreviousNode;
            }
        }
        /// <summary>
        /// Van-e csúcs kiválasztva
        /// </summary>
        public Boolean NodeSelected
        {
            get { return (_SelectedNode != null); }
        }
        /// <summary>
        /// Van-e él kiválasztva
        /// </summary>
        public Boolean ArcSelected
        {
            get { return (_SelectedArc != null); }
        }
        /// <summary>
        /// Csúcsok száma
        /// </summary>
        public Int32 NodeCount
        {
            get { return _Adj.Count; }
        }
        /// <summary>
        /// Élek száma
        /// </summary>
        public Int32 ArcCount
        {
            get 
            { 
                // végig megyünk az éllistákon, és összeadjuk a hosszukat
                Int32 arcCount = 0;
                foreach(GraphNode node in _Adj.Keys)
                    arcCount += _Adj[node].Count;
                return arcCount;
            }
        }
        /// <summary>
        /// Kiválasztott csúcs
        /// </summary>
        public GraphNode SelectedNode
        {
            get { return _SelectedNode; }
        }
        /// <summary>
        /// Éllista
        /// </summary>
        public Dictionary<GraphNode, List<GraphArc>> AdjList
        {
            get { return _Adj; }
        }

        /// <summary>
        /// Új, üres gráf létrehozása
        /// </summary>
        public Graph()
        {
            // üres gráf esetén mindent kiürítünk
            _Adj = new Dictionary<GraphNode, List<GraphArc>>();
            _NextNumber = 1; // az első szám az 1-es lesz
            _SelectedNode = null;
            _SelectedArc = null;
        }

        /// <summary>
        /// Gráf törlése
        /// </summary>
        public void Clear()
        {
            _Adj.Clear();
            _NextNumber = 1;
            _SelectedNode = null;
            _SelectedArc = null;
        }
        /// <summary>
        /// Új csúcs felvétele
        /// </summary>
        /// <param name="location">A csúcs pozíciója a képernyőn</param>
        public void AddNode(Point location)
        {
            _SelectedArc = null;
            _SelectedNode = new GraphNode(_NextNumber, location); // rögtön az új csúcs lesz kiválasztva
            _Adj.Add(_SelectedNode, new List<GraphArc>()); // felvesszük az új csúcsot az éllistába
            _NextNumber++;
        }
        /// <summary>
        /// Új csúcs felvétele
        /// </summary>
        /// <param name="number">A csúcs sorszáma</param>
        /// <param name="location">A csúcs pozíciója a képernyőn</param>
        public void AddNode(Int32 number, Point location)
        {
            // előbb megkeressük, hogy olyan sorszámú csúcs létezik-e már
            Boolean contains = false;
            foreach (GraphNode node in _Adj.Keys)
                if (node.Number == number)
                    contains = true;
            // ha nem létezik, csak akkor vesszük fel
            if (!contains)
            {
                _SelectedArc = null;
                _SelectedNode = new GraphNode(number, location);
                _Adj.Add(_SelectedNode, new List<GraphArc>());
                _NextNumber = number + 1;
            }
        }
        /// <summary>
        /// Csúcs kiválasztása
        /// </summary>
        /// <param name="location">A csúcs pozíciója a képernyőn</param>
        public void SelectNode(Point location)
        {
            //letároljuk az előző Nodeot
            _PreviousNode = _SelectedNode;
            // minden új kiválasztásnál töröljük az eddigi kiválasztásokat
            _SelectedNode = null;
            _SelectedArc = null;
            // megkeressük, hogy van-e csúcs a megadott pozíció környékén
            foreach (GraphNode node in _Adj.Keys)
            {
                if (node.Near(location))
                {
                    _SelectedNode = node; // ha van, akkor megjelöljük
                }
            }
            if (_SelectedNode == null || _SelectedNode == _PreviousNode) //ha nem sikerült másik csúcsot találni, akkor az előző nodeot sincs értelme tárolni.
            {
                _PreviousNode = null;
            }
        }
        /// <summary>
        /// Kiválasztott csúcs elmozgatása
        /// </summary>
        /// <param name="targetLocation">A csúcs új pozíciója a képernyőn</param>
        public void MoveNode(Point targetLocation)
        {
            if (_SelectedNode != null)
                _SelectedNode.Location = targetLocation;
        }
        
        /// <summary>
        /// Kiválasztott csúcs törlése
        /// </summary>
        public void RemoveNode()
        {
            RemoveNode(_SelectedNode); // meghívjuk a törlést a kiválasztott csúcssal
        }
        
        /// <summary>
        /// Csúcs törlése
        /// </summary>
        /// <param name="removeKey">Törlendő csúcs</param>
        public void RemoveNode(GraphNode removeKey)
        {
            // ha a kiválasztott csúcsot töröljük, akkor megszűntetjük a kiválasztást
            if (removeKey == _SelectedNode)
                _SelectedNode = null;

            // ha nem null értéket kaptunk törlésre
            if (removeKey != null)
            {
                _Adj.Remove(removeKey); // kitöröljük a csúcsot, és a belőle kiinduló éleket

                // utána még ki kell törölni azon éleket, amik bele vezetnek
                foreach (GraphNode key in _Adj.Keys)
                {
                    // végigmegyünk minden éllistán
                    for (Int32 j = 0; j < _Adj[key].Count; j++)
                    {
                        if (_Adj[key][j].Target == removeKey)
                        {
                            if (_Adj[key][j] == _SelectedArc)
                                _SelectedArc = null;
                            // ha az adott él van kiválasztva, akkor megszűntetjük a kiválasztást

                            _Adj[key].RemoveAt(j); // törlünk az adott pozíción
                            j--; // mivel ekkor visszalép a lista, ezért vissza kell vennünk az indexből
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Él felvétele
        /// </summary>
        /// <param name="fromLocation">Kiinduló pozíció</param>
        /// <param name="toLocation">Cél pozíció</param>
        /// <param name="weight">Él súlya</param>
        public void AddArc(Point fromLocation, Point toLocation, Int32 weight)
        {
            // meg kell keresnünk, hogy melyik csúcsból melyik csúcsba vesszük fel az élt
            GraphNode fromKey = null, toKey = null;
            foreach (GraphNode key in _Adj.Keys)
            {
                if (key.Near(fromLocation))
                    fromKey = key;
                if (key.Near(toLocation))
                    toKey = key;
            }
            AddArc(fromKey, toKey, weight);
        }
        /// <summary>
        /// Él felvétele
        /// </summary>
        /// <param name="fromNumber">Kiinduló csúcs száma</param>
        /// <param name="toNumber">Cél csúcs száma</param>
        /// <param name="weight">Él súlya</param>
        public void AddArc(Int32 fromNumber, Int32 toNumber, Int32 weight)
        {
            GraphNode fromKey = null, toKey = null;
            // meg kell keresnünk, hogy melyik csúcsból melyik csúcsba vesszük fel az élt
            foreach (GraphNode key in _Adj.Keys)
            {
                if (key.Number == fromNumber)
                    fromKey = key;
                if (key.Number == toNumber)
                    toKey = key;
            }
            AddArc(fromKey, toKey, weight);
        }
        /// <summary>
        /// Él felvétele
        /// </summary>
        /// <param name="fromKey">Kiinduló csúcs</param>
        /// <param name="toKey">Cél csúcs</param>
        /// <param name="weight">Él súlya</param>
        public void AddArc(GraphNode fromKey, GraphNode toKey, Int32 weight)
        {
            if (fromKey != null && toKey != null)
            {
                Boolean containsArc = false;
                // előbb ellenőrizzük, hogy nincs-e már él a két csúcs között ezzel az irányítással
                for (Int32 i = 0; i < _Adj[fromKey].Count && !containsArc; i++)
                {
                    if (_Adj[fromKey][i].Target == toKey)
                        containsArc = true;
                }
                // ha nincs, akkor felvesszük
                if (!containsArc)
                {
                    _SelectedNode = null;
                    _PreviousNode = null;
                    _SelectedArc = new GraphArc(fromKey, toKey, weight); // létrehozzuk az élet
                    _Adj[fromKey].Add(_SelectedArc); // felvesszük a megfelelő listába
                }
            }
        }
        /// <summary>
        /// Él kiválasztása
        /// </summary>
        /// <param name="location">Az él pozíciója a képernyőn</param>
        public void SelectArc(Point location)
        {
            _SelectedNode = null;
            _SelectedArc = null;
            foreach (GraphNode node in _Adj.Keys)
                foreach (GraphArc arc in _Adj[node])
                {
                    if (arc.Near(location))
                    {
                        _SelectedArc = arc; // ha van él az adott pozíció környékén, akkor felvesszük
                        _PreviousNode = null;
                    }
                }
        }
        /// <summary>
        /// Kiválasztott él súlyának módosítása
        /// </summary>
        /// <param name="weightModifier">Súlyérték módosítás mértéke</param>
        public void ModifyArc(Int32 weightModifier)
        {
            if (_SelectedArc != null)
            {
                _SelectedArc.Weight += weightModifier;
                if (_SelectedArc.Weight < 0)
                {
                    _SelectedArc.Weight = 0;
                }
            }
        }
        
       
       
        /// <summary>
        /// Kiválasztott él törlése
        /// </summary>
        public void RemoveArc()
        { 
            if (_SelectedArc != null)
            {
                _Adj[_SelectedArc.Source].Remove(_SelectedArc);
                _SelectedArc = null;
            }   
        }

        /// <summary>
        /// Gráf kirajzolása
        /// </summary>
        /// <param name="gr"></param>
        public void Draw(Graphics gr)
        {
            Font font = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold);

            foreach (GraphNode node in _Adj.Keys)
                foreach (GraphArc arc in _Adj[node])
                {
                    // ha ki van választva, akkor pirossal, különben kékkel rajzolunk
                    if (arc == _SelectedArc)
                        arc.Draw(gr, Color.Red, font);
                    else
                        arc.Draw(gr, Color.Blue, font);
                }

            foreach (GraphNode node in _Adj.Keys)
            {
                // ha ki van választva, akkor pirossal,
                if (node == _SelectedNode)
                    node.Draw(gr, Color.Red, font);
                // ha előzőleg ki volt választva, akkor zölddel,
                else if (node == _PreviousNode)
                    node.Draw(gr, Color.Green, font);
                // különben kékkel rajzolunk
                else
                    node.Draw(gr, Color.Blue, font);
            }
        }
        /// <summary>
        /// Gráf betöltése fájlból
        /// </summary>
        /// <param name="fileName">Fájlnév</param>
        /// <returns>Sikeres volt-e a betöltés</returns>
        public Boolean Load(String fileName)
        {
            try
            {
                Clear(); //  kitöröljük az eddigi gráfot

                StreamReader sr = new StreamReader(fileName);
                String[] line = sr.ReadLine().Split(' '); // a szóközök mentén tagoljuk az adatokat                
                Int32 nodeCount = Int32.Parse(line[0]);
                Int32 arcCount = Int32.Parse(line[1]);
                for (Int32 i = 0; i < nodeCount; i++)
                {
                    line = sr.ReadLine().Split(' ');
                    AddNode(Int32.Parse(line[0]), new Point(Int32.Parse(line[1]), Int32.Parse(line[2])));
                }
                for (Int32 i = 0; i < arcCount; i++)
                {
                    line = sr.ReadLine().Split(' ');
                    AddArc(Int32.Parse(line[0]), Int32.Parse(line[1]), Int32.Parse(line[2]));
                }
                sr.Close();
                return true;
            }
            catch (IOException) // ha közben bármi hiba lép fel, akkor hamisat adunk vissza
            {
                return false;
            }
        }
        /// <summary>
        /// Gráf mentése fájlba
        /// </summary>
        /// <param name="fileName">Fájlnév</param>
        /// <returns>Sikeres volt-e a mentés</returns>
        public Boolean Save(String fileName)
        {
            try
            {
                StreamWriter sw = new StreamWriter(fileName);
                sw.WriteLine(NodeCount + " " + ArcCount);
                foreach(GraphNode node in _Adj.Keys)
                {
                    sw.WriteLine(node.Number + " " + node.Location.X + " " + node.Location.Y);
                }
                foreach (GraphNode node in _Adj.Keys)
                    foreach (GraphArc arc in _Adj[node])
                    {
                        sw.WriteLine(arc.Source.Number + " " + arc.Target.Number + " " + arc.Weight);
                    }
                sw.Close();
                return true;
            }
            catch (IOException) // ha közben bármi hiba lép fel, akkor hamisat adunk vissza
            {
                return false;
            }
        }
    }
}
