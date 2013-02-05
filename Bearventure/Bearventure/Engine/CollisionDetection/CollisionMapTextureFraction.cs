using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Bearventure.Engine.CollisionDetection
{
    public class CollisionMapTextureFraction
    {
        private string asset;
        private int index;
        /// <summary>
        /// Gets the texture fraction.
        /// </summary>
        public Texture2D Texture
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the 2-dimentional array of texturedata from the texture fraction.
        /// </summary>
        public Color[,] Data
        {
            get;
            private set;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="index">All image fractions are indexed with a number by the level editor.</param>
        public CollisionMapTextureFraction(string asset, int index)
        {
            this.asset = asset;
            this.index = index;
        }
        /// <summary>
        /// Load all data from the texture.
        /// </summary>
        public void LoadData()
        {     
            Color[] data = new Color[Texture.Width * Texture.Height];
            Texture.GetData(data);
            Data = TextureData2D(Texture, data);
        }
        /// <summary>
        /// Load the texture.
        /// </summary>
        /// <param name="content"></param>
        public void LoadTexture(ContentManager content)
        {
            Texture = content.Load<Texture2D>(asset + index);
            
        }
        /// <summary>
        /// Clear the texture from memory.
        /// </summary>
        public void Clear()
        {
            Texture = null;
        }

        private Color[,] TextureData2D(Texture2D texture, Color[] data)
        {
            Color[,] colors2D = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                    colors2D[x, y] = data[x + y * texture.Width];

            return colors2D;
        }
    }
}
