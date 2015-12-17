using System;
using System.Drawing;

namespace AStarDemo.Tools
{
    internal class Select : ITool
    {
        public ToolId Id { get { return ToolId.Select; } }
        public Bitmap Icons { get { return Properties.Resources.Select; } }
        public void Begin(Graphics g, Point pos)
        {
            // XXX: save start pos
        }
        public void Update(Graphics g, Point pos)
        {
            // XXX: draw selection box
        }
        public void End()
        {
            // XXX: get objects inside box and mark them as selected
        }
    }
}
