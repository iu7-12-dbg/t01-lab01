namespace Core.Math
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
        
        bool Contains(double x, double y)
        { return Contains(new Vector2(x, y)); }

        bool Contains(Vector2 v)
        { return Min.X<=v.X && v.X<=Max.X && Min.Y<=v.Y && v.Y<=Max.Y; }

        bool Contains(Box2 b)
        { return Contains(b.Min) && Contains(b.Max); }

        void Include(Vector2 p)
        {
            Min.Set(SysMath.Min(Min.X, p.X), SysMath.Min(Min.Y, p.Y));
            Max.Set(SysMath.Max(Max.X, p.X), SysMath.Max(Max.Y, p.Y));
        }
    }
}
