using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Bearventure.Engine.CollisionDetection
{
    public class CollisionMap
    {
        /// <summary>
        /// Gets the amount of fractions the map is cropped to.
        /// </summary>
        public int Fractions
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the factor the texture sizes are divided by.
        /// </summary>
        public int ResizeFactor
        {
            get;
            private set;
        }

        public CollisionMapTextureFraction[] CroppedTextures
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the size of an individual crop.
        /// </summary>
        public Point CropSize
        {
            get;
            private set;
        }

        public CollisionMap(string asset, int crops, int resizeFactor)
        {
            Fractions = crops;

            ResizeFactor = resizeFactor;

            CroppedTextures = new CollisionMapTextureFraction[crops];

            for (int i = 0; i < crops; i++)
                CroppedTextures[i] = new CollisionMapTextureFraction(asset, i);
            
        }

        /// <summary>
        /// Loads all the textures in CroppedTextures.
        /// </summary>
        /// <param name="content"></param>
        public void LoadAllTextures(ContentManager content)
        {
            foreach (CollisionMapTextureFraction tx in CroppedTextures)
                tx.LoadTexture(content);

            CropSize = new Point(CroppedTextures[0].Texture.Width, CroppedTextures[0].Texture.Height);
        }

        public void LoadAllTextureData()
        {
            foreach (CollisionMapTextureFraction tx in CroppedTextures)
                tx.LoadData();
        }
    }
}
