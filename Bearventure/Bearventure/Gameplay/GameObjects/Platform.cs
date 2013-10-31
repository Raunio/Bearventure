using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Bearventure.Gameplay.Characters;
using Microsoft.Xna.Framework.Content;

namespace Bearventure.Gameplay.GameObjects
{
    public class Platform : GameplayObject
    {
        Constants.PlatformType type;
        Behaviour behaviour;

        public Platform(ContentManager content, Constants.PlatformType type, Vector2 position)
        {
            this.type = type;
            this.position = position;

            Initialize(content);
        }

        public Platform(ContentManager content, Constants.PlatformType type, Vector2 position, Behaviour behaviour)
        {
            this.type = type;
            this.position = position;
            this.behaviour = behaviour;

            Initialize(content);
        }

        private void Initialize(ContentManager content)
        {
            switch (type)
            {
                case Constants.PlatformType.Basic:
                    currentAnimation = new Animation(content.Load<Texture2D>(Constants.BasicPlatform), 0, 80, 25, 0, 1, 25);
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            currentAnimation.Animate(gameTime);
            if (behaviour != null)
            {
                behaviour.Apply(gameTime);
            }
        }
    }
}
