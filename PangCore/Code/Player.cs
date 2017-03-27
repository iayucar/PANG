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
    public class Player : ComponentBase
    {
        /// <summary>
        /// State of the player: because a player typically has many different animations, we use this to determine which animation should be drawn in each case, and
        /// to control some logic of the game. For example, if the state changes to "Fire", we probably want to instruct the game to throw a new shot somehow
        /// You could also think of this as Animation enum.
        /// </summary>
        public enum eState
        {
            WalkingLeft,
            WalkingRight,
            Firing,
            StairsUpDown,
            Dying,
            Idle
        }
        /// <summary>
        /// Base texture with player sprites (resource name)
        /// </summary>
        public static string mTextureResourceName;
        /// <summary>
        /// Base texture with player sprites (stream of bytes)
        /// </summary>
        public static System.IO.Stream mTextureStream;
        /// <summary>
        /// Collection of rectangles for each animation, basing on player's state
        /// </summary>
        public static Dictionary<eState, List<SMX.Maths.Rectangle>> mSourceRects = new Dictionary<eState, List<Rectangle>>();
        /// <summary>
        /// Index of current rectangle inside each animation
        /// </summary>
        private int mSourceRectIdx = 0;
        /// <summary>
        /// Private counter to control animation frames
        /// </summary>
        private float mAnimTime = 0;
        private eState mState = eState.Idle;
        public Ladder mInLadder;
        public bool mStarting = false;
        public bool mShieldActive = false;
        /// <summary>
        /// Private counter to control how much time we have been in Starting State
        /// </summary>
        private float mTimeStarting = 0;
        public int mNumLives = 3;
        private bool mFlipHorizontal;
        /// <summary>
        /// Private timing to control when it was the last time we fired (this way we don't allow the player to re-fire again until a certain amount of time has happened)
        /// </summary>
        private DateTime mLastFireTime;
        /// <summary>
        /// Private counter to control how much time we have been in Dying State
        /// </summary>
        private float mTimeDying = 0;



        public eState State
        {
            get { return mState; }
            set
            {
                eState oldval = mState;
                mState = value;
                if (mState != oldval)
                {
                    StateChanged(oldval, mState);
                }
            }
        }
        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFrmMain"></param>
        public Player() : base ()
        {
            mScaleFactorX = 3.4f;
            mScaleFactorY = 3.4f;


            if (mSourceRects == null || mSourceRects.Count == 0)
            {
                mSourceRects.Add(eState.WalkingLeft, new List<Rectangle>() { new SMX.Maths.Rectangle { X = 12, Y = 2, Width = 30, Height = 32 }, new SMX.Maths.Rectangle { X = 46, Y = 3, Width = 30, Height = 31 }, new SMX.Maths.Rectangle { X = 80, Y = 2, Width = 30, Height = 32 }, new SMX.Maths.Rectangle { X = 114, Y = 3, Width = 28, Height = 31 }, new SMX.Maths.Rectangle { X = 148, Y = 3, Width = 30, Height = 31 } });
                mSourceRects.Add(eState.WalkingRight, new List<Rectangle>() { new SMX.Maths.Rectangle { X = 12, Y = 2, Width = 30, Height = 32 }, new SMX.Maths.Rectangle { X = 46, Y = 3, Width = 30, Height = 31 }, new SMX.Maths.Rectangle { X = 80, Y = 2, Width = 30, Height = 32 }, new SMX.Maths.Rectangle { X = 114, Y = 3, Width = 28, Height = 31 }, new SMX.Maths.Rectangle { X = 148, Y = 3, Width = 30, Height = 31 } });
                mSourceRects.Add(eState.Idle, new List<Rectangle>() { new SMX.Maths.Rectangle { X = 12, Y = 112, Width = 26, Height = 32 } });
                mSourceRects.Add(eState.Firing, new List<Rectangle>() { new SMX.Maths.Rectangle { X = 44, Y = 112, Width = 27, Height = 32 } });
                mSourceRects.Add(eState.StairsUpDown, new List<Rectangle>() { new SMX.Maths.Rectangle { X = 13, Y = 36, Width = 24, Height = 32 }, new SMX.Maths.Rectangle { X = 49, Y = 36, Width = 24, Height = 32 }, new SMX.Maths.Rectangle { X = 81, Y = 36, Width = 26, Height = 32 }, new SMX.Maths.Rectangle { X = 115, Y = 36, Width = 26, Height = 32 } });
                mSourceRects.Add(eState.Dying, new List<Rectangle>() { new SMX.Maths.Rectangle { X = 81, Y = 112, Width = 41, Height = 30 } });
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pOld"></param>
        /// <param name="pNew"></param>
        private void StateChanged(eState pOld, eState pNew)
        {
            if (pOld == eState.Firing)
            {
                // FIRE
                Game.CurrentLevel.Fire(mPos);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override void LoadContents()
        {
            // This is a singleton, so load it only once
            if (mTextureStream == null)
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                mTextureResourceName = Game.ResourcesNameSpace + ".Resources.Pang.png";

                mTextureStream = assembly.GetManifestResourceStream(mTextureResourceName);
                RendererBase.Ref.LoadTexture(mTextureResourceName, mTextureStream);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        private void SetStatesFromInput()
        {
            // No permito cambiar el estado si estoy muriendo
            if (State == eState.Dying)
                return;

            TimeSpan dif = DateTime.Now - mLastFireTime;

            // Cuando estoy disparando, mantener el estado durante al menos 0.1 segundos, para que se vea la animación 
            if (State == eState.Firing && dif.TotalSeconds < 0.1f)
                return;

            // Ver si queremos disparar, y si ha transcurrido el tiempo suficiente desde la última vez que disparamos
            if (InputBase.Ref.IsActionPressed(eAction.Fire))
            {
                if (dif.TotalSeconds > 0.5f)
                {
                    State = eState.Firing;
                    mLastFireTime = DateTime.Now;
                    return;
                }
            }

            if (InputBase.Ref.IsActionPressed(eAction.Left))
            {
                State = eState.WalkingLeft;
                return;
            }
            if (InputBase.Ref.IsActionPressed(eAction.Right))
            {
                State = eState.WalkingRight;
                return;
            }
            if (InputBase.Ref.IsActionPressed(eAction.Up) && mInLadder != null)
            {
                State = eState.StairsUpDown;
                return;
            }
            if (InputBase.Ref.IsActionPressed(eAction.Down) && mInLadder != null)
            {
                State = eState.StairsUpDown;
                return;
            }

            State = eState.Idle;
        }
        /// <summary>
        /// 
        /// </summary>
        public void HitByBall()
        {
            if (mShieldActive)
            {
                mShieldActive = false;
            }
            else
            {
                mNumLives--;
                this.State = eState.Dying;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void RecoverPlayer()
        {
            mTimeStarting = 0;
            this.mStarting = true;
            this.mShieldActive = false;
            this.mPos = Game.CurrentLevel.InitialPlayerPos;
            this.State = eState.Idle;
        }

        /// <summary>
        /// 
        /// </summary>
        public void CollectPrize(Prize.ePrize pPrize)
        {
            switch (pPrize)
            {
                case Prize.ePrize.Shield:
                    mShieldActive = true;
                    break;
                case Prize.ePrize.ExtraLife:
                    mNumLives++;
                    break;
                case Prize.ePrize.ExtraTime:
                    Game.CurrentLevel.mTimeRemainingSecs += 20;
                    break;
                case Prize.ePrize.Pause:
                    Game.CurrentLevel.mPauseTime += 5;
                    break;
                case Prize.ePrize.Dinamite:
                    Game.CurrentLevel.mDinamiteAllBalls = true;
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDt"></param>
        public override void OnFrameMove(float pDt)
        {
            if (mSourceRectIdx >= mSourceRects[mState].Count)
                mSourceRectIdx = 0;

            // Parse Input
            SetStatesFromInput();


            // Change animation frame
            if (mSourceRects[mState].Count > 1)
            {

                float fps = 10;
                float period = 1 / fps;
                mAnimTime += pDt;
                if (mAnimTime > period)
                {
                    mAnimTime = 0;
                    mSourceRectIdx++;
                    if (mSourceRectIdx >= mSourceRects[mState].Count)
                        mSourceRectIdx = 0;
                }
            }


            // Set velocity according to gravity to let the player fall
            mVelocity = Game.mGravityVel_PixelsPerSecond;

            // Cancel velocity if we are in certain states, like dying or when we are in a ladder
            if (this.State == eState.Dying)
            {
                // No movement when dying
                mVelocity = Vector2.Zero;

                // Check if dying animation ended
                mTimeDying += pDt;
                if (mTimeDying > 3)
                {
                    mTimeDying = 0;

                    if (mNumLives <= 0)
                        Game.PlayerDied();
                    else RecoverPlayer();
                    return;
                }
            }
            else if (mInLadder != null)
            {
                mVelocity = Vector2.Zero;
            }


            // Flip walking animation if walking left
            mFlipHorizontal = mState == eState.WalkingLeft;
            switch (mState)
            {
                case eState.WalkingLeft:
                    mVelocity -= new Vector2(320, 0);
                    break;
                case eState.WalkingRight:
                    mVelocity += new Vector2(320, 0);
                    break;
            }


            // Manage starting state
            if (mStarting)
            {
                mTimeStarting += pDt;
                if (mTimeStarting > 3)
                {
                    this.mTimeStarting = 0;
                    this.mStarting = false;
                }
            }

            CheckIntersectionsAndAdjustVelocity(pDt);

            mPos += mVelocity * pDt;
            RefreshDrawRectangle();
        }
        /// <summary>
        /// 
        /// </summary>
        public void RefreshDrawRectangle()
        {
            if (mSourceRectIdx >= mSourceRects[mState].Count)
                mSourceRectIdx = 0;

            var rect = mSourceRects[mState][mSourceRectIdx];
            mDrawRectangle = new SMX.Maths.Rectangle((int)mPos.X, (int)mPos.Y, (int)((float)rect.Width * mScaleFactorX), (int)((float)rect.Height * mScaleFactorY));
        }
        /// <summary>
        /// 
        /// </summary>
        private void CheckIntersectionsAndAdjustVelocity(float pDt)
        {
            List<SMX.Maths.Rectangle> hitRects;
            Game.CurrentLevel.GetCollisionRectangles(out hitRects);
            foreach (SMX.Maths.Rectangle rect in hitRects)
            {
                Vector2 outVel;
                Vector2 hitNormal;
                float hitTime;
                if (mDrawRectangle.Intersects(rect))
                {
                    hitNormal = rect.GetNormalFromPoint(mPos);
                    hitNormal.Normalize();

                    // Solo reducir la velocidad si la normal de colisión va en contra del movimiento (para permitir salir si hay algo de penetracion)
                    if (Math.Sign(mVelocity.X) != Math.Sign(hitNormal.X))
                        mVelocity.X *= Math.Abs(hitNormal.Y);
                    if (Math.Sign(mVelocity.Y) != Math.Sign(hitNormal.Y))
                        mVelocity.Y *= Math.Abs(hitNormal.X);
                }
                else if (SMX.Maths.Rectangle.WillIntersect_SweepBoxBox(mDrawRectangle, rect, mVelocity * pDt, out outVel, out hitNormal, out hitTime))
                {
                    hitNormal = rect.GetNormalFromPoint(mPos);
                    hitNormal.Normalize();

                    if (hitTime <= 0)
                    {
                        // Solo reducir la velocidad si la normal de colisión va en contra del movimiento (para permitir salir si hay algo de penetracion)
                        if (Math.Sign(mVelocity.X) != Math.Sign(hitNormal.X))
                            mVelocity.X *= Math.Abs(hitNormal.Y);
                        if (Math.Sign(mVelocity.Y) != Math.Sign(hitNormal.Y))
                            mVelocity.Y *= Math.Abs(hitNormal.X);

                    }
                    else mVelocity = outVel / pDt;
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public override void OnFrameRender()
        {
            // If starting, show player blinking
            if (mStarting)
            {
                int timeInt = (int)mTimeStarting;
                float blinker = mTimeStarting - timeInt;
                if (blinker > 0.25f && blinker < 0.5f)
                    return;
                if (blinker > 0.75f && blinker < 1f)
                    return;
            }

            RefreshDrawRectangle();

            var rect = mSourceRects[mState][mSourceRectIdx];
            if (mFlipHorizontal)
                RendererBase.Ref.DrawSprite(mTextureResourceName, rect.X, rect.Y, rect.Width, rect.Height, mDrawRectangle.X + mDrawRectangle.Width, mDrawRectangle.Y, -mDrawRectangle.Width, mDrawRectangle.Height);
            else
                RendererBase.Ref.DrawSprite(mTextureResourceName, rect.X, rect.Y, rect.Width, rect.Height, mDrawRectangle.X, mDrawRectangle.Y, mDrawRectangle.Width, mDrawRectangle.Height);

            // Draw remaining lives
            int x = 10, y = Game.DefaultGameHeight - 50;
            for (int i = 0; i < mNumLives; i++)
            {
                RendererBase.Ref.DrawSprite(mTextureResourceName, 154, 44, 16, 16, x, y, 48, 48);
                x += 55;
            }

        }

    }

}
