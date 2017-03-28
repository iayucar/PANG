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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SMX.Maths;

namespace SMX 
{
    public class RendererGDI : RendererBase
    {
        public Dictionary<string, System.Drawing.Image> mTextures = new Dictionary<string, Image>();
        public System.Drawing.Graphics mGraphics;
        public Font mTextFont;


        /// <summary>
        /// 
        /// </summary>
        public RendererGDI()
        {
            mTextFont = new Font("Arial", 16, FontStyle.Bold);

            RendererBase.Ref = this;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            foreach(System.Drawing.Image img in mTextures.Values)
                img.Dispose();
            mTextures.Clear();

            mTextFont.Dispose();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pTextureName"></param>
        /// <param name="pStream"></param>
        public override void LoadTexture(string pTextureName, System.IO.Stream pStream)
        {
            if (mTextures.ContainsKey(pTextureName))
                return;

            System.Drawing.Image img = System.Drawing.Image.FromStream(pStream);
            mTextures.Add(pTextureName, img);
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
        public override void DrawSprite(string pTextureName, int pSrcX, int pSrcY, int pSrcWidth, int pSrcHeight, float pDestX, float pDestY, float pDestWidth, float pDestHeight)
        {
            mGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            mGraphics.DrawImage(mTextures[pTextureName], GetScreenCoords(pDestX, pDestY, pDestWidth, pDestHeight).ToGDI(), pSrcX, pSrcY, pSrcWidth, pSrcHeight, GraphicsUnit.Pixel);
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
        public override void DrawSprite(string pTextureName, float pDestX, float pDestY, float pDestWidth, float pDestHeight)
        {
            Image img = mTextures[pTextureName];
            mGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            mGraphics.DrawImage(img, GetScreenCoords(pDestX, pDestY, pDestWidth, pDestHeight).ToGDI(), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
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
        public override void DrawLine(float pX, float pY, float pDestX, float pDestY, float pWidth, bool pDashed, SMX.Maths.Color4 pColor)
        {
            Pen pen = new Pen(pColor.ToGDI(), pWidth);
            if (pDashed)
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            else pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

            System.Drawing.Point start = GetScreenCoords(pX, pY, 1, 1).ToGDI().Location;
            System.Drawing.Point end = GetScreenCoords(pDestX, pDestY, 1, 1).ToGDI().Location;
            mGraphics.DrawLine(pen, start, end);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        /// <param name="pRadius"></param>
        /// <param name="pColor"></param>
        public override void DrawCircle(float pX, float pY, float pRadius, float pWidth, bool pDashed, Maths.Color4 pColor)
        {
            SMX.Maths.Vector2 start = GetScreenCoords(pX, pY);
            SMX.Maths.Vector2 radScreen = GetScreenCoords(pRadius, pRadius);


            Pen pen = new Pen(pColor.ToGDI(), pWidth);
            if (pDashed)
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            else pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

            mGraphics.DrawEllipse(pen, start.X, start.Y, radScreen.X, radScreen.Y);
          
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pText"></param>
        /// <param name="pSize"></param>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        /// <param name="pColor"></param>
        public override void DrawText(string pText, eTextSize pSize, int pX, int pY, SMX.Maths.Color4 pColor)
        {
            System.Drawing.Font font = new Font("Arial", (int)pSize, FontStyle.Bold);
            SolidBrush brush = new SolidBrush(pColor.ToGDI());

            SMX.Maths.Rectangle r = GetScreenCoords(pX, pY, 0, 0);
            mGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            mGraphics.DrawString(pText, font, brush, new PointF(r.X, r.Y));
        }
      
    }
}
