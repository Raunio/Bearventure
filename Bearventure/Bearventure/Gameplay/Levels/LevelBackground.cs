using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XmlItems;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Bearventure.Gameplay.Characters;

namespace Bearventure.Gameplay.Levels
{
    class LevelBackground
    {
        private string asset;
        private string path;
        private Texture2D edgeTexture;
        private int zone_height;
        private int zone_width;
        private int resizeFactor = 2;

        /// <summary>
        /// Gets the amount of fractions the background object has.
        /// </summary>
        public int Fractions
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the all images as an array
        /// </summary>
        public Texture2D[,] Backgrounds
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the width of the map
        /// </summary>
        public int Width
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the height of the map
        /// </summary>
        public int Height
        {
            get;
            private set;
        }

        public LevelBackground(LevelInfo levelInfo, ContentManager content)
        {
            asset = levelInfo.BackgroundAssetName;
            Fractions = levelInfo.BackgroundFractions;
            path = "Levels/" + levelInfo.Name;

            Backgrounds = new Texture2D[Fractions / 2, 2];
            edgeTexture = new Texture2D(ResolutionManager.graphicsDevice.GraphicsDevice, 1, 1);
            edgeTexture.SetData(new[] { Color.Yellow });

            LoadMap(content);
        }

        public LevelBackground(string path, string asset, int fractions, ContentManager content)
        {
            this.asset = asset;
            Fractions = fractions;
            this.path = path;

            Backgrounds = new Texture2D[Fractions / 2, 2];
            edgeTexture = new Texture2D(ResolutionManager.graphicsDevice.GraphicsDevice, 1, 1);
            edgeTexture.SetData(new[] { Color.Yellow });

            LoadMap(content);
        }

        private void LoadMap(ContentManager content)
        {
            int counter = 0;

            for (int x = 0; x < Fractions / 2; x++)
                for (int y = 0; y < 2; y++)
                {
                    Backgrounds[x, y] = content.Load<Texture2D>("Levels/" + path + "/" + asset + "_" + (counter));
                    counter++;
                }

            Width = Backgrounds[0, 0].Width * Fractions / 2 * resizeFactor;
            Height = Backgrounds[0, 0].Height * 2 * resizeFactor;

            zone_width = Backgrounds[0, 0].Width * resizeFactor;
            zone_height = Backgrounds[0, 0].Height * resizeFactor;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < Fractions / 2; x++)
                for (int y = 0; y < 2; y++)
                    spriteBatch.Draw(Backgrounds[x, y],
                        new Vector2(x * Backgrounds[x, y].Width * resizeFactor,
                            y * Backgrounds[x, y].Height * resizeFactor), null, Color.White,
                            0f, Vector2.Zero, resizeFactor, SpriteEffects.None, 0f);
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 positionOffset)
        {
            for (int x = 0; x < Fractions / 2; x++)
                for (int y = 0; y < 2; y++)
                    spriteBatch.Draw(Backgrounds[x, y],
                        new Vector2(positionOffset.X + x * Backgrounds[x, y].Width * resizeFactor,
                            positionOffset.Y + y * Backgrounds[x, y].Height * resizeFactor), null, Color.White,
                            0f, Vector2.Zero, resizeFactor, SpriteEffects.None, 0f);
        }
        /// <summary>
        /// Method used for testing
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawGrid(SpriteBatch spriteBatch)
        {
            for(int x = 0; x < Fractions / 2; x++)
                for(int y = 0; y < 2; y++)
                {
                    spriteBatch.Draw(edgeTexture, new Rectangle(Backgrounds[x, y].Width * x * resizeFactor,
                        Backgrounds[x, y].Height * y * resizeFactor, Backgrounds[x, y].Width * resizeFactor, 1), Color.White);
                    spriteBatch.Draw(edgeTexture, new Rectangle(Backgrounds[x, y].Width * x * resizeFactor,
                        Backgrounds[x, y].Height * y * resizeFactor, 1, Backgrounds[x, y].Height * resizeFactor), Color.White);
                }
        }
        /// <summary>
        /// Returns the zone the rectangle is on.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public int OnZone(Rectangle rectangle)
        {
            int zone = 0;

            for (int i = 0; i < Fractions / 2; i++)
            {
                if (rectangle.X > zone_width * i)
                {
                    if (rectangle.X < zone_width * (i + 1))
                    {
                        if (rectangle.Y > zone_height)
                        {
                            zone = i * resizeFactor + 1;
                        }
                        else
                        {
                            zone = i * resizeFactor;
                        }
                    }
                }
            }

            return zone;
        }

    }
}
