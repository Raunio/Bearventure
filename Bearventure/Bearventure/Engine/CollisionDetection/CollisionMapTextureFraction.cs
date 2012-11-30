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

        public Texture2D Texture
        {
            get;
            private set;
        }

        public Color[,] Data
        {
            get;
            private set;
        }

        public CollisionMapTextureFraction(string asset, int index)
        {
            this.asset = asset;
            this.index = index;
        }

        public void LoadData()
        {     
            Color[] data = new Color[Texture.Width * Texture.Height];
            Texture.GetData(data);
            Data = TextureData2D(Texture, data);
        }

        public void LoadTexture(ContentManager content)
        {
            Texture = content.Load<Texture2D>(asset + index);
            
        }

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
