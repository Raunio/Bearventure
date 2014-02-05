using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Bearventure.Gameplay.Characters;
using Bearventure.Gameplay.GameObjects;
using System.Diagnostics;

namespace Bearventure.Gameplay
{
    public abstract class GameplayObject
    {
        private int trueBBOffset;

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
                return CurrentAnimation.Rotation;
            }
            set
            {
                CurrentAnimation.Rotation = value;
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
        protected int BoundingBoxOffset
        {
            get;
            set;
        }
        protected int BoundingBoxAnimationOffset
        {
            get;
            set;
        }
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
        public Animation CurrentAnimation
        {
            get;
            set;
        }
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
                Color[] td = new Color[CurrentAnimation.FrameWidth * CurrentAnimation.FrameHeight];
                CurrentAnimation.FrameTexture.GetData(td);

                return td;
            }
        }
        public bool IsActive
        {
            get;
            protected set;
        }
        private Point boundingBoxSize;
        public Point BoundingBoxSize
        {
            get
            {
                if (boundingBoxSize == Point.Zero)
                    boundingBoxSize = new Point(CurrentAnimation.FrameWidth, CurrentAnimation.FrameHeight);

                return boundingBoxSize;

            }
            protected set
            {
                boundingBoxSize = value;
            }
        }
        /// <summary>
        /// Rectangle used primarily for collision detection.
        /// </summary>
        public Rectangle BoundingBox
        {
            get
            {
                int x = (int)(position.X - (CurrentAnimation.Origin.X * scale)) + BoundingBoxOffset + BoundingBoxAnimationOffset;
                int y = (int)(position.Y - (CurrentAnimation.Origin.Y * scale)) + BoundingBoxOffset;
                int width = (int)(BoundingBoxSize.X * scale) - BoundingBoxOffset * 2;
                int height = (int)(BoundingBoxSize.Y * scale) - BoundingBoxOffset;

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
            spriteBatch.Draw(CurrentAnimation.spriteSheet, position, CurrentAnimation.FrameRectangle, Color.White, CurrentAnimation.Rotation, CurrentAnimation.Origin, scale, CurrentAnimation.Effects, CurrentAnimation.LayerDepth);
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

        public void ChangeVelocity(float x, float y)
        {
            velocity.X = x;
            velocity.Y = y;
        }

        public void ChangeVelocity(float x, float y, string message)
        {
            velocity.X = x;
            velocity.Y = y;

            Debug.Write("Velocity changed to: " + x + "." + y + " " + message);

            CombatManager.Instance.CombatLog.Add("Velocity changed to: " + x + "." + y + " " + message);
        }
    }
}
