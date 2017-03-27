// ----------------------------------------------------------------------------
// Copyright (c) 2017 - Inaki Ayucar
// 
// Please note: All original arcade game assets are property of their respective 
//              owners. They are currently available at www.spriters-resource.com 
//              and have been included in this project for educational purposes 
//              only. 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of 
// this software and associated documentation files (the "Software"), to deal in the 
// Software without restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, subject to the 
// following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies 
// or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//
// ----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;

namespace SMX.Maths
{
    public struct Vector2 : IEquatable<Vector2>
    {
        /// <summary>Gets or sets the x-component of the vector.</summary>
        public float X;
        /// <summary>Gets or sets the y-component of the vector.</summary>
        public float Y;
        public static Vector2 Zero = new Vector2();
        public static Vector2 One = new Vector2(1, 1);
        public static Vector2 UnitX = new Vector2(1, 0);
        public static Vector2 UnitY = new Vector2(0, 1);

        #region Props
        [DisplayName("X")]
        [XmlIgnore]
        public float Design_X
        {
            get { return this.X; }
            set { this.X = value; }
        }
        [DisplayName("Y")]
        [XmlIgnore]
        public float Design_Y
        {
            get { return this.Y; }
            set { this.Y = value; }
        }
        /// <summary>
        /// Indexed access to vector components
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public float this[int index]
        {
            get
            {
                if (index == 0)
                    return this.X;
                else return this.Y;
            }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of Vector2.</summary>
        /// <param name="x">Initial value for the x-component of the vector.</param>
        /// <param name="y">Initial value for the y-component of the vector.</param>
        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
        /// <summary>
        /// Creates a new instance of Vector2.</summary>
        /// <param name="value">Value to initialize both components to.</param>
        public Vector2(float value)
        {
            this.X = this.Y = value;
        }
        /// <summary>
        /// Retrieves a string representation of the current object.</summary>
        /// <returns>String that represents the object.</returns>
        public override string ToString()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            return string.Format(currentCulture, "{{X:{0} Y:{1}}}", new object[] { this.X.ToString(currentCulture), this.Y.ToString(currentCulture) });
        }      
        /// <summary>
        /// Determines whether the specified System.Object is equal to the Vector2.
        /// </summary>
        /// <param name="other">The System.Object to compare with the current Vector2.</param>
        /// <returns>true if the specified System.Object is equal to the current Vector2; false otherwise.
        /// </returns>
        public bool Equals(Vector2 other)
        {
            return ((this.X == other.X) && (this.Y == other.Y));
        }
        /// <summary>
        /// Returns a value that indicates whether the current instance is equal to a specified object.</summary>
        /// <param name="obj">Object to make the comparison with.</param>
        /// <returns>true if the current instance is equal to the specified object; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is Vector2)
            {
                flag = this.Equals((Vector2)obj);
            }
            return flag;
        }
        /// <summary>
        /// Gets the hash code of the vector object.</summary>
        /// <returns>Hash code of the vector object.</returns>
        public override int GetHashCode()
        {
            return (this.X.GetHashCode() + this.Y.GetHashCode());
        }

        #region Length & Distance
        /// <summary>
        /// Calculates the length of the vector.</summary>
        /// <returns>Length of the vector.</returns>
        public float Length()
        {
            float num = (this.X * this.X) + (this.Y * this.Y);
            return (float)Math.Sqrt((double)num);
        }
        /// <summary>
        /// Calculates the length of the vector squared.</summary>
        /// <returns>The length of the vector squared.</returns>
        public float LengthSquared()
        {
            return ((this.X * this.X) + (this.Y * this.Y));
        }
        /// <summary>
        /// Turns the current vector into a unit vector.  The result is a vector one unit in length pointing in the same direction as the original vector.</summary>
        public void Normalize()
        {
            float num2 = (this.X * this.X) + (this.Y * this.Y);
            float num3 = ((float)Math.Sqrt((double)num2));
            if (num3 != 0)
            {
                float num = 1f / num3;
                this.X *= num;
                this.Y *= num;
            }
            else this.X = this.Y = 0f;
        }
        /// <summary>
        /// Creates a unit vector from the specified vector, writing the result to a user-specified variable.  The result is a vector one unit in length pointing in the same direction as the original vector.</summary>
        /// <param name="value">Source vector.</param>
        /// <param name="result">Normalized vector.</param>
        public static void Normalize(ref Vector2 value, out Vector2 result)
        {
            float num2 = (value.X * value.X) + (value.Y * value.Y);
            float num3 = ((float)Math.Sqrt((double)num2));
            if (num3 != 0f)
            {
                float num = 1f / num3;
                result.X = value.X * num;
                result.Y = value.Y * num;
            }
            else result.X = result.Y = 0f;
        }
        #endregion

