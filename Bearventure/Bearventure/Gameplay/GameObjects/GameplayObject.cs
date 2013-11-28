using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Bearventure.Gameplay.Characters;

namespace Bearventure.Gameplay
{
    public abstract class GameplayObject
    {
        public String TAG
        {
            get;
            protected set;
        }
        /// <summary>
        /// Gets or sets the rotation for the current spritesheet.
        /// </summary>
        public float SpriteRotation
        {
            get
            {
                return currentAnimation.Rotation;
            }
            set
            {
                currentAnimation.Rotation = value;
            }
        }
        protected int mass;
        protected int BoundingBoxOffset;
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
        /// Character jump times.
        /// </summary>
        public int jumpTime;
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
        public float decceleration;
        /// <summary>
        /// Color data used for collision
        /// </summary>
        public Color[] textureData
        {
            get
            {
                Color[] td = new Color[currentAnimation.FrameWidth * currentAnimation.FrameHeight];
                currentAnimation.FrameTexture.GetData(td);

                return td;
            }
        }
        public bool IsActive
        {
            get;
            protected set;
        }
        /// <summary>
        /// Rectangle used primarily for collision detection.
        /// </summary>
        public Rectangle BoundingBox
        {
            get
            {
                int x = (int)(position.X - (currentAnimation.Origin.X * scale)) + BoundingBoxOffset;
                int y = (int)(position.Y - (currentAnimation.Origin.Y * scale)) + BoundingBoxOffset;
                int width = (int)(currentAnimation.FrameWidth * scale) - BoundingBoxOffset * 2;
                int height = (int)(currentAnimation.FrameHeight * scale) - BoundingBoxOffset;

                return new Rectangle(x, y, width, height);
            }
        }
        /// <summary>
        /// Left or right.
        /// </summary>
        public Constants.DirectionX directionX;
        /// <summary>
        /// Up or down
        /// </summary>
        public Constants.DirectionY directionY;
        /// <summary>
        /// Object mass.
        /// </summary>
        public int Mass
        {
            get
            {
                return mass;
            }
        }
        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // TODO: Chop this too -Huemac
            spriteBatch.Draw(currentAnimation.spriteSheet, position, currentAnimation.FrameRectangle, Color.White, currentAnimation.Rotation, currentAnimation.Origin, scale, currentAnimation.Effects, currentAnimation.LayerDepth);
        }

        public abstract void Update(GameTime gameTime);
    }
}
