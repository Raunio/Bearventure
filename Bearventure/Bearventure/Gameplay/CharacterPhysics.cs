using Microsoft.Xna.Framework;
using Bearventure.Gameplay.Characters;
using Bearventure.Engine.CollisionDetection;
using Bearventure.Gameplay;

namespace Bearventure
{
    /// <summary>
    /// Used for applying basic physics to characters.
    /// </summary>
    public static class CharacterPhysics
    {
        private static float flipTimer;
        private static bool up = false;
        /// <summary>
        /// Gets or sets the gravity used for every character.
        /// </summary>
        public static float Gravity
        {
            get;
            set;
        }
        /// <summary>
        /// Apply basic physics to character.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="gameTime"></param>
        public static void Apply(Character subject, GameTime gameTime)
        {
            if (subject.Orientation == Constants.CharacterOrientation.Ground)
                ApplyGravity(subject);
            else
            {
                if (subject.state != Constants.CharacterState.Dead)
                    UpdateAltitude(subject, gameTime);
                else
                    ApplyGravity(subject);
            }
            
            switch (subject.state)
            {
                case Constants.CharacterState.Stopped:
                    Stop(subject);
                    break;
                case Constants.CharacterState.Walking:
                    Walk(subject);
                    break;
                case Constants.CharacterState.Running:
                    Run(subject);
                    break;
                case Constants.CharacterState.Jumping:
                    Jump(subject);
                    break;
                case Constants.CharacterState.Attacking:
                    Stop(subject);
                    break;
                case Constants.CharacterState.Falling:
                    Fall(subject);
                    break;
                case Constants.CharacterState.Knocked:
                    Knock(subject);
                    break;
                case Constants.CharacterState.DoubleJump:
                    DoubleJump(subject);
                    break;

            }

            HandleTerrainCollisions(subject);
            HandleObjectCollisions(subject);

            subject.position += subject.velocity;
        }
        /// <summary>
        /// Applies gravity to the subject.
        /// </summary>
        /// <param name="subject"></param>
        public static void ApplyGravity(Character subject)
        {
            if(!OnLadder(subject) || subject.TAG == "Enemy")
            subject.velocity.Y += Gravity;
        }
        /// <summary>
        /// Gets the color of the terrain the subject is on.
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public static Color OnTerrain(Character subject)
        {
            int collision = CollisionHandler.CollisionOccursWithMap(subject, subject.velocity);

            return CollisionHandler.TerrainType;

        }

