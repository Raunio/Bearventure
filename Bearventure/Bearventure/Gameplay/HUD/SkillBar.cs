using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bearventure.Gameplay.Characters;

namespace Bearventure.Gameplay.HUD
{
    public class SkillBar
    {
        Player player;
        SkillSlot[] skillSlots;
        Vector2 position;

        int slotWidth = 48;
        int slotHeight = 48;

        public int BarWidth
        {
            get
            {
                return slotWidth * skillSlots.Length;
            }
        }

        public int BarHeight
        {
            get
            {
                return slotHeight;
            }
        }

        public SkillBar(Player player)
        {
            this.player = player;
            skillSlots = new SkillSlot[4];

            skillSlots[0] = new SkillSlot(player.ActiveCombo);

            for (int i = 1; i < skillSlots.Length; i++)
            {
                if(player.UsableSkills[i - 1] != null)
                    skillSlots[i] = new SkillSlot(player.UsableSkills[i - 1]);
            }
        }

        public void Update(GameTime gameTime, Vector2 position)
        {
            this.position = position;

            for (int i = 0; i < skillSlots.Length; i++)
            {
                if(skillSlots[i] != null)
                    skillSlots[i].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < skillSlots.Length; i++)
            {
                if (skillSlots[i] != null)
                    skillSlots[i].Draw(spriteBatch, new Rectangle((int)position.X + slotWidth * i, (int)position.Y, slotWidth, slotHeight));
            }
        }
    }
}
