using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

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
        private SoundEffect puukotus;
        private SoundEffect badgerSpawn;
        private SoundEffect spawnerSound;
        private SoundEffect badgerDeath;

        private Vector2 playerPosition;

        public float MaxSoundEffectDistance
        {
            get;
            set;
        }

        public SoundEffect BadgerDeath
        {
            get
            {
                return badgerDeath;
            }
        }

        public SoundEffect BadgerAttack
        {
            get
            {
                return badgerAttack;
            }
        }

        public SoundEffect Puukotus
        {
            get
            {
                return puukotus;
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

        public SoundEffect BadgerSpawn
        {
            get
            {
                return badgerSpawn;
            }
        }

        public SoundEffect SpawnerSound
        {
            get
            {
                return spawnerSound;
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
            puukotus = content.Load<SoundEffect>(Constants.puukkoSound);
            badgerSpawn = content.Load<SoundEffect>(Constants.badgerSpawn);
            spawnerSound = content.Load<SoundEffect>(Constants.spawnerSound);
            badgerDeath = content.Load<SoundEffect>(Constants.badgerDeath);
            Console.WriteLine("SoundEffects loaded.");
        }

        public void UpdatePlayerPosition(Vector2 pos)
        {
            playerPosition = pos;
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

        public void PlayBadgerSpawn()
        {
            if (Globals.SoundsEnabled) badgerSpawn.Play();
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

        public void PlaySpawnSound(Vector2 spawnerPosition, Constants.EnemyType enemyType)
        {
            switch (enemyType)
            {
                case Constants.EnemyType.BlackMetalBadger:
                    PlaySoundFromPosition(spawnerPosition, badgerSpawn);
                    break;
            }
        }

        public void PlayDeathSound(Vector2 position, Constants.EnemyType enemyType)
        {
            switch (enemyType)
            {
                case Constants.EnemyType.BlackMetalBadger:
                    PlaySoundFromPosition(position, badgerDeath);
                    break;
            }
        }

        /// <summary>
        /// Plays a sound effect and calculates pan & volume values for it relative to player. 
        /// If the character is the player, then calculations will be ignored and volume and pan are set to 1.
        /// </summary>
        /// <param name="sound">The soundeffect</param>
        /// <param name="distance">distance to player</param>
        /// <param name="maxDistance">the maximum distance from where the sound can be heard</param>
        public void PlaySoundFromPosition(Vector2 position, SoundEffect sound)
        {
            if (!Globals.SoundsEnabled)
                return;

            Vector2 DistanceToPlayer = playerPosition - position;
            //float normDistance = DistanceToPlayer < 0 ? -DistanceToPlayer : DistanceToPlayer;
            float dist = DistanceToPlayer.Length() < 0 ? -DistanceToPlayer.Length() : DistanceToPlayer.Length();

            if (dist < SoundEffectManager.Instance.MaxSoundEffectDistance)
            {
                float pan = DistanceToPlayer.X / SoundEffectManager.Instance.MaxSoundEffectDistance;
                float volume = 1 - dist / SoundEffectManager.Instance.MaxSoundEffectDistance;

                SoundEffectInstance soundInstance = sound.CreateInstance();
                soundInstance.Pan = pan;
                soundInstance.Volume = volume;

                SoundEffectManager.Instance.PlaySound(soundInstance);
            }
        }
    }
}
