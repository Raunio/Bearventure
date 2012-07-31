using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bearventure
{
    /// <summary>
    /// Conditions are used by the Action- & StrategyPlanner -classes.
    /// </summary>
    public class Condition
    {       
        public enum ConditionType
        {
            HealthLowerThan,
            HealthEqualTo,
            HealthHigherThan,
            HealthOtherThan,
            PlayerState,
            VelocityLowerThan,
            VelocityEqualTo,
            VelocityHigherThan,
            VelocityOtherThan,
            DistanceToPlayerLowerThan,
            DistanceToPlayerEqualTo,
            DistanceToPlayerGreaterThan,
            DistanceToPlayerOtherThan,
        };

        private ConditionType type;
        private object mValue;

        public Condition(ConditionType type, object value)
        {
            this.type = type;
            this.mValue = value;
        }

        public ConditionType Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        public object Value
        {
            get
            {
                return mValue;
            }

            set
            {
                mValue = value;
            }
        }
        /// <summary>
        /// This method checks if the condition has been fulfilled.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="player"></param>
        /// <returns>True if fulfilled.</returns>
        public bool Fulfilled(Enemy subject, Character player)
        {
            switch (type)
            {
                case ConditionType.DistanceToPlayerEqualTo:
                    if (DistanceBetween((int)subject.position.X, (int)player.position.X) == (int)mValue) { return true; }
                    break;
                case ConditionType.DistanceToPlayerLowerThan:
                    if (DistanceBetween((int)subject.position.X, (int)player.position.X) < (int)mValue) { return true; }
                    break;
                case ConditionType.DistanceToPlayerGreaterThan:
                    if (DistanceBetween((int)subject.position.X, (int)player.position.X) > (int)mValue) { return true; }
                    break;
                case ConditionType.DistanceToPlayerOtherThan:
                    if (DistanceBetween((int)subject.position.X, (int)player.position.X) != (int)mValue) { return true; }
                    break;
                case ConditionType.HealthEqualTo:
                    if (subject.health == (int)mValue) { return true; }
                    break;
                case ConditionType.HealthHigherThan:
                    if (subject.health > (int)mValue) { return true; }
                    break;
                case ConditionType.HealthLowerThan:
                    if (subject.health < (int)mValue) { return true; }
                    break;
                case ConditionType.HealthOtherThan:
                    if (subject.health != (int)mValue) { return true; }
                    break;
                case ConditionType.PlayerState:
                    if (player.state == (Character.State)mValue) { return true; }
                    break;
                case ConditionType.VelocityEqualTo:
                    if (subject.velocity.X == (float)mValue) { return true; }
                    break;
                case ConditionType.VelocityHigherThan:
                    if (subject.velocity.X > (float)mValue) { return true; }
                    break;
                case ConditionType.VelocityLowerThan:
                    if (subject.velocity.X < (float)mValue) { return true; }
                    break;
                case ConditionType.VelocityOtherThan:
                    if (subject.velocity.X != (float)mValue) { return true; }
                    break;
                default:
                    return false;
            }

            return false;
        }

        private int DistanceBetween(int a, int b)
        {
            int distance = a - b;

            if (distance < 0)
            {
                distance *= -1;
            }

            return distance;
        }
    }
}
