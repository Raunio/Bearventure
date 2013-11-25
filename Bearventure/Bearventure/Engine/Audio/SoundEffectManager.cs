using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Bearventure
{
    class SoundEffectManager
    {
        private static SoundEffectManager instance = null;
        private ContentManager content;

        private SoundEffect menuSelect;
        private SoundEffect menuIndexChange;
        private SoundEffect badgerAttack;
        private SoundEffect step;
        private SoundEffect karhuJump;
        private SoundEffect quietStep;

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
            this.content = content;
            menuIndexChange = content.Load<SoundEffect>(Constants.MenuSelectedIndexChange);
            menuSelect = content.Load<SoundEffect>(Constants.MenuIndexSelected);
            badgerAttack = content.Load<SoundEffect>(Constants.BadgerAttack);
            step = content.Load<SoundEffect>(Constants.Step);
            karhuJump = content.Load<SoundEffect>(Constants.karhuJump);
            quietStep = content.Load<SoundEffect>(Constants.quietStep);
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

        public void Step()
        {
            if (Globals.SoundsEnabled) step.Play();
        }

        public void QuietStep()
        {
            if (Globals.SoundsEnabled) quietStep.Play();
        }

        public void KarhuJump()
        {
            if (Globals.SoundsEnabled) karhuJump.Play();
        }

        public void PlaySound(string asset)
        {
            SoundEffect sound = content.Load<SoundEffect>(asset);

            if (Globals.SoundsEnabled) sound.Play();
        }

    }
}