        private static void Stop(Character subject)
        {
            if (subject.velocity.X > subject.decceleration)
                subject.velocity.X -= subject.decceleration;

            else if (subject.velocity.X < -subject.decceleration)
                subject.velocity.X += subject.decceleration;

            else
                subject.velocity.X = 0;
        }
        private static void Walk(Character subject)
        {
            if (!OnGround(subject) && subject.Orientation == Constants.CharacterOrientation.Ground && !OnLadder(subject))
            {
                subject.state = Constants.CharacterState.Falling;
                return;
            }

            if (subject.directionX == Constants.DirectionX.Right)
            {
                if (subject.velocity.X < subject.walkSpeed)
                    subject.velocity.X += subject.acceleration;

                else if (subject.velocity.X > subject.walkSpeed)
                    subject.velocity.X = subject.walkSpeed;

            }
            else if (subject.directionX == Constants.DirectionX.Left)
            {
                if (subject.velocity.X > -subject.walkSpeed)
                    subject.velocity.X -= subject.acceleration;

                else if (subject.velocity.X < -subject.walkSpeed)
                    subject.velocity.X = -subject.walkSpeed;
            }
        }
        private static void Run(Character subject)
        {
            if (!OnGround(subject) && subject.Orientation == Constants.CharacterOrientation.Ground && !OnLadder(subject))
            {
                subject.state = Constants.CharacterState.Falling;
                return;
            }

            if (subject.directionX == Constants.DirectionX.Right)
            {
                if (subject.velocity.X < subject.runSpeed)
                    subject.velocity.X += subject.acceleration;

                else if (subject.velocity.X > subject.walkSpeed)
                    subject.velocity.X = subject.runSpeed;

            }
            else if (subject.directionX == Constants.DirectionX.Left)
            {
                if (subject.velocity.X > -subject.runSpeed)
                    subject.velocity.X -= subject.acceleration;

                else if (subject.velocity.X < -subject.walkSpeed)
                    subject.velocity.X = -subject.runSpeed;
            }
        }
        private static void Jump(Character subject)
        {
            subject.jumpTime = 0;

            if (OnGround(subject) && !OnLadder(subject))
            {
                subject.velocity.Y -= subject.jumpStrenght;
                subject.position.Y -= subject.jumpStrenght;
            }
            if(subject.velocity.Y > Gravity)
            {
                subject.state = Constants.CharacterState.Falling;
                return;
            }

            if (subject.directionX == Constants.DirectionX.Right)
            {
                if (subject.velocity.X < subject.walkSpeed / 2)
                    subject.velocity.X += subject.acceleration;

            }
            else if (subject.directionX == Constants.DirectionX.Left)
            {
                if (subject.velocity.X > -subject.walkSpeed / 2)
                    subject.velocity.X -= subject.acceleration;
            }
        }
        private static void DoubleJump(Character subject)
        {
            if (subject.state == Constants.CharacterState.DoubleJump && !OnLadder(subject))
            {
                if (subject.jumpTime == 0)
                {
                    subject.velocity.Y -= subject.jumpStrenght;
                    subject.position.Y -= subject.jumpStrenght;
                    subject.jumpTime++;
                }
            }
            if (subject.velocity.Y > Gravity)
            {
                subject.state = Constants.CharacterState.Falling;
                return;
            }

            if (subject.directionX == Constants.DirectionX.Right)
            {
                if (subject.velocity.X < subject.walkSpeed / 2)
                    subject.velocity.X += subject.acceleration;

            }
            else if (subject.directionX == Constants.DirectionX.Left)
            {
                if (subject.velocity.X > -subject.walkSpeed / 2)
                    subject.velocity.X -= subject.acceleration;
            }
        }
        private static void Fall(Character subject)
        {
            if (OnGround(subject))
            {
                subject.state = Constants.CharacterState.Stopped;
                return;
            }

            if (subject.directionX == Constants.DirectionX.Right)
            {
                if (subject.velocity.X < subject.walkSpeed / 2)
                    subject.velocity.X += subject.acceleration;

            }
            else if (subject.directionX == Constants.DirectionX.Left)
            {
                if (subject.velocity.X > -subject.walkSpeed / 2)
                    subject.velocity.X -= subject.acceleration;
            }
        }
        private static void Knock(Character subject)
        {
            float decceleration = subject.Mass / 100; // Magic number

            if (subject.velocity.X == 0 && OnGround(subject))
                subject.state = Constants.CharacterState.Stopped;
            else
            {
                if (subject.velocity.X > decceleration)
                    subject.velocity.X -= decceleration;
                else if (subject.velocity.X < -decceleration)
                    subject.velocity.X += decceleration;
                else
                    subject.velocity.X = 0;
            }

        }

