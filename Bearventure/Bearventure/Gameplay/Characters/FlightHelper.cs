using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bearventure
{
    public static class FlightHelper
    {
        public static void Fly(Enemy subject, Vector2 point)
        {
            float brakepoint = (subject.runSpeed / CharacterPhysics.Gravity) * 2;

            if (DistanceBetween(subject.position.Y, point.Y) > brakepoint)
            {
                if (IsAbove(point.Y, subject.position.Y) && subject.velocity.Y > -subject.runSpeed)
                    subject.velocity.Y -= subject.acceleration;
            }

            else if (DistanceBetween(subject.position.Y, point.Y) < brakepoint)
                subject.velocity.Y -= CharacterPhysics.Gravity;
        }

        private static float DistanceBetween(float a, float b)
        {
            return a - b < 0 ? b - a : a - b;
        }

        private static bool IsAbove(float a, float b)
        {
            return a < b ? true : false;
        }
    }
}
