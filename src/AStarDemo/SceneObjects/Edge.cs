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
        // because we don't have spatial index yet :(
        public Vertex VertexA;
        public Vertex VertexB;
        public float Thickness;
        private static Pen pen;
        private static long refCount = 0;
        
        public Edge(MEdge edge)
        {
            if (refCount++==0)
                pen = new Pen(Color.Black);
            Color = Root.Scene.Colors.TempEdge;
            Thickness = 2.0f;
            A = edge.A;
            B = edge.B;
            ZOrder = SceneObjectConstants.EdgeZOrder;
        }

        public override void Draw(Renderer renderer, Graphics graphics)
        {
            var invScale = 1/renderer.Scale;
            pen.Color = Selected ? Root.Scene.Colors.SelectedObject : Color;
            pen.Width = (float)(Thickness*invScale);
            graphics.DrawLine(pen, (float)A.X, (float)A.Y, (float)B.X, (float)B.Y);
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

        protected override void PureDispose()
        {
            if (--refCount==0)
            {
                pen.Dispose();
                pen = null;
            }
            base.PureDispose();
        }
    }
}
