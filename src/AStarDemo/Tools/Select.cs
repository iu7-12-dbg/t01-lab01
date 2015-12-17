using System;
using System.Drawing;
using Core.Math;

namespace AStarDemo.Tools
{
    internal class Select : ITool
    {
        public ToolId Id { get { return ToolId.Select; } }
        public Bitmap Icons { get { return Properties.Resources.Select; } }

        private bool active = false;
        private Vector2 startPos, endPos;
        private Pen dottedPen;

        public Select()
        {
            // XXX: dispose!
            dottedPen = new Pen(Root.Scene.Colors.SelectionBox);
            dottedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        }

        public void Begin(Vector2 pos)
        {
            active = true; // show selection box
            startPos = endPos = pos;
        }
        public void Update(Vector2 pos)
        { endPos = pos; }
        public void End(Vector2 pos)
        {
            active = false; // hide selection box
            // XXX: get objects inside box and mark them as selected
        }
        public void Draw(Renderer r, Graphics g)
        {
            if (!active)
                return;
            var invScale = 1/r.Scale;
            dottedPen.Width = (float)invScale;
            var start = startPos;
            var end = endPos;
            var x = (float)Math.Min(start.X, end.X);
            var y = (float)Math.Min(start.Y, end.Y);
            var diff = startPos-endPos;
            var w = (float)Math.Abs(diff.X);
            var h = (float)Math.Abs(diff.Y);
            g.DrawRectangle(dottedPen, x, y, w, h);
        }
    }
}
