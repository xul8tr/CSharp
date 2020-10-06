using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dijkstra.GUI
{
    public partial class MainForm : Form
    {
        private int NodeSize = 30;
        public MainForm()
        {
            InitializeComponent();
        }

        private void m_DrawPanel_Click(object sender, EventArgs e)
        {
            
            MouseEventArgs eventArgs = e as MouseEventArgs;
            if (eventArgs != null)
            {
                int locationX = eventArgs.Location.X;
                int locationY = eventArgs.Location.Y;
                if ((locationX - NodeSize > 0) &&
                    (locationX + NodeSize*1.5 < m_DrawPanel.Width) &&
                    (locationY - NodeSize > 0) &&
                    (locationY + NodeSize * 1.5 < m_DrawPanel.Height))
                {
                    locationX = locationX - NodeSize/2;
                    locationY = locationY - NodeSize/2;
                    Control myNode = new NodeControl();
                    myNode.Size = new Size(NodeSize, NodeSize);
                    myNode.Location = new Point(locationX, locationY);
                    m_DrawPanel.Controls.Add(myNode);
                }
            }
            //myButton.Location = e
        }
    }
}
