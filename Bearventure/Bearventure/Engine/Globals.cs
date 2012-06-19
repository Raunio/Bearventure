
namespace Bearventure
{
    public class Globals
    {
        private static bool soundsEnabled = true;
        private double difficultyFactor = 1.0;

        public static bool SoundsEnabled
        {
            get { return soundsEnabled; }
            set
            {
                soundsEnabled = value;
                if (soundsEnabled) SoundEffectManager.Instance.IndexSelect();
            }
        }

        public double DifficultyFactor
        {
            get { return difficultyFactor; }
            set { difficultyFactor = value; }
        }
    }
}
