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
        private SoundEffect matotest;
        private SoundEffect karhuCombo1;
        private SoundEffect karhuCombo2;
        private SoundEffect karhuCombo3;
        private SoundEffect badgerSkill;

        public SoundEffect BadgerAttack
        {
            get
            {
                return badgerAttack;
            }
        }

        public SoundEffect MatoTest
        {
            get
            {
                return matotest;
            }
        }

        public SoundEffect KarhuCombo1
        {
            get
            {
                return karhuCombo1;
            }
        }

        public SoundEffect KarhuCombo2
        {
            get
            {
                return karhuCombo2;
            }
        }

        public SoundEffect KarhuCombo3
        {
            get
            {
                return karhuCombo3;
            }
        }

        public SoundEffect BadgerSkill
        {
            get
            {
                return badgerSkill;
            }
        }

        public SoundEffect QuietStep
        {
            get
            {
                return step;
            }
        }

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
            matotest = content.Load<SoundEffect>(Constants.matoTest);
            karhuCombo1 = content.Load<SoundEffect>(Constants.KarhuHit1);
            karhuCombo2 = content.Load<SoundEffect>(Constants.KarhuHit2);
            karhuCombo3 = content.Load<SoundEffect>(Constants.KarhuHit3);
            badgerSkill = content.Load<SoundEffect>(Constants.BadgerSkill);
            Console.WriteLine("SoundEffects loaded.");
        }

        public void PlayIndexChange()
        {
            if (Globals.SoundsEnabled) menuIndexChange.Play();
        }

        public void PlayIndexSelect()
        {
            if (Globals.SoundsEnabled) menuSelect.Play();
        }

        public void PlayBadgerAttack()
        {
            if (Globals.SoundsEnabled) badgerAttack.Play();
        }

        public void PlayStep()
        {
            if (Globals.SoundsEnabled) step.Play();
        }

        public void PlayQuietStep()
        {
            if (Globals.SoundsEnabled) quietStep.Play();
        }

        public void PlayKarhuJump()
        {
            if (Globals.SoundsEnabled) karhuJump.Play();
        }

        public void PlayMatoTest()
        {
            if (Globals.SoundsEnabled) matotest.Play();
        }

        public void PlaySound(string asset)
        {
            SoundEffect sound = content.Load<SoundEffect>(asset);

            if (Globals.SoundsEnabled) sound.Play();
        }

        public void PlaySound(SoundEffect sound)
        {
            if (Globals.SoundsEnabled) sound.Play();
        }

        public void PlaySound(SoundEffectInstance sound)
        {
            if (Globals.SoundsEnabled) sound.Play();
        }

    }
}
