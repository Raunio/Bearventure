
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
                if (soundsEnabled) SoundEffectManager.Instance.PlayIndexSelect();
                else MusicManager.Instance.StopMusic(); 
            }
        }
        private static bool soundsEnabled;

        public static double DifficultyFactor
        {
            get;
            set;
        }

        public static bool DisplayHealthBars
        {
            get;
            set;
        }

        public static bool GoreEnabled
        {
            get;
            set;
        }
    }
}
