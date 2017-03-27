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
using System.Threading.Tasks;
using SMX.Maths;

namespace SMX
{
    public class Arrow : ComponentBase
    {
        /// <summary>
        /// Current height of the arrow
        /// </summary>
        public float mHeight;
        /// <summary>
        /// Arrow vertical speed, in pixels / second
        /// </summary>
        private float mArrowVerticalSpeedPxSec = 500;
        /// <summary>
        /// Base tile texture (stream of bytes) for Arrows
        /// </summary>
        public static System.IO.Stream mTextureStream;
        /// <summary>
        /// Base tile texture (name of the resource) for Arrows
        /// </summary>
        public static string mTextureResourceName;
        /// <summary>
        /// Texture for ending of the arrow (stream of bytes)
        /// </summary>
        public static System.IO.Stream mTextureEndingStream;
        /// <summary>
        /// Texture for the ending of the arrow (name of the resource)
        /// </summary>
        public static string mTextureEndingResourceName;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFrmMain"></param>
        public Arrow(Vector2 pOriginalPos) : base ()
        {
            mPos = pOriginalPos;

            // Give initial height, so it covers player
            mHeight = 100;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void LoadContents()
        {
            // This is a singleton, so load it only once
            if (mTextureStream == null)
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

                mTextureResourceName = Game.ResourcesNameSpace + ".Resources.ArrowBase.png";
                mTextureStream = assembly.GetManifestResourceStream(mTextureResourceName);
                RendererBase.Ref.LoadTexture(mTextureResourceName, mTextureStream);

                mTextureEndingResourceName = Game.ResourcesNameSpace + ".Resources.Arrow_1.png";
                mTextureEndingStream = assembly.GetManifestResourceStream(mTextureEndingResourceName);
                RendererBase.Ref.LoadTexture(mTextureEndingResourceName, mTextureEndingStream);
            }
        }
        /// <summary>
        /// Update arrow logic
        /// </summary>
        /// <param name="pDt"></param>
        public override void OnFrameMove(float pDt)
        {
            // Update Height
            mHeight += mArrowVerticalSpeedPxSec * pDt;

            // Check if reached end of game area
            if (mHeight >= Game.DefaultGameHeight)
            {
                mHeight = Game.DefaultGameHeight;
                Game.CurrentLevel.ArrowDied(this);
            }
        }
        /// <summary>
        /// Renders the arrow
        /// </summary>
        public override void OnFrameRender()
        {
            float width = 60;
            float halfWidth = width / 2;
            float initY = mPos.Y + 118;
            float initX = mPos.X + 48;

            // Calc full drawing rect
            mDrawRectangle = new SMX.Maths.Rectangle((int)(initX - halfWidth), (int)(initY - mHeight), (int)width, (int)mHeight);

            // Draw as many tiles as necessary to cover the area
            int textureHeight = 64;
            int numdiv = (int)((float)mDrawRectangle.Height / (float)textureHeight);
            int startY = (int)initY;
            for (int i = 0; i < numdiv; i++)
            {
                RendererBase.Ref.DrawSprite(mTextureResourceName, 0, 0, 21, 32, mDrawRectangle.X, startY - textureHeight, mDrawRectangle.Width, textureHeight);
                startY -= textureHeight;
            }

            // Draw remaining
            int remaining = startY - mDrawRectangle.Y;
            RendererBase.Ref.DrawSprite(mTextureResourceName, 0, 0, 21, 32, mDrawRectangle.X, mDrawRectangle.Y, mDrawRectangle.Width, remaining);

            // Draw Arrow ending
            RendererBase.Ref.DrawSprite(mTextureEndingResourceName, 0, 0, 32, 32, mDrawRectangle.X - 8, mDrawRectangle.Y - 20, 64, 64);
        }
    }
}
