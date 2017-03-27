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

namespace SMX
{
    public partial class FrmMain : Form
    {
        //public Dictionary<string, System.Drawing.Image> mTextures = new Dictionary<string, Image>();
        public RendererBase mRenderer;
        public InputBase mInput;


        /// <summary>
        /// 
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();


            this.Location = new Point();


            mRenderer = new RendererDX(this.doubleBufferPanel1);
            mInput = new InputWPF();

            Game.LoadContents();
            mRenderer.SetGameWindow(Game.DefaultGameWidth, Game.DefaultGameHeight, this.doubleBufferPanel1.ClientRectangle.Width, this.doubleBufferPanel1.ClientRectangle.Height);

        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //private float CalcDt()
        //{
        //    // A hi-resolution timer would be used in normal circumstances
        //    mStopwatch.Stop();
        //    System.TimeSpan span = mStopwatch.Elapsed;
        //    mStopwatch.Reset();
        //    mStopwatch.Start();
        //    float dt = (float)span.TotalSeconds;

        //    // Filter out negative or too big DTs to protect the code
        //    dt = Math.Max(0.00001f, dt);
        //    dt = Math.Min(0.15f, dt);

        //    return dt;
        //}
        /// <summary>
        /// 
        /// </summary>
        public void Dostep()
        {
            // Update Frame
            Game.OnFrameMove();

            // Render Frame
            (mRenderer as RendererDX).BeginDraw();
            Game.OnFrameRender();
#if(DEBUG)
            mRenderer.DrawText(Game.mFPS.ToString(), eTextSize._24, 10, 10, Game.mFPS > 30 ? SMX.Maths.Color4.Yellow : SMX.Maths.Color4.RedStandard);
#endif
            (mRenderer as RendererDX).EndDraw();
        }
        /// <summary>
        /// 
        /// </summary>
        public void GameOver()
        {
            System.Windows.Forms.MessageBox.Show("Game Over");
        }      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void doubleBufferPanel1_MouseDown(object sender, MouseEventArgs e)
        {
        }
        


        //#region Screen Coords calculations
        //protected float mScaleX, mScaleY;
        //protected float mOffsetX, mOffsetY;
        //public int mGameWidth, mGameHeight;
        //public int mScreenWidth, mScreenHeight;

      
        ///// <summary>
        ///// Este método es una copia del propio código del PictureBox, mirado en el codigo fuente de .Net
        ///// </summary>
        ///// <param name="pContainerClientRectangleWidth"></param>
        ///// <param name="pContainerClientRectangleHeight"></param>
        ///// <param name="pImageWidth"></param>
        ///// <param name="pImageHeight"></param>
        ///// <returns></returns>
        //public static SMX.Maths.Rectangle CalcZoomRectangle(int pContainerClientRectangleWidth, int pContainerClientRectangleHeight, int pImageWidth, int pImageHeight)
        //{
        //    SMX.Maths.Rectangle ret = new SMX.Maths.Rectangle();

        //    float ratio = Math.Min((float)pContainerClientRectangleWidth / (float)pImageWidth, (float)pContainerClientRectangleHeight / (float)pImageHeight);
        //    ret.Width = (int)(pImageWidth * ratio);
        //    ret.Height = (int)(pImageHeight * ratio);
        //    ret.X = (pContainerClientRectangleWidth - ret.Width) / 2;
        //    ret.Y = (pContainerClientRectangleHeight - ret.Height) / 2;
        //    return ret;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="pGameWidth"></param>
        ///// <param name="pGameHeight"></param>
        ///// <param name="pScreenWidth"></param>
        ///// <param name="pScreenHeight"></param>
        //public void SetGameWindow(int pGameWidth, int pGameHeight, int pScreenWidth, int pScreenHeight)
        //{
        //    mGameWidth = pGameWidth;
        //    mGameHeight = pGameHeight;
        //    mScreenWidth = pScreenWidth;
        //    mScreenHeight = pScreenHeight;

