using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SMX
{
    public class SoundWPF : SoundBase
    {
        private Dictionary<string, System.Media.SoundPlayer> mSounds = new Dictionary<string, System.Media.SoundPlayer>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pSoundName"></param>
        /// <param name="pStream"></param>
        public override void LoadSound(string pSoundName, Stream pStream)
        {
            if (mSounds.ContainsKey(pSoundName))
                return;

            System.Media.SoundPlayer player = new System.Media.SoundPlayer(pStream);
            mSounds.Add(pSoundName, player);
        }        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pSoundName"></param>
        public override void PlaySound(string pSoundName, bool pLooping = false)
        {
            if (!mSounds.ContainsKey(pSoundName))
                return;

            if (pLooping)
                mSounds[pSoundName].PlayLooping();
            else mSounds[pSoundName].Play();
        }
        /// <summary>
        /// 
        /// </summary>
        public override void StopAllSounds()
        {
            foreach (string soundname in mSounds.Keys)
                StopSound(soundname);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pSoundName"></param>
        public override void StopSound(string pSoundName)
        {
            if (!mSounds.ContainsKey(pSoundName))
                return;
            mSounds[pSoundName].Stop();
        }
    }
}
