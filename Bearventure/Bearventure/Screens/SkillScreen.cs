using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bearventure
{
    class SkillScreen : MenuScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public SkillScreen()
            : base("SKILLS")
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            // Create our menu entries.
            MenuEntry applyMenuEntry = new MenuEntry("Apply");
            MenuEntry cancelMenuEntry = new MenuEntry("Cancel");

            // Hook up menu event handlers.
            applyMenuEntry.Selected += ApplyMenuEntrySelected;
            cancelMenuEntry.Selected += CancelMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(applyMenuEntry);
            MenuEntries.Add(cancelMenuEntry);
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Restart menu entry is selected.
        /// </summary>
        void ApplyMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            MessageBoxScreen confirmRestartMessageBox = new MessageBoxScreen(Constants.RestartPromptText);

            confirmRestartMessageBox.Accepted += ConfirmRestartMessageBoxAccepted;

            ScreenManager.AddScreen(confirmRestartMessageBox, ControllingPlayer);
        }
        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void CancelMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(Constants.PauseExitPromptText);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                           new MainMenuScreen());

            MusicManager.Instance.StopMusic();
            MusicManager.Instance.PlayMenuMusic();
        }
        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to restart" message box.This uses the loading screen to
        /// transition from the game back to the current level screen.
        /// </summary>
        /// // TODO: Restarts to current screen rather than level1screen
        void ConfirmRestartMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {

            LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new BackgroundScreen(),
                                                           new Level1Screen());

            MusicManager.Instance.StopMusic();
            MusicManager.Instance.PlayLevel1Music();
        }


        #endregion
    }
}
