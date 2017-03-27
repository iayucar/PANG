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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMX
{
    public class InputGDI : InputBase
    {
        private bool mLeftPressed;
        private bool mRightPressed;
        private bool mUpPressed;
        private bool mDownPressed;
        private bool mFirePressed;


        /// <summary>
        /// 
        /// </summary>
        public InputGDI()
        {
            InputBase.Ref = this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pAction"></param>
        /// <returns></returns>
        public override bool IsActionPressed(eAction pAction)
        {
            switch (pAction)
            {
                case eAction.Left:
                    return mLeftPressed;
                case eAction.Right:
                    return mRightPressed;
                case eAction.Up:
                    return mUpPressed;
                case eAction.Down:
                    return mDownPressed;
                case eAction.Fire:
                    return mFirePressed;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void PreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Left:
                    mLeftPressed = true;
                    break;
                case Keys.Right:
                    mRightPressed = true;
                    break;
                case Keys.Up:
                    mUpPressed = true;
                    break;
                case Keys.Down:
                    mDownPressed = true;
                    break;
                case Keys.Z:
                    mFirePressed = true;
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void KeyUp(KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Left:
                    mLeftPressed = false;
                    break;
                case Keys.Right:
                    mRightPressed = false;
                    break;
                case Keys.Up:
                    mUpPressed = false;
                    break;
                case Keys.Down:
                    mDownPressed = false;
                    break;
                case Keys.Z:
                    mFirePressed = false;
                    break;
            }
        }
    }
}
