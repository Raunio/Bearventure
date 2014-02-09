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
        public Vector2 Position
        {
            get
            {
                return new Vector2(position.X, position.Y);
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
        protected Vector2 position;
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

        private float targetScale = 1f;

        private float scalingSpeed;

        private float targetRotation = 0f;

        private float rotatingSpeed;

        private bool considerDirection;

        private float opacity = 1f;
        private float targetOpacity;
        private float opacitySpeed;

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
        public bool IsRotating
        {
            get
            {
                if (SpriteRotation != targetRotation && SpriteRotation != -targetRotation)
                    return true;
                return false;
            }
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
                int x = (int)(Position.X - (CurrentAnimation.Origin.X * scale)) + BoundingBoxOffset + BoundingBoxAnimationOffset;
                int y = (int)(Position.Y - (CurrentAnimation.Origin.Y * scale)) + BoundingBoxOffset;
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
        public float Opacity
        {
            get
            {
                return opacity;
            }
        }
        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // TODO: Chop this too -Huemac
            spriteBatch.Draw(CurrentAnimation.spriteSheet, Position, CurrentAnimation.FrameRectangle, new Color(255, 255, 255, Opacity), CurrentAnimation.Rotation, CurrentAnimation.Origin, scale, CurrentAnimation.Effects, CurrentAnimation.LayerDepth);
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

        public void ChangePosition(Vector2 pos)
        {
            position.X = pos.X;
            position.Y = pos.Y;
        }

        public void AdjustPosition(Vector2 amount)
        {
            position += amount;
        }

        public void SetScaling(float from, float to, float speed)
        {
            scale = from;
            targetScale = to;
            scalingSpeed = speed;
        }

        public void SetRotating(float from, float to, float speed, bool considerDirection)
        {
            SpriteRotation = from;
            targetRotation = to;
            rotatingSpeed = speed;
            this.considerDirection = considerDirection;
        }

        public void SetOpacity(float from, float to, float speed)
        {
            this.opacity = from;
            this.targetOpacity = to;
            this.opacitySpeed = speed;
        }

        protected void UpdateRotating()
        {
            float targetRot = targetRotation;

            if (considerDirection)
            {
                if (directionX == Constants.DirectionX.Right)
                    targetRot = -targetRot;
            }

            if (SpriteRotation < targetRot - rotatingSpeed)
            {
                SpriteRotation += rotatingSpeed;
            }
            else if (SpriteRotation > targetRot + rotatingSpeed)
            {
                SpriteRotation -= rotatingSpeed;
            }
            else
                SpriteRotation = targetRot;
        }

        protected void UpdateScaling()
        {
            if (scale < targetScale - scalingSpeed)
            {
                scale += scalingSpeed;
            }
            else if (scale > targetScale + scalingSpeed)
            {
                scale -= scalingSpeed;
            }
            else
                scale = targetScale;
        }

        protected void UpdateOpacity()
        {
            if (opacity < targetOpacity - opacitySpeed)
            {
                opacity += opacitySpeed;
            }
            else if (opacity > targetOpacity + opacitySpeed)
            {
                opacity -= opacitySpeed;
            }
            else
                opacity = targetOpacity;
        }
    }
}
