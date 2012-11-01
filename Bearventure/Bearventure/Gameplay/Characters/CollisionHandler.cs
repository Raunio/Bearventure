using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Characters;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Bearventure
{
    public class CollisionHandler
    {
        private static Texture2D mapText;
        private static Color[] mapData1D;
        private static Color[,] mapData2D;

        public static void Initialize(Texture2D mapCollisionTexture)
        {
            mapText = mapCollisionTexture;
            mapData1D = new Color[mapText.Width * mapText.Height];
            mapText.GetData(mapData1D);

            mapData2D = MapData2D;
        }

        public static int CollisionOccursWithMap(Character subject, Vector2 movement)
        {
            int collision_y = 0;
            int collision_x = 0;

            int obstacle_height = 0;

            for (int x = CollisionAreaRectangleY(subject, movement.Y).X; x < CollisionAreaRectangleY(subject, movement.Y).Right; x++)
            {
                for (int y = CollisionAreaRectangleY(subject, movement.Y).Y; y < CollisionAreaRectangleY(subject, movement.Y).Bottom; y++)
                {
                    if (mapData2D[x, y] != Color.Transparent)
                    {
                        TerrainType = mapData2D[x, y];

                        if (CollisionAreaRectangleY(subject, movement.Y).Y >= subject.BoundingBox.Bottom)
                            collision_y = subject.BoundingBox.Bottom;

                        else
                            collision_y = subject.BoundingBox.Top;
                    }
                }

                if (collision_y > 0)
                    break;
            }

            for (int x = CollisionAreaRectangleX(subject, movement.X).X; x < CollisionAreaRectangleX(subject, movement.X).Right; x++)
            {
                for (int y = CollisionAreaRectangleX(subject, movement.X).Y; y < CollisionAreaRectangleX(subject, movement.X).Bottom; y++)
                {
                    if (mapData2D[x, y] != Color.Transparent)
                    {
                        if (CollisionAreaRectangleX(subject, movement.X).X <= subject.BoundingBox.Left)
                            collision_x = subject.BoundingBox.Left;

                        else
                            collision_x = subject.BoundingBox.Right;

                        obstacle_height++;
                    }
                }
            }

            HeightDifference = obstacle_height;

            return collision_x + collision_y;
        }

        private static Rectangle CollisionAreaRectangleY(Character subject, float movement)
        {
            
            if (movement < 0)
                return new Rectangle(subject.BoundingBox.X, subject.BoundingBox.Top + (int)movement, subject.BoundingBox.Width, 1);
            else
                return new Rectangle(subject.BoundingBox.X, subject.BoundingBox.Bottom + (int)movement, subject.BoundingBox.Width, 1);
        }

        private static Rectangle CollisionAreaRectangleX(Character subject, float movement)
        {
            if (movement < 0)
                return new Rectangle(subject.BoundingBox.Left + (int)movement, subject.BoundingBox.Y, 1, subject.BoundingBox.Height);
            else
                return new Rectangle(subject.BoundingBox.Right + (int)movement, subject.BoundingBox.Y, 1, subject.BoundingBox.Height);
        }

        private static Color[,] MapData2D
        {
            get
            {
                Color[,] colors2D = new Color[mapText.Width, mapText.Height];
                for (int x = 0; x < mapText.Width; x++)
                    for (int y = 0; y < mapText.Height; y++)
                        colors2D[x, y] = mapData1D[x + y * mapText.Width];

                return colors2D;
            }
        }

        public static Color TerrainType
        {
            get;
            private set;
        }

        public static int HeightDifference
        {
            get;
            private set;
        }
    }
}
