﻿namespace Core.Math
{
    using SysMath = System.Math;

    public struct Box2
    {
        public Vector2 Min;
        public Vector2 Max;
        
        public Box2(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }

        public Box2(Vector2 center, double radius)
        {
            var r = new Vector2(radius, radius);
            Min = center-r;
            Max = center+r;
        }

        public void Set(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }

        public double X1
        {
            get { return Min.X; }
            set { Min.X = value; }
        }
        public double X2
        {
            get { return Max.X; }
            set { Max.X = value; }
        }
        public double Y1
        {
            get { return Min.Y; }
            set { Min.Y = value; }
        }
        public double Y2
        {
            get { return Max.Y; }
            set { Max.Y = value; }
        }

        public bool Contains(double x, double y)
        { return Contains(new Vector2(x, y)); }

        public bool Contains(Vector2 v)
        { return Min.X<=v.X && v.X<=Max.X && Min.Y<=v.Y && v.Y<=Max.Y; }

        public bool Contains(Box2 b)
        { return Contains(b.Min) && Contains(b.Max); }

        public void Merge(Vector2 p)
        {
            Min.Set(SysMath.Min(Min.X, p.X), SysMath.Min(Min.Y, p.Y));
            Max.Set(SysMath.Max(Max.X, p.X), SysMath.Max(Max.Y, p.Y));
        }

        public void Merge(Box2 b)
        {
            Merge(b.Min);
            Merge(b.Max);
        }

        public void Shrink(double s)
        {
            Min += s;
            Max -= s;
        }

        public void Shrink(Vector2 s)
        {
            Min += s;
            Max -= s;
        }

        public void Grow(double s)
        {
            Min -= s;
            Max += s;
        }

        public void Grow(Vector2 s)
        {
            Min -= s;
            Max += s;
        }

        public static bool operator==(Box2 lhs, Box2 rhs)
        { return lhs.Min==rhs.Min && lhs.Max==rhs.Max; }

        public static bool operator!=(Box2 lhs, Box2 rhs)
        { return lhs.Min!=rhs.Min || lhs.Max!=rhs.Max; }
        
        public override int GetHashCode()
        { return unchecked(Min.GetHashCode()+Max.GetHashCode()); }

        public override bool Equals(object other)
        {
            if (other is Box2)
            {
                var otherBox = (Box2)other;
                return otherBox==this;
            }
            return false;
        }
        
        public bool Equals(Box2 other)
        { return other==this; }

        public bool IsEmpty { get { return this==Empty; } }
        
        public static readonly Box2 Empty = new Box2(Vector2.MaxValue, Vector2.MinValue);
    }
}
