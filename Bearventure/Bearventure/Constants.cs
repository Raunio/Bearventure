using System;

namespace Bearventure
{
    class Constants
    {
        #region MenuTexts
        public const String MenuExitPromptText = "Are you sure you want to exit?";
        public const String PauseExitPromptText = "Are you sure you want to end game?";
        #endregion
        #region SoundEffects
        public const String MenuSelectedIndexChange = "SoundEffects/IndexChange";
        public const String MenuIndexSelected = "SoundEffects/IndexSelect";
        #endregion
        #region Music
        public const String Level1Music = "Music/Level1";
        public const String MenuMusic = "Music/Splash";
        #endregion
        #region Fonts
        public const String GameFont = "Fonts/GameFont";
        #endregion

        #region Textures
        #endregion
        #region Resolution
        public enum ScreenMode
        {
            v1080p,  // 1920x1080
            v720p,   // 1280x720
            v480p60, // 720x480
            v360p,   // 640x360
            v240p    // 320x240
        }
        #endregion
    }
}
