using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Bearventure
{
    class MusicManager
    {
        private static MusicManager instance = null;

        private Song menuMusic;
        private Song level1Song;

        private MusicManager() { }

        public void Initialize() { }

        public static MusicManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new MusicManager();
                
                return instance;
            }
        }

        public float Volume
        {
            get;
            set;
        }

        public void LoadContent(ContentManager content)
        {
            level1Song = content.Load<Song>(Constants.Level1Music);
            menuMusic = content.Load<Song>(Constants.MenuMusic);
        }

        public void PlayLevel1Music()
        {
            if (Globals.SoundsEnabled)
            {
                MediaPlayer.Volume = Volume;
                MediaPlayer.Play(level1Song);
                MediaPlayer.IsRepeating = true;
            }
        }
        public void PlayMenuMusic()
        {
            if (Globals.SoundsEnabled)
            {
                MediaPlayer.Volume = Volume;
                MediaPlayer.Play(menuMusic);
                MediaPlayer.IsRepeating = true;
            }
        }

        public void StopMusic()
        {
            MediaPlayer.Stop();
        }
    }
}
