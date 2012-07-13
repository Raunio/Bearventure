#region Using-statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
#endregion

namespace Bearventure
{
    public class Animation
    {
        #region Members

        private Texture2D SpriteSheet;

        private Vector2 origin;

        private Rectangle frameRectangle;
        private int spriteSheetRow;
        private int startFrame;
        private int endFrame;
        private int currentFrame;
        private int frameWidth;
        private int frameHeight;
        private float animTimer;
        private float frameInterval;
        private float rotation;
        private SpriteEffects spriteEffects;
        private float layerDepth;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="_spriteSheet">Spritesheet used by the animation.</param>
        /// <param name="_spriteSheetRow">The row in which the frames are placed in the sheet. 0 means the first, uppermost row.</param>
        /// <param name="_frameWidth">The width of a individual frame.</param>
        /// <param name="_frameHeight">The height of a individual frame.</param>
        /// <param name="_startFrame">The first frame of the animation.</param>
        /// <param name="_endFrame">The last frame of the animation.</param>
        /// <param name="_frameInterval">Interval between frames in milliseconds</param>
        public Animation(Texture2D _spriteSheet, int _spriteSheetRow, int _frameWidth, int _frameHeight, int _startFrame, int _endFrame, float _frameInterval)
        {
            SpriteSheet = _spriteSheet;
            spriteSheetRow = _spriteSheetRow;
            frameWidth = _frameWidth;
            frameHeight = _frameHeight;
            spriteEffects = SpriteEffects.None;
            layerDepth = 0f;
            startFrame = _startFrame;
            endFrame = _endFrame;
            animTimer = 0f;
            currentFrame = _startFrame;
            origin = new Vector2(frameWidth / 2, frameHeight / 2);
            rotation = 0;
            frameInterval = _frameInterval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_spriteSheet">Spritesheet used by the animation.</param>
        /// <param name="_spriteSheetRow">The row in which the frames are placed in the sheet. 0 means the first, uppermost row.</param>
        /// <param name="_frameWidth">The width of a individual frame.</param>
        /// <param name="_frameHeight">The height of a individual frame.</param>
        /// <param name="_startFrame">The first frame of the animation.</param>
        /// <param name="_endFrame">The last frame of the animation.</param>
        /// <param name="_frameInterval">Interval between frames in milliseconds</param>
        /// <param name="_spriteEffects">Sprite effects used for the animation</param>
        /// <param name="_layerDepth">Layer depth for the animation</param>
        /// <param name="_rotation">Spritesheet rotation</param>
        public Animation(Texture2D _spriteSheet, int _spriteSheetRow, int _frameWidth, int _frameHeight, int _startFrame, int _endFrame, float _frameInterval, SpriteEffects _spriteEffects, float _layerDepth, float _rotation)
        {
            SpriteSheet = _spriteSheet;
            spriteSheetRow = _spriteSheetRow;
            frameWidth = _frameWidth;
            frameHeight = _frameHeight;
            spriteEffects = _spriteEffects;
            layerDepth = _layerDepth;
            startFrame = _startFrame;
            endFrame = _endFrame;
            animTimer = 0f;
            currentFrame = _startFrame;
            origin = new Vector2(frameWidth / 2, frameHeight / 2);
            rotation = _rotation;
            frameInterval = _frameInterval;
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

        /// <summary>
        /// Used to animate the animation from left to right.
        /// </summary>
        /// <param name="gameTime">GameTime should be passed for the timer</param>
        public void AnimateForward(GameTime gameTime)
        {
            Update();

            animTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (animTimer >= frameInterval)
            {
                animTimer = 0;
                NextFrame();
            }
        }

        /// <summary>
        /// Used to animate the animation from right to left.
        /// </summary>
        /// <param name="gameTime">GameTime should be passed for the timer</param>
        public void AnimateBackward(GameTime gameTime)
        {
            Update();

            animTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (animTimer >= frameInterval)
            {
                animTimer = 0;
                PreviousFrame();
            }
        }

        private void NextFrame()
        {
            if (currentFrame < endFrame)
            {
                currentFrame++;
            }
            else
            {
                currentFrame = startFrame;
            }
        }

        private void PreviousFrame()
        {
            if (currentFrame > startFrame)
            {
                currentFrame--;
            }
            else
            {
                currentFrame = endFrame;
            }
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
        /// <summary>
        /// Change the framerate of the animation.
        /// </summary>
        /// <param name="newInterval">New framerate value.</param>
        public void ChangeSpeed(float newInterval)
        {
            frameInterval = newInterval;
        }
        #endregion
    }
}
