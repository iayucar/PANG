// ----------------------------------------------------------------------------
// Copyright (c) 2017 - Iñaki Ayucar
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
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SMX.Maths
{
    public struct Rectangle : IEquatable<Rectangle>
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
        private static Rectangle _empty;

        #region Props
        /// <summary>
        /// 
        /// </summary>
        public int Left
        {
            get
            {
                return this.X;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Right
        {
            get
            {
                return (this.X + this.Width);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Top
        {
            get
            {
                return this.Y;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Bottom
        {
            get
            {
                return (this.Y + this.Height);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Point Location
        {
            get
            {
                return new Point(this.X, this.Y);
            }
            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Point Center
        {
            get
            {
                return new Point(this.X + (this.Width / 2), this.Y + (this.Height / 2));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static Rectangle Empty
        {
            get
            {
                return _empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return ((((this.Width == 0) && (this.Height == 0)) && (this.X == 0)) && (this.Y == 0));
            }
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Rectangle(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Rectangle Clone()
        {
            return new Rectangle(this.X, this.Y, this.Width, this.Height);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        public void Offset(Point amount)
        {
            this.X += amount.X;
            this.Y += amount.Y;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public void Offset(int offsetX, int offsetY)
        {
            this.X += offsetX;
            this.Y += offsetY;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Contains(int x, int y)
        {
            return ((((this.X <= x) && (x < (this.X + this.Width))) && (this.Y <= y)) && (y < (this.Y + this.Height)));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(Rectangle value)
        {
            return ((((this.X <= value.X) && ((value.X + value.Width) <= (this.X + this.Width))) && (this.Y <= value.Y)) && ((value.Y + value.Height) <= (this.Y + this.Height)));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Intersects(Rectangle value)
        {
            return ((((value.X < (this.X + this.Width)) && (this.X < (value.X + value.Width))) && (value.Y < (this.Y + this.Height))) && (this.Y < (value.Y + value.Height)));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="v"></param>
        /// <param name="outVel"></param>
        /// <param name="hitNormal"></param>
        /// <param name="hitTime"></param>
        /// <returns></returns>
        public static bool WillIntersect_SweepBoxBox(Rectangle a, Rectangle b, Vector2 v, out Vector2 outVel, out Vector2 hitNormal, out float hitTime)
        {
            //Initialise out info
            outVel = v;
            hitNormal = Vector2.Zero;
            hitTime = 0.0f;

            // Return early if a & b are already overlapping
            if (a.Intersects(b)) 
                return false;

            // Treat b as stationary, so invert v to get relative velocity
            v = -v;

            
            float outTime = 1.0f;
            Vector2 overlapTime = Vector2.Zero;

            // X axis overlap
            if (v.X <= 0)
            {
                if (b.Right < a.Left) return false;
                if (b.Right > a.Left) outTime = Math.Min((a.Left - b.Right) / v.X, outTime);

                if (a.Right < b.Left)
                {
                    overlapTime.X = (a.Right - b.Left) / v.X;
                    hitTime = Math.Max(overlapTime.X, hitTime);
                }
            }
            else if (v.X > 0)
            {
                if (b.Left > a.Right) return false;
                if (a.Right > b.Left) outTime = Math.Min((a.Right - b.Left) / v.X, outTime);

                if (b.Right < a.Left)
                {
                    overlapTime.X = (a.Left - b.Right) / v.X;
                    hitTime = Math.Max(overlapTime.X, hitTime);
                }
            }

            if (hitTime > outTime) return false;

            //=================================

            // Y axis overlap
            if (v.Y <= 0)
            {
                if (b.Bottom < a.Top) return false;
                if (b.Bottom > a.Top) outTime = Math.Min((a.Top - b.Bottom) / v.Y, outTime);

                if (a.Bottom < b.Top)
                {
                    overlapTime.Y = (a.Bottom - b.Top) / v.Y;
                    hitTime = Math.Max(overlapTime.Y, hitTime);
                }
            }
            else if (v.Y > 0)
            {
                if (b.Top > a.Bottom) return false;
                if (a.Bottom > b.Top) outTime = Math.Min((a.Bottom - b.Top) / v.Y, outTime);

                if (b.Bottom < a.Top)
                {
                    overlapTime.Y = (a.Top - b.Bottom) / v.Y;
                    hitTime = Math.Max(overlapTime.Y, hitTime);
                }
            }

            if (hitTime > outTime) return false;

            // Scale resulting velocity by normalized hit time
            outVel = -v * hitTime;

            // Hit normal is along axis with the highest overlap time
            if (overlapTime.X > overlapTime.Y)
            {
                hitNormal = new Vector2(Math.Sign(v.X), 0);
            }
            else
            {
                hitNormal = new Vector2(0, Math.Sign(v.Y));
            }

            return true;
        }        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="pRadius"></param>
        /// <returns></returns>
        public bool IntersectsCircle(Vector2 pCenter, float pRad)
        {
            float w = this.Width / 2;
            float h = this.Height / 2;
            var rectangleCenter = new Vector2(this.X + w, this.Y + h);

            var dx = Math.Abs(pCenter.X - rectangleCenter.X);
            var dy = Math.Abs(pCenter.Y - rectangleCenter.Y);

            if (dx > (pRad + w) || dy > (pRad + h)) 
                return false;

            float circleDistanceX = Math.Abs(pCenter.X - this.X - w);
            float circleDistanceY = Math.Abs(pCenter.Y - this.Y - h);
            //hacer las colisiones con esto
            if (circleDistanceX <= w || circleDistanceY <= h)
                return true;

            var cornerDistanceSq = Math.Pow(circleDistanceX - w, 2) + Math.Pow(circleDistanceY - h, 2);
            return (cornerDistanceSq <= pRad * pRad);
        }
        /// <summary>
        /// Obtiene la normal que se corresponde a la cara más cercana del rect al punto "point"
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector2 GetNormalFromPoint(Vector2 point)
        {
            Vector2 normal = Vector2.Zero;
            float min = float.MaxValue;
            float distance;
            Vector2 center = new Vector2(this.Center.X, this.Center.Y);
            Vector2 Extents = new Vector2(this.Width, this.Height);
            point -= center;
            distance = Math.Abs(Extents.X - Math.Abs(point.X));
            if (distance < min)
            {
                min = distance;
                normal = Math.Sign(point.X) * Vector2.UnitX;    // Cardinal axis for X
            }
            distance = Math.Abs(Extents.Y - Math.Abs(point.Y));
            if (distance < min)
            {
                min = distance;
                normal = Math.Sign(point.Y) * Vector2.UnitY;    // Cardinal axis for Y
            }           
            return normal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Rectangle other)
        {
            return ((((this.X == other.X) && (this.Y == other.Y)) && (this.Width == other.Width)) && (this.Height == other.Height));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is Rectangle)
            {
                flag = this.Equals((Rectangle)obj);
            }
            return flag;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            System.Globalization.CultureInfo currentCulture = System.Globalization.CultureInfo.CurrentCulture;
            return string.Format(currentCulture, "{{X:{0} Y:{1} Width:{2} Height:{3}}}", new object[] { this.X.ToString(currentCulture), this.Y.ToString(currentCulture), this.Width.ToString(currentCulture), this.Height.ToString(currentCulture) });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (((this.X.GetHashCode() + this.Y.GetHashCode()) + this.Width.GetHashCode()) + this.Height.GetHashCode());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Rectangle a, Rectangle b)
        {
            return ((((a.X == b.X) && (a.Y == b.Y)) && (a.Width == b.Width)) && (a.Height == b.Height));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Rectangle a, Rectangle b)
        {
            if (((a.X == b.X) && (a.Y == b.Y)) && (a.Width == b.Width))
            {
                return (a.Height != b.Height);
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        static Rectangle()
        {
            _empty = new Rectangle();
        }
      
    }
}
