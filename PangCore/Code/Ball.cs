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
    public class Ball : ComponentBase
    {       
        /// <summary>
        /// Size of ball: XL, L, M, S
        /// </summary>
        private eBallSize mBallSize = eBallSize.XL;
        /// <summary>
        /// Type of ball (basically colors)
        /// </summary>
        private eBallType mBallType = eBallType.Red;
        /// <summary>
        /// Position originally read from Level XML files, to be able to restore the level to its original state
        /// </summary>
        public Vector2 mOriginalPosition;
        /// <summary>
        /// Collection of rectangles, using the balltype and ballsize as keys, returns the rectangle inside the balloons base texture eeach sprite should use
        /// </summary>
        public static Dictionary<eBallType, Dictionary<eBallSize, SMX.Maths.Rectangle>> mSpriteRectangles = new Dictionary<eBallType, Dictionary<eBallSize, Rectangle>>();
        /// <summary>
        /// Base texture with ball sprites (resource name)
        /// </summary>
        public static string mTextureResourceName;
        /// <summary>
        /// Base texture with ball sprites (stream of bytes)
        /// </summary>
        public static System.IO.Stream mTextureStream;
        /// <summary>
        /// Determines if all balls are paused
        /// </summary>
        public static bool BallsPaused = false;

        #region Props
        /// <summary>
        /// Center of the ball: basically the upper-left corner + (size / 2)
        /// </summary>
        public Vector2 Center
        {
            get
            {
                return new Vector2(this.mDrawRectangle.X + (this.mDrawRectangle.Width / 2f), this.mDrawRectangle.Y + (this.mDrawRectangle.Height / 2f));
            }
        }
        /// <summary>
        /// Radius of the ball, in pixels
        /// </summary>
        public float Radius
        {
            get
            {
                float rad = 0f;
                switch (this.mBallSize)
                {
                    case eBallSize.XL:
                        rad = 24f;
                        break;
                    case eBallSize.L:
                        rad = 16f;
                        break;
                    case eBallSize.M:
                        rad = 8f;
                        break;
                    case eBallSize.S:
                        rad = 4f;
                        break;
                }
                return rad * mScaleFactorX;
            }
        }
        public eBallType BallType
        {
            get { return mBallType; }
            set { mBallType = value; }
        }
        public eBallSize BallSize
        {
            get { return mBallSize; }
            set { mBallSize = value; }
        }        
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public Ball(eBallSize pSize, eBallType pType): base()
        {
            mScaleFactorX = 5f;
            mScaleFactorY = 5f;

            mBallSize = pSize;
            mBallType = pType;


            if (mSpriteRectangles == null || mSpriteRectangles.Count == 0)
            {
                Dictionary<eBallSize, SMX.Maths.Rectangle> dictRed, dictBlue, dictGreen;
                dictRed = new Dictionary<eBallSize, SMX.Maths.Rectangle>();
                dictGreen = new Dictionary<eBallSize, SMX.Maths.Rectangle>();
                dictBlue = new Dictionary<eBallSize, SMX.Maths.Rectangle>();

                dictRed.Add(eBallSize.XL, new Rectangle { X = 1, Y = 6, Width = 48, Height = 40 });
                dictRed.Add(eBallSize.L, new Rectangle { X = 52, Y = 13, Width = 32, Height = 26 });
                dictRed.Add(eBallSize.M, new Rectangle { X = 86, Y = 19, Width = 16, Height = 14 });
                dictRed.Add(eBallSize.S, new Rectangle { X = 106, Y = 23, Width = 8, Height = 7 });

                dictBlue.Add(eBallSize.XL, new Rectangle { X = 1, Y = 56, Width = 48, Height = 40 });
                dictBlue.Add(eBallSize.L, new Rectangle { X = 52, Y = 63, Width = 32, Height = 26 });
                dictBlue.Add(eBallSize.M, new Rectangle { X = 86, Y = 69, Width = 16, Height = 14 });
                dictBlue.Add(eBallSize.S, new Rectangle { X = 106, Y = 73, Width = 8, Height = 7 });

                dictGreen.Add(eBallSize.XL, new Rectangle { X = 1, Y = 105, Width = 48, Height = 40 });
                dictGreen.Add(eBallSize.L, new Rectangle { X = 52, Y = 112, Width = 32, Height = 26 });
                dictGreen.Add(eBallSize.M, new Rectangle { X = 86, Y = 118, Width = 16, Height = 14 });
                dictGreen.Add(eBallSize.S, new Rectangle { X = 106, Y = 122, Width = 8, Height = 7 });

                mSpriteRectangles.Add(eBallType.Red, dictRed);
                mSpriteRectangles.Add(eBallType.Blue, dictBlue);
                mSpriteRectangles.Add(eBallType.Green, dictGreen);
            }

        }
        /// <summary>
        /// Returns the next size for new balls, provided the size of the ball that is dying
        /// </summary>
        public static eBallSize GetNextSize(eBallSize pSize)
        {
            switch (pSize)
            {
                case eBallSize.XL:
                    return eBallSize.L;
                case eBallSize.L:
                    return eBallSize.M;
                case eBallSize.M:
                    return eBallSize.S;
            }
            return eBallSize.S;
        }        
        /// <summary>
        /// Performs content loading at initialization
        /// </summary>
        public override void LoadContents()
        {
            // This is a singleton, so load it only once
            if (mTextureStream == null)
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                mTextureResourceName = Game.ResourcesNameSpace + ".Resources.Balloons.png";

                mTextureStream = assembly.GetManifestResourceStream(mTextureResourceName);
                RendererBase.Ref.LoadTexture(mTextureResourceName, mTextureStream);
            }
        }
        /// <summary>
        /// Returns the maximum vertical speed of balls, according to their size. This is a way to actually limit bounding height of balls
        /// </summary>
        /// <returns></returns>
        private float GetMaxVelY()
        {
            switch (mBallSize)
            {
                case eBallSize.XL:
                    return 1100;
                case eBallSize.L:
                    return 900;
                case eBallSize.M:
                    return 700;
                case eBallSize.S:
                    return 500;
                default:
                    return 1000;
            }
        }
        /// <summary>
        /// Update ball logic
        /// </summary>
        /// <param name="pDt"></param>
        public override void OnFrameMove(float pDt)
        {
            RefreshDrawRectangle();


            if (!Ball.BallsPaused)
            {
                mVelocity += Game.mGravityVel_PixelsPerSecond * pDt;

                // Esto es un buen metodo para controlar la altura de rebote. Cuando se crean las bolas, ponerles una vel.Y igual a este limite
                float limiteVelY = GetMaxVelY();
                if (mVelocity.Y > limiteVelY)
                    mVelocity.Y = limiteVelY;
                if (mVelocity.Y < -limiteVelY)
                    mVelocity.Y = -limiteVelY;

                List<SMX.Maths.Rectangle> hitRects;
                Game.CurrentLevel.GetCollisionRectangles(out hitRects);
                foreach (SMX.Maths.Rectangle rect in hitRects)
                {
                    if (mDrawRectangle.Intersects(rect))
                    {
                        Vector2 normal = rect.GetNormalFromPoint(mPos);
                        if (normal.X < 0 && mVelocity.X > 0)
                            mVelocity.X *= -1;
                        if (normal.X > 0 && mVelocity.X < 0)
                            mVelocity.X *= -1;
                        if (normal.Y < 0 && mVelocity.Y > 0)
                            mVelocity.Y *= -1;
                        if (normal.Y > 0 && mVelocity.Y < 0)
                            mVelocity.Y *= -1;
                    }
                }

                // Controlar velocidad horizontal, que siempre debe ser constante
                float maxVelHorizontal = 150;
                if (mVelocity.X >= 0)
                    mVelocity.X = maxVelHorizontal;
                if (mVelocity.X < 0)
                    mVelocity.X = -maxVelHorizontal;

                // Update the position
                mPos += mVelocity * pDt;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void RefreshDrawRectangle()
        {
            var rect = mSpriteRectangles[mBallType][mBallSize];
            mDrawRectangle = new SMX.Maths.Rectangle((int)mPos.X, (int)mPos.Y, (int)((float)rect.Width * mScaleFactorX), (int)((float)rect.Height * mScaleFactorY));
        }
        /// <summary>
        /// 
        /// </summary>
        public override void OnFrameRender()
        {
            // Make balls blink if pause is ending
            //if (Ball.BallsPaused && mMainForm.mGame.CurrentLevel.mPauseTime > 0 && mMainForm.mGame.CurrentLevel.mPauseTime <= 3)
            //{
            //    float blinker = (mMainForm.mGame.CurrentLevel.mPauseTime) % 0.4f;
            //    if (blinker > 0.25f && blinker < 0.5f)
            //        return;
            //    if (blinker > 0.75f && blinker < 1f)
            //        return;
            //}

            RefreshDrawRectangle();

            var rect = mSpriteRectangles[mBallType][mBallSize];
            RendererBase.Ref.DrawSprite(mTextureResourceName, rect.X, rect.Y, rect.Width, rect.Height, mDrawRectangle.X, mDrawRectangle.Y, mDrawRectangle.Width, mDrawRectangle.Height);

        }
        /// <summary>
        /// Creates and configures a ball from an XML node
        /// </summary>
        /// <param name="pBallNd"></param>
        /// <returns></returns>
        public static Ball FromXml(System.Xml.XmlNode pBallNd)
        {
            eBallSize size = (eBallSize)Enum.Parse(typeof(eBallSize), pBallNd.Attributes["Size"].Value);
            eBallType type = (eBallType)Enum.Parse(typeof(eBallType), pBallNd.Attributes["Type"].Value);
            Ball b = new Ball(size, type);
            b.mOriginalPosition = b.mPos = SMX.Maths.Vector2.ReadVector2FromXmlAttribute(pBallNd, "Position");
            return b;
        }
    }

   
}
