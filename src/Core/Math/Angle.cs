using System;

namespace Core.Math
{
    public struct Angle : IFormattable, IEquatable<Angle>
    {
        #region Struct fields

        private double angle;

        #endregion

        #region Constructors

        public static Angle FromRadians(double radians)
        { return new Angle {Radians = radians}; }

        public static Angle FromDegrees(double degrees)
        { return new Angle {Degrees = degrees}; }

        #endregion

        #region Accessors & Mutators

        public double Radians
        {
            get { return angle; }
            set { angle = value; }
        }

        public double Degrees
        {
            get { return RadiansToDegrees(angle); }
            set { angle = DegreesToRadians(value); }
        }

        #endregion

        #region Operators

        public static implicit operator double(Angle angle)
        { return angle.angle; }

        public static implicit operator Angle(double angle)
        { return FromRadians(angle); }

        public static bool operator==(Angle a, Angle b)
        { return System.Math.Abs(a-b)<=EqualityTolerence; }

        public static bool operator!=(Angle a, Angle b)
        { return !(a==b); }

        #endregion

        #region Functions

        private static double RadiansToDegrees(double radians)
        { return radians*ConversionFactor; }

        private static double DegreesToRadians(double degrees)
        { return degrees/ConversionFactor; }

        #endregion

        #region Standard Functions

        /// <summary>
        /// Textual description of the Angle
        /// </summary>
        /// <returns>String representation of the angle</returns>
        public override string ToString()
        { return ToString(null, null); }

        /// <summary>
        /// Textual description of the Angle
        /// </summary>
        /// <param name="format">Formatting string</param>
        /// <param name="formatProvider">The culture specific fromatting provider</param>
        /// <returns>String representation of the angle</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        { return angle.ToString(format, formatProvider); }

        /// <summary>
        /// Get the hashcode
        /// </summary>
        /// <returns>Hashcode for the object instance</returns>
        public override int GetHashCode()
        { return angle.GetHashCode(); }

        /// <summary>
        /// Comparator
        /// </summary>
        /// <param name="other">The other object (which should be a Angle) to compare to</param>
        /// <returns>Truth if two angles are equal within a tolerence</returns>
        public override bool Equals(object other)
        {
            if (other is Angle)
            {
                var ang = (Angle)other;
                return ang.Equals(this);
            }
            return false;
        }

        /// <summary>
        /// Comparator
        /// </summary>
        /// <param name="other">The other angle to compare to</param>
        /// <returns>Truth if two angles are equal within a tolerence</returns>
        public bool Equals(Angle other)
        { return other==this; }

        #endregion

        #region Constants

        private const double ConversionFactor = 360/(2*System.Math.PI);

        /// <summary>
        /// The tolerence used when determining the equality of two angles. 
        /// </summary>
        public const double EqualityTolerence = Double.Epsilon;

        #endregion
    }
}
