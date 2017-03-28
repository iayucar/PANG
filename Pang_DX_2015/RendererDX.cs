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
using SharpDX;
using SharpDX.Direct3D10;
using SharpDX.Direct2D1;
using DXGI = SharpDX.DXGI;
using SMX.Maths;


namespace SMX 
{
    public class RendererDX : RendererBase
    {

        public Dictionary<string, Bitmap> mTextures = new Dictionary<string, Bitmap>();
        public SMX.DoubleBufferPanel mRenderPanel;
        public event System.EventHandler Exit;

        /// <summary>
        /// En SDX, vamos a dibujar siempre a FullHd, ya que el re-escalado se hace directamente al hacer presetnSwapChain
        /// </summary>
        public int mRenderWidth = 1920;
        /// <summary>
        /// En SDX, vamos a dibujar siempre a FullHd, ya que el re-escalado se hace directamente al hacer presetnSwapChain
        /// </summary>
        public int mRenderHeight = 1080;

        private SharpDX.Direct3D10.Device1 mDevice;
        private DXGI.SwapChain mSwapChain;
        private Texture2D mBackBuffer;
        private RenderTargetView mBackBufferView;
        public Factory mFactory2D { get; private set; }
        public SharpDX.DirectWrite.Factory mFactoryDWrite { get; private set; }
        public RenderTarget mRenderTarget2D { get; private set; }
        public SolidColorBrush SceneColorBrush { get; private set; }
        public bool mResizeBuffersOnNextFrame = false;
        private Dictionary<eTextSize, SharpDX.DirectWrite.TextFormat> mTextFormats = new Dictionary<eTextSize, SharpDX.DirectWrite.TextFormat>();

