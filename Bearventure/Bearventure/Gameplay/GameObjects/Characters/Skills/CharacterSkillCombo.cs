using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Bearventure.Engine.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Bearventure.Gameplay.Characters.Skills
{
    public class CharacterSkillCombo
    {
        /// <summary>
        /// Gets or sets the name of the skill combo.
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the description of the skill combo.
        /// </summary>
        public string Description
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the array of skills this combo holds.
        /// </summary>
        public List<CharacterSkill> SkillArray
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the time in which the combo resets to its first skill in milliseconds.
        /// </summary>
        public float ResetTime
        {
            get;
            set;
        }
        /// <summary>
        /// Gets the currently active skill of the combo.
        /// </summary>
        public CharacterSkill ActiveSkill
        {
            get;
            private set;
        }

        public SoundEffect ActivationSound
        {
            get;
            set;
        }

        public SoundEffectRandomizer SoundEffectRand
        {
            get;
            set;
        }

        public Texture2D Icon
        {
            get;
            set;
        }

        private int skillCounter = 0;
        private float resetTimer;

        public CharacterSkillCombo() { }
        /// <summary>
        /// Tries to activate the skill that is next in line.
        /// </summary>
        public void SetNextSkill()
        {
            if (ActiveSkill == null)
                ActiveSkill = SkillArray[0];              

            if (!ActiveSkill.IsActive)
            {
                if(ActivationSound != null)
                    SoundEffectManager.Instance.PlaySound(ActivationSound);
                if (SoundEffectRand != null)
                {
                    SoundEffect s = SoundEffectRand.GetRandomSound(true);

                    if (s != null)
                        SoundEffectManager.Instance.PlaySound(s);
                }

                ActiveSkill = SkillArray[skillCounter];
                skillCounter++;

                if (skillCounter > SkillArray.Count - 1)
                {
                    skillCounter = 0;
                }

                resetTimer = 0;
            }
        }

        public void Update(GameTime gameTime)
        {
            resetTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (resetTimer >= ResetTime)
                skillCounter = 0;

            foreach (CharacterSkill s in SkillArray)
                s.Update(gameTime);
        }

    }
}
