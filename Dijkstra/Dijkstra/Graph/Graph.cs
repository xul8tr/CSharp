using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Dijkstra.Graph
{
    class Graph
    {
        private int nodeCount;
        private int arcCount;
        private string errorMessage;
        public Dictionary<int, Point> Nodes;
        public Dictionary<int, Dictionary<int,int>> Arcs;
        public int MaxX;
        public int MaxY;
        private List<string> fileContent;
        
        public bool LoadGraph()
        {
            bool success;
            FileHandling.FileHandling myFileHandling = new FileHandling.FileHandling();
            string fileToOpen = myFileHandling.Open();
            StreamReader myStreamReader = new StreamReader(fileToOpen);
            fileContent = new List<string>();
            Nodes = new Dictionary<int, Point>();
            Arcs = new Dictionary<int, Dictionary<int,int>>();
            while (!myStreamReader.EndOfStream)
            {
                fileContent.Add(myStreamReader.ReadLine());
            }
            success = CheckFormat();
            success = success && GetNodes();
            success = success && GetArcs();
            if (!success)
            {
                DisplayError();
            }
            return success;
        }

        private void DisplayError()
        {
            MainForm.DisplayError(errorMessage);
        }

        private bool GetArcs()
        {
            bool success;
            try
            {
                success = true;
                for (int i = 1 + nodeCount; i <= nodeCount + arcCount && success; i++)
                {
                    string[] actualRow = fileContent[i].Split(new char[] { ' ' });
                    int arcStartPoint;
                    int arcEndPoint = 0;
                    int arcWeight = 0;
                    success = int.TryParse(actualRow[0], out arcStartPoint);
                    success = success && int.TryParse(actualRow[1], out arcEndPoint);
                    success = success && int.TryParse(actualRow[2], out arcWeight);
                    success = success && arcWeight >= 0;
                    //Point location = new Point(arcStartPoint, arcEndPoint);
                    //success = success && !Arcs.ContainsKey(location);
                    //if (success)
                    //{
                    //    Arcs.Add(location, arcWeight);
                    //}
                    //else
                    //{
                    //    errorMessage = "File error in line: " + (i + 1);
                    //}
                    Dictionary<int, int> myDict;
                    if (success)
                    {
                        if (Arcs.ContainsKey(arcStartPoint))
                        {
                            myDict = Arcs[arcStartPoint];
                        }
                        else
                        {
                            myDict = new Dictionary<int, int>();
                            Arcs.Add(arcStartPoint,myDict);
                        }
                        myDict.Add(arcEndPoint,arcWeight);
                    }
                    else
                    {
                        errorMessage = "File error in line: " + (i + 1);
                    }
                }
            }
            catch (Exception exception)
            {
                errorMessage = "Exception: "+exception.Message;
                success = false;
            }
            return success;
        }

        private bool GetNodes()
        {
            bool success;
            try
            {
                success = true;
                for (int i = 1; i <= nodeCount; i++)
                {
                    string[] actualRow = fileContent[i].Split(new char[] {' '});
                    int nodeName; 
                    int locationX = 0;
                    int locationY = 0;
                    success = int.TryParse(actualRow[0], out nodeName);
                    success = success && int.TryParse(actualRow[1], out locationX);
                    success = success && int.TryParse(actualRow[2], out locationY);
                    success = success && !Nodes.ContainsKey(nodeName);
                    Point location = new Point(locationX, locationY);
                    if (success)
                    {
                        Nodes.Add(nodeName, location);
                        if (locationX>MaxX)
                        {
                            MaxX = locationX;
                        }
                        if(locationY>MaxY)
                        {
                            MaxY = locationY;
                        }
                    }
                    else
                    {
                        errorMessage = "File error in line: " + (i + 1);
                    }
                }
            }
            catch (Exception exception)
            {
                errorMessage = "Exception: " + exception.Message;
                success = false;
            }
            return success;
        }

        private bool CheckFormat()
        {
            bool success;
            if (fileContent.Count <= 0)
            {
                errorMessage = "The file is empty.";
                return false;
            }
            string[] firstRow = fileContent[0].Split(new char[] {' '});
            success = int.TryParse(firstRow[0], out nodeCount);
            success = success && int.TryParse(firstRow[1], out arcCount);
            int sum = nodeCount + arcCount;
            success = success && fileContent.Count == sum + 1;
            if (!success)
            {
                errorMessage = "Error on the first line.";
            }
            return success;
        }
    }
}
