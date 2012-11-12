using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XmlItems;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Bearventure.Gameplay.Levels
{
    class LevelBackground
    {
        private string asset;
        private string path;
        public int Fractions
        {
            get;
            set;
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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < Fractions / 2; x++)
                for (int y = 0; y < 2; y++)
                    spriteBatch.Draw(Backgrounds[x, y], new Vector2(Backgrounds[x, y].Width * x, Backgrounds[x, y].Height * y), Color.White);
        }

    }
}
