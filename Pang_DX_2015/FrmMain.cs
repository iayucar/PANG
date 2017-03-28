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
        public SoundBase mSound;

        /// <summary>
        /// 
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();


            this.Location = new Point();

            // Instantiate API dependent stuff
            mRenderer = new RendererDX(this.doubleBufferPanel1);
            mInput = new InputWPF();
            mSound = new SoundWPF();

            // Load Contents
            Game.LoadContents();
            Game.GameOver += Game_GameOver;
            mRenderer.SetGameWindow(Game.DefaultGameWidth, Game.DefaultGameHeight, this.doubleBufferPanel1.ClientRectangle.Width, this.doubleBufferPanel1.ClientRectangle.Height);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Game_GameOver(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Game Over");
            this.Close();
        }

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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void doubleBufferPanel1_Resize(object sender, EventArgs e)
        {
            // If screen size changes, refresh parameters for coords calculation
            mRenderer.SetGameWindow(Game.DefaultGameWidth, Game.DefaultGameHeight, this.doubleBufferPanel1.ClientRectangle.Width, this.doubleBufferPanel1.ClientRectangle.Height);

        }
  

    }

}
