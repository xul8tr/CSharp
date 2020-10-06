using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BombermanBasics;
using System.Drawing;

namespace Bomberman
{
    public class Field : BombermanBasics.Field
    {
        internal delegate void SimpleEvent();
        internal delegate void HighlightEvent(System.Windows.Media.Color color);
        internal event SimpleEvent OnDraw;
        internal event HighlightEvent OnHighlighted;
        internal event SimpleEvent OnUnHighlighted;

        public Field(int teamCount, int x, int y) : base(false, teamCount, x, y)
        {
        }

        internal void Draw()
        {
            OnDraw();
        }

        internal void Highlight(System.Windows.Media.Color color)
        {
            if (OnHighlighted != null)
            {
                OnHighlighted(color);
            }
        }

        internal void UnHighlight()
        {
            if (OnUnHighlighted != null)
            {
                OnUnHighlighted();
            }
        }
    }
}
