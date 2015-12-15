using System;

namespace Core.Math
{
    public struct Edge
    {
        public Vector2 A;
        public Vector2 B;

        public Edge(Vector2 a, Vector2 b)
        {
            A = a;
            B = b;
        }

        public Vector2 this[int i]
        {
            get
            {
                switch (i)
                {
                case 0: return A;
                case 1: return B;
                default: throw new ArgumentException();
                }
            }
            set
            {
                switch (i)
                {
                case 0:
                    A = value;
                    break;
                case 1:
                    B = value;
                    break;
                default:
                    throw new ArgumentException();
                }
            }
        }

        public double Length
        { get { return A.Distance(B); } }

        public double CrossingAngle(Edge edge)
        { return (A-B).CrossingAngle(edge.A-edge.B); }

        public double CrossingAngle(Vector2 vector)
        { return vector.CrossingAngle(A-B); }

        public bool Intersect(Edge edge)
        { return Intersect(this, edge); }

        public static bool Intersect(Edge first, Edge second)
        {
            VectorUtils.LineClockDir test1a, test1b, test2a, test2b;
            test1a = VectorUtils.GetClockPosition(first.A, first.B, second.A);
            test1b = VectorUtils.GetClockPosition(first.A, first.B, second.B);
            if (test1a!=test1b)
            {
                test2a = VectorUtils.GetClockPosition(second.A, second.B, first.A);
                test2b = VectorUtils.GetClockPosition(second.A, second.B, first.B);
                if (test2a!=test2b)
                    return true;
            }
            return false;
        }
        
        public static bool Intersect(Ray ray, double rayLength, Vector2 rectCenter, Rectangle rect)
        {
            var rayStart = Vector2.Rotate(Vector2.UnitX, ray.Direction.OriginAngle());
            var rayEnd = rayStart;
            rayEnd.Length = rayLength;
            var pts = new Vector2[4];
            rectCenter -= ray.Origin;
            for (var i = 0; i<4; ++i)
                pts[i] = rect[i]+rectCenter;
            return Intersect(new Edge(rayStart, rayEnd), new Edge(pts[0], pts[2])) ||
                Intersect(new Edge(rayStart, rayEnd), new Edge(pts[1], pts[3]));
        }
    }
}
