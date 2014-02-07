using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bearventure.Gameplay.Characters;
using Microsoft.Xna.Framework.Content;

namespace Bearventure.Gameplay.HUD
{
    public class StatusBar
    {
        #region Members

        private GraphicsDeviceManager graphics;
        private SpriteFont font;

        private int currentValue;
        private int targetValue;
        private int minValue;

        private float animTimer;

        private float updateSpeed = 25;

        private bool isDisabled;

        #endregion
        #region Gets and Sets
        /// <summary>
        /// Gets and sets the position of the health bar.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return new Vector2(Base.X + Base.Width / 2, Base.Y + Base.Height / 2);
            }
            set
            {
                Base = new Rectangle((int)(value.X - Base.Width / 2), (int)(value.Y - Base.Height / 2), Base.Width, Base.Height);
            }
        }
        /// <summary>
        /// Gets and sets the position offset of the health bar.
        /// </summary>
        public Vector2 Offset
        {
            get;
            set;
        }
        /// <summary>
        /// Gets and sets the rectangular shape of the health bar.
        /// </summary>
        public Rectangle Base
        {
            get;
            set;
        }
        /// <summary>
        /// Gets and sets the color of the bar edges.
        /// </summary>
        public Color EdgeColor
        {
            get
            {
                if (edgeColor == null)
                    return Color.Black;

                return edgeColor;
            }
            set
            {
                edgeColor = value;
            }
        }
        private Color edgeColor;
        /// <summary>
        /// Gets and sets the thickness of the bar edges.
        /// </summary>
        public int EdgeThickness
        {
            get
            {
                if (edgeThickness < 1)
                    return 1;

                return edgeThickness;
            }
            set
            {
                edgeThickness = value;
            }
        }
        private int edgeThickness;
        /// <summary>
        /// Gets and sets wether to show edges on the health bar.
        /// </summary>
        public bool ShowEdges
        {
            get
            {
                return showEdges;
            }

            set
            {
                showEdges = value;
            }
        }
        private bool showEdges;
        /// <summary>
        /// Gets and sets the texture of the health bar.
        /// </summary>
        public Texture2D BarTexture
        {
            get;
            set;
        }
        /// <summary>
        /// Gets and sets the color of the health bar.
        /// </summary>
        public Color BarColor
        {
            get
            {
                if (barColor == null)
                    return Color.Red;

                return barColor;
            }
            set
            {
                barColor = value;
            }
        }
        /// <summary>
        /// Gets and sets the Texture for the edges of the bar.
        /// </summary>
        public Texture2D EdgeTexture
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets wether to display values as text on top of the bar.
        /// </summary>
        public bool ShowText
        {
            get;
            set;
        }

        private Texture2D SolidColorTexture(Color color)
        {
                solidColorTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);

                solidColorTexture.SetData(new[] { color });

                return solidColorTexture;
        }
        private Texture2D solidColorTexture;

        private Color barColor;
        /// <summary>
        /// Gets the current value of the bar.
        /// </summary>
        public int CurrentValueDraw
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the maximum value of the bar.
        /// </summary>
        public int MaximumValue
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets or sets the color of displayed text.
        /// </summary>
        public Color TextColor
        {
            get;
            set;
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initialize the health bar.
        /// </summary>
        /// <param name="graphics"></param>
        public void Initialize(GraphicsDeviceManager graphicsDeviceManager, ContentManager content)
        {
            graphics = graphicsDeviceManager;
            font = content.Load<SpriteFont>(Constants.GameFont);
        }
        /// <summary>
        /// rectangle defines the rectangular shape and size of the health bar.
        /// </summary>
        /// <param name="rectangle"></param>
        public StatusBar(Rectangle rectangle)
        {
            this.Base = rectangle;
            EdgeColor = Color.Black;

            targetValue = 1;
            MaximumValue = 1;
            minValue = 1;
        }

        #endregion


        public void Update(GameTime gameTime, Vector2 Position, int minVal, int maxVal, int currentVal)
        {
            if (isDisabled)
                return;

            animTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (animTimer > updateSpeed)
            {
                UpdateHealthBar();
                animTimer = 0;
            }

            this.Position = Position;

            CurrentValueDraw = currentVal;
            MaximumValue = maxVal;
            minValue = minVal;
            targetValue = currentVal;
        }

        private void UpdateHealthBar()
        {
            if (currentValue < targetValue)
                currentValue++;
            else if (currentValue > targetValue)
                currentValue--;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isDisabled)
                return;

            int unit = Base.Width / MaximumValue;

            int current = unit * currentValue;

            Rectangle healthBar = new Rectangle(Base.X, Base.Y, current, Base.Height);

            if (BarTexture == null)
                BarTexture = SolidColorTexture(BarColor);

            spriteBatch.Draw(BarTexture, healthBar, Color.White);

            if (ShowEdges)
                DrawEdges(spriteBatch);
            if (ShowText)
                DrawText(spriteBatch);
        }

        private void DrawEdges(SpriteBatch spriteBatch)
        {
            if (EdgeTexture == null)
                EdgeTexture = SolidColorTexture(EdgeColor);

            spriteBatch.Draw(EdgeTexture, new Rectangle(Base.X - EdgeThickness, Base.Y - EdgeThickness, Base.Width + EdgeThickness * 2, EdgeThickness), Color.White);
            spriteBatch.Draw(EdgeTexture, new Rectangle(Base.X - EdgeThickness, Base.Bottom, Base.Width + EdgeThickness * 2, EdgeThickness), Color.White);

            spriteBatch.Draw(EdgeTexture, new Rectangle(Base.X - EdgeThickness, Base.Y - EdgeThickness, EdgeThickness, Base.Height + EdgeThickness * 2), Color.White);
            spriteBatch.Draw(EdgeTexture, new Rectangle(Base.Right, Base.Y - EdgeThickness, EdgeThickness, Base.Height + EdgeThickness * 2), Color.White);
        }

        private void DrawText(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, CurrentValueDraw.ToString() + "/" + MaximumValue, new Vector2(Position.X, Position.Y - 15), TextColor);
        }

        public void Disable()
        {
            isDisabled = true;
        }
    }
}
