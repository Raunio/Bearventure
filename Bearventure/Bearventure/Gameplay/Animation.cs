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

        private int spriteSheetRow;
        private int currentFrame;
        private float animTimer;
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
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.Effects = SpriteEffects.None;
            this.LayerDepth = 0f;
            this.StartFrame = startFrame;
            this.EndFrame = endFrame;
            animTimer = 0f;
            currentFrame = startFrame;
            this.Origin = new Vector2(frameWidth / 2, frameHeight / 2);
            this.Rotation = 0;
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
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.Effects = spriteEffects;
            this.LayerDepth = layerDepth;
            this.StartFrame = startFrame;
            this.EndFrame = endFrame;
            animTimer = 0f;
            currentFrame = startFrame;
            this.Origin = new Vector2(frameWidth / 2, frameHeight / 2);
            this.Rotation = rotation;
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
            get;
            set;
        }

        /// <summary>
        /// Rectangle that picks individual frames from the animation spritesheet.
        /// </summary>
        public Rectangle FrameRectangle
        {
            get;
            set;
        }

        /// <summary>
        /// The starting frame of the animation.
        /// </summary>
        public int StartFrame
        {
            get;
            set;
        }

        /// <summary>
        /// The final frame of the animation.
        /// </summary>
        public int EndFrame
        {
            get;
            set;
        }

        /// <summary>
        /// The width of a individual frame.
        /// </summary>
        public int FrameWidth
        {
            get;
            set;
        }

        /// <summary>
        /// The height of a individual frame.
        /// </summary>
        public int FrameHeight
        {
            get;
            set;
        }

        /// <summary>
        /// Spritesheet rotation.
        /// </summary>
        public float Rotation
        {
            get;
            set;
        }

        /// <summary>
        /// SpriteEffects used for the animation.
        /// </summary>
        public SpriteEffects Effects
        {
            get;
            set;
        }

        /// <summary>
        /// Layer-depth used for the animation
        /// </summary>
        public float LayerDepth
        {
            get;
            set;
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
            if (currentFrame < EndFrame)
                currentFrame++;
            else
                currentFrame = StartFrame;
        }

        private void PreviousFrame()
        {
            if (currentFrame > StartFrame)
                currentFrame--;
            else
                currentFrame = EndFrame;
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
            FrameRectangle = new Rectangle(currentFrame * FrameWidth, spriteSheetRow * FrameHeight, FrameWidth, FrameHeight);
        }

        #endregion
    }
}
