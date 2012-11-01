using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bearventure
{
    public abstract class Character
    {
        #region Enumerations
        public enum State
        {
            Walking,
            Running,
            Attacking,
            Stopped,
            Jumping,
            Falling,
            Disabled,
            UsingSkill,
        };

        public enum Direction
        {
            Left,
            Right
        };

        public enum Orientation
        {
            Ground,
            Air,
        };

        #endregion
        #region Members
        protected Texture2D spriteSheet;
        /// <summary>
        /// Character position.
        /// </summary>
        public Vector2 position;
        /// <summary>
        /// Character velocity.
        /// </summary>
        public Vector2 velocity;
        /// <summary>
        /// Current animation of character
        /// </summary>
        public Animation currentAnimation;
        protected float scale = 1f;
        /// <summary>
        /// Character walking speed.
        /// </summary>
        public float walkSpeed;
        /// <summary>
        /// Character running speed.
        /// </summary>
        public float runSpeed;
        /// <summary>
        /// Character acceleration.
        /// </summary>
        public float acceleration;
        /// <summary>
        /// Character brake strenght.
        /// </summary>
        public float deacceleration;
        /// <summary>
        /// Attacks per second.
        /// </summary>
        public float attackSpeed;
        /// <summary>
        /// Character mass.
        /// </summary>
        public int mass;
        /// <summary>
        /// Represents the state of the character
        /// </summary>
        public State state;
        /// <summary>
        /// Left or right.
        /// </summary>
        public Direction direction;
        /// <summary>
        /// Character current health.
        /// </summary>
        public int health;
        /// <summary>
        /// Character maximun health.
        /// </summary>
        public int max_health;
        /// <summary>
        /// Character jump strenght.
        /// </summary>
        public int jumpStrenght;
        /// <summary>
        /// Character health regeneration per five seconds.
        /// </summary>
        public int health_regen;
        private float regen_timer;
        /// <summary>
        /// Character orientation. Air or Ground.
        /// </summary>
        public Orientation orientation
        {
            protected set;
            get;
        }
        /// <summary>
        /// Color data used for collision
        /// </summary>
        public Color[] textureData
        {
            get
            {
                Color[] td =  new Color[currentAnimation.FrameWidth * currentAnimation.FrameHeight];
                currentAnimation.FrameTexture.GetData(td);

                return td;
            }
        }
        /// <summary>
        /// Object that calculates collisions
        /// </summary>
        public CollisionHandler collisionHandler;

        #endregion

        #region Methods
        /// <summary>
        /// Rectangle used primarily for collision detection.
        /// </summary>
        public Rectangle BoundingBox
        {
            get
            {
                int x = (int)position.X - (int)currentAnimation.Origin.X * (int)scale;
                int y = (int)position.Y - (int)currentAnimation.Origin.Y * (int)scale;
                int width = currentAnimation.FrameWidth * (int)scale;
                int height = currentAnimation.FrameHeight * (int)scale;

                return new Rectangle(x, y, width, height);
            }
        }
        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // TODO: Chop this too -Huemac
            spriteBatch.Draw(spriteSheet, position, currentAnimation.FrameRectangle, Color.White, currentAnimation.Rotation, currentAnimation.Origin, scale, currentAnimation.Effects, currentAnimation.LayerDepth);
        }

        public abstract void Update(GameTime gameTime);

        public void RegenerateHealth(GameTime gameTime)
        {
            regen_timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (regen_timer >= 5f && health < max_health)
            {
                health += health_regen;
                regen_timer = 0;

                if (health > max_health)
                    health = max_health;
            }
        }

        #endregion
    }
}
