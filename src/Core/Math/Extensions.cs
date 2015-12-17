namespace Core.Math
{
    public static class Extensions
    {
        public static double Clamp(this double self, double min, double max)
        {
            if (self<min)
                return min;
            else if (self>max)
                return max;
            else
                return self;
        }

        public static float Clamp(this float self, float min, float max)
        {
            if (self<min)
                return min;
            else if (self>max)
                return max;
            else
                return self;
        }
    }
}
