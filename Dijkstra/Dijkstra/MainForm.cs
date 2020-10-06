using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace Dijkstra
{
    public partial class MainForm : Form
    {
        private Graph.Graph myGraph;
        private bool graphLoaded;
        private Bitmap myBitmap;
        private int nodeSize = 20;
        public MainForm()
        {
            InitializeComponent();
            myGraph = new Graph.Graph();
            //myBitmap = new Bitmap();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myGraph.LoadGraph())
            {
                graphLoaded = true;
                DrawGraph();
            }
        }

        static public void DisplayError(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Dijkstra - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void DrawGraph()
        {
            if (graphLoaded)
            {
                Graphics myGraphics = splitContainer.Panel1.CreateGraphics();
                Brush myBrush = new SolidBrush(Color.Blue);
                Pen myPen = new Pen(myBrush, 3f);
                //splitContainer.Panel1.BackColor = Color.White;
                foreach (KeyValuePair<int, Point> node in myGraph.Nodes)
                {
                    myGraphics.DrawString(node.Key.ToString(), Font, Brushes.Black, node.Value.X + nodeSize / 4, node.Value.Y + nodeSize / 4);
                    myGraphics.DrawEllipse(myPen, node.Value.X, node.Value.Y, nodeSize, nodeSize);
                }
                foreach (KeyValuePair<int, Dictionary<int, int>> arc in myGraph.Arcs)
                {
                    int arcStartPointX = myGraph.Nodes[arc.Key].X;
                    int arcStartPointY = myGraph.Nodes[arc.Key].Y;
                    foreach (KeyValuePair<int, int> pair in arc.Value)
                    {
                        int arcEndPointX = myGraph.Nodes[pair.Key].X;
                        int arcEndPointY = myGraph.Nodes[pair.Key].Y;
                        myGraphics.DrawLine(myPen, arcStartPointX, arcStartPointY, arcEndPointX, arcEndPointY);
                    }
                }
                myGraphics.Dispose();
                myBrush.Dispose();
                myPen.Dispose();
            }
        }

        private void RedrawRequest(object sender, EventArgs e)
        {
            DrawGraph();
        }
    }
}
