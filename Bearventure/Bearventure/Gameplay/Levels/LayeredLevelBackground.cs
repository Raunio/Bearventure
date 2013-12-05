using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Bearventure.Gameplay.Levels
{
    class LayeredLevelBackground
    {
        private List<BackgroundLayer> layers;
        private BackgroundLayer[] layersInDrawingOrder;

        private struct BackgroundLayer
        {
            public LevelBackground background;
            public int layerDepth;
            public Vector2 parallax;
            public Vector2 position;
        };

        public int Fractions
        {
            get
            {
                return layersInDrawingOrder[0].background.Fractions;
            }
        }

        public int LevelWidth
        {
            get
            {
                return layersInDrawingOrder[0].background.Width;
            }
        }

        public int LevelHeight
        {
            get
            {
                return layersInDrawingOrder[0].background.Height;
            }
        }

        public LayeredLevelBackground()
        {
            layers = new List<BackgroundLayer>();
        }
        /// <summary>
        /// Adds a new layer to the LayeredLevelBackground.
        /// </summary>
        /// <param name="layer">The background</param>
        /// <param name="layerDepth">Defines the layer depth of the background layer. 0 is the foremost layer depth.</param>
        /// <param name="scrollSpeedModifier">0 for static background or no scrolling, 1 if you want the background to "follow camera"</param>
        public void AddLayer(LevelBackground layer, int layerDepth, Vector2 parallax)
        {
            BackgroundLayer bgLayer = new BackgroundLayer();
            bgLayer.background = layer;
            bgLayer.layerDepth = layerDepth;
            bgLayer.parallax = parallax;

            layers.Add(bgLayer);
        }
        /// <summary>
        /// Initialize needs to be called after adding all the layers.
        /// </summary>
        public void Initialize()
        {
            layersInDrawingOrder = new BackgroundLayer[layers.Count];

            int layerDepthCounter = 0;
            int indexCounter = 0;

            while (layers.Count > 0)
            {
                for (int i = 0; i < layers.Count; i++)
                {
                    if (layers[i].layerDepth == layerDepthCounter)
                    {
                        layersInDrawingOrder[indexCounter] = layers[i];
                        indexCounter++;
                        layers.RemoveAt(i);
                    }
                }

                layerDepthCounter++;
            }

            BackgroundLayer[] tempArray = new BackgroundLayer[layersInDrawingOrder.Length];

            indexCounter = 0;

            for(int i = layersInDrawingOrder.Length - 1; i > -1; i--)
            {
                tempArray[indexCounter] = layersInDrawingOrder[i];
                tempArray[indexCounter].position = Vector2.Zero;
                indexCounter++;
            }

            layersInDrawingOrder = tempArray;
        }

        public void Update(Vector2 cameraPosition)
        {
            for(int i = 0; i < layersInDrawingOrder.Length; i++)
            {
                layersInDrawingOrder[i].position = cameraPosition * layersInDrawingOrder[i].parallax;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (BackgroundLayer layer in layersInDrawingOrder)
            {
                layer.background.Draw(spriteBatch, layer.position);
            }
        }
    }
}
