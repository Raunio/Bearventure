#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements

#endregion

using Microsoft.Xna.Framework;
namespace Bearventure
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry ungulateMenuEntry;
        MenuEntry languageMenuEntry;
        MenuEntry soundsEnabledMenuEntry;
        MenuEntry elfMenuEntry;
        MenuEntry barsMenuEntry;
        MenuEntry resolutionMenuEntry;

        Constants.ScreenMode screenMode;

        enum Ungulate
        {
            BactrianCamel,
            Dromedary,
            Llama,
        }

        static Ungulate currentUngulate = Ungulate.Dromedary;

        static string[] languages = { "Finnish", "English" };
        static int currentLanguage = 1;

        //static bool frobnicate = true;

        static int derp = 10;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            screenMode = ResolutionManager.CurrentScreenMode;

            // Create our menu entries.
            ungulateMenuEntry = new MenuEntry(string.Empty);
            languageMenuEntry = new MenuEntry(string.Empty);
            soundsEnabledMenuEntry = new MenuEntry(string.Empty);
            elfMenuEntry = new MenuEntry(string.Empty);
            barsMenuEntry = new MenuEntry(string.Empty);
            resolutionMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            ungulateMenuEntry.Selected += UngulateMenuEntrySelected;
            languageMenuEntry.Selected += LanguageMenuEntrySelected;
            soundsEnabledMenuEntry.Selected += FrobnicateMenuEntrySelected;
            elfMenuEntry.Selected += ElfMenuEntrySelected;
            barsMenuEntry.Selected += BarsMenuEntrySelected;
            resolutionMenuEntry.Selected += ResolutionMenuEntrySelected;
            back.Selected += OnCancel;
            back.Selected += ResolutionChanged;

            // Add entries to the menu.
            MenuEntries.Add(ungulateMenuEntry);
            MenuEntries.Add(languageMenuEntry);
            MenuEntries.Add(soundsEnabledMenuEntry);
            MenuEntries.Add(barsMenuEntry);
            MenuEntries.Add(elfMenuEntry);
            MenuEntries.Add(resolutionMenuEntry);
            MenuEntries.Add(back);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            ungulateMenuEntry.Text = "Preferred ungulate: " + currentUngulate;
            languageMenuEntry.Text = "Language: " + languages[currentLanguage];
            soundsEnabledMenuEntry.Text = "Sounds enabled: " + (Globals.SoundsEnabled ? "on" : "off");
            elfMenuEntry.Text = "Derp: " + derp;
            barsMenuEntry.Text = "Display enemy health bars: " + (Globals.DisplayHealthBars ? "on" : "off");
            resolutionMenuEntry.Text = "Display resolution: " + screenMode.ToString();
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Ungulate menu entry is selected.
        /// </summary>
        void UngulateMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentUngulate++;

            if (currentUngulate > Ungulate.Llama)
                currentUngulate = 0;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        void LanguageMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentLanguage = (currentLanguage + 1) % languages.Length;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Frobnicate menu entry is selected.
        /// </summary>
        void FrobnicateMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Globals.SoundsEnabled = !Globals.SoundsEnabled;

            SetMenuEntryText();
        }

        void BarsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Globals.DisplayHealthBars = !Globals.DisplayHealthBars;

            SetMenuEntryText();
        }

        void ResolutionMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (screenMode != Constants.ScreenMode.v1080p)
                screenMode--;
            else
                screenMode = Constants.ScreenMode.v240p;

            SetMenuEntryText();
        }

        void ResolutionChanged(object sender, PlayerIndexEventArgs e)
        {
            if (screenMode != ResolutionManager.CurrentScreenMode)
            {
                ResolutionManager.SetVirtualResolution(screenMode);
                ResolutionManager.SetResolution(screenMode, true);
            }
        }


        /// <summary>
        /// Event handler for when the Elf menu entry is selected.
        /// </summary>
        void ElfMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            derp++;

            SetMenuEntryText();
        }


        #endregion
    }
}
