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
using System.Threading.Tasks;
using SMX.Maths;

namespace SMX
{
    public abstract class ComponentBase
    {
        /// <summary>
        /// Current position in screen
        /// </summary>
        public Vector2 mPos;
        /// <summary>
        /// Rectangle in world coords where the componente will be drawn
        /// </summary>
        public SMX.Maths.Rectangle mDrawRectangle;
        /// <summary>
        /// Scale factor to modify the size of the element when rendering
        /// </summary>
        public float mScaleFactorX = 1, mScaleFactorY = 1;
        /// <summary>
        /// Velocity of the element, in pixels per second. If the object is static, this will always be zero
        /// </summary>
        public SMX.Maths.Vector2 mVelocity;
       
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFrmMain"></param>
        public ComponentBase()
        {
        }
        /// <summary>
        /// Base method to update the simulation on step forward
        /// </summary>
        /// <param name="pDt">Elapsed Time since last frame, in seconds</param>
        public abstract void OnFrameMove(float pDt);
        /// <summary>
        /// Base method for rendering
        /// </summary>
        public abstract void OnFrameRender();
        /// <summary>
        /// Base method to load contents
        /// </summary>
        public abstract void LoadContents();       

    }
}
