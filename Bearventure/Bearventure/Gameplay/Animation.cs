#region Using-statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Bearventure
{
    public class Animation
    {
        #region Members

        private Texture2D spriteSheet;

        private Vector2 origin;

        private Rectangle frameRectangle;
        private int spriteSheetRow;
        private int startFrame;
        private int endFrame;
        private int currentFrame;
        private int frameWidth;
        private int frameHeight;
        private float animTimer;
        private float rotation;
        private SpriteEffects spriteEffects;
        private float layerDepth;
        private float interval;
        private bool backwards;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="spriteSheet">Spritesheet used by the animation.</param>
        /// <param name="spriteSheetRow">The row in which the frames are placed in the sheet. 0 means the first, uppermost row.</param>
        /// <param name="frameWidth">The width of a individual frame.</param>
        /// <param name="frameHeight">The height of a individual frame.</param>
        /// <param name="startFrame">The first frame of the animation.</param>
        /// <param name="endFrame">The last frame of the animation.</param>
        /// <param name="speed">Animation speed. Lower value = faster animation</param>
        /// <param name="backwards">True to animate from right to left.</param>
        public Animation(Texture2D spriteSheet, int spriteSheetRow, int frameWidth, int frameHeight, int startFrame, int endFrame, float speed, bool backwards = false)
        {
            this.spriteSheet = spriteSheet;
            this.spriteSheetRow = spriteSheetRow;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            spriteEffects = SpriteEffects.None;
            layerDepth = 0f;
            this.startFrame = startFrame;
            this.endFrame = endFrame;
            animTimer = 0f;
            currentFrame = startFrame;
            origin = new Vector2(frameWidth / 2, frameHeight / 2);
            rotation = 0;
            interval = speed;
            this.backwards = backwards;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteSheet">Spritesheet used by the animation.</param>
        /// <param name="spriteSheetRow">The row in which the frames are placed in the sheet. 0 means the first, uppermost row.</param>
        /// <param name="frameWidth">The width of a individual frame.</param>
        /// <param name="frameHeight">The height of a individual frame.</param>
        /// <param name="startFrame">The first frame of the animation.</param>
        /// <param name="endFrame">The last frame of the animation.</param>
        /// <param name="spriteEffects">Sprite effects used for the animation</param>
        /// <param name="layerDepth">Layer depth for the animation</param>
        /// <param name="rotation">Spritesheet rotation</param>
        /// <param name="speed">Animation speed. Lower value = faster animation.</param>
        /// <param name="backwards">True to animate from right to left.</param>
        public Animation(Texture2D spriteSheet, int spriteSheetRow, int frameWidth, int frameHeight, int startFrame, int endFrame, float speed, SpriteEffects spriteEffects, float layerDepth, float rotation, bool backwards = false)
        {
            this.spriteSheet = spriteSheet;
            this.spriteSheetRow = spriteSheetRow;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.spriteEffects = spriteEffects;
            this.layerDepth = layerDepth;
            this.startFrame = startFrame;
            this.endFrame = endFrame;
            animTimer = 0f;
            currentFrame = startFrame;
            origin = new Vector2(frameWidth / 2, frameHeight / 2);
            this.rotation = rotation;
            interval = speed;
            this.backwards = backwards;
        }
        #endregion

        #region Gets

        /// <summary>
        /// Frame Origin.
        /// </summary>
        public Vector2 Origin
        {
            get
            {
                return origin;
            }
        }

        /// <summary>
        /// Rectangle that picks individual frames from the animation spritesheet.
        /// </summary>
        public Rectangle FrameRectangle
        {
            get
            {
                return frameRectangle;
            }
        }

        /// <summary>
        /// The starting frame of the animation.
        /// </summary>
        public int StartFrame
        {
            get
            {
                return startFrame;
            }
        }

        /// <summary>
        /// The final frame of the animation.
        /// </summary>
        public int EndFrame
        {
            get
            {
                return endFrame;
            }
        }

        /// <summary>
        /// The width of a individual frame.
        /// </summary>
        public int FrameWidth
        {
            get
            {
                return frameWidth;
            }
        }

        /// <summary>
        /// The height of a individual frame.
        /// </summary>
        public int FrameHeight
        {
            get
            {
                return frameHeight;
            }
        }

        /// <summary>
        /// Spritesheet rotation.
        /// </summary>
        public float Rotation
        {
            get
            {
                return rotation;
            }
        }

        /// <summary>
        /// SpriteEffects used for the animation.
        /// </summary>
        public SpriteEffects Sprite_Effects
        {
            get
            {
                return spriteEffects;
            }
        }

        /// <summary>
        /// Layer-depth used for the animation
        /// </summary>
        public float LayerDepth
        {
            get
            {
                return layerDepth;
            }
        }
        #endregion
        #region Methods
        public void Animate(GameTime gameTime)
        {
            switch (backwards)
            {
                case true:
                    AnimateBackward(gameTime);
                    break;
                case false:
                    AnimateForward(gameTime);
                    break;
            }
        }
        private void AnimateForward(GameTime gameTime)
        {
            Update();

            animTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (animTimer >= interval)
            {
                animTimer = 0;
                NextFrame();
            }
        }

        private void AnimateBackward(GameTime gameTime)
        {
            Update();

            animTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (animTimer >= interval)
            {
                animTimer = 0;
                PreviousFrame();
            }
        }

        private void NextFrame()
        {
            if (currentFrame < endFrame)
                currentFrame++;
            else
                currentFrame = startFrame;
        }

        private void PreviousFrame()
        {
            if (currentFrame > startFrame)
                currentFrame--;
            else
                currentFrame = endFrame;
        }
        /// <summary>
        /// Jump to frame
        /// </summary>
        public void GoToFrame(int frame)
        {
            currentFrame = frame;
            Update();
        }

        private void Update()
        {
            frameRectangle = new Rectangle(currentFrame * frameWidth, spriteSheetRow * frameHeight, frameWidth, frameHeight);
        }

        #endregion
    }
}
