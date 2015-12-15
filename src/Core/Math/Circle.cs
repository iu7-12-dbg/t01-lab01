namespace Core.Math
{
    public struct Circle
    {
        public Vector2 Center;
        public double Radius;

        public Circle(Vector2 center, double radius)
        {
            Center = center;
            Radius = radius;
        }
    }
}
