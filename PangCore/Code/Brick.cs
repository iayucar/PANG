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
    public class Brick : ComponentBase
    {      
        /// <summary>
        /// Determines if the brick can be destroyed with an arrow hit, or if it's indestructible
        /// </summary>
        public bool mBreakable = true;
        /// <summary>
        /// Orientation of the brick: horizontal or vertical
        /// </summary>
        public eBrickOrientation mOrientation = eBrickOrientation.Horizontal;
        /// <summary>
        /// Size of the brick: S, M, L
        /// </summary>
        private eBrickSize mBrickSize = eBrickSize.Big;
        /// <summary>
        /// Type of prize this brick contains when broken. None as default
        /// </summary>
        public Prize.ePrize mPrize = Prize.ePrize.None;
        /// <summary>
        /// BREAKABLE BRICKS: Collection of rectangles for each sprite, using the orientation and brick size as keys
        /// </summary>
        public static Dictionary<eBrickOrientation, Dictionary<eBrickSize, SMX.Maths.Rectangle>> mSourceRectsBreakable = new Dictionary<eBrickOrientation, Dictionary<eBrickSize, Rectangle>>();
        /// <summary>
        /// NON-BREAKABLE BRICKS: Collection of rectangles for each sprite, using the orientation and brick size as keys
        /// </summary>
        public static Dictionary<eBrickOrientation, Dictionary<eBrickSize, SMX.Maths.Rectangle>> mSourceRectsNonBreakable = new Dictionary<eBrickOrientation, Dictionary<eBrickSize, Rectangle>>();
        /// <summary>
        /// Base texture with all sprites for bricks (resource name)
        /// </summary>
        public static string mTextureResourceName;
        /// <summary>
        /// Base texture with all sprites for bricks (stream of bytes)
        /// </summary>
        public static System.IO.Stream mTextureStream;

        #region Props
        /// <summary>
        /// Property to give quick access to current sprite rectangle, basing on brick breakability, orientation and brick size
        /// </summary>
        private SMX.Maths.Rectangle CurrentSourceRect
        {
            get
            {
                SMX.Maths.Rectangle rect;
                if (mBreakable)
                    rect = mSourceRectsBreakable[mOrientation][mBrickSize];
                else rect = mSourceRectsNonBreakable[mOrientation][mBrickSize];
                return rect;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pOrientation"></param>
        /// <param name="pSize"></param>
        /// <param name="pBreakable"></param>
        /// <param name="pPrize"></param>
        /// <param name="pMainForm"></param>
        public Brick(eBrickOrientation pOrientation, eBrickSize pSize, bool pBreakable, Prize.ePrize pPrize) : base ()
        {
            mScaleFactorX = 2f;
            mScaleFactorY = 2f;

            mOrientation = pOrientation;
            mBrickSize = pSize;
            mBreakable = pBreakable;
            mPrize = pPrize;
          
            // Create rectangles for brick sprites
            if (mSourceRectsBreakable == null || mSourceRectsBreakable.Count == 0)
            {
                Dictionary<eBrickSize, SMX.Maths.Rectangle> dictHoriz, dictVert;
                dictHoriz = new Dictionary<eBrickSize, SMX.Maths.Rectangle>();
                dictVert = new Dictionary<eBrickSize, SMX.Maths.Rectangle>();

                dictHoriz.Add(eBrickSize.Big, new Rectangle { X = 9, Y = 8, Width = 63, Height = 14 });
                dictHoriz.Add(eBrickSize.Med, new Rectangle { X = 153, Y = 7, Width = 34, Height = 15 });
                dictHoriz.Add(eBrickSize.Small, new Rectangle { X = 241, Y = 7, Width = 21, Height = 15 });

                dictVert.Add(eBrickSize.Big, new Rectangle { X = 122, Y = 42, Width = 14, Height = 111 });
                dictVert.Add(eBrickSize.Med, new Rectangle { X = 147, Y = 107, Width = 14, Height = 46 });
                dictVert.Add(eBrickSize.Small, new Rectangle { X = 174, Y = 121, Width = 14, Height = 32 });

                mSourceRectsBreakable.Add(eBrickOrientation.Horizontal, dictHoriz);
                mSourceRectsBreakable.Add(eBrickOrientation.Vertical, dictVert);
            }

            if (mSourceRectsNonBreakable == null || mSourceRectsNonBreakable.Count == 0)
            {
                Dictionary<eBrickSize, SMX.Maths.Rectangle> dictHoriz, dictVert;
                dictHoriz = new Dictionary<eBrickSize, SMX.Maths.Rectangle>();
                dictVert = new Dictionary<eBrickSize, SMX.Maths.Rectangle>();

                dictHoriz.Add(eBrickSize.Big, new Rectangle { X = 80, Y = 8, Width = 62, Height = 14 });
                dictHoriz.Add(eBrickSize.Med, new Rectangle { X = 195, Y = 7, Width = 34, Height = 15 });
                dictHoriz.Add(eBrickSize.Small, new Rectangle { X = 274, Y = 7, Width = 22, Height = 15 });

                dictVert.Add(eBrickSize.Big, new Rectangle { X = 9, Y = 41, Width = 14, Height = 111 });
                dictVert.Add(eBrickSize.Med, new Rectangle { X = 35, Y = 106, Width = 14, Height = 46 });
                dictVert.Add(eBrickSize.Small, new Rectangle { X = 62, Y = 121, Width = 14, Height = 31 });

                mSourceRectsNonBreakable.Add(eBrickOrientation.Horizontal, dictHoriz);
                mSourceRectsNonBreakable.Add(eBrickOrientation.Vertical, dictVert);
            }

        }
        /// <summary>
        /// Contents loading
        /// </summary>
        public override void LoadContents()
        {
            // This is a singleton, so only load this once
            if (mTextureStream == null)
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                mTextureResourceName = Game.ResourcesNameSpace + ".Resources.Bricks.png";

                mTextureStream = assembly.GetManifestResourceStream(mTextureResourceName);
                RendererBase.Ref.LoadTexture(mTextureResourceName, mTextureStream);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void RefreshDrawRectangle()
        {
            SMX.Maths.Rectangle rect = CurrentSourceRect;
            mDrawRectangle = new SMX.Maths.Rectangle((int)mPos.X, (int)mPos.Y, (int)((float)rect.Width * mScaleFactorX), (int)((float)rect.Height * mScaleFactorY));
        }
        /// <summary>
        /// 
        /// </summary>
        public override void OnFrameRender()
        {
            RefreshDrawRectangle();

            var rect = CurrentSourceRect;
            RendererBase.Ref.DrawSprite(mTextureResourceName, rect.X, rect.Y, rect.Width, rect.Height, mDrawRectangle.X, mDrawRectangle.Y, mDrawRectangle.Width, mDrawRectangle.Height);
        }

        /// <summary>
        /// Logic update for bricks 
        /// </summary>
        /// <param name="pDt"></param>
        public override void OnFrameMove(float pDt)
        {


        }
        /// <summary>
        /// Creates and configures a new brick, from an xml node
        /// </summary>
        /// <param name="pBrickNd"></param>
        /// <returns></returns>
        public static Brick FromXml(System.Xml.XmlNode pBrickNd)
        {
            eBrickSize size = (eBrickSize)Enum.Parse(typeof(eBrickSize), pBrickNd.Attributes["Size"].Value);
            eBrickOrientation ori = (eBrickOrientation)Enum.Parse(typeof(eBrickOrientation), pBrickNd.Attributes["Orientation"].Value);
            Prize.ePrize prize = Prize.ePrize.None;
            if (pBrickNd.Attributes["Prize"] != null)
                prize = (Prize.ePrize)Enum.Parse(typeof(Prize.ePrize), pBrickNd.Attributes["Prize"].Value);
            bool breakable = bool.Parse(pBrickNd.Attributes["Breakable"].Value);
            Brick brk = new Brick(ori, size, breakable, prize);
            brk.mPos = SMX.Maths.Vector2.ReadVector2FromXmlAttribute(pBrickNd, "Position");
            return brk;
        }
    }

}
