using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMX.Maths
{
    public class IntersectionInfo
    {
        public float cx, cy, time, nx, ny, ix, iy;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="time"></param>
        /// <param name="nx"></param>
        /// <param name="ny"></param>
        /// <param name="ix"></param>
        /// <param name="iy"></param>
        public IntersectionInfo(float x, float y, float time, float nx, float ny, float ix, float iy)
        {
            this.cx = x;
            this.cy = y;
            this.time = time;
            this.nx = nx;
            this.ny = ny;
            this.ix = ix;
            this.iy = iy;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="time"></param>
        /// <param name="nx"></param>
        /// <param name="ny"></param>
        /// <param name="ix"></param>
        /// <param name="iy"></param>
        public IntersectionInfo(float x, float y)
        {
            this.cx = 0;
            this.cy = 0;
            this.time = 0;
            this.nx = x;
            this.ny = y;
            this.ix = 0;
            this.iy = 0;
        }
    }
}
