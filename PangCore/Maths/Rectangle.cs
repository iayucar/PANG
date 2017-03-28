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
        /// Determines if the rectangle is intersecting with a circle, and return the collision normal if so
        /// </summary>
        /// <param name="circle_x"></param>
        /// <param name="circle_y"></param>
        /// <param name="circle_r"></param>
        /// <returns></returns>
        public Vector2? IntersectsCircle(float circle_x, float circle_y, float circle_r)
        {
            // compute a center-to-center vector
            Vector2 half = new Vector2((float)this.Width / 2f, (float)this.Height / 2f);
            Vector2 center = new Vector2(circle_x - ((float)this.X + half.X), circle_y - ((float)this.Y + half.Y));
            Vector2 side = new Maths.Vector2(Math.Abs(center.X) - half.X, Math.Abs(center.Y) - half.Y);

            if (side.X > circle_r || side.Y > circle_r) // outside
                return null;
            if (side.X < -circle_r && side.Y < -circle_r) // inside
                return new Vector2(0, 1);
            if (side.X < 0 || side.Y < 0) // intersects side or corner
            {
                float dxx = 0, dyy = 0;
                if (Math.Abs(side.X) <= circle_r && side.Y < 0)
                    dxx = center.X * side.X < 0 ? -1 : 1;
                else if (Math.Abs(side.Y) <= circle_r && side.X < 0)
                    dyy = center.Y * side.Y < 0 ? -1 : 1;

                return new Vector2(dxx, dyy);
            }

            // circle is near the corner
            bool isbounce = side.X * side.X + side.Y * side.Y < circle_r * circle_r;
            if (!isbounce)
                return null;
            
            float norm = (float)Math.Sqrt(side.X * side.X + side.Y * side.Y);
            float dx = center.X < 0 ? -1 : 1;
            float dy = center.Y < 0 ? -1 : 1;

            return new Vector2(dx * side.X / norm, dy * side.Y / norm);

        } 
        /// <summary>
        /// Gets the normal corresponding to the closest rectangle face to a point
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
