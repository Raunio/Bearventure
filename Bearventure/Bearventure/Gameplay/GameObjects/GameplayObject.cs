using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Bearventure.Gameplay.Characters;
using Bearventure.Gameplay.GameObjects;

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
        /// Some complex-shaped objects might want to use per pixel collision instead of rectangular collision detection.
        /// </summary>
        public Texture2D CollisionMap
        {
            get
            {
                return collisionMap;
            }
            protected set
            {
                collisionMap = value;
                Color[] data = new Color[collisionMap.Width * collisionMap.Height];
                collisionMap.GetData(data);
                CollisionMapData = TextureData2D(collisionMap, data);
            }
        }
        public Color[,] CollisionMapData
        {
            get;
            private set;
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
        /// <summary>
        /// Gets or sets an array of attachment points for the character. Attachemnt point positions are relative to character BoundingBox and
        /// are used by other game objects when they attach to the game object.
        /// </summary>
        public AttachmentPoint[] AttachmentPoints
        {
            get;
            set;
        }
        private Texture2D collisionMap;
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

        public void Deactivate()
        {
            IsActive = false;
        }

        private Color[,] TextureData2D(Texture2D texture, Color[] data)
        {
            Color[,] colors2D = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                    colors2D[x, y] = data[x + y * texture.Width];

            return colors2D;
        }
    }
}
