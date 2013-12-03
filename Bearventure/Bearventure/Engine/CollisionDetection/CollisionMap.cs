using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        /// <summary>
        /// Gets an array of CroppedTextureFraction objects
        /// </summary>
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

        public Point MapSize
        {
            get;
            private set;
        }
        private Texture2D edgeTexture;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="asset">Texture asset name</param>
        /// <param name="crops">Amount of fractions/crops</param>
        /// <param name="resizeFactor">The resize multiplier of the textures</param>
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

            edgeTexture = new Texture2D(ResolutionManager.graphicsDevice.GraphicsDevice, 1, 1);
            edgeTexture.SetData(new[] { Color.Yellow });
        }
        /// <summary>
        /// Loads all texture color data to memory.
        /// </summary>
        public void LoadAllTextureData()
        {
            foreach (CollisionMapTextureFraction tx in CroppedTextures)
                tx.LoadData();
        }
        /// <summary>
        /// Draws the whole collision map. Mainly for testing purposes.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawMap(SpriteBatch spriteBatch)
        {
            for(int x = 0; x < CroppedTextures.Length / 2; x++)
            {
                for(int y = 0; y < 2; y++)
                {
                    spriteBatch.Draw(CroppedTextures[x * 2 + y].Texture, 
                        new Vector2(x * CroppedTextures[x * 2 + y].Texture.Width * ResizeFactor, y * CroppedTextures[x * 2 + y].Texture.Height * ResizeFactor), 
                            null, 
                            Color.White, 
                            0f, 
                            Vector2.Zero, 
                            ResizeFactor, 
                            SpriteEffects.None, 
                            0f);
                }
            }
        }

        public void DrawGrid(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < Fractions / 2; x++)
                for (int y = 0; y < 2; y++)
                {
                    spriteBatch.Draw(edgeTexture, new Rectangle(CroppedTextures[x * 2 + y].Texture.Width * x * ResizeFactor, 
                        CroppedTextures[x * 2 + y].Texture.Height * y * ResizeFactor, 
                        CroppedTextures[x * 2 + y].Texture.Width * ResizeFactor, 
                        1), 
                        Color.White);

                    spriteBatch.Draw(edgeTexture, new Rectangle(CroppedTextures[x * 2 + y].Texture.Width * x * ResizeFactor, 
                        CroppedTextures[x * 2 + y].Texture.Height * y * ResizeFactor, 
                        1, 
                        CroppedTextures[x * 2 + y].Texture.Height * ResizeFactor), 
                        Color.White);
                }
        }
    }
}
