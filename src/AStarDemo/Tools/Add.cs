using System;
using System.Drawing;
using Core.Math;

namespace AStarDemo.Tools
{
    internal class Add : ITool
    {
        public ToolId Id { get { return ToolId.Add; } }
        public Bitmap Icons { get { return Properties.Resources.Add; } }

        public void Begin(Vector2 pos)
        {
            // XXX: add & capture object
        }
        public void Update(Vector2 pos)
        {
            // XXX: move object
        }
        public void End(Vector2 pos)
        {
            // XXX: release object
        }
        public void Draw(Renderer r, Graphics g)
        {
            // XXX: draw tool
        }
    }
}
