using System;

namespace Core.Math
{
    using SysMath = System.Math;

    public struct Matrix23 : IFormattable, IEquatable<Matrix23>
    {
        #region Struct variables

        public double S00, S10;
        public double S01, S11;
        public double S02, S12;

        #endregion

        #region Costructors

        public Matrix23(double s00, double s01, double s02, double s10, double s11, double s12)
        {
            S00 = s00;
            S10 = s10;
            S01 = s01;
            S11 = s11;
            S02 = s02;
            S12 = s12;
        }
        
        public static Matrix23 FromColumns(double[] elements)
        {
            return new Matrix23(
                elements[0], elements[2], elements[4],
                elements[1], elements[3], elements[5]);
        }

        public static Matrix23 FromColumns(float[] elements)
        {
            return new Matrix23(
                elements[0], elements[2], elements[4],
                elements[1], elements[3], elements[5]);
        }

        public Matrix23(Vector2 col0, Vector2 col1, Vector2 col2) :
            this(col0.X, col1.X, col2.X, col0.Y, col1.Y, col2.Y)
        {}

        #endregion

        #region Operators

        public static Matrix23 operator*(Matrix23 m1, Matrix23 m2)
        {
            return new Matrix23(
                m1.S00*m2.S00 + m1.S01*m2.S10, m1.S00*m2.S01 + m1.S01*m2.S11, m1.S00*m2.S02 + m1.S01*m2.S12 + m1.S02,
                m1.S10*m2.S00 + m1.S11*m2.S10, m1.S10*m2.S01 + m1.S11*m2.S11, m1.S10*m2.S02 + m1.S11*m2.S12 + m1.S12);
        }

        public static Vector2 operator *(Matrix23 m1, Vector2 v1)
        {
            return new Vector2(
                m1.S00*v1.X + m1.S01*v1.Y + m1.S02,
                m1.S10*v1.X + m1.S11*v1.Y + m1.S12);
        }

        public static Matrix23 operator +(Matrix23 m1, Matrix23 m2)
        {
            return new Matrix23(
                m1.S00+m2.S00, m1.S01+m2.S01, m1.S02+m2.S02,
                m1.S10+m2.S10, m1.S11+m2.S11, m1.S12+m2.S12);
        }

        public static Matrix23 operator -(Matrix23 m1, Matrix23 m2)
        {
            return new Matrix23(
                m1.S00-m2.S00, m1.S01-m2.S01, m1.S02-m2.S02,
                m1.S10-m2.S10, m1.S11-m2.S11, m1.S12-m2.S12);
        }

        public static bool operator==(Matrix23 m1, Matrix23 m2)
        {
            return
                SysMath.Abs(m1.S00-m2.S00) <= EqualityTolerence &&
                SysMath.Abs(m1.S10-m2.S10) <= EqualityTolerence &&
                SysMath.Abs(m1.S01-m2.S01) <= EqualityTolerence &&
                SysMath.Abs(m1.S11-m2.S11) <= EqualityTolerence &&
                SysMath.Abs(m1.S02-m2.S02) <= EqualityTolerence &&
                SysMath.Abs(m1.S12-m2.S12) <= EqualityTolerence;
        }

        public static bool operator !=(Matrix23 m1, Matrix23 m2)
        { return !(m1==m2); }

        #endregion

        #region Functions

        public static Matrix23 Translation(Vector2 offset)
        {
            return new Matrix23(
                1, 0, offset.X,
                0, 1, offset.Y);
        }

        public static Matrix23 Rotation(Angle angle)
        {
            var sin = SysMath.Sin(angle);
            var cos = SysMath.Cos(angle);
            return new Matrix23(
                cos, -sin, 0,
                sin, +cos, 0);
        }

        public static Matrix23 Scaling(Vector2 scale)
        {
            return new Matrix23(
                scale.X, 0, 0,
                0, scale.Y, 0);
        }

        public static Matrix23 Invert(Matrix23 m)
        {
            var d = m.S00*m.S11 - m.S01*m.S10;
            return new Matrix23(
                +m.S11/d, -m.S01/d, (m.S01*m.S12 - m.S11*m.S02)/d,
                -m.S10/d, +m.S00/d, (m.S10*m.S02 - m.S00*m.S12)/d);
        }

        #endregion

        #region Accessors & Mutators

        public Vector2 Offset
        {
            get { return Col2; }
            set { Col2 = value; }
        }

        public Angle Turn
        {
            get { return SysMath.Atan2(S10, S00); }
            set
            {
                var scale = Scale;
                S00 = scale.X;
                S11 = scale.Y;
                S01 = 0;
                S10 = 0;
                this *= Rotation(value);
            }
        }

        public Vector2 Scale
        {
            get
            {
                return new Vector2(SysMath.Sign(S00)*Col0.Length, SysMath.Sign(S11)*Col1.Length);
            }
            set
            {
                var scale = Scale;
                this *= Scaling(new Vector2(1 / scale.X, 1 / scale.Y));
                this *= Scaling(value);
            }
        }

        public Vector2 Col0
        {
            get { return new Vector2(S00, S10); }
            set
            {
                S00 = value.X;
                S10 = value.Y;
            }
        }

        public Vector2 Col1
        {
            get { return new Vector2(S01, S11); }
            set
            {
                S01 = value.X;
                S11 = value.Y;
            }
        }

        public Vector2 Col2
        {
            get { return new Vector2(S02, S12); }
            set
            {
                S02 = value.X;
                S12 = value.Y;
            }
        }

        #endregion

        #region Standard Functions

        /// <summary>
        /// Textual description of the Matrix2X3
        /// </summary>
        /// <returns>String representation of the matrix</returns>
        public override string ToString()
        { return ToString(null, null); }

        /// <summary>
        /// Textual description of the Matrix2X3
        /// </summary>
        /// <param name="format">Formatting string: not supported</param>
        /// <param name="formatProvider">The culture specific fromatting provider</param>
        /// <returns>String representation of the matrix</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return String.Format("({0}, {1}, {2}), ({3}, {4}, {5})", S00, S01, S02, S10, S11, S12);
        }

        /// <summary>
        /// Get the hashcode
        /// </summary>
        /// <returns>Hashcode for the object instance</returns>
        public override int GetHashCode()
        {
            return unchecked(Col0.GetHashCode() + Col1.GetHashCode() + Col2.GetHashCode());
        }

        /// <summary>
        /// Comparator
        /// </summary>
        /// <param name="other">The other object (which should be a Matrix2X3) to compare to</param>
        /// <returns>Truth if two matrices are equal within a tolerence</returns>
        public override bool Equals(object other)
        {
            if (other is Matrix23)
            {
                var matrix = (Matrix23)other;
                return matrix==this;
            }
            return false;
        }

        /// <summary>
        /// Comparator
        /// </summary>
        /// <param name="other">The other Matrix2X3 to compare to</param>
        /// <returns>Truth if two matrices are equal within a tolerence</returns>
        public bool Equals(Matrix23 other)
        { return other==this; }

        #endregion

        #region Constants

        public static readonly Matrix23 Identity = new Matrix23(1, 0, 0, 0, 1, 0);

        /// <summary>
        /// The tolerence used when determining the equality of two matrices. 
        /// </summary>
        public const double EqualityTolerence = Double.Epsilon;

        #endregion
    }
}
