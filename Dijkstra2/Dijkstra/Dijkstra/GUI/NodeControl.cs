using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Dijkstra.GUI
{
    class NodeControl : Control
    {
        private Color m_DrawColor = Color.Blue;
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle drawRectangle = new Rectangle(1, 1, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2);
            Pen myPen = new Pen(m_DrawColor, 2f);
            e.Graphics.DrawEllipse(myPen, drawRectangle);
            myPen.Dispose();
        }
        protected override void OnClick(EventArgs e)
        {
            m_DrawColor = Color.Red;
            this.Invalidate();
        }

        protected override void OnMouseHover(EventArgs e)
        {
            this.Location = PointToClient(Control.MousePosition);
            base.OnMouseHover(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            //m_DrawColor = Color.Blue;
            //this.Invalidate();
            base.OnLostFocus(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            //this.Capture = true;
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            //if (this.Capture)
            //{
            //    this.Location = e.Location;
            //}
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            //this.Capture = false;
            base.OnMouseUp(e);
        }
        
        protected override void OnCreateControl()
        {
            
            base.OnCreateControl();
        }
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}