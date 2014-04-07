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
                spriteBatch.Draw(skill.Icon, drawRect, Color.White);
            else
                spriteBatch.Draw(combo.Icon, drawRect, Color.White);
        }
    }
}
