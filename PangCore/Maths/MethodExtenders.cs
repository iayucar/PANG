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

namespace SMX.Maths
{
	/// <summary>
	/// Contains commonly used precalculated values and numeric calculations.
	/// </summary>
	public static class MethodExtenders
	{
		/// <summary>
		/// Represents the smallest positive <see cref="System.Single"/> greater than zero. This field is constant.
		/// </summary>
		public const float Epsilon = 1.0e-6f;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pValue"></param>
        /// <returns></returns>
        public static float FloatFromString(string pValue)
        {
            return float.Parse(pValue, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pValue"></param>
        /// <returns></returns>
        public static double DoubleFromString(string pValue)
        {
            return double.Parse(pValue, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="pSrc"></param>
        /// <returns></returns>
        public static Dictionary<T, V> Clone<T, V>(this Dictionary<T, V> pSrc)
        {
            Dictionary<T, V> ret = new Dictionary<T, V>();
            foreach (T key in pSrc.Keys)
                ret.Add(key, pSrc[key]);
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pSrc"></param>
        /// <returns></returns>
        public static List<T> Clone<T>(this List<T> pSrc)
        {
            List<T> ret = new List<T>();
            foreach (T val in pSrc)
                ret.Add(val);

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="pDoc"></param>
        /// <param name="pNd"></param>
        /// <param name="pAttName"></param>
        public static void ToXmlAttribute(this string a, System.Xml.XmlDocument pDoc, System.Xml.XmlNode pNd, string pAttName)
        {
            System.Xml.XmlAttribute att = pDoc.CreateAttribute(pAttName);

            // To Simax string
            att.Value = a;

            pNd.Attributes.Append(att);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="pDoc"></param>
        /// <param name="pNd"></param>
        /// <param name="pAttName"></param>
        public static void ToXmlAttribute(this float a, System.Xml.XmlDocument pDoc, System.Xml.XmlNode pNd, string pAttName)
        {
            System.Xml.XmlAttribute att = pDoc.CreateAttribute(pAttName);
            
            // To Simax string
            att.Value = a.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);

            pNd.Attributes.Append(att);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="pDoc"></param>
        /// <param name="pNd"></param>
        /// <param name="pAttName"></param>
        public static void ToXmlAttribute(this double a, System.Xml.XmlDocument pDoc, System.Xml.XmlNode pNd, string pAttName)
        {
            System.Xml.XmlAttribute att = pDoc.CreateAttribute(pAttName);

            // To Simax string
            att.Value = a.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);

            pNd.Attributes.Append(att);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="pDoc"></param>
        /// <param name="pNd"></param>
        /// <param name="pAttName"></param>
        public static void ToXmlAttribute(this bool a, System.Xml.XmlDocument pDoc, System.Xml.XmlNode pNd, string pAttName)
        {
            System.Xml.XmlAttribute att = pDoc.CreateAttribute(pAttName);

            // To Simax string
            att.Value = a.ToString();

            pNd.Attributes.Append(att);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="pDoc"></param>
        /// <param name="pNd"></param>
        /// <param name="pAttName"></param>
        public static void ToXmlAttribute(this int a, System.Xml.XmlDocument pDoc, System.Xml.XmlNode pNd, string pAttName)
        {
            System.Xml.XmlAttribute att = pDoc.CreateAttribute(pAttName);

            // To Simax string
            att.Value = a.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);

            pNd.Attributes.Append(att);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static float FloatFromXmlAttribute(System.Xml.XmlNode pNode, string pAttName)
        {
            if (pNode.Attributes[pAttName] == null)
                throw new System.ApplicationException("Attribute: " + pAttName + " not found in node " + pNode.ToString());

            return FloatFromString(pNode.Attributes[pAttName].Value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int IntFromXmlAttribute(System.Xml.XmlNode pNode, string pAttName)
        {
            if (pNode.Attributes[pAttName] == null)
                throw new System.ApplicationException("Attribute: " + pAttName + " not found in node " + pNode.ToString());

            return int.Parse(pNode.Attributes[pAttName].Value, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }
    }
}
