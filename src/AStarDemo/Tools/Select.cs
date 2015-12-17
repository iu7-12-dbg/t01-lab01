using System;
using System.Drawing;
using Core.Math;

namespace AStarDemo.Tools
{
    internal class Select : ITool
    {
        public ToolId Id { get { return ToolId.Select; } }
        public Bitmap Icons { get { return Properties.Resources.Select; } }
        
        public void Begin(Vector2 pos)
        {
            // XXX: save start pos
        }
        public void Update(Vector2 pos)
        {
            // XXX: draw selection box
        }
        public void End(Vector2 pos)
        {
            // XXX: get objects inside box and mark them as selected
        }
        public void Draw(Renderer r, Graphics g)
        {
            // XXX: draw tool
        }
    }
}
