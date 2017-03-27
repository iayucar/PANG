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
    public static class Game
    {
        /// <summary>
        /// Stopwatch used to measure time
        /// </summary>
        private static System.Diagnostics.Stopwatch mStopwatch = new System.Diagnostics.Stopwatch();
        /// <summary>
        /// Assembly name, to be used across the game to load embedded resources
        /// </summary>
        public static string ResourcesNameSpace = "SMX";
        /// <summary>
        /// Gravity velocity, in pixels per second
        /// </summary>
        public static Vector2 mGravityVel_PixelsPerSecond = new Vector2(0, 800);
        /// <summary>
        /// Player instance
        /// </summary>
        public static Player mPlayer = new Player();
        /// <summary>
        /// Private counter for FPS statistics
        /// </summary>
        private static int mFPSCounter;
        /// <summary>
        /// Private time counter
        /// </summary>
        private static float mTimeCounter;
        /// <summary>
        /// Number of frames per second the game is able to render
        /// </summary>
        public static int mFPS;        
        /// <summary>
        /// Collection of levels actually present in the game
        /// </summary>
        public static List<Level> mLevels = new List<Level>();
        /// <summary>
        /// Index of the current level actually being played
        /// </summary>
        public static int mCurrentLevel = 0;

        #region Props
        public static Level CurrentLevel
        {
            get
            {
                if (mCurrentLevel < 0 || mCurrentLevel > mLevels.Count - 1)
                    return null;
                else return mLevels[mCurrentLevel];
            }
        }
        public static int DefaultGameWidth
        {
            get { return 1920; }
        }
        public static int DefaultGameHeight
        {
            get { return 1080; }
        }

        #endregion

       
        /// <summary>
        /// 
        /// </summary>
        public static void PlayerDied()
        {
            //mFrmMain.GameOver();
        }
        /// <summary>
        /// 
        /// </summary>
        public static void LevelComplete()
        {
            if (mCurrentLevel < mLevels.Count - 1)
            {
                // Level Finished
                mCurrentLevel++;
                InitLevel(mCurrentLevel);
            }
            else
            {
                // Game Finished
                //mFrmMain.GameOver();
            }
        }
        /// <summary>
        /// Loads all levels available
        /// </summary>
        private static void LoadLevels()
        {
            // Get the list of XML and png level files
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string[] names = assembly.GetManifestResourceNames();
            List<string> xmlnames = new List<string>();
            List<string> pngNames = new List<string>();
            foreach (string name in names)
            {
                if (!name.Contains("Levels"))
                    continue;

                string ext = System.IO.Path.GetExtension(name).ToLowerInvariant();
                if (ext == ".xml")
                    xmlnames.Add(name);
                if (ext == ".png")
                    pngNames.Add(name);
            }


            // Load levels
            mLevels.Clear();
            foreach (string levelName in xmlnames)
            {
                System.IO.Stream strm = assembly.GetManifestResourceStream(levelName);
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.Load(strm);

                // Load level config from XML
                System.Xml.XmlNode levelNd = doc.SelectSingleNode("./descendant::Level");
                Level lev = Level.FromXml(levelNd);

                // Check if background png file exists
                string pngName = System.IO.Path.ChangeExtension(levelName, ".png");
                if (!pngNames.Contains(pngName))
                    pngName = System.IO.Path.ChangeExtension(levelName, ".Png");
                if (!pngNames.Contains(pngName))
                    pngName = System.IO.Path.ChangeExtension(levelName, ".PNG");
                if (!pngNames.Contains(pngName))
                    throw new System.ApplicationException("Background not found for level: " + levelName);
                lev.mBackgroundResourceName = pngName;

                // Actually load level contents
                lev.LoadContents();

                // Fill collection
                mLevels.Add(lev);
            }
        }
        /// <summary>
        /// Performs the content loading at initialization
        /// </summary>
        public static void LoadContents()
        {
            mStopwatch.Reset();

            mPlayer.LoadContents();

            if (mLevels == null || mLevels.Count == 0)
                LoadLevels();

            InitLevel(2);
        }
        /// <summary>
        /// Sets a level index, and resets player position
        /// </summary>
        /// <param name="pLevelIdx"></param>
        public static void InitLevel(int pLevelIdx)
        {
            mCurrentLevel = pLevelIdx;
            mPlayer.mPos = CurrentLevel.InitialPlayerPos;
        }
        /// <summary>
        /// Calculates current dt
        /// </summary>
        /// <returns></returns>
        private static float CalcDt()
        {
            // A hi-resolution timer would be used in normal circumstances
            mStopwatch.Stop();
            System.TimeSpan span = mStopwatch.Elapsed;
            mStopwatch.Reset();
            mStopwatch.Start();
            float dt = (float)span.TotalSeconds;

            // Filter out negative or too big DTs to protect the code
            dt = Math.Max(0.00001f, dt);
            dt = Math.Min(0.15f, dt);

            return dt;
        }
        /// <summary>
        /// Logic update
        /// </summary>
        /// <param name="pDt"></param>
        public static void OnFrameMove()
        {
            // Calc time
            float dt = CalcDt();

            // Calc FPS
            mFPSCounter++;
            mTimeCounter += dt;
            if (mTimeCounter >= 1)
            {
                mFPS = mFPSCounter;
                mFPSCounter = 0;
                mTimeCounter = 0;
            }

            mPlayer.OnFrameMove(dt);

            if (CurrentLevel != null)
                CurrentLevel.OnFrameMove(dt);
        }
        /// <summary>
        /// Frame Renderer
        /// </summary>
        public static void OnFrameRender()
        {
            if (CurrentLevel != null)
                CurrentLevel.OnFrameRender();

            if (mPlayer != null)
                mPlayer.OnFrameRender();
        }
    }
}
