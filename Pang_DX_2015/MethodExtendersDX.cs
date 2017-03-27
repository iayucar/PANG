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


namespace SMX.Maths
{
	/// <summary>
	/// Contains commonly used precalculated values and numeric calculations.
	/// </summary>
	public static class MethodExtendersDX
	{

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SharpDX.Mathematics.Interop.RawColor4 ToSDX(this SMX.Maths.Color4 pSrc)
        {
            return new SharpDX.Mathematics.Interop.RawColor4(pSrc.Red, pSrc.Green, pSrc.Blue, pSrc.Alpha);
        }

        /// <summary>
        /// 
        /// </summary>
        public static SharpDX.RectangleF ToSDXRectangleF(this SMX.Maths.Rectangle pSrc)
        {
            return new SharpDX.RectangleF(pSrc.X, pSrc.Y, pSrc.Width, pSrc.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        public static SharpDX.Vector2 ToSDX(this SMX.Maths.Vector2 pSrc)
        {
            return new SharpDX.Vector2(pSrc.X, pSrc.Y);
        }

    }
}
