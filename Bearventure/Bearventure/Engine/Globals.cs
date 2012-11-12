
namespace Bearventure
{
    public class Globals
    {
        public static bool SoundsEnabled
        {
            get
            {
                return soundsEnabled;
            }

            set
            {
                soundsEnabled = value;
                if (soundsEnabled) SoundEffectManager.Instance.IndexSelect();
            }
        }
        private static bool soundsEnabled;
        public static double DifficultyFactor
        {
            get;
            set;
        }
    }
}
