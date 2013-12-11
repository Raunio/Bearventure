using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Bearventure.Gameplay.Characters;
using Microsoft.Xna.Framework.Content;
using Bearventure.Engine;
using Bearventure.Gameplay.GameObjects;
using Bearventure.Gameplay;

namespace Bearventure.Engine.CollisionDetection
{
    public class CollisionHandler
    {
        private static int resizeFactor;

        #region Members
        
        private static int zone_width;
        private static int zone_height;
        private static int map_width;
        private static int map_height;

        private static List<GameplayObject> gameObjects = new List<GameplayObject>();

        private static ContentManager content;

        #endregion
        #region Initialization
        /// <summary>
        /// Initialize CollisionHandler by passing it the collision map. An individual image fraction is then refered to as a zone.
        /// </summary>
        public static void Initialize(CollisionMap _map, List<Enemy> enem, Player player, List<Platform> plat, List<Ladder> ladders, ContentManager _content)
        {
            for (int i = 0; i < enem.Count; i++)
                gameObjects.Add(enem[i]);
            for (int i = 0; i < plat.Count; i++)
                gameObjects.Add(plat[i]);
            for (int i = 0; i < ladders.Count; i++)
                gameObjects.Add(ladders[i]);

            gameObjects.Add(player);

            content = _content;

            Map = _map;

            resizeFactor = Map.ResizeFactor;

            Map.LoadAllTextures(_content);
            Map.LoadAllTextureData();

            zone_height = Map.CropSize.Y;
            zone_width = Map.CropSize.X;

            map_width = Map.CropSize.X * Map.Fractions / 2;
            map_height = Map.CropSize.Y * 2;
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

            for (int i = 0; i < Map.Fractions / 2; i++)
            {
                if(Right >= zone_width * i)
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
                    if(!on_zones.Contains(x * 2 + y))
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

            while (X >= zone_width)
                X -= zone_width;

            if (X < 0 || Right < 0)
                X += Width;

            return new Rectangle(X, Y, Width, Height);
        }
        /// <summary>
        /// Returns Left, Right, Top, Bottom of the subjects BoundingBox if a collision happens. The method creates 2 rectangles 
        /// (CollisionRectangleY, CollisionRectangleX) which are positioned in relation to the characters BoundingBox and velocity.
        /// The rectangles are then scanned for colors other than Transparent.
        /// </summary>
        /// <param name="subject">The character being checked for collisions</param>
        /// <param name="movement">The velocity of the character.</param>
        /// <returns></returns>
        public static int CollisionOccursWithMap(Character subject, Vector2 movement)
        {
            if (CollisionAreaRectangleY(subject, movement.Y).Bottom >= map_height || CollisionAreaRectangleY(subject, movement.Y).Top <= 0 || CollisionAreaRectangleX(subject,movement.X).Left <= 0 || CollisionAreaRectangleX(subject,movement.X).Right >= map_width)
            {
                return -1;
            }

            int collision_y = 0;
            int collision_x = 0; 

            if (movement.Y != 0)
            {

                List<int> zones = OnZones(CollisionAreaRectangleY(subject, movement.Y));

                int Top = Adjust(CollisionAreaRectangleY(subject, movement.Y)).Y;
                int Bottom = Adjust(CollisionAreaRectangleY(subject, movement.Y)).Bottom;
                int Left = Adjust(CollisionAreaRectangleY(subject, movement.Y)).X;
                int Right = Adjust(CollisionAreaRectangleY(subject, movement.Y)).Right;

                int zone = zones[0];

                for (int x = Left; x < Right; x++)
                {
                    // Check if the true position of the rectangle exceeds the x-axis boundaries of a zone
                    // Init the next zone and reset variables in case it does.
                    if (x >= zone_width)
                    {
                        x -= zone_width;
                        Right -= zone_width;
                        zone = zones[1];
                    }

                    for (int y = Top; y < Bottom; y++)
                    {
                        // Check if the pixel isn't transparent and has an aplha value of 255, a collision happens. 
                        // The alpha value must be 255 so that smoothened edges don't count.
                        if ((Map.CroppedTextures[zone].Data[x, y] != Color.Transparent && Map.CroppedTextures[zone].Data[x, y] != Color.White) && Map.CroppedTextures[zone].Data[x, y].A == 255)
                        {
                            // Assing the color of the pixel to TerrainType which can be used to detect terrain upon collision.
                            TerrainType = Map.CroppedTextures[zone].Data[x, y];

                            // If the collision-rectangle is located beneath the characters BoundingBox divided by the resize multiplier of the map texture fractions,
                            // the collision occurs between the bottom of the characters BoundingBox and terrain.
                            if (CollisionAreaRectangleY(subject, movement.Y).Y >= subject.BoundingBox.Bottom / resizeFactor)
                            {
                                collision_y = subject.BoundingBox.Bottom;
                                CalculateDistanceToTerrain(zone, x, y, Bottom);    
                            }
                            // If the conditions above do not meet, assume that the top of the BoundingBox collides with terrain.
                            else
                            {
                                collision_y = subject.BoundingBox.Top;
                            }
                        }
                    }
                    // If a collision occurs, no need to check further.
                    if (collision_y > 0)
                        break;
                }
            }
           
            // This is practicly the same as above, but with x-axis.
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
                else
                {
                    if (Top >= zone_height)
                    {
                        Top -= zone_height;
                        Bottom -= zone_height;
                    }
                }

                int zone = zones[0];

                for (int x = Left; x < Right; x++)
                {
                    for (int y = Top; y < Bottom; y++)
                    {
                        if (y >= zone_height)
                        {
                            Bottom -= zone_height;
                            y -= zone_height;
                            zone = zones[1];
                        }

                        if ((Map.CroppedTextures[zone].Data[x, y] != Color.Transparent && Map.CroppedTextures[zone].Data[x, y] != Color.White) && Map.CroppedTextures[zone].Data[x, y].A == 255)
                        {
                            if (Left <= subject.BoundingBox.Left / resizeFactor)
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

        private static void CalculateDistanceToTerrain(int zone, int x, int y, int bottom)
        {
            for (int i = x; i > 0; i--)
            {
                for (int j = y; j > 0; j--)
                {
                    if (Map.CroppedTextures[zone].Data[i, j] == Color.Transparent || Map.CroppedTextures[zone].Data[i, j].A != 255)
                    {
                        DistanceToTerrain = (int)(j - bottom);
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// Returns Left, Right, Top, Bottom of the subjects BoundingBox if a collision happens. The method creates 2 rectangles 
        /// (CollisionRectangleY, CollisionRectangleX) which are positioned in relation to the characters BoundingBox and velocity.
        /// The method then chekcs if the rectangles intercect any existing characters BoundingBox.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="movement"></param>
        /// <returns></returns>
        public static ObjectCollisionEvent CollisionOccursWithObject(GameplayObject subject, Vector2 movement)
        {
            ObjectCollisionEvent collisionEvent = null;

            int collision = 0;

            Rectangle Y = CollisionAreaRectangleY(subject, subject.velocity.Y);
            Rectangle X = CollisionAreaRectangleX(subject, subject.velocity.X);

            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (subject == gameObjects[i] || !gameObjects[i].IsActive)
                    continue;

                Rectangle targetBox = new Rectangle(gameObjects[i].BoundingBox.X / resizeFactor, gameObjects[i].BoundingBox.Y / resizeFactor,
                    gameObjects[i].BoundingBox.Width / resizeFactor, gameObjects[i].BoundingBox.Height / resizeFactor);

                Rectangle targetBoxScaled = gameObjects[i].BoundingBox;

                if (Y.Intersects(targetBox))
                {
                    if (Y.Y < targetBox.Y + targetBox.Height / 2)
                    {
                        collision = subject.BoundingBox.Bottom;
                    }
                    else
                    {
                        collision = subject.BoundingBox.Top;
                    }
                }
                if (X.Intersects(targetBox))
                {
                    if (X.X < targetBox.X + targetBox.Width / 2)
                    {
                        collision += subject.BoundingBox.Right;
                    }
                    else
                    {
                        collision += subject.BoundingBox.Left;
                    }
                }

                if(subject.BoundingBox.Intersects(targetBoxScaled) && collision == 0)
                {
                    if (subject.BoundingBox.X < targetBox.X + targetBox.Width / 2)
                        collision = subject.BoundingBox.Right;
                    else
                        collision = subject.BoundingBox.Left;

                    if (subject.BoundingBox.Y < targetBox.Y + targetBox.Height / 2)
                        collision = subject.BoundingBox.Bottom;
                    else
                        collision = subject.BoundingBox.Top;
                }


                if (collision != 0)
                {
                    collisionEvent = new ObjectCollisionEvent(subject, gameObjects[i], collision);
                    return collisionEvent;
                }                
        }

        return null;
        }
        /// <summary>
        /// Returns a point which indicates the amount the subject overlaps another character.
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public static int OverlapsCharacter(GameplayObject subject)
        {
            int overlap = 0;

            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i] != subject)
                {
                    if (gameObjects[i].BoundingBox.Intersects(subject.BoundingBox) && gameObjects[i].IsActive && gameObjects[i].TAG != "Ladder")
                    {
                        Rectangle a = subject.BoundingBox;
                        Rectangle b = gameObjects[i].BoundingBox;

                        if (a.Right > b.Left)
                        {
                            if (a.Right < b.Right)
                            {
                                return -a.Right + b.Left;
                            }
                            else
                            {
                                return b.Right - a.Left;
                            }
                        }
                    }
                }
            }

            return overlap;
        }
        /// <summary>
        /// A rectangle which is positioned in relation to the characters BoundingBox and velocity.Y
        /// </summary>
        /// <param name="subject">The character</param>
        /// <param name="movement">Characters y-scale movement</param>
        /// <returns></returns>
        private static Rectangle CollisionAreaRectangleY(GameplayObject subject, float movement)
        {          
            if (movement < 0)
                return new Rectangle(subject.BoundingBox.X / resizeFactor,
                    (int)(subject.BoundingBox.Top / resizeFactor + movement / resizeFactor), 
                    subject.BoundingBox.Width / resizeFactor, 1);
            else
                return new Rectangle(subject.BoundingBox.X / resizeFactor, 
                    (int)(subject.BoundingBox.Bottom / resizeFactor + movement / resizeFactor),
                    subject.BoundingBox.Width / resizeFactor, 1);
        }
        /// <summary>
        /// A rectangle which is positioned in relation to the characters BoundingBox and velocity.X
        /// </summary>
        /// <param name="subject">The character</param>
        /// <param name="movement">Characters x-scale movement</param>
        /// <returns></returns>
        private static Rectangle CollisionAreaRectangleX(GameplayObject subject, float movement)
        {
            if (movement < 0)
                return new Rectangle((int)(subject.BoundingBox.Left / resizeFactor + movement / resizeFactor),
                    subject.BoundingBox.Y / resizeFactor, 1, 
                    subject.BoundingBox.Height / resizeFactor);
            else
                return new Rectangle((int)(subject.BoundingBox.Right / resizeFactor + movement / resizeFactor),
                    subject.BoundingBox.Y / resizeFactor, 1, 
                    subject.BoundingBox.Height / resizeFactor);
        }

        public static void DrawCollisionRectangles(SpriteBatch spriteBatch, GameplayObject subject, Vector2 movement)
        {
            Rectangle xRect = CollisionAreaRectangleX(subject, movement.X);
            Rectangle yRect = CollisionAreaRectangleY(subject, movement.Y);

            xRect.X *= resizeFactor;
            xRect.Y *= resizeFactor;
            yRect.X *= resizeFactor;
            yRect.Y *= resizeFactor;
            xRect.Height *= resizeFactor;
            yRect.Width *= resizeFactor;

            spriteBatch.Draw(content.Load<Texture2D>("Sprites/tosi"), xRect, Color.White);
            spriteBatch.Draw(content.Load<Texture2D>("Sprites/tosi"), yRect, Color.White);
        }

        public static void ClearData()
        {
            gameObjects.Clear();
            Map = null;
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
        /// Returns the velocity of the object that the last calculated subject is on.
        /// </summary>
        public static Vector2 ObjectVelocity
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
        /// <summary>
        /// Gets the push velocity of the last calculated subject.
        /// </summary>
        public static float PushVelocity
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the collision paskdas ghojgfok
        /// </summary>
        public static CollisionMap Map
        {
            get;
            private set;
        }

        public static int DistanceToTerrain
        {
            get;
            private set;
        }
    }
}
