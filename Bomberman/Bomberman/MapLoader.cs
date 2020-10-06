using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Drawing;

namespace Bomberman
{
    /// <summary>
    /// Class that handles configuration and stores it in an XML file (config.xml)
    /// </summary>
    public class MapLoader
    {
        public int MapSize
        {
            get
            {
                return int.Parse(document.Root.Element("mapSize").Value);
            }
        }

        public List<Point> ListOfMines
        {
            get
            {
                return document.Root.Element("mines").Elements("mine").Select(x => new Point(int.Parse(x.Element("x").Value), int.Parse(x.Element("y").Value))).ToList();
            }
        }

        private XDocument document;

        public MapLoader(string mapPath)
        {
            try
            {
                document = XDocument.Load(mapPath);
            }
            catch (Exception e)
            {
                throw new Exception("Could not load map file (" + mapPath + "), exceptionMessage:" + e.Message);
            }
        }
    }
}
