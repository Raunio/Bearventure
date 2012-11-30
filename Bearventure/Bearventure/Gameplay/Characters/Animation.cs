#region Using-statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
#endregion

namespace Bearventure.Gameplay.Characters
{
    public class Animation
    {
        #region Members

        private int spriteSheetRow;
        private float animTimer;
        private float interval;
        private bool backwards;
        private bool looping;
        private float freezeTimer;

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
        /// <param name="looping">True to loop animation</param>
        public Animation(Texture2D spriteSheet, int spriteSheetRow, int frameWidth, int frameHeight, int startFrame, int endFrame, float speed, bool backwards = false, bool looping = true)
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
            CurrentFrame = startFrame;
            this.Origin = new Vector2(frameWidth / 2, frameHeight / 2);
            this.Rotation = 0;
            interval = speed;
            this.backwards = backwards;
            this.looping = looping;
            HasFinished = false;
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
        /// <param name="looping">True to loop animation</param>
        public Animation(Texture2D spriteSheet, int spriteSheetRow, int frameWidth, int frameHeight, int startFrame, int endFrame, float speed, SpriteEffects spriteEffects, float layerDepth, float rotation, bool backwards = false, bool looping = true)
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
            CurrentFrame = startFrame;
            this.Origin = new Vector2(frameWidth / 2, frameHeight / 2);
            this.Rotation = rotation;
            interval = speed;
            this.backwards = backwards;
            this.looping = looping;
            HasFinished = false;
        }
        #endregion

        #region Gets & Sets
        public Texture2D spriteSheet
        {
            get;
            private set;
        }
        /// <summary>
        /// Frame Origin.
        /// </summary>
        public Vector2 Origin
        {
            get;
            private set;
        }

        /// <summary>
        /// Rectangle that picks individual frames from the animation spritesheet.
        /// </summary>
        public Rectangle FrameRectangle
        {
            get;
            private set;
        }

        /// <summary>
        /// The starting frame of the animation.
        /// </summary>
        public int StartFrame
        {
            get;
            private set;
        }

        /// <summary>
        /// The final frame of the animation.
        /// </summary>
        public int EndFrame
        {
            get;
            private set;
        }

        /// <summary>
        /// The width of a individual frame.
        /// </summary>
        public int FrameWidth
        {
            get;
            private set;
        }

        /// <summary>
        /// The height of a individual frame.
        /// </summary>
        public int FrameHeight
        {
            get;
            private set;
        }

        /// <summary>
        /// Spritesheet rotation.
        /// </summary>
        public float Rotation
        {
            get;
            private set;
        }

        /// <summary>
        /// SpriteEffects used for the animation.
        /// </summary>
        public SpriteEffects Effects
        {
            get;
            private set;
        }

        /// <summary>
        /// Layer-depth used for the animation
        /// </summary>
        public float LayerDepth
        {
            get;
            private set;
        }

        public int CurrentFrame
        {
            get;
            private set;
        }
        /// <summary>
        /// True if animation has reached its end.
        /// </summary>
        public bool HasFinished
        {
            get;
            private set;
        }
        /// <summary>
        /// Returns Texture2D object of current frame
        /// </summary>
        public Texture2D FrameTexture
        {
            get
            {
                GraphicsDevice graph = spriteSheet.GraphicsDevice;
                RenderTarget2D render = new RenderTarget2D(graph, FrameWidth, FrameHeight);
                SpriteBatch spriteBatch = new SpriteBatch(graph);

                graph.SetRenderTarget(render);
                graph.Clear(new Color(0, 0, 0, 0));

                spriteBatch.Begin();

                spriteBatch.Draw(spriteSheet, Vector2.Zero, FrameRectangle, Color.White);

                spriteBatch.End();

                graph.SetRenderTarget(null);

                return (Texture2D)render;
            }
        }
        
        public struct FrameFreezer
        {
            public List<int> Frames;
            public float Amount;
        };

        private bool IsCurrentFrameFreezable
        {
            get
            {
                foreach (int i in FreezeFrames.Frames)
                {
                    if (CurrentFrame == i)
                        return true;
                }

                return false;
            }
        }
        /// <summary>
        /// Gets and sets the frames that the animation will freeze to for a time euqal to Amount in milliseconds.
        /// </summary>
        public FrameFreezer FreezeFrames
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

        public void Reset()
        {
            HasFinished = false;
            if (backwards == true)
                CurrentFrame = EndFrame;
            else
                CurrentFrame = StartFrame;
        }

        private void AnimateForward(GameTime gameTime)
        {
            Update();

            animTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (animTimer >= interval)
            {
                if (FreezeFrames.Frames != null && IsCurrentFrameFreezable)
                {
                    freezeTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (freezeTimer >= FreezeFrames.Amount + interval)
                    {
                        freezeTimer = 0;
                        animTimer = 0;
                        NextFrame();
                    }
                }
                else
                {
                    animTimer = 0;
                    NextFrame();
                }
            }
        }

        private void AnimateBackward(GameTime gameTime)
        {
            Update();

            animTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (animTimer >= interval)
            {
                if (FreezeFrames.Frames != null && IsCurrentFrameFreezable)
                {
                    freezeTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (freezeTimer >= FreezeFrames.Amount)
                    {
                        animTimer = 0;
                        PreviousFrame();
                    }
                }
                else
                {
                    animTimer = 0;
                    PreviousFrame();
                }
            }
        }

        private void NextFrame()
        {
            if (CurrentFrame < EndFrame)
                CurrentFrame++;
            else
            {
                if (looping == true)
                    CurrentFrame = StartFrame;

                else if (HasFinished == false)
                {
                    HasFinished = true;
                }
            }
        }

        private void PreviousFrame()
        {
            if (CurrentFrame > StartFrame)
                CurrentFrame--;
            else
            {
                if (looping == true)
                    CurrentFrame = EndFrame;

                else if (HasFinished == false)
                {
                    HasFinished = true;
                }
            }
        }
        /// <summary>
        /// Jump to frame
        /// </summary>
        public void GoToFrame(int frame)
        {
            CurrentFrame = frame;
            Update();
        }

        private void Update()
        {
            FrameRectangle = new Rectangle(CurrentFrame * FrameWidth, spriteSheetRow * FrameHeight, FrameWidth, FrameHeight);
        }


        #endregion
    }
}
