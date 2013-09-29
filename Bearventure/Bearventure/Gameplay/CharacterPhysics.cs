using Microsoft.Xna.Framework;
using Bearventure.Gameplay.Characters;
using Bearventure.Engine.CollisionDetection;

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
            }

            HandleTerrainCollisions(subject);
            HandleCharacterCollisions(subject);

            FixOverlaps(subject);

            subject.position += subject.velocity;
        }
        /// <summary>
        /// Applies gravity to the subject.
        /// </summary>
        /// <param name="subject"></param>
        public static void ApplyGravity(Character subject)
        {
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
            if (!OnGround(subject) && subject.Orientation == Constants.CharacterOrientation.Ground)
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
            if (!OnGround(subject) && subject.Orientation == Constants.CharacterOrientation.Ground)
            {
                subject.state = Constants.CharacterState.Falling;
                return;
            }

            if (subject.directionX == Constants.DirectionX.Right)
            {
                if (subject.velocity.X < subject.runSpeed)
                    subject.velocity.X += subject.acceleration;

                else if (subject.velocity.X > subject.runSpeed)
                    subject.velocity.X = subject.runSpeed;
            }
            else if (subject.directionX == Constants.DirectionX.Left)
            {
                if (subject.velocity.X > -subject.runSpeed)
                    subject.velocity.X -= subject.acceleration;

                else if (subject.velocity.X < -subject.runSpeed)
                    subject.velocity.X = -subject.runSpeed;
            }
        }
        private static void Jump(Character subject)
        {
            if (OnGround(subject))
            {
                subject.velocity.Y -= subject.jumpStrenght;
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
            float decceleration = subject.mass / 75; // Magic number

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

                if (CollisionHandler.HeightDifference < 10)
                    subject.position.Y -= CollisionHandler.HeightDifference;

                subject.velocity.X = 0;
            }

            else if ((collision == Top + Left || collision == Top + Right) && CollisionHandler.TerrainType == Constants.Solid)
            {
                subject.velocity.X = 0;
                subject.velocity.Y = 0;
            }
        }
        private static void HandleCharacterCollisions(Character subject)
        {
            int collision = CollisionHandler.CollisionOccursWithObject(subject, subject.velocity);

            int Top = subject.BoundingBox.Top;
            int Bottom = subject.BoundingBox.Bottom;
            int Left = subject.BoundingBox.Left;
            int Right = subject.BoundingBox.Right;


            if (collision == Top)
            {
                subject.velocity.Y = 0;
            }
            else if (collision == Bottom)
            {
                subject.velocity.Y = 0;
                //Vector2 ownVelocity = subject.velocity - CollisionHandler.ObjectVelocity;
                //subject.velocity = subject.velocity + CollisionHandler.ObjectVelocity;
            }
            else if (collision == Left)
            {
                //subject.velocity.X = -CollisionHandler.PushVelocity;

                subject.velocity.X = 0;
            }
            else if (collision == Right)
            {
                //subject.velocity.X = CollisionHandler.PushVelocity;

                subject.velocity.X = 0;
            }
            else if (collision == Top + Left || collision == Top + Right)
            {
                subject.velocity.Y = 0;
                subject.velocity.X = 0;
            }
            else if (collision == Bottom + Left || collision == Bottom + Right)
            {
                subject.velocity.Y = 0;
                subject.velocity.X = 0;
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

            collision = CollisionHandler.CollisionOccursWithObject(subject, new Vector2(subject.velocity.X, subject.velocity.Y + 2));

            if (collision == subject.BoundingBox.Bottom ||
                collision == subject.BoundingBox.Bottom + subject.BoundingBox.Right ||
                collision == subject.BoundingBox.Bottom + subject.BoundingBox.Left)
            {
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
            int RightCheck = CollisionHandler.CollisionOccursWithMap(subject, new Vector2(1, 0));
            int LeftCheck = CollisionHandler.CollisionOccursWithMap(subject, new Vector2(-1, 0));

            if (RightCheck == subject.BoundingBox.Right)
                return true;
            if (LeftCheck == subject.BoundingBox.Left)
                return true;

            RightCheck = CollisionHandler.CollisionOccursWithObject(subject, new Vector2(1, 0));
            LeftCheck = CollisionHandler.CollisionOccursWithObject(subject, new Vector2(-1, 0));

            if (RightCheck == subject.BoundingBox.Right)
                return true;
            if (LeftCheck == subject.BoundingBox.Left)
                return true;

            return false;
        }
        /// <summary>
        /// Can be called to fix overlaps when a character is stuck.
        /// </summary>
        /// <param name="subject"></param>
        private static void FixOverlaps(Character subject)
        {
            if(subject.IsDisabled)
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
        /// Flippidy flap
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
