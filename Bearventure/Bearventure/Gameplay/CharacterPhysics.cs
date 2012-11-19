using Microsoft.Xna.Framework;

namespace Bearventure
{
    /// <summary>
    /// Used for applying basic physics to characters.
    /// </summary>
    public static class CharacterPhysics
    {
        #region Members
        #endregion

        #region Methods
        /// <summary>
        /// Apply basic physics to character.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="gameTime"></param>
        public static void Apply(Character subject, GameTime gameTime)
        {
            ApplyGravity(subject);
            
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
            }

            HandleCollisions(subject);
            subject.position += subject.velocity;
        }

        public static float Gravity
        {
            get;
            set;
        }

        public static void ApplyGravity(Character subject)
        {
            subject.velocity.Y += Gravity;
        }

        private static void Stop(Character subject)
        {
            if (subject.velocity.X > subject.deacceleration)
                subject.velocity.X -= subject.deacceleration;

            else if (subject.velocity.X < -subject.deacceleration)
                subject.velocity.X += subject.deacceleration;

            else
                subject.velocity.X = 0;

            if (subject.orientation == Constants.CharacterOrientation.Air)
            {
                if (subject.velocity.Y > subject.deacceleration)
                    subject.velocity.Y -= Gravity;

                else if (subject.velocity.Y < -subject.deacceleration)
                    subject.velocity.Y += Gravity;
                else
                    subject.velocity.Y = 0;
            }
        }
        private static void Walk(Character subject)
        {
            if (subject.velocity.Y > Gravity)
            {
                subject.state = Constants.CharacterState.Falling;
                return;
            }

            if (subject.direction == Constants.Direction.Right)
            {
                if (subject.velocity.X < subject.walkSpeed)
                    subject.velocity.X += subject.acceleration;

                else if (subject.velocity.X > subject.walkSpeed)
                    subject.velocity.X = subject.walkSpeed;

            }
            else if (subject.direction == Constants.Direction.Left)
            {
                if (subject.velocity.X > -subject.walkSpeed)
                    subject.velocity.X -= subject.acceleration;

                else if (subject.velocity.X < -subject.walkSpeed)
                    subject.velocity.X = -subject.walkSpeed;
            }
        }
        private static void Run(Character subject)
        {
            if (subject.velocity.Y > Gravity)
            {
                subject.state = Constants.CharacterState.Falling;
                return;
            }

            if (subject.direction == Constants.Direction.Right)
            {
                if (subject.velocity.X < subject.runSpeed)
                    subject.velocity.X += subject.acceleration;

                else if (subject.velocity.X > subject.runSpeed)
                    subject.velocity.X = subject.runSpeed;
            }
            else if (subject.direction == Constants.Direction.Left)
            {
                if (subject.velocity.X > -subject.runSpeed)
                    subject.velocity.X -= subject.acceleration;

                else if (subject.velocity.X < -subject.runSpeed)
                    subject.velocity.X = -subject.runSpeed;
            }
        }
        private static void Jump(Character subject)
        {
            int collision = CollisionHandler.CollisionOccursWithMap(subject, subject.velocity);
            int Bottom = subject.BoundingBox.Bottom;
            int Left = subject.BoundingBox.Left;
            int Right = subject.BoundingBox.Right;

            if (collision == Bottom || (collision == Bottom + Right || collision == Bottom + Left))
            {
                subject.velocity.Y -= subject.jumpStrenght;
            }
            else
            {
                if (subject.velocity.Y > Gravity)
                {
                    subject.state = Constants.CharacterState.Falling;
                }
            }

            if (subject.direction == Constants.Direction.Right)
            {
                if (subject.velocity.X < subject.walkSpeed / 2)
                    subject.velocity.X += subject.acceleration;

            }
            else if (subject.direction == Constants.Direction.Left)
            {
                if (subject.velocity.X > -subject.walkSpeed / 2)
                    subject.velocity.X -= subject.acceleration;
            }
        }
        private static void Fall(Character subject)
        {
            if (subject.direction == Constants.Direction.Right)
            {
                if (subject.velocity.X < subject.walkSpeed / 2)
                    subject.velocity.X += subject.acceleration;

            }
            else if (subject.direction == Constants.Direction.Left)
            {
                if (subject.velocity.X > -subject.walkSpeed / 2)
                    subject.velocity.X -= subject.acceleration;
            }
        }

        private static void HandleCollisions(Character subject)
        {
            int collision = CollisionHandler.CollisionOccursWithMap(subject, subject.velocity);

            int Top = subject.BoundingBox.Top;
            int Bottom = subject.BoundingBox.Bottom;
            int Left = subject.BoundingBox.Left;
            int Right = subject.BoundingBox.Right;

            if (collision == Bottom && CollisionHandler.TerrainType == Constants.Solid)
            {
                subject.velocity.Y = 0;
            }

            else if (collision == Top && CollisionHandler.TerrainType == Constants.Solid)
                subject.velocity.Y = 0;
            else if ((collision == Left || collision == Right) && CollisionHandler.TerrainType == Constants.Solid)
                subject.velocity.X = 0;
            else if ((collision == Bottom + Left || collision == Bottom + Right) && CollisionHandler.TerrainType == Constants.Solid)
            {
                subject.velocity.Y = 0;

                if (CollisionHandler.HeightDifference < 5)
                    subject.position.Y -= CollisionHandler.HeightDifference;

                subject.velocity.X = 0;
            }

            else if ((collision == Top + Left || collision == Top + Right) && CollisionHandler.TerrainType == Constants.Solid)
            {
                subject.velocity.X = 0;
                subject.velocity.Y = 0;
            }
        }

        public static Color OnTerrain(Character subject)
        {
            int collision = CollisionHandler.CollisionOccursWithMap(subject, subject.velocity);

            return CollisionHandler.TerrainType;

        }

        public static bool OnGround(Character subject)
        {
            int collision = CollisionHandler.CollisionOccursWithMap(subject, new Vector2(subject.velocity.X, subject.velocity.Y + 1));

            if (collision == subject.BoundingBox.Bottom ||
                collision == subject.BoundingBox.Bottom + subject.BoundingBox.Right ||
                collision == subject.BoundingBox.Bottom + subject.BoundingBox.Left)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
