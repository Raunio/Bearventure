using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bearventure.Gameplay.Characters.Skills;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bearventure.Gameplay.HUD
{
    public class SkillSlot
    {
        CharacterSkill skill;
        CharacterSkillCombo combo;

        float slotScale = 1f;

        float recoveryIndicatorScale = 1f;
        float recoveryIndicatorTargetScale = 2f;
        float recoveryIndicatorSpeed = 0.1f;

        float recoveryIndicatorOpacity = 0.3f;
        float recoveryIndicatorTargetOpacity = 0f;
        float recoveryIndicatorFadingSpeed = 0.005f;
        float recoveryIndicatorInitialOpacity = 0.3f;

        float targetScale = 1f;
        float scalingSpeed = 0.15f;
        float initialScale = 1f;
        float ultimateScale = 0.75f;

        float slotOpacity = 1f;

        float targetOpacity = 1f;

        bool hasActivated;
        bool hasRecovered;

        Color tintColor = Color.White;
        Color readyTint = Color.White;
        Color cooldownTint = new Color(100, 100, 100);

        public CharacterSkill Skill
        {
            get
            {
                return skill;
            }
        }

        public CharacterSkillCombo Combo
        {
            get
            {
                return combo;
            }
        }

        public SkillSlot(CharacterSkill skill)
        {
            this.skill = skill;
            hasRecovered = true;
        }

        public SkillSlot(CharacterSkillCombo combo)
        {
            this.combo = combo;
        }

        public void Update(GameTime gameTime)
        {
            if (combo != null && combo.ActiveSkill != null)
            {
                if(combo.ActiveSkill.IsActive && !hasActivated)
                {
                    SetScaling(ultimateScale);
                    hasActivated = true;
                }
                else if (!combo.ActiveSkill.IsActive)
                {
                    hasActivated = false;
                }
            }

            if (skill != null)
            {
                if (skill.IsActive && !hasActivated)
                {
                    SetScaling(ultimateScale);
                    hasActivated = true;
                    hasRecovered = false;     
                }

                else if (!skill.IsActive)
                {
                    hasActivated = false;
                }

                if (skill.IsReady && !hasRecovered)
                {
                    recoveryIndicatorScale = Interpolate(recoveryIndicatorScale, recoveryIndicatorTargetScale, recoveryIndicatorSpeed);
                    recoveryIndicatorOpacity = Interpolate(recoveryIndicatorInitialOpacity, recoveryIndicatorTargetOpacity, recoveryIndicatorFadingSpeed);

                    if (recoveryIndicatorScale >= recoveryIndicatorTargetScale - recoveryIndicatorSpeed)
                    {
                        hasRecovered = true;
                        tintColor = Color.White;
                    }
                }
                else if (!skill.IsReady)
                {
                    tintColor = cooldownTint;
                }
            }  

            if (slotScale == ultimateScale)
                SetScaling(initialScale);


            slotScale = Interpolate(slotScale, targetScale, scalingSpeed);

        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (combo == null)
            {
                spriteBatch.Draw(skill.Icon, new Vector2(position.X + skill.Icon.Width / 2, position.Y + skill.Icon.Height / 2), null, tintColor, 0f, new Vector2(skill.Icon.Width / 2, skill.Icon.Height / 2), slotScale, SpriteEffects.None, 0f);
                
                if (skill != null && skill.CooldownTimer >= skill.Cooldown && !hasRecovered)
                    spriteBatch.Draw(skill.Icon, new Vector2(position.X + skill.Icon.Width / 2, position.Y + skill.Icon.Height / 2), null, new Color(255, 255, 255, recoveryIndicatorOpacity), 0f, new Vector2(skill.Icon.Width / 2, skill.Icon.Height / 2), recoveryIndicatorScale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(combo.Icon, new Vector2(position.X + combo.Icon.Width / 2, position.Y + combo.Icon.Height / 2), null, new Color(255, 255, 255, slotOpacity), 0f, new Vector2(combo.Icon.Width / 2, combo.Icon.Height / 2), slotScale, SpriteEffects.None, 0f);
            }
        }

        private void SetScaling(float targetScale)
        {
            this.targetScale = targetScale;
        }

        private void SetFading(float targetOpacity)
        {
            this.targetOpacity = targetOpacity;
        }

        private float Interpolate(float from, float to, float speed)
        {
            if (from < to - speed)
            {
                from += speed;
            }
            else if (from > to + speed)
            {
                from -= speed;
            }
            else
            {
                from = targetScale;
            }

            return from;
        }

    }
}