        //    SMX.Maths.Rectangle dR = CalcZoomRectangle(this.mScreenWidth, this.mScreenHeight, mGameWidth, mGameHeight);
        //    mOffsetX = dR.X;
        //    mOffsetY = dR.Y;
        //    mScaleX = (float)dR.Width / (float)mGameWidth;
        //    mScaleY = (float)dR.Height / (float)mGameHeight;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="pX"></param>
        ///// <param name="pY"></param>
        ///// <param name="pWidth"></param>
        ///// <param name="pHeight"></param>
        ///// <returns></returns>
        //public SMX.Maths.Rectangle GetScreenCoords(int pX, int pY, int pWidth, int pHeight)
        //{
        //    return new SMX.Maths.Rectangle((int)(((float)pX * mScaleX) + mOffsetX), (int)(((float)pY * mScaleY) + mOffsetY), (int)((float)pWidth * mScaleX), (int)((float)pHeight * mScaleY));
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="pX"></param>
        ///// <param name="pY"></param>
        ///// <param name="pWidth"></param>
        ///// <param name="pHeight"></param>
        ///// <returns></returns>
        //public SMX.Maths.Rectangle GetScreenCoords(float pX, float pY, float pWidth, float pHeight)
        //{
        //    return new SMX.Maths.Rectangle((int)((pX * mScaleX) + mOffsetX), (int)((pY * mScaleY) + mOffsetY), (int)(pWidth * mScaleX), (int)(pHeight * mScaleY));
        //}
        //#endregion

