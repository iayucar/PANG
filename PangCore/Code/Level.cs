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
using SMX.Maths;

namespace SMX
{
    public class Level
    {
        /// <summary>
        /// Name of the level
        /// </summary>
        public string mName;
        /// <summary>
        /// Position where the player should be located when initializing the level
        /// </summary>
        public Vector2 InitialPlayerPos;
        /// <summary>
        /// Name of the background texture 
        /// </summary>
        public string mBackgroundResourceName;
        /// <summary>
        /// Background texture stream
        /// </summary>
        public System.IO.Stream mBackgroundTextureStream;
        /// <summary>
        /// List of balls currently present in the level
        /// </summary>
        public List<Ball> mBalls = new List<Ball>();
        /// <summary>
        /// List of Bricks currently present in the level
        /// </summary>
        public List<Brick> mBricks = new List<Brick>();
        /// <summary>
        /// List of Ladders currently present in the level
        /// </summary>
        public List<Ladder> mLadders = new List<Ladder>();
        /// <summary>
        /// Collection of arrows in the level (created in real time when the player shoots)
        /// </summary>       
        public List<Arrow> mArrows = new List<Arrow>();
        /// <summary>
        /// Collection of prizes in the level (created in real time when the player hits something that has a prize)
        /// </summary>       
        public List<Prize> mPrizes = new List<Prize>();
        /// <summary>
        /// Initial limit of time that the level will have (read from the level xml)
        /// </summary>
        private float mTimeRemainingInitialSecs = 90;
        /// <summary>
        /// Actual counter of the time remaining
        /// </summary>
        public float mTimeRemainingSecs = 90;
        /// <summary>
        /// Time where the balls are paused
        /// </summary>
        public float mPauseTime = 0;
        /// <summary>
        /// When true, kills all balls if they are bigger than S
        /// </summary>
        public bool mDinamiteAllBalls = false;
        /// <summary>
        /// Collection of collision rectangles for the level (typically a set of rectangles to specify the bounds of the screen)
        /// </summary>        
        private List<SMX.Maths.Rectangle> mLevelRectangles = new List<Rectangle>();
        /// <summary>
        /// All levels have a bricks border. This is the width/height of that border
        /// </summary>
        public static int sLevelBorder = 50;

        public Level()
        {

            // Add screen borders so things bounce off
            mLevelRectangles.Add(new Rectangle(0, 0, sLevelBorder, Game.DefaultGameHeight));
            mLevelRectangles.Add(new Rectangle(Game.DefaultGameWidth - sLevelBorder, 0, sLevelBorder, Game.DefaultGameHeight));
            mLevelRectangles.Add(new Rectangle(0, 0, Game.DefaultGameWidth, sLevelBorder));
            mLevelRectangles.Add(new Rectangle(0, Game.DefaultGameHeight - sLevelBorder, Game.DefaultGameWidth, sLevelBorder));

        }
        /// <summary>
        /// 
        /// </summary>
        public void GetCollisionRectangles(out List<SMX.Maths.Rectangle> pHitRects)
        {
            pHitRects = new List<Rectangle>();
            foreach (SMX.Maths.Rectangle rect in mLevelRectangles)
                pHitRects.Add(rect);

            foreach (Brick brk in mBricks)
                pHitRects.Add(brk.mDrawRectangle);
        }        
        /// <summary>
        /// 
        /// </summary>
        public void LoadContents()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

            // Load Background
            mBackgroundTextureStream = assembly.GetManifestResourceStream(mBackgroundResourceName);
            RendererBase.Ref.LoadTexture(mBackgroundResourceName, mBackgroundTextureStream);

