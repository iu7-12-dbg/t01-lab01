using System;
using System.Drawing;
using Core.Math;

namespace AStarDemo.SceneObjects
{
    internal class Background : SceneObject
    {
        public Background(Color color)
        {
            Color = color;
            ZOrder = SceneObjectConstants.BackgroundZOrder;
            GroupId = -1;
        }

        public override void Draw(Renderer renderer, Graphics graphics)
        { graphics.Clear(Color); }

        public override Box2 GetBBox()
        { return Box2.Empty; }
    }
}