        #region Render
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void doubleBufferPanel1_Resize(object sender, EventArgs e)
        {
            // If screen size changes, refresh parameters for coords calculation

            mRenderer.SetGameWindow(Game.DefaultGameWidth, Game.DefaultGameHeight, this.doubleBufferPanel1.ClientRectangle.Width, this.doubleBufferPanel1.ClientRectangle.Height);

        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="pTextureName"></param>
        ///// <param name="pSrcX"></param>
        ///// <param name="pSrcY"></param>
        ///// <param name="pSrcWidth"></param>
        ///// <param name="pSrcHeight"></param>
        ///// <param name="pDestX"></param>
        ///// <param name="pDestY"></param>
        ///// <param name="pDestWidth"></param>
        ///// <param name="pDestHeight"></param>
        //public void DrawSprite(string pTextureName, int pSrcX, int pSrcY, int pSrcWidth, int pSrcHeight, float pDestX, float pDestY, float pDestWidth, float pDestHeight)
        //{
        //    mGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
        //    mGraphics.DrawImage(mTextures[pTextureName], GetScreenCoords(pDestX, pDestY, pDestWidth, pDestHeight).ToGDI(), pSrcX, pSrcY, pSrcWidth, pSrcHeight, GraphicsUnit.Pixel);
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="pTextureName"></param>
        ///// <param name="pSrcX"></param>
        ///// <param name="pSrcY"></param>
        ///// <param name="pSrcWidth"></param>
        ///// <param name="pSrcHeight"></param>
        ///// <param name="pDestX"></param>
        ///// <param name="pDestY"></param>
        ///// <param name="pDestWidth"></param>
        ///// <param name="pDestHeight"></param>
        //public void DrawSprite(string pTextureName, float pDestX, float pDestY, float pDestWidth, float pDestHeight)
        //{
        //    Image img = mTextures[pTextureName];
        //    mGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
        //    mGraphics.DrawImage(img, GetScreenCoords(pDestX, pDestY, pDestWidth, pDestHeight).ToGDI(), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="pX"></param>
        ///// <param name="pY"></param>
        ///// <param name="pDestX"></param>
        ///// <param name="pDestY"></param>
        ///// <param name="pWidth"></param>
        ///// <param name="pDashed"></param>
        ///// <param name="pColor"></param>
        //public void DrawLine(float pX, float pY, float pDestX, float pDestY, float pWidth, bool pDashed, SMX.Maths.Color4 pColor)
        //{
        //    Pen pen = new Pen(pColor.ToGDI(), pWidth);
        //    if (pDashed)
        //        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        //    else pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

        //    Point start = GetScreenCoords(pX, pY, 1, 1).ToGDI().Location;
        //    Point end = GetScreenCoords(pDestX, pDestY, 1, 1).ToGDI().Location;
        //    mGraphics.DrawLine(pen, start, end);
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="pText"></param>
        ///// <param name="pSize"></param>
        ///// <param name="pX"></param>
        ///// <param name="pY"></param>
        ///// <param name="pColor"></param>
        //public void DrawText(string pText, int pSize, int pX, int pY, SMX.Maths.Color4 pColor)
        //{
        //    System.Drawing.Font font = new Font("Arial", pSize, FontStyle.Bold);
        //    SolidBrush brush = new SolidBrush(pColor.ToGDI());

        //    SMX.Maths.Rectangle r = GetScreenCoords(pX, pY, 0, 0);
        //    mGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
        //    mGraphics.DrawString(pText, font, brush, new PointF(r.X, r.Y));
        //}
        /// <summary>
        /// 
        /// </summary>
        private void DrawDebugInfoPang(PaintEventArgs e)
        {
            //Game pangGame = mGame as Game;
            //if (pangGame.CurrentLevel != null)
            //{
            //    Rectangle r;
            //    foreach (Ball b in pangGame.CurrentLevel.mBalls)
            //    {
            //        r = GetScreenCoords(b.Center.X - b.Radius, b.Center.Y - b.Radius, b.Radius * 2, b.Radius * 2).ToGDI();
            //        e.Graphics.DrawEllipse(Pens.Yellow, r);
            //    }

            //    foreach (Brick b in pangGame.CurrentLevel.mBricks)
            //    {
            //        r = GetScreenCoords(b.mDrawRectangle.X, b.mDrawRectangle.Y, b.mDrawRectangle.Width, b.mDrawRectangle.Height).ToGDI();
            //        e.Graphics.DrawRectangle(Pens.Blue, r);
            //    }

            //    foreach (Arrow b in pangGame.CurrentLevel.mArrows)
            //    {
            //        r = GetScreenCoords(b.mDrawRectangle.X, b.mDrawRectangle.Y, b.mDrawRectangle.Width, b.mDrawRectangle.Height).ToGDI();
            //        e.Graphics.DrawRectangle(Pens.Green, r);
            //    }

            //    foreach (Ladder b in pangGame.CurrentLevel.mLadders)
            //    {
            //        r = GetScreenCoords(b.mDrawingRectangle.X, b.mDrawingRectangle.Y, b.mDrawingRectangle.Width, b.mDrawingRectangle.Height).ToGDI();
            //        e.Graphics.DrawRectangle(Pens.Green, r);
            //    }

            //    if (pangGame.mPlayer != null)
            //    {
            //        r = GetScreenCoords(pangGame.mPlayer.mDrawRectangle.X, pangGame.mPlayer.mDrawRectangle.Y, pangGame.mPlayer.mDrawRectangle.Width, pangGame.mPlayer.mDrawRectangle.Height).ToGDI();
            //        e.Graphics.DrawRectangle(Pens.Red, r);
            //    }


            //}

        }
        #endregion

        //#region Texture Management
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="pTextureName"></param>
        ///// <param name="pStream"></param>
        //public void LoadTexture(string pTextureName, System.IO.Stream pStream)
        //{
        //    if (mTextures.ContainsKey(pTextureName))
        //        return;

        //    System.Drawing.Image img = System.Drawing.Image.FromStream(pStream);
        //    mTextures.Add(pTextureName, img);
        //}
        //#endregion
    

    }

}
