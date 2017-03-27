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
    public class Prize : ComponentBase
    {
        /// <summary>
        /// Enum with the types of prizes
        /// </summary>
        public enum ePrize
        {
            Laser,
            Anchor,
            DoubleArrow,
            Dinamite,
            Shield,
            ExtraTime,
            Pause,
            ExtraLife,
            None,
        }
        /// <summary>
        /// Type of prize of this particular prize
        /// </summary>
        public ePrize mType = ePrize.None;
        /// <summary>
        /// Collection of rectangles inside the texture with sprites. Prizes can be animated (shield, dinamite...) so each type of prize can have more than one sprite (rect)
        /// </summary>
        public static Dictionary<ePrize, List<SMX.Maths.Rectangle>> mSourceRects = new Dictionary<ePrize, List<Rectangle>>();
        /// <summary>
        /// Private index to control animation frames
        /// </summary>
        private int mSourceRectIdx = 0;
        /// <summary>
        /// Private counter to control animation timing
        /// </summary>
        private float mAnimTime = 0;
        /// <summary>
        /// Base tile texture (stream of bytes) for Arrows
        /// </summary>
        public static System.IO.Stream mTextureStream;
        /// <summary>
        /// Base tile texture (name of the resource) for Arrows
        /// </summary>
        public static string mTextureResourceName;


        /// <summary>
        /// 
        /// </summary>
        public Prize(ePrize pPrize)
            : base()
        {
            mScaleFactorX = 5f;
            mScaleFactorY = 5f;

            mType = pPrize;

            if (mSourceRects == null || mSourceRects.Count == 0)
            {
                mSourceRects = new Dictionary<ePrize, List<SMX.Maths.Rectangle>>();

                //<!--Laser-->
                //  new SMX.Maths.Rectangle { X=0,  Y=464,  Width=16,  Height=16}
                //<!--Anchor-->
                //  new SMX.Maths.Rectangle { X=16,  Y=464,  Width=16,  Height=16}
                //<!--Double Arrow-->
                //  new SMX.Maths.Rectangle { X=32,  Y=464,  Width=16,  Height=16}
                //<!--Dinamite-->
                //  new SMX.Maths.Rectangle { X=48,  Y=464,  Width=16,  Height=16}
                //  new SMX.Maths.Rectangle { X=64,  Y=464,  Width=16,  Height=16}
                //  new SMX.Maths.Rectangle { X=80,  Y=464,  Width=16,  Height=16}
                //<!--Shield-->
                //  new SMX.Maths.Rectangle { X=96,  Y=464,  Width=16,  Height=16}
                //  new SMX.Maths.Rectangle { X=112,  Y=464,  Width=16,  Height=16}
                //  new SMX.Maths.Rectangle { X=128,  Y=464,  Width=16,  Height=16}
                //  new SMX.Maths.Rectangle { X=144,  Y=464,  Width=16,  Height=16}
                //<!--Extra Time-->
                //  new SMX.Maths.Rectangle { X=160,  Y=464,  Width=16,  Height=16}
                //<!--Pause-->
                //  new SMX.Maths.Rectangle { X=176,  Y=464,  Width=16,  Height=16}
                //<!--Extra Life-->
                //  new SMX.Maths.Rectangle { X=192,  Y=464,  Width=16,  Height=16}
                mSourceRects.Add(ePrize.Laser, new List<Rectangle>() { new SMX.Maths.Rectangle { X = 0, Y = 464, Width = 16, Height = 16 } });
                mSourceRects.Add(ePrize.Anchor, new List<Rectangle>() { new SMX.Maths.Rectangle { X = 16, Y = 464, Width = 16, Height = 16 } });
                mSourceRects.Add(ePrize.DoubleArrow, new List<Rectangle>() { new SMX.Maths.Rectangle { X = 32, Y = 464, Width = 16, Height = 16 } });
                mSourceRects.Add(ePrize.Dinamite, new List<Rectangle>() { new SMX.Maths.Rectangle { X = 48, Y = 464, Width = 16, Height = 16 }, new SMX.Maths.Rectangle { X = 64, Y = 464, Width = 16, Height = 16 }, new SMX.Maths.Rectangle { X = 80, Y = 464, Width = 16, Height = 16 } });
                mSourceRects.Add(ePrize.Shield, new List<Rectangle>() { new SMX.Maths.Rectangle { X = 96, Y = 464, Width = 16, Height = 16 }, new SMX.Maths.Rectangle { X = 112, Y = 464, Width = 16, Height = 16 }, new SMX.Maths.Rectangle { X = 128, Y = 464, Width = 16, Height = 16 }, new SMX.Maths.Rectangle { X = 144, Y = 464, Width = 16, Height = 16 } });
                mSourceRects.Add(ePrize.ExtraTime, new List<Rectangle>() { new SMX.Maths.Rectangle { X = 160, Y = 464, Width = 16, Height = 16 } });
                mSourceRects.Add(ePrize.Pause, new List<Rectangle>() { new SMX.Maths.Rectangle { X = 176, Y = 464, Width = 16, Height = 16 } });
                mSourceRects.Add(ePrize.ExtraLife, new List<Rectangle>() { new SMX.Maths.Rectangle { X = 192, Y = 464, Width = 16, Height = 16 } });
            }
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
                mTextureResourceName = Game.ResourcesNameSpace + ".Resources.Sprites.png";

                mTextureStream = assembly.GetManifestResourceStream(mTextureResourceName);
                RendererBase.Ref.LoadTexture(mTextureResourceName, mTextureStream);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void RefreshDrawRectangle()
        {
            var rect = mSourceRects[mType][mSourceRectIdx];
            mDrawRectangle = new SMX.Maths.Rectangle((int)mPos.X, (int)mPos.Y, (int)((float)rect.Width * mScaleFactorX), (int)((float)rect.Height * mScaleFactorY));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDt"></param>
        public override void OnFrameMove(float pDt)
        {
            // Change animation frame
            if (mSourceRects[mType].Count > 1)
            {
                float fps = 10;
                float period = 1 / fps;
                mAnimTime += pDt;
                if (mAnimTime > period)
                {
                    mAnimTime = 0;
                    mSourceRectIdx++;
                    if (mSourceRectIdx >= mSourceRects[mType].Count)
                        mSourceRectIdx = 0;
                }
            }
            RefreshDrawRectangle();

            mVelocity += Game.mGravityVel_PixelsPerSecond * pDt;

            List<SMX.Maths.Rectangle> hitRects;
            Game.CurrentLevel.GetCollisionRectangles(out hitRects);
            foreach (Rectangle rect in hitRects)
            {
                if (mDrawRectangle.Intersects(rect))
                    mVelocity = Vector2.Zero;
            }

            mVelocity.X = 0;
            
            mPos += mVelocity * pDt;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void OnFrameRender()
        {
            RefreshDrawRectangle();
            var rect = mSourceRects[mType][mSourceRectIdx];

            RendererBase.Ref.DrawSprite(mTextureResourceName, rect.X, rect.Y, rect.Width, rect.Height, mDrawRectangle.X, mDrawRectangle.Y, mDrawRectangle.Width, mDrawRectangle.Height);

        }
    }
}
