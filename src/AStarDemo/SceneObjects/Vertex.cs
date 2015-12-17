using System;
using System.Drawing;
using Core.Math;

namespace AStarDemo.SceneObjects
{
    internal class Vertex : SceneObject
    {
        public Vector2 Location;
        public float Radius;
        public bool ShowLocation;

        public Vertex()
        {
            Radius = 2.0f;
            Color = Color.Black;
            ZOrder = SceneObjectConstants.VertexZOrder;
            ShowLocation = true;
        }

        public override void Draw(Renderer renderer, Graphics graphics)
        {
            using (var brush = new SolidBrush(Selected ? Color.Red : Color))
            {
                var invScale = 1/renderer.Scale;
                var d = (float)(2*Radius*invScale);
                var r = d/2;
                graphics.FillEllipse(brush, (float)Location.X-r, (float)Location.Y-r, d, d);
                if (ShowLocation)
                {
                    var gTransform = graphics.Transform;
                    graphics.ResetTransform();
                    var myTransform = Matrix23.FromColumns(gTransform.Elements);
                    var pos = myTransform*(Location+d);
                    var str = Location.ToString();
                    var strRect = graphics.MeasureString(str, renderer.Font);
                    graphics.FillRectangle(renderer.FontBackgroundBrush, (float)pos.X, (float)pos.Y,
                        strRect.Width, strRect.Height);
                    graphics.DrawString(str, renderer.Font, Brushes.Gray, (float)pos.X, (float)pos.Y);
                    graphics.Flush();
                    graphics.Transform = gTransform;
                }
                brush.Dispose();
            }
        }

        public override Box2 GetBBox()
        { return new Box2(Location, Radius); }
    }
}
