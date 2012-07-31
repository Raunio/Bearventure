
namespace Bearventure
{
    public class Globals
    {
        public static bool SoundsEnabled
        {
            get;
            set
            {
                SoundsEnabled = value;
                if (SoundsEnabled) SoundEffectManager.Instance.IndexSelect();
            }
        }

        public static double DifficultyFactor
        {
            get;
            set;
        }
    }
}
