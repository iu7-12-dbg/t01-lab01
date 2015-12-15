using System;
using System.Drawing;

namespace Core.Math
{
    public struct Rectangle
    {
        private Vector2 size;
        private Matrix23 transform;

        public Rectangle(double width, double height)
        {
            size = new Vector2(width, height);
            transform = Matrix23.Identity;
        }

        public Rectangle(Vector2 size) :
            this(size.X, size.Y)
        {}

        public Vector2 Offset
        {
            get { return transform.Offset; }
            set { transform.Offset = value; }
        }

        public Angle Turn
        {
            get { return transform.Turn; }
            set { transform.Turn = value; }
        }

        public Vector2 Scale
        {
            get { return transform.Scale; }
            set { transform.Scale = value; }
        }

        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }

        public Vector2 this[int i]
        {
            get
            {
                switch (i)
                {
                case 0: return transform * new Vector2(+size.X/2, +size.Y/2);
                case 1: return transform * new Vector2(-size.X/2, +size.Y/2);
                case 2: return transform * new Vector2(-size.X/2, -size.Y/2);
                case 3: return transform * new Vector2(+size.X/2, -size.Y/2);
                default: throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        public Vector2[] ToArrayOfVector2()
        { return new Vector2[] {this[0], this[1], this[2], this[3]}; }

        public PointF[] ToArrayOfPointF()
        { return new PointF[] {this[0], this[1], this[2], this[3]}; }

        /// <summary>
        /// Calculates distance from observer to each rectangle vertex and angles of rectangle vertices
        /// relative to the vector connecting the observer with the center of rectangle.
        /// </summary>
        /// <param name="observer">Observer coordinates</param>
        /// <returns></returns>
        public RectangleAngularSizes GetAngularSizes(Vector2 observer)
        {
            var rect = this;
            // observer must be located outside of the rectangle
            var sizes = RectangleAngularSizes.Default;
            sizes.CornerVisible = true;
            rect.Offset -= observer;
            var pts = new Vector2[4];
            for (var i = 0; i<4; i++)
                pts[i] = rect[i];
            var offset = rect.Offset;
            var frontCornerDist = offset.Length;
            // compare angles
            for (var i = 0; i<4; i++)
            {
                var relAngle = offset.RelativeAngle(pts[i]);
                if (relAngle>0)
                {
                    if (relAngle>sizes.LeftAngle)
                    {
                        sizes.Left = pts[i];
                        sizes.LeftAngle = relAngle;
                    }
                }
                else
                {
                    if (relAngle<sizes.RightAngle)
                    {
                        sizes.Right = pts[i];
                        sizes.RightAngle = relAngle;
                    }
                }
                var dist = pts[i].Length;
                if (dist<frontCornerDist)
                {
                    sizes.Corner = pts[i];
                    sizes.CornerAngle = relAngle;
                    frontCornerDist = dist;
                }
            }
            if (Angle.FromRadians(sizes.CornerAngle-sizes.LeftAngle)==0 ||
                Angle.FromRadians(sizes.CornerAngle-sizes.RightAngle)==0)
            {
                sizes.CornerVisible = false;
            }
            return sizes;
        }
    }

    public struct RectangleAngularSizes
    {
        public double LeftAngle;
        public double RightAngle;
        public double CornerAngle;
        public Vector2 Left;
        public Vector2 Right;
        public Vector2 Corner;
        public bool CornerVisible;

        public RectangleAngularSizes(double leftAngle, Vector2 left, double rightAngle, Vector2 right,
            double cornerAngle, Vector2 corner, bool cornerVisible)
        {
            LeftAngle = leftAngle;
            RightAngle = rightAngle;
            CornerAngle = cornerAngle;
            Left = left;
            Right = right;
            Corner = corner;
            CornerVisible = cornerVisible;
        }

        public static readonly RectangleAngularSizes Default = new RectangleAngularSizes(
            0, Vector2.UnitX, 0, Vector2.UnitX, 0, Vector2.UnitX, false);
    }
}
