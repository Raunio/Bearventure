using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

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
            Flying
        };

        public enum Direction
        {
            Left,
            Right
        };
        #endregion
        #region Members
        protected Texture2D SpriteSheet;
        /// <summary>
        /// Character position.
        /// </summary>
        public Vector2 position;
        /// <summary>
        /// Character velocity.
        /// </summary>
        public Vector2 velocity;
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
        /// Range of vision.
        /// </summary>
        public int vision;
        /// <summary>
        /// Character attack range.
        /// </summary>
        public int attackRange;
        #endregion

        #region Methods
        /// <summary>
        /// Rectangle used primarily for collision detection.
        /// </summary>
        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)position.X - (int)currentAnimation.Origin.X * (int)scale, (int)position.Y - (int)currentAnimation.Origin.Y * (int)scale, currentAnimation.FrameWidth * (int)scale, currentAnimation.FrameHeight * (int)scale);
         
            }
        }
        /// <summary>
        /// piirrä hommofgd
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(SpriteSheet, position, currentAnimation.FrameRectangle, Color.White, currentAnimation.Rotation, currentAnimation.Origin, scale, currentAnimation.Sprite_Effects, currentAnimation.LayerDepth);
            
        }
        /// <summary>
        /// Method which should be called in the Update of the game to apply gravity to the character.
        /// </summary>
        /// <param name="amount">Amount of gravity</param>
        public virtual void ApplyGravity(float amount)
        {
            velocity.Y += amount;
            position.Y += velocity.Y;
        }

        public abstract void Update(GameTime gameTime);
        #endregion
    }
}