            // Load contents for each element in level
            foreach (Brick brk in mBricks)
                brk.LoadContents();
            foreach (Ball b in mBalls)
                b.LoadContents();
            foreach (Ladder l in mLadders)
                l.LoadContents();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private bool CheckIntersect_Arrow_Ball(Arrow a, Ball b)
        {
            Rectangle arrowRect = a.mDrawRectangle.Clone();
            arrowRect.Width -= 10;
            if (arrowRect.IntersectsCircle(b.Center, b.Radius).HasValue)
            {
                ArrowDied(a);

                KillBall(b);

                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private bool CheckIntersect_Arrow_Brick(Arrow a, Brick b)
        {
            if (a.mDrawRectangle.Intersects(b.mDrawRectangle))
            {
                // Ball Hit by arrow
                if (b.mBreakable)
                    mBricks.Remove(b);
                
                ArrowDied(a);


                if (b.mBreakable && b.mPrize != Prize.ePrize.None)
                {
                    AddPrize(b.mPrize, b.mPos);
                }

                return true;
            }
            return false;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPrize"></param>
        /// <param name="pPos"></param>
        public void AddPrize(Prize.ePrize pPrize, Vector2 pPos)
        {
            Prize p = new Prize(pPrize);
            p.LoadContents();
            p.mPos = pPos;
            mPrizes.Add(p);
        }
        /// <summary>
        /// Kills a ball and creates the next balls
        /// </summary>
        /// <param name="b"></param>
        public void KillBall(Ball b)
        {
            // Ball Hit by arrow
            mBalls.Remove(b);

            // Check if we should add prize
            if (b.BallSize != eBallSize.S)
            {
                Random r = new Random(DateTime.Now.Millisecond);
                double f = r.NextDouble();
                if (f > 0.15f)
                {
                    int numPrizes = Enum.GetValues(typeof(Prize.ePrize)).Length;
                    int prize = (int)(f * numPrizes);
                    Prize.ePrize type = (Prize.ePrize)prize;
                    if (type != Prize.ePrize.None)
                        AddPrize(type, b.mPos);
                }
            }

            // Create 2 new balls
            if (b.BallSize != eBallSize.S)
            {
                eBallSize newSize = Ball.GetNextSize(b.BallSize);
                float newVerticalVelocity = -Math.Min(Math.Abs(b.mVelocity.Y), Ball.GetMaxVelY(newSize));

                Ball b1 = new Ball(newSize, b.BallType);
                b1.LoadContents();
                b1.mPos = b.Center - new Maths.Vector2(b1.Radius * 2f, b1.Radius);
                b1.mVelocity.X = -Math.Abs(b.mVelocity.X);
                b1.mVelocity.Y = newVerticalVelocity;

                Ball b2 = new Ball(newSize, b.BallType);
                b2.LoadContents();
                b2.mPos = b.Center + new Maths.Vector2(0, -b2.Radius);
                b2.mVelocity.X = Math.Abs(b.mVelocity.X);
                b2.mVelocity.Y = newVerticalVelocity;

                // First call to OnFrameMove to update positions and velocities
                b1.OnFrameMove(0.001f);
                b2.OnFrameMove(0.001f);
                mBalls.Add(b1);
                mBalls.Add(b2);
            }


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPlayer"></param>
        /// <param name="b"></param>
        private bool CheckIntersect_Player_Ball(Player pPlayer, Ball b)
        {
            // No collisions if balls are paused
            if (Ball.BallsPaused)
                return false;
            // Do not check collsions if player is already dying or starting
            if (pPlayer.State == Player.eState.Dying || pPlayer.mStarting)
                return false;
            if (b.mDrawRectangle.Width == 0 || b.mDrawRectangle.Height == 0)
                return false;

            // Leave some margin to be less strict in the intersections
            Rectangle playerRect = pPlayer.mDrawRectangle.Clone();
            playerRect.Width -= 10;
            if (playerRect.IntersectsCircle(b.Center, b.Radius - 10).HasValue)
            {
                pPlayer.HitByBall();
                
                KillBall(b);
                
                return true;
            }

            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDt"></param>
        public void OnFrameMove(float pDt)
        {
            // Check level complete
            if (mBalls.Count == 0)
            {
                Game.LevelComplete();
                return;
            }

            // Manage pause time
            mPauseTime -= pDt;
            if (mPauseTime < 0)
                mPauseTime = 0;
            Ball.BallsPaused = mPauseTime > 0;


            // The management of balls, arrows, prizes, etc, could end up destroying elements like bricks, arrows... To allow directly modifying the collections without
            // exceptions in the loops, create backup lists
            List<Ball> ballsBackup = mBalls.ToList();
            List<Arrow> arrowsBackup = mArrows.ToList();
            List<Brick> bricksBackup = mBricks.ToList();
            List<Prize> prizesBackup = mPrizes.ToList();

            // Load contents for each element in level
            foreach (Brick brk in bricksBackup)
                brk.OnFrameMove(pDt);


            foreach (Ball b in ballsBackup)
            {
                // Check if all balls should be dinamited
                if (mDinamiteAllBalls && b.BallSize != eBallSize.S)
                {
                    KillBall(b);
                    break;
                }


                b.OnFrameMove(pDt);

                if (CheckIntersect_Player_Ball(Game.mPlayer, b))
                    break;
            }

            foreach (Arrow a in arrowsBackup)
            {
                a.OnFrameMove(pDt);

                // Check hits with balls
                foreach (Ball b in ballsBackup)
                {
                    // If there's a hit, do not continue checking with other balls
                    if (CheckIntersect_Arrow_Ball(a, b))
                        break;
                }

                // Check hits with bricks
                foreach (Brick b in bricksBackup)
                {
                    // If there's a hit, do not continue checking with other bricks
                    if (CheckIntersect_Arrow_Brick(a, b))
                        break;
                }
            }


            foreach (Prize p in prizesBackup)
            {
                p.OnFrameMove(pDt);

                // Check if player collects the prize
                if (Game.mPlayer.State != Player.eState.Dying && p.mDrawRectangle.Intersects(Game.mPlayer.mDrawRectangle))
                {
                    Game.mPlayer.CollectPrize(p.mType);

                    mPrizes.Remove(p);
                }
            }



            foreach (Ladder l in mLadders)
                l.OnFrameMove(pDt);
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pArrow"></param>
        public void ArrowDied(Arrow pArrow)
        {
            if (mArrows.Contains(pArrow))
                mArrows.Remove(pArrow);
        }
        /// <summary>
        /// Shots a new fire
        /// </summary>
        /// <param name="pPos"></param>
        public void Fire(Vector2 pPos)
        {
            Arrow arrow = new Arrow(pPos);
            arrow.LoadContents();
            mArrows.Add(arrow);
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnFrameRender()
        {
            // Draw background first (so everything else is drawn on top of it)
            RendererBase.Ref.DrawSprite(mBackgroundResourceName, 0, 0, Game.DefaultGameWidth, Game.DefaultGameHeight);


            foreach (Ladder lad in mLadders)
                lad.OnFrameRender();

            foreach (Brick brk in mBricks)
                brk.OnFrameRender();

            foreach (Ball b in mBalls)
                b.OnFrameRender();

            foreach (Arrow a in mArrows)
                a.OnFrameRender();

            foreach (Prize p in mPrizes)
                p.OnFrameRender();

            // Draw time remaining
            RendererBase.Ref.DrawText(((int)mTimeRemainingSecs).ToString(), eTextSize._18, Game.DefaultGameWidth - 140, 40, Color4.White);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pNd"></param>
        /// <returns></returns>
        public static Level FromXml(System.Xml.XmlNode pLevelNd)
        {
            Level lev = new Level();

            lev.InitialPlayerPos = Vector2.ReadVector2FromXmlAttribute(pLevelNd, "StartPosition");
            lev.mName = pLevelNd.Attributes["Name"].Value;

            System.Xml.XmlNodeList bricks = pLevelNd.SelectNodes("./descendant::Brick");
            lev.mBricks.Clear();
            foreach (System.Xml.XmlNode brk in bricks)
                lev.mBricks.Add(Brick.FromXml(brk));

            System.Xml.XmlNodeList balls = pLevelNd.SelectNodes("./descendant::Ball");
            lev.mBalls.Clear();
            foreach (System.Xml.XmlNode brk in balls)
                lev.mBalls.Add(Ball.FromXml(brk));

            System.Xml.XmlNodeList ladders = pLevelNd.SelectNodes("./descendant::Ladder");
            lev.mLadders.Clear();
            foreach (System.Xml.XmlNode lad in ladders)
                lev.mLadders.Add(Ladder.FromXml(lad));

            return lev;
        }

    }
}
