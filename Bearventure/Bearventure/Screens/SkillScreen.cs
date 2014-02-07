using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Bearventure
{
    class SkillScreen : MenuScreen
    {
        Texture2D skillWindowBorder;
        Vector2 skillWindowBorderPos;
        Point resolution;
        ContentManager content;

        public SkillScreen()
            :base("Mad Skillz Brah!")
        {
            
        }

        public override void Activate(bool instancePreserved)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            content = new ContentManager(ScreenManager.Game.Services, "Content");
            resolution = ResolutionManager.GetVirtualResolution();
            skillWindowBorder = content.Load<Texture2D>("Bitmaps/skillBlock");
            skillWindowBorderPos = new Vector2(resolution.X * 0.1f, resolution.Y * 0.1f);
        }

        public void Update()
        {

        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.Draw(skillWindowBorder, skillWindowBorderPos, Color.White);
            spriteBatch.End();

        }
    }
}
