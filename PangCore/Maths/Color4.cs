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
using System.ComponentModel;
using System.Globalization;
using System.Drawing;

namespace SMX.Maths
{
    public struct Color4
    {
        public float Red;
        public float Green;
        public float Blue;
        public float Alpha;

        public static Color4 White
        {
            get { return new Color4(uint.MaxValue); }
        }
        public static Color4 Zero
        {
            get { return new Color4(0); }
        }
        public static Color4 Black
        {
            get { return new Color4(1, 0, 0, 0); }
        }
        public static Color4 Yellow
        {
            get { return new Color4(0xff00ffff); }
        }
        public static Color4 RedStandard
        {
            get { return new Color4(1, 1, 0, 0); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="argb"></param>
        public Color4(uint argb)
        {
            this.Alpha = (float)(((double)((argb >> 0x18) & 0xff)) / 255.0);
            this.Blue = (float)(((double)((argb >> 0x10) & 0xff)) / 255.0);
            this.Green = (float)(((double)((argb >> 8) & 0xff)) / 255.0);
            this.Red = (float)(((double)(argb & 0xff)) / 255.0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public Color4(float a, float r, float g, float b)
        {
            this.Alpha = a;
            this.Blue = b;
            this.Green = g;
            this.Red = r;
        }
      
    }
}

