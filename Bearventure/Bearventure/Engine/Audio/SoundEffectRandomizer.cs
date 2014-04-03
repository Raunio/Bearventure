using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Bearventure.Engine.Audio
{
    public class SoundEffectRandomizer
    {
        private List<SoundEffect> sounds = new List<SoundEffect>();
        private static Random rand = new Random();

        public void AddSound(SoundEffect sound)
        {
            sounds.Add(sound);
            
        }

        public SoundEffect GetRandomSound(bool countEmpty)
        {
            int i = rand.Next(sounds.Count + Convert.ToInt32(countEmpty));

            if (i >= sounds.Count)
                return null;
            else
                return sounds[i];
        }
    }
}
