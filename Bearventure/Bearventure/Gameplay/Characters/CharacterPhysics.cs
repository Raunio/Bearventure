using Microsoft.Xna.Framework;

namespace Bearventure
{
    /// <summary>
    /// Used for applying basic physics to characters.
    /// </summary>
    public static class CharacterPhysics
    {
        #region Members

        private static float gravity;

        #endregion

        #region Methods
        /// <summary>
        /// Apply basic physics to character.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="gameTime"></param>
        public static void Apply(Character subject, GameTime gameTime)
        {
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
                case Character.State.Flying:

                    break;
                case Character.State.Attacking:
                    Stop(subject);
                    break;
            }

            ApplyGravity(subject);
            subject.position += subject.velocity;
        }

        public static float Gravity
        {
            get { return gravity; }
            set { gravity = value; }
        }

        private static void ApplyGravity(Character subject)
        {
            subject.velocity.Y += gravity;
        }

        private static void Stop(Character subject)
        {
            if (subject.velocity.X > subject.deacceleration)
                subject.velocity.X -= subject.deacceleration;

            else if (subject.velocity.X < -subject.deacceleration)
                subject.velocity.X += subject.deacceleration;

            else
                subject.velocity.X = 0;
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

                else if (subject.velocity.X > subject.runSpeed)
                    subject.velocity.X = -subject.runSpeed;
            }
        }
        private static void Jump(Character subject)
        {
            if (subject.hasJumped == false)
            {
                subject.velocity.Y += subject.jumpStrenght;
                subject.hasJumped = true;
            }
        }
        private static void Fly(Character subject)
        {

        }

        #endregion
    }
}