        #region Operators
        /// <summary>
        /// Returns a vector pointing in the opposite direction.</summary>
        /// <param name="value">Source vector.</param>
        /// <returns>Vector pointing in the opposite direction.</returns>
        public static Vector2 operator -(Vector2 value)
        {
            Vector2 vector;
            vector.X = -value.X;
            vector.Y = -value.Y;
            return vector;
        }
        /// <summary>
        /// Tests vectors for equality.</summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <returns>true if the vectors are equal; false otherwise.</returns>
        public static bool operator ==(Vector2 value1, Vector2 value2)
        {
            return ((value1.X == value2.X) && (value1.Y == value2.Y));
        }
        /// <summary>
        /// Tests vectors for inequality.</summary>
        /// <param name="value1">Vector to compare.</param>
        /// <param name="value2">Vector to compare.</param>
        /// <returns>Returns true if the vectors are not equal,  false otherwise.</returns>
        public static bool operator !=(Vector2 value1, Vector2 value2)
        {
            if (value1.X == value2.X)
            {
                return (value1.Y != value2.Y);
            }
            return true;
        }
        /// <summary>
        /// Adds two vectors.</summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">Source vector.</param>
        /// <returns>Sum of the source vectors.</returns>
        public static Vector2 operator +(Vector2 value1, Vector2 value2)
        {
            Vector2 vector;
            vector.X = value1.X + value2.X;
            vector.Y = value1.Y + value2.Y;
            return vector;
        }
        /// <summary>
        /// Subtracts a vector from a vector.</summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="value2">source vector.</param>
        /// <returns>Result of the subtraction.</returns>
        public static Vector2 operator -(Vector2 value1, Vector2 value2)
        {
            Vector2 vector;
            vector.X = value1.X - value2.X;
            vector.Y = value1.Y - value2.Y;
            return vector;
        }
        /// <summary>
        /// Multiplies a vector by a scalar value.</summary>
        /// <param name="value">Source vector.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <returns>Result of the multiplication.</returns>
        public static Vector2 operator *(Vector2 value, float scaleFactor)
        {
            Vector2 vector;
            vector.X = value.X * scaleFactor;
            vector.Y = value.Y * scaleFactor;
            return vector;
        }
        /// <summary>
        /// Multiplies a vector by a scalar value.</summary>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <param name="value">Source vector.</param>
        /// <returns>Result of the multiplication.</returns>
        public static Vector2 operator *(float scaleFactor, Vector2 value)
        {
            Vector2 vector;
            vector.X = value.X * scaleFactor;
            vector.Y = value.Y * scaleFactor;
            return vector;
        }
        /// <summary>
        /// Divides a vector by a scalar value.</summary>
        /// <param name="value1">Source vector.</param>
        /// <param name="divider">The divisor.</param>
        /// <returns>The source vector divided by b.</returns>
        public static Vector2 operator /(Vector2 value1, float divider)
        {
            if (divider == 0)
                throw new System.ApplicationException("Invalid argument. Cannot divide by zero");

            Vector2 vector;
            float num = 1f / divider;
            vector.X = value1.X * num;
            vector.Y = value1.Y * num;
            return vector;
        }
        #endregion

        #region XML
        /// <summary>
        /// Lee un Vector3 a partir del valor de un attributo, en el format XXXX YYYY (valores separados por un espacio)
        /// </summary>
        /// <param name="pVector3Node"></param>
        public static Vector2 ReadVector2FromXmlAttribute(System.Xml.XmlNode pVector2Node, string pAttrName)
        {
            string value = pVector2Node.Attributes[pAttrName].Value;
            return ReadVector2FromString(value);
        }
        /// <summary>
        /// Lee un Vector3 a partir de una cadena, en el format XXXX YYYY (valores separados por un espacio)
        /// </summary>
        /// <param name="pSpaceSeparatedValues"></param>
        /// <returns></returns>
        public static Vector2 ReadVector2FromString(string pSpaceSeparatedValues)
        {
            string[] values = pSpaceSeparatedValues.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length != 2)
                throw new System.ApplicationException("Invalid format reading Vector3 from string");

            Vector2 ret = new Vector2();
            ret.X = MethodExtenders.FloatFromString(values[0]);
            ret.Y = MethodExtenders.FloatFromString(values[1]);
            return ret;
        }
        /// <summary>
        /// Salva el Vector3 como un attributo string con el formato XXXX YYYY (valores separados por un espacio).
        /// </summary>
        /// <param name="pDoc"></param>
        /// <param name="pNd"></param>
        /// <param name="pAttName"></param>
        public void ToXmlAttribute_SpaceSeparated(System.Xml.XmlDocument pDoc, System.Xml.XmlNode pNd, string pAttName)
        {
            string val = this.ToString_SpaceSeparated();
            val.ToXmlAttribute(pDoc, pNd, pAttName);
        }
        /// <summary>
        /// Devuelve el Vector3 como un único string con el formato XXXX YYYY (valores separados por un espacio).
        /// Acepta un parámetro "format" donde se pueden especificicar decimales de tipo "f2", etc
        /// </summary>
        /// <param name="pFormat"></param>
        /// <returns></returns>
        public string ToString_SpaceSeparated(string pFormat = "")
        {
            if (string.IsNullOrEmpty(pFormat))
                return string.Format("{0} {1}", this.X.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat), this.Y.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
            else return string.Format("{0} {1}", this.X.ToString(pFormat, System.Globalization.CultureInfo.InvariantCulture.NumberFormat), this.Y.ToString(pFormat, System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
        }
        #endregion
    }

 

 
}

 

