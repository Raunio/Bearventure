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
            Flying,
            Hovering,
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
        public bool hasJumped = false;

        #endregion

        #region Methods
        /// <summary>
        /// Rectangle used primarily for collision detection.
        /// </summary>
        public Rectangle BoundingBox
        {
            get
            {
                // TODO: Chop this monster. -Huemac
                return new Rectangle((int)position.X - (int)currentAnimation.Origin.X * (int)scale, (int)position.Y - (int)currentAnimation.Origin.Y * (int)scale, currentAnimation.FrameWidth * (int)scale, currentAnimation.FrameHeight * (int)scale);
            }
        }
        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // TODO: Chop this too -Huemac
            spriteBatch.Draw(spriteSheet, position, currentAnimation.FrameRectangle, Color.White, currentAnimation.Rotation, currentAnimation.Origin, scale, currentAnimation.Sprite_Effects, currentAnimation.LayerDepth);
        }

        public abstract void Update(GameTime gameTime);

        #endregion
    }
}