        private static void HandleTerrainCollisions(Character subject)
        {
            int collision = CollisionHandler.CollisionOccursWithMap(subject, subject.velocity);

            int Top = subject.BoundingBox.Top;
            int Bottom = subject.BoundingBox.Bottom;
            int Left = subject.BoundingBox.Left;
            int Right = subject.BoundingBox.Right;

            if ((collision == Bottom || collision == Top) && CollisionHandler.TerrainType == Constants.Solid)
            {
                subject.velocity.Y = 0;
            }
            else if (collision == Right && CollisionHandler.TerrainType == Constants.Solid)
            {
                subject.velocity.X = 0;
            }
            else if (collision == Left && CollisionHandler.TerrainType == Constants.Solid)
            {
                subject.velocity.X = 0;
            }

            else if ((collision == Bottom + Left || collision == Bottom + Right) && CollisionHandler.TerrainType == Constants.Solid)
            {
                subject.velocity.Y = 0;

                if (CollisionHandler.HeightDifference < subject.BoundingBox.Height / 10)
                    subject.position.Y -= CollisionHandler.HeightDifference;

                subject.velocity.X = 0;
            }

            else if ((collision == Top + Left || collision == Top + Right) && CollisionHandler.TerrainType == Constants.Solid)
            {
                subject.velocity.X = 0;
                subject.velocity.Y = 0;
            }

            //else if ((collision == Top || collision == Bottom || collision == Left || collision == Right) && CollisionHandler.TerrainType == Constants.Ladder)
            //{
            //    subject.velocity.X = 0;
            //    subject.velocity.Y = 0; 
            //}
        }
        public static void HandleObjectCollisions(GameplayObject subject)
        {
            ObjectCollisionEvent collision = CollisionHandler.CollisionOccursWithObject(subject, subject.velocity);

            if (collision == null)
                return;

            int Top = subject.BoundingBox.Top;
            int Bottom = subject.BoundingBox.Bottom;
            int Left = subject.BoundingBox.Left;
            int Right = subject.BoundingBox.Right;

            if (collision.CollisionLocation == Top)
            {
                if(collision.B.TAG != "Ladder" && !collision.IsZeroMassCollision)
                    subject.velocity.Y = 0;
            }
            else if (collision.CollisionLocation == Bottom && !collision.IsZeroMassCollision)
            {
                if (collision.B.TAG != "Ladder")
                {
                    subject.velocity.Y = 0;

                    subject.position.X += collision.A.TAG == "Player" ? collision.B.velocity.X : collision.A.velocity.X;
                }
            }
            else if (collision.CollisionLocation == Left && collision.B.TAG != "Ladder")
            {
                if (collision.EventCausedByPlayer && collision.B.TAG == "Enemy" && collision.B.Mass > 0)
                {
                    subject.velocity.X /= 2;
                    collision.B.position.X += subject.velocity.X;
                }
                else if (collision.A.Mass == -1)
                {
                    collision.B.position.X += subject.velocity.X;
                }
                else if(!collision.IsZeroMassCollision)
                {
                    subject.velocity.X = 0;
                }
            }
            else if (collision.CollisionLocation == Right && collision.B.TAG != "Ladder")
            {
                if (collision.EventCausedByPlayer && collision.B.TAG == "Enemy" && collision.B.Mass > 0)
                {
                    subject.velocity.X /= 2;
                    collision.B.position.X += subject.velocity.X;
                }
                else if (collision.A.Mass == -1 && !Blocked((Character)subject, Constants.DirectionX.Right))
                {
                    collision.B.position.X += subject.velocity.X;
                }
                else if (!collision.IsZeroMassCollision)
                {
                    subject.velocity.X = 0;
                }
            }
            else if (collision.CollisionLocation == Top + Left && collision.B.TAG != "Ladder" && !collision.IsZeroMassCollision)
            {
                subject.velocity.Y = 0;

                if (collision.EventCausedByPlayer && collision.B.TAG == "Enemy" && collision.B.Mass > 0)
                {
                    subject.velocity.X /= 2;
                    collision.B.position.X += subject.velocity.X;
                }
                else if (collision.A.Mass == -1)
                {
                    collision.B.position.X += subject.velocity.X;
                }
                else if (!collision.IsZeroMassCollision)
                {
                    subject.velocity.X = 0;
                }
            }
            else if (collision.CollisionLocation == Bottom + Left && collision.B.TAG != "Ladder" && !collision.IsZeroMassCollision)
            {
                subject.velocity.Y = 0;

                if (collision.EventCausedByPlayer && collision.B.TAG == "Enemy" && collision.B.Mass > 0)
                {
                    subject.velocity.X /= 2;
                    collision.B.position.X += subject.velocity.X;
                }
                else if (collision.A.Mass == -1)
                {
                    collision.B.position.X += subject.velocity.X;
                }
                else if (!collision.IsZeroMassCollision)
                {
                    subject.velocity.X = 0;
                }
            }
            else if (collision.CollisionLocation == Top + Right && collision.B.TAG != "Ladder" && !collision.IsZeroMassCollision)
            {
                subject.velocity.Y = 0;

                if (collision.EventCausedByPlayer && collision.B.TAG == "Enemy" && collision.B.Mass > 0)
                {
                    subject.velocity.X /= 2;
                    collision.B.position.X += subject.velocity.X;
                }
                else if (collision.A.Mass == -1)
                {
                    collision.B.position.X += subject.velocity.X;
                }
                else if (!collision.IsZeroMassCollision)
                {
                    subject.velocity.X = 0;
                }
            }
            else if (collision.CollisionLocation == Bottom + Right && collision.B.TAG != "Ladder" && !collision.IsZeroMassCollision)
            {
                subject.velocity.Y = 0;

                if (collision.EventCausedByPlayer && collision.B.TAG == "Enemy" && collision.B.Mass > 0)
                {
                    subject.velocity.X /= 2;
                    collision.B.position.X += subject.velocity.X;
                }
                else if (collision.A.Mass == -1)
                {
                    collision.B.position.X += subject.velocity.X;
                }
                else if (!collision.IsZeroMassCollision)
                {
                    subject.velocity.X = 0;
                }
            }

        }
        /// <summary>
        /// Checks if the character is currently standing on solid ground.
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public static bool OnGround(Character subject)
        {
            int collision = CollisionHandler.CollisionOccursWithMap(subject, new Vector2(subject.velocity.X, subject.velocity.Y + 2));
             

            if (collision == subject.BoundingBox.Bottom ||
                collision == subject.BoundingBox.Bottom + subject.BoundingBox.Right ||
                collision == subject.BoundingBox.Bottom + subject.BoundingBox.Left)
            {
                return true;
            }

            ObjectCollisionEvent objectCollision = CollisionHandler.CollisionOccursWithObject(subject, new Vector2(subject.velocity.X, subject.velocity.Y + 2));

            if (objectCollision != null && (objectCollision.CollisionLocation == subject.BoundingBox.Bottom ||
                objectCollision.CollisionLocation == subject.BoundingBox.Bottom + subject.BoundingBox.Right ||
                objectCollision.CollisionLocation == subject.BoundingBox.Bottom + subject.BoundingBox.Left))
            {
                return true;
            }

            return false;
        }

