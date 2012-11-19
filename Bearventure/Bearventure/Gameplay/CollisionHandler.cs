using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Bearventure.Gameplay.Characters;

namespace Bearventure
{
    public class CollisionHandler
    {
        #region Members
        public static Texture2D[] mapText;
        private static Color[] mapData1D;
        private static List<Color[,]> mapData2D = new List<Color[,]>();
        private static int zone_width;
        private static int zone_height;
        #endregion
        #region Initialization
        /// <summary>
        /// Initialize CollisionHandler by passing it the collision map. An individual image fraction is then refered to as a zone.
        /// </summary>
        /// <param name="mapCollisionTextures"></param>
        public static void Initialize(Texture2D[] mapCollisionTextures)
        {
            mapText = mapCollisionTextures;

            zone_height = 0;
            zone_width = 0;

            for (int i = 0; i < mapCollisionTextures.Length; i++)
            {
                mapData1D = new Color[mapText[i].Width * mapText[i].Height];
                mapText[i].GetData(mapData1D);
                mapData2D.Add(TextureData2D(mapText[i], mapData1D));

                zone_height += mapText[i].Height;
                zone_width += mapText[i].Width;
            }

            zone_height /= mapText.Length;
            zone_width /= mapText.Length;
            
        }
        #endregion
        /// <summary>
        /// Calculates the zones in which the passed rectangle is currently on.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns>The list of zones.</returns>
        private static List<int> OnZones(Rectangle rectangle)
        {
            List<int> on_zones = new List<int>();
            List<int> zones_x = new List<int>();
            List<int> zones_y = new List<int>();

            int Left = rectangle.X;
            int Right = rectangle.Right;
            int Top = rectangle.Top;
            int Bottom = rectangle.Bottom;

            if (Bottom >= zone_height)
            {
                if(Top < zone_height)
                    zones_y.Add(0);

                zones_y.Add(1);
            }
            else
            {
                zones_y.Add(0);
            }

            for (int i = 0; i < mapText.Length / 2; i++)
            {
                if(Right > zone_width * i)
                {
                    if(Right <= zone_width * (i + 1))
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
                    on_zones.Add(x * 2 + y);
                }
            }


            return on_zones;
        }
        /// <summary>
        /// Adjusts a rectangles bounds so that they do not exceed the bounds of an individual zone.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        private static Rectangle Adjust(Rectangle rectangle)
        {
            int X = rectangle.X;
            int Right = rectangle.Right;
            int Y = rectangle.Y;
            int Bottom = rectangle.Bottom;
            int Height = rectangle.Height;
            int Width = rectangle.Width;

            if (Bottom > zone_height)
            {
                Y -= zone_height;
                if (Y < 0)
                    Y += Height;
            }

            for (int i = 0; i < mapText.Length / 2; i++)
            {
                if (Right > zone_width * (i + 1))
                    X -= zone_width;
                if (Right < 0)
                    X += zone_width;

                
            }

            if (X < 0 || Right < 0)
                X += Width;

            return new Rectangle(X, Y, Width, Height);
        }

