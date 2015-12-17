using System;
using System.Drawing;

namespace AStarDemo.Tools
{
    internal class Add : ITool
    {
        public ToolId Id { get { return ToolId.Add; } }
        public Bitmap Icons { get { return Properties.Resources.Add; } }

        public void Begin(Graphics g, Point pos)
        {
            // XXX: add & capture object
        }
        public void Update(Graphics g, Point pos)
        {
            // XXX: move object
        }
        public void End()
        {
            // XXX: release object
        }
    }
}
