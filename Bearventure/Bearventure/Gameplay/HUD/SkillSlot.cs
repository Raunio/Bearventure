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

        float scaleSpeed = 0.1f;

        bool hasMorphed;

        bool fadeIn;

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
        }

        public SkillSlot(CharacterSkillCombo combo)
        {
            this.combo = combo;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, Rectangle drawRect)
        {
            if (combo == null)
                spriteBatch.Draw(skill.Icon, new Vector2(drawRect.X, drawRect.Y), drawRect, Color.White, 0f, Vector2.Zero, slotScale, SpriteEffects.None, 0f);
            else
                spriteBatch.Draw(combo.Icon, drawRect, Color.White);
        }

    }
}