        /// <summary>
        /// Returns Left, Right, Top, Bottom of the subjects BoundingBox if a collision happens. The method creates 2 rectangles 
        /// (CollisionRectangleY, CollisionRectangleX) which are positioned in relation to the characters BoundingBox and velocity.
        /// The rectangles are then scanned for colors other than Transparent. The color then is passed to TerrainType.
        /// </summary>
        /// <param name="subject">The character we are checking for collisions</param>
        /// <param name="movement">The velocity of the character.</param>
        /// <returns></returns>
        public static int CollisionOccursWithMap(Character subject, Vector2 movement)
        {
            int collision_y = 0;
            int collision_x = 0; 

            if (movement.Y != 0)
            {
                List<int> zones = OnZones(CollisionAreaRectangleY(subject, movement.Y));

                int Top = Adjust(CollisionAreaRectangleY(subject, movement.Y)).Y;
                int Bottom = Adjust(CollisionAreaRectangleY(subject, movement.Y)).Bottom;
                int Left = CollisionAreaRectangleY(subject, movement.Y).X;
                int Right = CollisionAreaRectangleY(subject, movement.Y).Right;

                if (zones.Count == 1)
                {
                    Left = Adjust(CollisionAreaRectangleY(subject, movement.Y)).X;
                    Right = Adjust(CollisionAreaRectangleY(subject, movement.Y)).Right;
                }

                int zone = zones[0];

                for (int x = Left; x < Right; x++)
                {

                    while (x >= zone_width)
                    {
                        x -= zone_width;
                        Right -= zone_width;

                        zone = zones[1];
                    }

                    for (int y = Top; y < Bottom; y++)
                    {
                        if (mapData2D[zone][x, y] != Color.Transparent)
                        {
                            TerrainType = mapData2D[zone][x, y];

                            if (CollisionAreaRectangleY(subject, movement.Y).Y >= subject.BoundingBox.Bottom)
                            {
                                collision_y = subject.BoundingBox.Bottom;
                            }

                            else
                            {
                                collision_y = subject.BoundingBox.Top;
                            }
                        }
                    }

                    if (collision_y > 0)
                        break;
                }
            }
           

            if (movement.X != 0)
            {
                List<int> zones = OnZones(CollisionAreaRectangleX(subject, movement.X));

                int obstacle_height = 0;

                int Top = CollisionAreaRectangleX(subject, movement.X).Y;
                int Bottom = CollisionAreaRectangleX(subject, movement.X).Bottom;
                int Left = Adjust(CollisionAreaRectangleX(subject, movement.X)).X;
                int Right = Adjust(CollisionAreaRectangleX(subject, movement.X)).Right;

                if (zones.Count == 1)
                {
                    Top = Adjust(CollisionAreaRectangleX(subject, movement.X)).Y;
                    Bottom = Adjust(CollisionAreaRectangleX(subject, movement.X)).Bottom;
                }

                int zone = zones[0];

                for (int x = Left; x < Right; x++)
                {
                    for (int y = Top; y < Bottom; y++)
                    {
                        if (y >= zone_height && zones.Count > 1)
                        {
                            Bottom -= zone_height;
                            y -= zone_height;

                            zone = zones[1];
                        }

                        if (mapData2D[zone][x, y] != Color.Transparent)
                        {
                            if (Left <= subject.BoundingBox.Left)
                            {
                                collision_x = subject.BoundingBox.Left;
                            }

                            else
                            {
                                collision_x = subject.BoundingBox.Right;
                            }

                            obstacle_height++;
                        }
                    }  
                }

                HeightDifference = obstacle_height;
            }          

            return collision_x + collision_y;
        }
        /// <summary>
        /// A rectangle which is positioned in relation to the characters BoundingBox and velocity.Y
        /// </summary>
        /// <param name="subject">The character</param>
        /// <param name="movement">Characters y-scale movement</param>
        /// <returns></returns>
        private static Rectangle CollisionAreaRectangleY(Character subject, float movement)
        {
            
            if (movement < 0)
                return new Rectangle(subject.BoundingBox.X, subject.BoundingBox.Top + (int)movement, subject.BoundingBox.Width, 1);
            else
                return new Rectangle(subject.BoundingBox.X, subject.BoundingBox.Bottom + (int)movement, subject.BoundingBox.Width, 1);
        }
        /// <summary>
        /// A rectangle which is positioned in relation to the characters BoundingBox and velocity.X
        /// </summary>
        /// <param name="subject">The character</param>
        /// <param name="movement">Characters x-scale movement</param>
        /// <returns></returns>
        private static Rectangle CollisionAreaRectangleX(Character subject, float movement)
        {
            if (movement < 0)
                return new Rectangle(subject.BoundingBox.Left + (int)movement, subject.BoundingBox.Y, 1, subject.BoundingBox.Height);
            else
                return new Rectangle(subject.BoundingBox.Right + (int)movement, subject.BoundingBox.Y, 1, subject.BoundingBox.Height);
        }

        private static Color[,] TextureData2D(Texture2D texture, Color[] data)
        {
            Color[,] colors2D = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                    colors2D[x, y] = data[x + y * texture.Width];

            return colors2D;
        }
        /// <summary>
        /// Returns the color of the terrain on which the last calculated subject is on.
        /// </summary>
        public static Color TerrainType
        {
            get;
            private set;
        }
        /// <summary>
        /// Returns the "angle" of the slope the last calculated subject is on.
        /// </summary>
        public static int HeightDifference
        {
            get;
            private set;
        }
    }
}
