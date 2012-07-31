using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bearventure
{
    public class Player : Character
    {
        Animation stopped;

        public Player(Texture2D spriteSheet)
        {
            this.spriteSheet = spriteSheet;
            stopped = new Animation(spriteSheet, 0, 20, 20, 1, 1, 40);
            currentAnimation = stopped;
        }

        public override void Update(GameTime gameTime)
        {
            position.X = Mouse.GetState().X;
            position.Y = Mouse.GetState().Y;
            currentAnimation.Animate(gameTime);
        }
    }
}
