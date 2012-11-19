using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Bearventure
{
    class SoundEffectManager
    {
        private static SoundEffectManager instance = null;

        private SoundEffect menuSelect;
        private SoundEffect menuIndexChange;
        private SoundEffect badgerAttack;

        private SoundEffectManager() { }

        public void Initialize() { }

        public static SoundEffectManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new SoundEffectManager();

                return instance;
            }
        }

        public void LoadContent(ContentManager content)
        {
            menuIndexChange = content.Load<SoundEffect>(Constants.MenuSelectedIndexChange);
            menuSelect = content.Load<SoundEffect>(Constants.MenuIndexSelected);
            badgerAttack = content.Load<SoundEffect>(Constants.Attack);
            Console.WriteLine("SoundEffects loaded.");
        }

        public void IndexChange()
        {
            if (Globals.SoundsEnabled) menuIndexChange.Play();
        }

        public void IndexSelect()
        {
            if (Globals.SoundsEnabled) menuSelect.Play();
        }

        public void BadgerAttack()
        {
            if (Globals.SoundsEnabled) badgerAttack.Play();
        }

    }
}
