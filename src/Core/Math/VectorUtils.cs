namespace Core.Math
{
    using SysMath = System.Math;

    public static class VectorUtils
    {
        public enum CourseSector
        {
            Up,
            Down,
            Left,
            Right
        }
        
        public static CourseSector GetAngleCourseSector(double angle)
        {
            const double pi = SysMath.PI;
            if (-pi/4<angle && angle<=0 || 0<angle && angle<pi/4)
                return CourseSector.Right;
            if (pi/4<=angle && angle<=3*pi/4)
                return CourseSector.Up;
            if (3*pi/4<angle || angle<-3*pi/4)
                return CourseSector.Left;
            return CourseSector.Down;
        }
        
        public enum LineClockDir
        {
            Clockwise,
            CClockwise,
            Line
        }

        public static LineClockDir GetClockPosition(Vector2 pt1, Vector2 pt2, Vector2 pt3)
        {
            var tester = (pt2.X-pt1.X)*(pt3.Y-pt1.Y) - (pt3.X-pt1.X)*(pt2.Y-pt1.Y);
            if (tester>0)
                return LineClockDir.CClockwise;
            if (tester<0)
                return LineClockDir.Clockwise;
            else
                return LineClockDir.Line;
        }
    }
}
