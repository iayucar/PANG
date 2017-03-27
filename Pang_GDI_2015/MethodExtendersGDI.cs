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
using System.Drawing;

namespace SMX.Maths
{
	/// <summary>
	/// Contains commonly used precalculated values and numeric calculations.
	/// </summary>
	public static class MethodExtendersGDI
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDoc"></param>
        /// <param name="pNode"></param>
        /// <param name="pVec"></param>
        public static void ToXmlAttribute(this System.Drawing.Color pCol, System.Xml.XmlDocument pDoc, System.Xml.XmlNode pNode, string pAttribNamePrefixToRGBA)
        {
            System.Xml.XmlAttribute att = pDoc.CreateAttribute(pAttribNamePrefixToRGBA + "R");
            att.InnerText = pCol.R.ToString();
            pNode.Attributes.Append(att);

            att = pDoc.CreateAttribute(pAttribNamePrefixToRGBA + "G");
            att.InnerText = pCol.G.ToString();
            pNode.Attributes.Append(att);

            att = pDoc.CreateAttribute(pAttribNamePrefixToRGBA + "B");
            att.InnerText = pCol.B.ToString();
            pNode.Attributes.Append(att);

            att = pDoc.CreateAttribute(pAttribNamePrefixToRGBA + "A");
            att.InnerText = pCol.A.ToString();
            pNode.Attributes.Append(att);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Color ToGDI(this SMX.Maths.Color4 pSrc)
        {
            return Color.FromArgb((int)(pSrc.Alpha * 255.0), (int)(pSrc.Red * 255.0), (int)(pSrc.Green * 255.0), (int)(pSrc.Blue * 255.0));
        }

        /// <summary>
        /// 
        /// </summary>
        public static System.Drawing.Rectangle ToGDI(this SMX.Maths.Rectangle pSrc)
        {
            return new System.Drawing.Rectangle(pSrc.X, pSrc.Y, pSrc.Width, pSrc.Height);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pgdi"></param>
        /// <returns></returns>
        public static Rectangle FromGDI(System.Drawing.Rectangle pgdi)
        {
            return new Rectangle(pgdi.X, pgdi.Y, pgdi.Width, pgdi.Height);
        }
    }
}