        public static bool OnLadder(Character subject)
        {
            ObjectCollisionEvent collision = CollisionHandler.CollisionOccursWithObject(subject, subject.velocity);

            if (collision != null && collision.B.TAG == "Ladder")
            {
                float targetPosition = collision.B.BoundingBox.Left + collision.B.BoundingBox.Width / 2;

                if (subject.position.X > targetPosition + subject.walkSpeed / 2)
                {
                    if (subject.velocity.X > -subject.walkSpeed)
                        subject.velocity.X -= subject.acceleration;

                    else if (subject.velocity.X < -subject.walkSpeed)
                        subject.velocity.X = -subject.walkSpeed;
                }
                else if (subject.position.X < targetPosition - subject.walkSpeed / 2)
                {
                    if (subject.velocity.X < subject.walkSpeed)
                        subject.velocity.X += subject.acceleration;

                    else if (subject.velocity.X > subject.walkSpeed)
                        subject.velocity.X = subject.walkSpeed;
                }
                else
                    subject.velocity.X = 0;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the character is being blocked from either of its sides.
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public static bool Blocked(Character subject)
        {
            int RightCheck = CollisionHandler.CollisionOccursWithMap(subject, new Vector2(2, 0));
            int LeftCheck = CollisionHandler.CollisionOccursWithMap(subject, new Vector2(-2, 0));

            if (RightCheck == subject.BoundingBox.Right)
                return true;
            if (LeftCheck == subject.BoundingBox.Left)
                return true;

            //RightCheck = CollisionHandler.CollisionOccursWithObject(subject, new Vector2(2, 0)) == null ? 0 : CollisionHandler.CollisionOccursWithObject(subject, new Vector2(2, 0)).CollisionLocation;
            //LeftCheck = CollisionHandler.CollisionOccursWithObject(subject, new Vector2(-2, 0)) == null ? 0 : CollisionHandler.CollisionOccursWithObject(subject, new Vector2(-2, 0)).CollisionLocation;

            ObjectCollisionEvent RightEvent = CollisionHandler.CollisionOccursWithObject(subject, new Vector2(2, 0));
            ObjectCollisionEvent LeftEvent = CollisionHandler.CollisionOccursWithObject(subject, new Vector2(-2, 0));

            if (RightEvent == null)
            {
                RightCheck = 0;
            }
            else
            {
                RightCheck = RightEvent.CollisionLocation;
            }

            if (LeftEvent == null)
            {
                LeftCheck = 0;
            }
            else
            {
                LeftCheck = LeftEvent.CollisionLocation;
            }

            if (RightCheck == subject.BoundingBox.Right)
                return true;
            if (LeftCheck == subject.BoundingBox.Left)
                return true;

            return false;
        }
        /// <summary>
        /// Checks if the character is blocked from the given direction
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static bool Blocked(Character subject, Constants.DirectionX direction)
        {
            int RightCheck = CollisionHandler.CollisionOccursWithMap(subject, new Vector2(2, 0));
            int LeftCheck = CollisionHandler.CollisionOccursWithMap(subject, new Vector2(-2, 0));

            if (RightCheck == subject.BoundingBox.Right && direction == Constants.DirectionX.Right)
                return true;
            if (LeftCheck == subject.BoundingBox.Left && direction == Constants.DirectionX.Left)
                return true;

            RightCheck = CollisionHandler.CollisionOccursWithObject(subject, new Vector2(2, 0)) == null ? 0 : CollisionHandler.CollisionOccursWithObject(subject, new Vector2(1, 0)).CollisionLocation;
            LeftCheck = CollisionHandler.CollisionOccursWithObject(subject, new Vector2(-2, 0)) == null ? 0 : CollisionHandler.CollisionOccursWithObject(subject, new Vector2(1, 0)).CollisionLocation;

            if (RightCheck == subject.BoundingBox.Right && direction == Constants.DirectionX.Right)
                return true;
            if (LeftCheck == subject.BoundingBox.Left && direction == Constants.DirectionX.Left)
                return true;

            return false;
        }
        /// <summary>
        /// Can be called to fix overlaps when a character is stuck.
        /// </summary>
        /// <param name="subject"></param>
        public static void FixOverlaps(Character subject)
        {
            subject.position.X += CollisionHandler.OverlapsCharacter(subject);
        }
        private static void UpdateAltitude(Character subject, GameTime gameTime)
        {
            if (subject.directionY == Constants.DirectionY.Down)
            {
                if (subject.velocity.Y < subject.walkSpeed)
                    subject.velocity.Y += subject.acceleration;
                else if (subject.velocity.Y > subject.walkSpeed)
                    subject.velocity.Y = subject.walkSpeed;
            }
            else if (subject.directionY == Constants.DirectionY.Up)
            {
                if (subject.velocity.Y > -subject.walkSpeed)
                    subject.velocity.Y -= subject.acceleration;
                else if (subject.velocity.Y < -subject.walkSpeed)
                    subject.velocity.Y = -subject.walkSpeed;
            }
            else if (subject.directionY == Constants.DirectionY.Hold)
            {
                if (subject.velocity.Y < -subject.decceleration)
                    subject.velocity.Y += subject.decceleration;
                else if (subject.velocity.Y > subject.decceleration)
                    subject.velocity.Y -= subject.decceleration;
                else
                    subject.velocity.Y = 0;
            }

            FlipFlap(subject, gameTime);
        }
        /// <summary>
        /// Flippidy flap / Owl flight
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="gameTime"></param>
        private static void FlipFlap(Character subject, GameTime gameTime)
        {
            int interval = 200;
            float amount = 1.3f;

            flipTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (flipTimer >= interval)
            {
                up = !up;
                flipTimer = 0;
            }

            if (up)
            {
                subject.position.Y -= amount;
            }
            else
                subject.position.Y += amount;
        }
    }
}
