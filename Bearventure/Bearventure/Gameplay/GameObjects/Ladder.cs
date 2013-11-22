using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Bearventure.Gameplay.Characters;
using Microsoft.Xna.Framework.Graphics;

namespace Bearventure.Gameplay.GameObjects
{
    public class Ladder : GameplayObject
    {
        Constants.LadderType type;

        public Ladder(ContentManager content, Constants.LadderType type, Vector2 position)
        {
            this.type = type;
            this.position = position;

            TAG = "Ladder";
            IsActive = true;
            this.BoundingBoxOffset = 30;

            Initialize(content);
        }
        private void Initialize(ContentManager content)
        {
            switch (type)
            {
                case Constants.LadderType.Wooden:
                    currentAnimation = new Animation(content.Load<Texture2D>(Constants.WoodenLadder), 0, 92, 238, 0, 0, 25);
                    break;

            }
        }
        public override void Update(GameTime gameTime)
        {
            currentAnimation.Animate(gameTime);
        }
    }
}
