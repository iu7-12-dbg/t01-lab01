namespace Core.Math
{
    public struct Ray
    {
        public Vector2 Origin;
        public Vector2 Direction;

        public Ray(Vector2 origin, Vector2 direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public double GetDistanceTo(Vector2 point)
        { return GetDistanceTo(this, point); }

        public static double GetDistanceTo(Ray ray, Vector2 point)
        {
            // get the vector from the raypoint to the position of the point
            var t = point-ray.Origin;
            // get the dot product of the ray direction and our temporary vector
            var dot = t.Dot(ray.Direction);
            // get the point on the line
            var line = ray.Origin+ray.Direction*dot;
            var dst = line-point;
            // return the distance
            return dst.Length;
        }
        
        public bool Intersect(Ray ray)
        { return Intersect(this, ray); }

        public static bool Intersect(Ray first, Ray second)
        {
            Vector2 dummy;
            return Intersect(first, second, out dummy);
        }

        public bool Intersect(Ray ray, out Vector2 intersectionPoint)
        { return Intersect(this, ray, out intersectionPoint); }

        public static bool Intersect(Ray first, Ray second, out Vector2 intersectionPoint)
        {
            var asx = first.Origin.X;
            var asy = first.Origin.Y;
            var adx = first.Direction.X;
            var ady = first.Direction.Y;
            var bsx = second.Origin.X;
            var bsy = second.Origin.Y;
            var bdx = second.Direction.X;
            var bdy = second.Direction.Y;
            var x1 = bsy-asy+(bdy/bdx)*(asx-bsx);
            var x2 = ady-(bdy*adx)/bdx;
            var u = x1/x2;
            var v = (asx+adx*u-bsx)/bdx;
            intersectionPoint = first.Origin+first.Direction*u;
            return u>=0 && v>=0;
        }

        public bool Intersect(Circle circle)
        { return Intersect(this, circle); }

        public static bool Intersect(Ray ray, Circle circle)
        {
            var dist = ray.GetDistanceTo(circle.Center);
            return dist <= circle.Radius;
        }
    }
}
