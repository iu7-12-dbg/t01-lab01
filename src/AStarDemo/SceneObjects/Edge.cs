using System;
using System.Drawing;
using Core.Math;
using MEdge = Core.Math.Edge;

namespace AStarDemo.SceneObjects
{
    internal class Edge : SceneObject
    {
        public Vector2 A;
        public Vector2 B;
        public float Thickness;

        public Edge(MEdge edge)
        {
            Color = Color.CornflowerBlue;
            Thickness = 1.0F;
            A = edge.A;
            B = edge.B;
            ZOrder = SceneObjectConstants.EdgeZOrder;
        }

        public override void Draw(Renderer renderer, Graphics graphics)
        {
            //renderer.Scale
            var pen = new Pen(Selected ? Color.Red : Color, Thickness);
            graphics.DrawLine(pen, (float)A.X, (float)A.Y, (float)B.X, (float)B.Y);
            pen.Dispose();
        }

        public override Box2 GetBBox()
        {
            var box = Box2.Empty;
            box.Merge(A);
            box.Merge(B);
            box.Grow(Thickness/2);
            return box;
        }

        public static implicit operator MEdge(Edge src)
        { return new MEdge(src.A, src.B); }
    }
}
