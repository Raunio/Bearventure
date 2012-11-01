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
                case Character.State.Stopped:
                    Stop(subject);
                    break;
                case Character.State.Walking:
                    Walk(subject);
                    break;
                case Character.State.Running:
                    Run(subject);
                    break;
                case Character.State.Jumping:
                    Jump(subject);
                    break;
                case Character.State.Attacking:
                    Stop(subject);
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

            if (subject.orientation == Character.Orientation.Air)
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
            if (subject.direction == Character.Direction.Right)
            {
                if (subject.velocity.X < subject.walkSpeed)
                    subject.velocity.X += subject.acceleration;

                else if (subject.velocity.X > subject.walkSpeed)
                    subject.velocity.X = subject.walkSpeed;

            }
            else if (subject.direction == Character.Direction.Left)
            {
                if (subject.velocity.X > -subject.walkSpeed)
                    subject.velocity.X -= subject.acceleration;

                else if (subject.velocity.X < -subject.walkSpeed)
                    subject.velocity.X = -subject.walkSpeed;
            }
        }
        private static void Run(Character subject)
        {
            if (subject.direction == Character.Direction.Right)
            {
                if (subject.velocity.X < subject.runSpeed)
                    subject.velocity.X += subject.acceleration;

                else if (subject.velocity.X > subject.runSpeed)
                    subject.velocity.X = subject.runSpeed;
            }
            else if (subject.direction == Character.Direction.Left)
            {
                if (subject.velocity.X > -subject.runSpeed)
                    subject.velocity.X -= subject.acceleration;

                else if (subject.velocity.X < -subject.runSpeed)
                    subject.velocity.X = -subject.runSpeed;
            }
        }
        private static void Jump(Character subject)
        {
            subject.velocity.Y -= subject.jumpStrenght;
        }

        private static void HandleCollisions(Character subject)
        {
            int collision = CollisionHandler.CollisionOccursWithMap(subject, subject.velocity);

            int Top = subject.BoundingBox.Top;
            int Bottom = subject.BoundingBox.Bottom;
            int Left = subject.BoundingBox.Left;
            int Right = subject.BoundingBox.Right;

            if (collision == Bottom)
                subject.velocity.Y = 0;
            else if (collision == Top)
                subject.velocity.Y = -(subject.velocity.Y * 0.5f);
            else if (collision == Left || collision == Right)
                subject.velocity.X = -(subject.velocity.X * 0.5f);
            else if (collision == Bottom + Left || collision == Bottom + Right)
            {               
                subject.velocity.Y = 0;

                if (CollisionHandler.HeightDifference < 5)
                    subject.position.Y -= CollisionHandler.HeightDifference;
                else
                    subject.velocity.X = -2 * (subject.velocity.X * 0.5f);
            }

            else if (collision == Top + Left || collision == Top + Right)
            {
                subject.velocity.X = -(subject.velocity.X * 0.5f);
                subject.velocity.Y = -(subject.velocity.Y * 0.5f);
            }
        }

        #endregion
    }
}
