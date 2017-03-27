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

namespace SMX
{
    public class RendererBase
    {       
        protected float mScaleX, mScaleY;
        protected float mOffsetX, mOffsetY;
        public int mGameWidth, mGameHeight;
        public int mScreenWidth, mScreenHeight;
        public static RendererBase Ref;

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {

        }
        /// <summary>
        /// Loads a texture
        /// </summary>
        /// <param name="pTextureName"></param>
        /// <param name="pStream"></param>
        public virtual void LoadTexture(string pTextureName, System.IO.Stream pStream)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pGameWidth"></param>
        /// <param name="pGameHeight"></param>
        /// <param name="pScreenWidth"></param>
        /// <param name="pScreenHeight"></param>
        public virtual void SetGameWindow(int pGameWidth, int pGameHeight, int pScreenWidth, int pScreenHeight)
        {
            mGameWidth = pGameWidth;
            mGameHeight = pGameHeight;
            mScreenWidth = pScreenWidth;
            mScreenHeight = pScreenHeight;

            SMX.Maths.Rectangle dR = CalcZoomRectangle(this.mScreenWidth, this.mScreenHeight, mGameWidth, mGameHeight);
            mOffsetX = dR.X;
            mOffsetY = dR.Y;
            mScaleX = (float)dR.Width / (float)mGameWidth;
            mScaleY = (float)dR.Height / (float)mGameHeight;
        }
        /// <summary>
        /// Este método es una copia del propio código del PictureBox, mirado en el codigo fuente de .Net
        /// </summary>
        /// <param name="pContainerClientRectangleWidth"></param>
        /// <param name="pContainerClientRectangleHeight"></param>
        /// <param name="pImageWidth"></param>
        /// <param name="pImageHeight"></param>
        /// <returns></returns>
        public static SMX.Maths.Rectangle CalcZoomRectangle(int pContainerClientRectangleWidth, int pContainerClientRectangleHeight, int pImageWidth, int pImageHeight)
        {
            SMX.Maths.Rectangle ret = new SMX.Maths.Rectangle();

            float ratio = Math.Min((float)pContainerClientRectangleWidth / (float)pImageWidth, (float)pContainerClientRectangleHeight / (float)pImageHeight);
            ret.Width = (int)(pImageWidth * ratio);
            ret.Height = (int)(pImageHeight * ratio);
            ret.X = (pContainerClientRectangleWidth - ret.Width) / 2;
            ret.Y = (pContainerClientRectangleHeight - ret.Height) / 2;
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        /// <param name="pWidth"></param>
        /// <param name="pHeight"></param>
        /// <returns></returns>
        public SMX.Maths.Vector2 GetScreenCoords(int pX, int pY)
        {
            return new SMX.Maths.Vector2(((float)pX * mScaleX) + mOffsetX, ((float)pY * mScaleY) + mOffsetY);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        /// <param name="pWidth"></param>
        /// <param name="pHeight"></param>
        /// <returns></returns>
        public SMX.Maths.Vector2 GetScreenCoords(float pX, float pY)
        {
            return new SMX.Maths.Vector2((pX * mScaleX) + mOffsetX, (pY * mScaleY) + mOffsetY);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        /// <param name="pWidth"></param>
        /// <param name="pHeight"></param>
        /// <returns></returns>
        public SMX.Maths.Rectangle GetScreenCoords(int pX, int pY, int pWidth, int pHeight)
        {
            return new SMX.Maths.Rectangle((int)(((float)pX * mScaleX) + mOffsetX), (int)(((float)pY * mScaleY) + mOffsetY), (int)((float)pWidth * mScaleX), (int)((float)pHeight * mScaleY));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        /// <param name="pWidth"></param>
        /// <param name="pHeight"></param>
        /// <returns></returns>
        public SMX.Maths.Rectangle GetScreenCoords(float pX, float pY, float pWidth, float pHeight)
        {
            return new SMX.Maths.Rectangle((int)((pX * mScaleX) + mOffsetX), (int)((pY * mScaleY) + mOffsetY), (int)(pWidth * mScaleX), (int)(pHeight * mScaleY));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pTextureName"></param>
        /// <param name="pSrcX"></param>
        /// <param name="pSrcY"></param>
        /// <param name="pSrcWidth"></param>
        /// <param name="pSrcHeight"></param>
        /// <param name="pDestX"></param>
        /// <param name="pDestY"></param>
        /// <param name="pDestWidth"></param>
        /// <param name="pDestHeight"></param>
        public virtual void DrawSprite(string pTextureName, int pSrcX, int pSrcY, int pSrcWidth, int pSrcHeight, float pDestX, float pDestY, float pDestWidth, float pDestHeight)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pTextureName"></param>
        /// <param name="pSrcX"></param>
        /// <param name="pSrcY"></param>
        /// <param name="pSrcWidth"></param>
        /// <param name="pSrcHeight"></param>
        /// <param name="pDestX"></param>
        /// <param name="pDestY"></param>
        /// <param name="pDestWidth"></param>
        /// <param name="pDestHeight"></param>
        public virtual void DrawSprite(string pTextureName, float pDestX, float pDestY, float pDestWidth, float pDestHeight)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        /// <param name="pDestX"></param>
        /// <param name="pDestY"></param>
        /// <param name="pWidth"></param>
        /// <param name="pDashed"></param>
        /// <param name="pColor"></param>
        public virtual void DrawLine(float pX, float pY, float pDestX, float pDestY, float pWidth, bool pDashed, SMX.Maths.Color4 pColor)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pText"></param>
        /// <param name="pSize"></param>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        /// <param name="pColor"></param>
        public virtual void DrawText(string pText, eTextSize pSize, int pX, int pY, SMX.Maths.Color4 pColor)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        /// <param name="pWidth"></param>
        /// <param name="pHeight"></param>
        /// <param name="pColor"></param>
        public virtual void FillRectangle(float pX, float pY, float pWidth, float pHeight, SMX.Maths.Color4 pColor)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void BeginDraw()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void EndDraw()
        {

        }
    }
}
