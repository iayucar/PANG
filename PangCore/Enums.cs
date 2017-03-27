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
    /// <summary>
    /// Size of text to be drawn on the screen
    /// </summary>
    public enum eTextSize
    {
        _14 = 14,
        _16 = 16,
        _18 = 18,
        _20 = 20,
        _22 = 22,
        _24 = 24,
        _26 = 26,
        _28 = 28,
    }
    /// <summary>
    /// Possible sizes of balls in game
    /// </summary>
    public enum eBallSize
    {
        XL,
        L,
        M,
        S
    }
    /// <summary>
    /// Types of balls in game
    /// </summary>
    public enum eBallType
    {
        Red,
        Green,
        Blue,
    }
    /// <summary>
    /// Possible orientations of bricks
    /// </summary>
    public enum eBrickOrientation
    {
        Horizontal,
        Vertical,
    }
    /// <summary>
    /// Size of bricks in game
    /// </summary>
    public enum eBrickSize
    {
        Big,
        Med,
        Small,
    }
}
