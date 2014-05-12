using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Bearventure.Gameplay.HUD
{
    public class CombatTextManager
    {
        private List<FloatingText> floatingTexts = new List<FloatingText>();
        private SpriteFont font;

        public void Initialize(ContentManager content)
        {
            font = content.Load<SpriteFont>(Constants.GameFont);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < floatingTexts.Count; i++)
            {
                if (!floatingTexts[i].IsAlive)
                    floatingTexts.RemoveAt(i);
                else
                    floatingTexts[i].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < floatingTexts.Count; i++)
            {
                floatingTexts[i].Draw(spriteBatch, font);
            }
        }

        public void AddText(FloatingText text)
        {
            floatingTexts.Add(text);
        }
    }
}