        #region Props
        /// <summary>
        /// Returns the backbuffer used by the SwapChain
        /// </summary>
        public Texture2D BackBuffer
        {
            get
            {
                return mBackBuffer;
            }
        }
        /// <summary>
        /// Return the Handle to display to.
        /// </summary>
        protected IntPtr DisplayHandle
        {
            get
            {
                return mRenderPanel.Handle;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public RendererDX(SMX.DoubleBufferPanel pRenderPanel)
        {
            RendererBase.Ref = this;

            mRenderPanel = pRenderPanel;

            // Init D3D: fixed rendering at 1920x1080
            //InitializeD3D10(mRenderWidth, mRenderHeight); //mRenderPanel.Width, mRenderPanel.Height);
            InitializeD3D10(mRenderPanel.Width, mRenderPanel.Height);

            // Init D2D
            InitializeD2D();
        }
        /// <summary>
        /// 
        /// </summary>
        protected void InitializeD3D10(int pWidth, int pHeight)
        {
            // SwapChain description
            var desc = new DXGI.SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription = new DXGI.ModeDescription(pWidth, pHeight, new DXGI.Rational(60, 1), DXGI.Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = DisplayHandle,
                SampleDescription = new DXGI.SampleDescription(1, 0),
                SwapEffect = DXGI.SwapEffect.Discard,
                Usage = DXGI.Usage.RenderTargetOutput
            };

            // Create Device and SwapChain
            SharpDX.Direct3D10.Device1.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, desc, SharpDX.Direct3D10.FeatureLevel.Level_10_0, out mDevice, out mSwapChain);

            // Ignore all windows events
            DXGI.Factory factory = mSwapChain.GetParent<DXGI.Factory>();
            factory.MakeWindowAssociation(DisplayHandle, DXGI.WindowAssociationFlags.IgnoreAll);

            // New RenderTargetView from the backbuffer
            mBackBuffer = Texture2D.FromSwapChain<Texture2D>(mSwapChain, 0);
            mBackBufferView = new RenderTargetView(mDevice, mBackBuffer);

        }
        /// <summary>
        /// 
        /// </summary>
        private void InitializeD2D()
        {
            mFactory2D = new SharpDX.Direct2D1.Factory();
            using (var surface = BackBuffer.QueryInterface<DXGI.Surface>())
            {
                mRenderTarget2D = new RenderTarget(mFactory2D, surface, new RenderTargetProperties(new PixelFormat(DXGI.Format.Unknown, AlphaMode.Premultiplied)));
            }
            mRenderTarget2D.AntialiasMode = AntialiasMode.PerPrimitive;

            mFactoryDWrite = new SharpDX.DirectWrite.Factory();

            SceneColorBrush = new SolidColorBrush(mRenderTarget2D, Color.White);

            // Initialize fonts
            foreach (eTextSize size in Enum.GetValues(typeof(eTextSize)))
                mTextFormats.Add(size, new SharpDX.DirectWrite.TextFormat(mFactoryDWrite, "Arial", (int)size));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pGameWidth"></param>
        /// <param name="pGameHeight"></param>
        /// <param name="pScreenWidth"></param>
        /// <param name="pScreenHeight"></param>
        public override void SetGameWindow(int pGameWidth, int pGameHeight, int pScreenWidth, int pScreenHeight)
        {
            base.SetGameWindow(pGameWidth, pGameHeight, pScreenWidth, pScreenHeight);


            mResizeBuffersOnNextFrame = true;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            foreach (Bitmap b in mTextures.Values)
                b.Dispose();
            mTextures.Clear();
            
            foreach (eTextSize size in mTextFormats.Keys)
                mTextFormats[size].Dispose();
            mTextFormats.Clear();
        }
        /// <summary>
        /// Loads a Direct2D Bitmap from a file using System.Drawing.Image.FromFile(...)
        /// </summary>
        /// <param name="renderTarget">The render target.</param>
        /// <param name="file">The file.</param>
        /// <returns>A D2D1 Bitmap</returns>
        public static Bitmap LoadFromStream(RenderTarget renderTarget, System.IO.Stream pStream)
        {
            // Loads from file using System.Drawing.Image
            using (var bitmap = (System.Drawing.Bitmap)System.Drawing.Image.FromStream(pStream))
            {
                var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bitmapProperties = new BitmapProperties(new PixelFormat(DXGI.Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied));
                var size = new Size2(bitmap.Width, bitmap.Height);

                // Transform pixels from BGRA to RGBA
                int stride = bitmap.Width * sizeof(int);
                using (var tempStream = new DataStream(bitmap.Height * stride, true, true))
                {
                    // Lock System.Drawing.Bitmap
                    var bitmapData = bitmap.LockBits(sourceArea, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                    // Convert all pixels 
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        int offset = bitmapData.Stride * y;
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            // Not optimized 
                            byte B = System.Runtime.InteropServices.Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte G = System.Runtime.InteropServices.Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte R = System.Runtime.InteropServices.Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte A = System.Runtime.InteropServices.Marshal.ReadByte(bitmapData.Scan0, offset++);
                            int rgba = R | (G << 8) | (B << 16) | (A << 24);
                            tempStream.Write(rgba);
                        }

                    }
                    bitmap.UnlockBits(bitmapData);
                    tempStream.Position = 0;

                    return new Bitmap(renderTarget, size, tempStream, stride, bitmapProperties);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pTextureName"></param>
        /// <param name="pStream"></param>
        public override void LoadTexture(string pTextureName, System.IO.Stream pStrm)
        {
            if (mTextures.ContainsKey(pTextureName))
                return;

            mTextures.Add(pTextureName, LoadFromStream(mRenderTarget2D, pStrm));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pTextureName"></param>
        /// <param name="pStream"></param>
        public void LoadTexture(string pTextureName, System.Reflection.Assembly pAssembly)
        {            
            if (mTextures.ContainsKey(pTextureName))
                return;

            
            System.IO.Stream strm = pAssembly.GetManifestResourceStream(pTextureName);
            LoadTexture(pTextureName, strm);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pTextureName"></param>
        /// <returns></returns>
        private void EnsureTextureExists(string pTextureName)
        {
#if(DEBUG)
            if (!mTextures.ContainsKey(pTextureName))
            {

                throw new System.ApplicationException("Texture not found: " + pTextureName);
            }
#endif
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
            EnsureTextureExists(pTextureName);

            Bitmap img = mTextures[pTextureName];


            // Draw the TextLayout
            RectangleF? destRect = GetScreenCoords(pDestX, pDestY, Math.Abs(pDestWidth), pDestHeight).ToSDXRectangleF();
            RectangleF? srcRect = new RectangleF(pSrcX, pSrcY, pSrcWidth, pSrcHeight);

            Matrix3x2 prev = mRenderTarget2D.Transform;
            Matrix3x2 newmat = Matrix3x2.Identity;

            // Check for horizontal mirror
            if (pDestWidth < 0)
            {
                newmat.M11 = -1;
                destRect = new RectangleF(destRect.Value.X * -1, destRect.Value.Y, destRect.Value.Width, destRect.Value.Height);
            }
            mRenderTarget2D.Transform = newmat;


            mRenderTarget2D.DrawBitmap(img, destRect, 1.0f, BitmapInterpolationMode.Linear, srcRect);
            mRenderTarget2D.Transform = prev;
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
            EnsureTextureExists(pTextureName);
            Bitmap img = mTextures[pTextureName];
            RectangleF destRect = GetScreenCoords(pDestX, pDestY, Math.Abs(pDestWidth), pDestHeight).ToSDXRectangleF();

            Matrix3x2 prev = mRenderTarget2D.Transform;
            Matrix3x2 newmat = Matrix3x2.Identity;

            // Check for horizontal mirror
            if (pDestWidth < 0)
            {
                newmat.M11 = -1;
                destRect = new RectangleF(destRect.X * -1, destRect.Y, destRect.Width, destRect.Height);
            }
            mRenderTarget2D.Transform = newmat;

            mRenderTarget2D.DrawBitmap(img, destRect, 1.0f, BitmapInterpolationMode.Linear);

            mRenderTarget2D.Transform = prev;
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
            SolidColorBrush brush = new SolidColorBrush(mRenderTarget2D, pColor.ToSDX());
            SharpDX.Vector2 start = GetScreenCoords(pX, pY).ToSDX();
            SharpDX.Vector2 end = GetScreenCoords(pDestX, pDestY).ToSDX();

            if (pDashed)
            {
                StrokeStyleProperties props = new StrokeStyleProperties();
                props.DashCap = CapStyle.Flat;
                props.EndCap = CapStyle.Flat;
                props.StartCap = CapStyle.Flat;
                props.DashOffset = 0;
                props.DashStyle = DashStyle.Dash;
                props.LineJoin = LineJoin.Round;
                SharpDX.Direct2D1.StrokeStyle strokeStyle = new StrokeStyle(mFactory2D, props);
                mRenderTarget2D.DrawLine(start, end, brush, pWidth, strokeStyle);
            }
            else mRenderTarget2D.DrawLine(start, end, brush, pWidth);
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
            SolidColorBrush brush = new SolidColorBrush(mRenderTarget2D, pColor.ToSDX());
            SharpDX.Vector2 start = GetScreenCoords(pX, pY).ToSDX();
            SMX.Maths.Vector2 radScreen = GetScreenCoords(pRadius, pRadius);

            if (pDashed)
            {
                StrokeStyleProperties props = new StrokeStyleProperties();
                props.DashCap = CapStyle.Flat;
                props.EndCap = CapStyle.Flat;
                props.StartCap = CapStyle.Flat;
                props.DashOffset = 0;
                props.DashStyle = DashStyle.Dash;
                props.LineJoin = LineJoin.Round;
                SharpDX.Direct2D1.StrokeStyle strokeStyle = new StrokeStyle(mFactory2D, props);
                mRenderTarget2D.DrawEllipse(new Ellipse(start, radScreen.X, radScreen.X), brush, pWidth, strokeStyle);
            }
            else mRenderTarget2D.DrawEllipse(new Ellipse(start, radScreen.X, radScreen.X), brush, pWidth);
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
            SharpDX.RectangleF r = GetScreenCoords(pX, pY, 9999, 9999).ToSDXRectangleF();

            mRenderTarget2D.DrawText(pText, mTextFormats[pSize], r, new SolidColorBrush(mRenderTarget2D, pColor.ToSDX()));
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="pText"></param>
        ///// <param name="pSize"></param>
        ///// <returns></returns>
        //public override SMX.Maths.Vector2 MeasureText(string pText, eTextSize pSize, float pMaxWidth = 99999, float pMaxHeight = 99999)
        //{
        //    SharpDX.DirectWrite.TextLayout layout = new SharpDX.DirectWrite.TextLayout(mFactoryDWrite, pText, mTextFormats[pSize], pMaxWidth, pMaxHeight);
        //    try
        //    {
        //        return new SMX.Maths.Vector2(layout.Metrics.Width, layout.Metrics.Height);
        //    }
        //    finally
        //    {
        //        if (layout != null)
        //            layout.Dispose();
        //    }
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="pText"></param>
        ///// <param name="pSize"></param>
        ///// <returns></returns>
        //public override SMX.Maths.Vector2 MeasureText(string[] pText, eTextSize pSize, float pLineMargin = 5, float pMaxWidth = 99999, float pMaxHeight = 99999)
        //{
        //    float maxWidth = float.MinValue, height = 0;
        //    foreach (string str in pText)
        //    {
        //        SharpDX.DirectWrite.TextLayout layout = new SharpDX.DirectWrite.TextLayout(mFactoryDWrite, str, mTextFormats[pSize], pMaxWidth, pMaxHeight);
        //        try
        //        {
        //            maxWidth = Math.Max(maxWidth, layout.Metrics.Width);
        //            height += layout.Metrics.Height + pLineMargin;
        //        }
        //        finally
        //        {
        //            if (layout != null)
        //                layout.Dispose();
        //        }
        //    }

        //    return new SMX.Maths.Vector2(maxWidth, height);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        /// <param name="pWidth"></param>
        /// <param name="pHeight"></param>
        /// <param name="pColor"></param>
        public override void FillRectangle(float pX, float pY, float pWidth, float pHeight, SMX.Maths.Color4 pColor)
        {
            Brush brush = new SolidColorBrush(mRenderTarget2D, pColor.ToSDX());
            mRenderTarget2D.FillRectangle(GetScreenCoords(pX, pY, pWidth, pHeight).ToSDXRectangleF(), brush);
        }
        /// <summary>
        /// 
        /// </summary>
        private void ResizeBuffers()
        {
            // Dispose
            if (mBackBuffer != null)
                mBackBuffer.Dispose();
            if (mBackBufferView != null)
                mBackBufferView.Dispose();
            if (mRenderTarget2D != null)
                mRenderTarget2D.Dispose();

            // Resize swap chain
            mSwapChain.ResizeBuffers(mSwapChain.Description.BufferCount, mScreenWidth, mScreenHeight, mSwapChain.Description.ModeDescription.Format, mSwapChain.Description.Flags);

            // Re-create
            mBackBuffer = Texture2D.FromSwapChain<Texture2D>(mSwapChain, 0);
            mBackBufferView = new RenderTargetView(mDevice, mBackBuffer);
            using (var surface = BackBuffer.QueryInterface<DXGI.Surface>())
            {
                mRenderTarget2D = new RenderTarget(mFactory2D, surface, new RenderTargetProperties(new PixelFormat(DXGI.Format.Unknown, AlphaMode.Premultiplied)));
            }
            mRenderTarget2D.AntialiasMode = AntialiasMode.PerPrimitive;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void BeginDraw()
        {
            if (mResizeBuffersOnNextFrame)
            {
                mResizeBuffersOnNextFrame = false;
                ResizeBuffers();
            }


            mDevice.Rasterizer.SetViewports(new Viewport(0, 0, mRenderWidth, mRenderHeight));
            mDevice.OutputMerger.SetTargets(mBackBufferView);

            mRenderTarget2D.BeginDraw();
            mRenderTarget2D.Clear(SMX.Maths.Color4.Black.ToSDX());
        }
        /// <summary>
        /// 
        /// </summary>
        public override void EndDraw()
        {
            mRenderTarget2D.EndDraw();

            bool WaitVerticalBlanking = false;
            mSwapChain.Present(WaitVerticalBlanking ? 1 : 0, DXGI.PresentFlags.None);
        }      

    }
}
