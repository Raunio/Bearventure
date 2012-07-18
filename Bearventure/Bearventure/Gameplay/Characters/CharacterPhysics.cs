using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bearventure
{
    /// <summary>
    /// Used for applying basic physics to characters.
    /// </summary>
    static class CharacterPhysics
    {
        #region Methods

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
            }
        }
        private static void Stop(Character subject)
        {
            if (subject.velocity.X > subject.deacceleration)
            {
                subject.velocity.X -= subject.deacceleration;
            }
            else if (subject.velocity.X < -subject.deacceleration)
            {
                subject.velocity.X += subject.deacceleration;
            }
            /*else
            {
                subject.velocity.X = 0;
            }*/
        }
        private static void Walk(Character subject)
        {
            if (subject.direction == Character.Direction.Right)
            {
                if (subject.velocity.X < subject.walkSpeed)
                {
                    subject.velocity.X += subject.acceleration;
                }
                else if (subject.velocity.X > subject.walkSpeed)
                {
                    subject.velocity.X = subject.walkSpeed;
                }
            }
            else if (subject.direction == Character.Direction.Left)
            {
                if (subject.velocity.X > -subject.walkSpeed)
                {
                    subject.velocity.X -= subject.acceleration;
                }
                else if (subject.velocity.X < -subject.walkSpeed)
                {
                    subject.velocity.X = -subject.walkSpeed;
                }
            }
        }
        private static void Run(Character subject)
        {
            if (subject.direction == Character.Direction.Right)
            {
                if (subject.velocity.X < subject.runSpeed)
                {
                    subject.velocity.X += subject.acceleration;
                }
                else if (subject.velocity.X > subject.runSpeed)
                {
                    subject.velocity.X = subject.runSpeed;
                }
            }
            else if (subject.direction == Character.Direction.Left)
            {
                if (subject.velocity.X > -subject.runSpeed)
                {
                    subject.velocity.X -= subject.acceleration;
                }
                else if (subject.velocity.X > subject.runSpeed)
                {
                    subject.velocity.X = -subject.runSpeed;
                }
            }
        }

        #endregion
    }
}
