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
        SkillBlock skillz;


        public SkillScreen()
            :base("Mad Skillz Brah!")
        {

            skillz = new SkillBlock("penis");
            
        }

        public override void Activate(bool instancePreserved)
        {
            base.Activate(instancePreserved);
            skillz.Initialize(ScreenManager.Game.Content);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            skillz.Draw(ScreenManager.SpriteBatch);
        }

    }
}
