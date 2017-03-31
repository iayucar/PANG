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


            // Initialize API-Dependent stuff
            mRenderer = new RendererGDI();
            mInput = new InputGDI();
            mSound = new SoundBase();

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

            // Frame Move
            Game.OnFrameMove();

            // Render
            this.doubleBufferPanel1.Refresh();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void doubleBufferPanel1_Paint(object sender, PaintEventArgs e)
        {
            (mRenderer as RendererGDI).mGraphics = e.Graphics;
            
            Game.OnFrameRender();

            e.Graphics.DrawString(Game.mFPS.ToString(), (mRenderer as RendererGDI).mTextFont, Game.mFPS > 30 ? Brushes.Yellow : Brushes.Red, new PointF(10, 10));
        }
        /// <summary>
        /// Handle keydown events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            (mInput as InputGDI).PreviewKeyDown(e);
        }
        /// <summary>
        /// Handle keyup events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            (mInput as InputGDI).KeyUp(e);
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
