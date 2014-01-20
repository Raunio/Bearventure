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

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            foreach (BackgroundLayer layer in layersInDrawingOrder)
            {
                layer.background.Draw(spriteBatch, layer.position, OnZones(camera.ViewPortRectangle));
            }
        }

        public void DrawGrid(SpriteBatch spriteBatch)
        {
            layersInDrawingOrder[0].background.DrawGrid(spriteBatch);
        }

        public List<int> OnZones(Rectangle rectangle)
        {
            List<int> on_zones = new List<int>();
            List<int> zones_x = new List<int>();
            List<int> zones_y = new List<int>();

            int Left = rectangle.X;
            int Right = rectangle.Right;
            int Top = rectangle.Top;
            int Bottom = rectangle.Bottom;

            int zone_height = layersInDrawingOrder.Last().background.Height / 2;
            int zone_width = layersInDrawingOrder.Last().background.Width / layersInDrawingOrder[0].background.Fractions / 2;

            if (Bottom >= zone_height)
            {
                if (Top < zone_height)
                    zones_y.Add(0);

                zones_y.Add(1);
            }
            else
            {
                zones_y.Add(0);
            }

            for (int i = 0; i < layersInDrawingOrder.Last().background.Fractions / 2; i++)
            {
                if (Right >= zone_width * i)
                {
                    if (Right <= zone_width * (i + 1))
                    {
                        if (Left < zone_width * i)
                        {
                            zones_x.Add(i - 1);
                        }

                        zones_x.Add(i);
                    }
                }

            }

            foreach (int y in zones_y)
            {
                foreach (int x in zones_x)
                {
                    if (!on_zones.Contains(x * 2 + y))
                        on_zones.Add(x * 2 + y);
                }
            }


            return on_zones;
        }

    }
}
