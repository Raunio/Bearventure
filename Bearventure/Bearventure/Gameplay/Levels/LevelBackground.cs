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

        private void LoadMap(ContentManager content)
        {
            int counter = 0;

            for (int x = 0; x < Fractions / 2; x++)
                for (int y = 0; y < 2; y++)
                {
                    Backgrounds[x, y] = content.Load<Texture2D>(path + "/Background/" + asset + "_" + (counter));
                    counter++;
                }

            Width = Backgrounds[0, 0].Width * Fractions / 2;
            Height = Backgrounds[0, 0].Height * 2;

            zone_width = Backgrounds[0, 0].Width * 2;
            zone_height = Backgrounds[0, 0].Height * 2;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < Fractions / 2; x++)
                for (int y = 0; y < 2; y++)
                    spriteBatch.Draw(Backgrounds[x, y], new Vector2(Backgrounds[x, y].Width * x * 4, Backgrounds[x, y].Height * y * 4), null, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
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
                    spriteBatch.Draw(edgeTexture, new Rectangle(Backgrounds[x, y].Width * x * 2, Backgrounds[x, y].Height * y * 2, Backgrounds[x, y].Width * 2, 1), Color.White);
                    spriteBatch.Draw(edgeTexture, new Rectangle(Backgrounds[x, y].Width * x * 2, Backgrounds[x, y].Height * y * 2, 1, Backgrounds[x, y].Height * 2), Color.White);
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
                            zone = i * 2 + 1;
                        }
                        else
                        {
                            zone = i * 2;
                        }
                    }
                }
            }

            return zone;
        }

    }
}
