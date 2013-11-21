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

namespace Bearventure.Engine.CollisionDetection
{
    public class CollisionHandler
    {
        private static int resizeFactor;

        #region Members
        
        private static int zone_width;
        private static int zone_height;

        private static List<Enemy> enemies = new List<Enemy>();
        private static List<Platform> platforms = new List<Platform>();
        private static Player _player;

        private static ContentManager content;

        #endregion
        #region Initialization
        /// <summary>
        /// Initialize CollisionHandler by passing it the collision map. An individual image fraction is then refered to as a zone.
        /// </summary>
        public static void Initialize(CollisionMap _map, List<Enemy> enem, Player player, List<Platform> plat, ContentManager _content)
        {
            _player = player;

            enemies = enem;

            platforms = plat;

            content = _content;

            Map = _map;

            resizeFactor = Map.ResizeFactor;

            Map.LoadAllTextures(_content);
            Map.LoadAllTextureData();

            zone_height = Map.CropSize.Y;
            zone_width = Map.CropSize.X;        
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
                                //CalculateDistanceToTerrain(zone, x, y, subject.BoundingBox.Bottom);    
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
            for (int i = x; i < 0; i--)
            {
                for (int j = y; j < 0; j--)
                {
                    if (Map.CroppedTextures[zone].Data[i, j] == Color.Transparent || Map.CroppedTextures[zone].Data[i, j].A != 255)
                    {
                        DistanceToTerrain = (int)(j * resizeFactor - bottom);
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
        public static ObjectCollisionEvent CollisionOccursWithObject(Character subject, Vector2 movement)
        {
            ObjectCollisionEvent collisionEvent = null;

            if (subject.Orientation != Constants.CharacterOrientation.Air)
            {
                int collision = 0;

                Rectangle Y = CollisionAreaRectangleY(subject, subject.velocity.Y);
                Rectangle X = CollisionAreaRectangleX(subject, subject.velocity.X);

                #region Check if player collides with enemies
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].state != Constants.CharacterState.Dead && enemies[i].Orientation != Constants.CharacterOrientation.Air)
                    {
                        Rectangle enemyBox = new Rectangle(enemies[i].BoundingBox.X / resizeFactor, enemies[i].BoundingBox.Y / resizeFactor, 
                            enemies[i].BoundingBox.Width / resizeFactor, enemies[i].BoundingBox.Height / resizeFactor);

                        if (Y.Intersects(enemyBox) && subject.BoundingBox != enemies[i].BoundingBox)
                        {
                            if (Y.Y < enemyBox.Y + enemyBox.Height / 2)
                            {
                                collision = subject.BoundingBox.Bottom;
                            }
                            else
                            {
                                collision = subject.BoundingBox.Top;
                            }
                        }
                        if (X.Intersects(enemyBox) && subject.BoundingBox != enemies[i].BoundingBox)
                        {
                            if (X.X < enemyBox.X + enemyBox.Width / 2)
                            {
                                collision += subject.BoundingBox.Left;
                            }
                            else
                            {
                                collision += subject.BoundingBox.Right;
                            }
                        }
                    }

                    if (collision != 0)
                    {
                        collisionEvent = new ObjectCollisionEvent(subject, enemies[i], collision);
                    }

                }
                #endregion

                #region Check if player collides with platforms
                for (int i = 0; i < platforms.Count; i++)
                {
                    Rectangle platformBox = new Rectangle(platforms[i].BoundingBox.X / resizeFactor, platforms[i].BoundingBox.Y / resizeFactor,
                        platforms[i].BoundingBox.Width / resizeFactor, platforms[i].BoundingBox.Height / resizeFactor);

                    if (Y.Intersects(platformBox) && subject.BoundingBox != platforms[i].BoundingBox)
                    {
                        if (Y.Y < platformBox.Y + platformBox.Height / 2)
                        {
                            collision = subject.BoundingBox.Bottom;
                        }
                        else
                        {
                            collision = subject.BoundingBox.Top;
                            ObjectVelocity = platforms[i].velocity;
                        }
                    }
                    if (X.Intersects(platformBox) && subject.BoundingBox != platforms[i].BoundingBox)
                    {
                        if (X.X < platformBox.X + platformBox.Width / 2)
                        {
                            collision += subject.BoundingBox.Left;
                        }
                        else
                        {
                            collision += subject.BoundingBox.Right;
                        }
                    }

                    if (collision != 0)
                    {
                        collisionEvent = new ObjectCollisionEvent(subject, platforms[i], collision);
                    }
                }
                #endregion

                Rectangle playerBox = new Rectangle(_player.BoundingBox.X / resizeFactor, _player.BoundingBox.Y / resizeFactor,
                            _player.BoundingBox.Width / resizeFactor, _player.BoundingBox.Height / resizeFactor);


                #region Check if enemy collides with player
                if (Y.Intersects(playerBox) && _player.BoundingBox != subject.BoundingBox)
                {
                    if (Y.Y < playerBox.Y + playerBox.Height / 2)
                    {
                        collision = subject.BoundingBox.Bottom;
                    }
                    else
                    {
                        collision = subject.BoundingBox.Top;
                        ObjectVelocity = _player.velocity;
                    }
                }
                if (X.Intersects(playerBox) && _player.BoundingBox != subject.BoundingBox)
                {
                    if (X.X < playerBox.X + playerBox.Width / 2)
                    {
                        collision += subject.BoundingBox.Left;
                    }
                    else
                    {
                        collision += subject.BoundingBox.Right;
                    }
                }

                if (collision != 0)
                {
                    collisionEvent = new ObjectCollisionEvent(subject, _player, collision);
                }

            #endregion
            }

            return collisionEvent;
        }
        /// <summary>
        /// Returns a point which indicates the amount the subject overlaps another character.
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public static int OverlapsCharacter(Character subject)
        {
            int overlap = 0;

            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] != subject)
                {
                    if (enemies[i].BoundingBox.Intersects(subject.BoundingBox) && enemies[i].state != Constants.CharacterState.Dead)
                    {
                        Rectangle a = subject.BoundingBox;
                        Rectangle b = enemies[i].BoundingBox;

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

                    //if(subject != player && 
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
        private static Rectangle CollisionAreaRectangleY(Character subject, float movement)
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
        private static Rectangle CollisionAreaRectangleX(Character subject, float movement)
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
