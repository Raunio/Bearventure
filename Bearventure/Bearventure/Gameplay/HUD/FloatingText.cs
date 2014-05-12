using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bearventure.Gameplay.HUD
{
    public class FloatingText
    {
        private String text;

        private Constants.FloatingTextType type;
        private Vector2 position;
        private Color textColor = Color.Yellow;

        private Vector2 velocity;

        private Vector2 acceleration;

        private Random rand = new Random();

        private float lifeTime = 1300;
        private float lifeCounter;

        private float scale = 0.9f;
        private float rotation = 0f;

        private float alpha = 1f;
        private float fadingSpeed = 0.025f;

        public FloatingText(String text, Constants.FloatingTextType type, Vector2 position)
        {
            this.text = text;
            this.type = type;
            this.position = position;

            if (type == Constants.FloatingTextType.Emphasised)
            {
                velocity = Vector2.Zero;
                textColor = Color.White;

                scale = 1.3f;
            }
            else
            {
                int r = rand.Next(0, 100);

                if (r <= 50)
                {
                    velocity = new Vector2(2, -5);
                    acceleration = new Vector2(0.125f, 0.175f);
                }
                else
                {
                    velocity = new Vector2(-2, -5);
                    acceleration = new Vector2(-0.125f, 0.175f);
                }
            }
            
        }

        public void Update(GameTime gameTime)
        {
            lifeCounter += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            velocity += acceleration;
            position += velocity;

            if (alpha > 0)
            {
                alpha -= fadingSpeed;

                //textColor.A = alpha;
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.DrawString(font, text, position, textColor * alpha, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public bool IsAlive
        {
            get
            {
                if (lifeCounter >= lifeTime)
                    return false;

                return true;
            }
        }
    }
}
