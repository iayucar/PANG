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
    public class Ladder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMainForm"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Ladder(int x, int y, int width, int height)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public void LoadContents()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDt"></param>
        public void OnFrameMove(float pDt)
        {

        }
        public void OnFrameRender()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nd"></param>
        /// <returns></returns>
        public static Ladder FromXml(System.Xml.XmlNode nd)
        {
            Vector2 pos = Vector2.ReadVector2FromXmlAttribute(nd, "Position");
            Vector2 size = Vector2.ReadVector2FromXmlAttribute(nd, "Size");
            Ladder lad = new Ladder( (int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
            return lad;
        }

    }

}
