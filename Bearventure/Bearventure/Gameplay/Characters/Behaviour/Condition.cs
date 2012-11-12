
namespace Bearventure
{
    /// <summary>
    /// Conditions are used by the Action- & StrategyPlanner -classes.
    /// </summary>
    public class Condition
    {
        public Condition(Constants.ConditionType type, object value)
        {
            this.Type = type;
            this.Value = value;
        }

        public Constants.ConditionType Type
        {
            get;
            set;
        }

        public object Value
        {
            get;
            set;
        }
        /// <summary>
        /// This method checks if the condition has been fulfilled.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="player"></param>
        /// <returns>True if fulfilled.</returns>
        public bool Fulfilled(Enemy subject, Character player)
        {
            switch (Type)
            {
                case Constants.ConditionType.DistanceToPlayerEqualTo:
                    if (DistanceBetween((int)subject.position.X, (int)player.position.X) == (int)Value) { return true; }
                    break;
                case Constants.ConditionType.DistanceToPlayerLowerThan:
                    if (DistanceBetween((int)subject.position.X, (int)player.position.X) < (int)Value) { return true; }
                    break;
                case Constants.ConditionType.DistanceToPlayerGreaterThan:
                    if (DistanceBetween((int)subject.position.X, (int)player.position.X) > (int)Value) { return true; }
                    break;
                case Constants.ConditionType.DistanceToPlayerOtherThan:
                    if (DistanceBetween((int)subject.position.X, (int)player.position.X) != (int)Value) { return true; }
                    break;
                case Constants.ConditionType.HealthEqualTo:
                    if (subject.health == (int)Value) { return true; }
                    break;
                case Constants.ConditionType.HealthHigherThan:
                    if (subject.health > (int)Value) { return true; }
                    break;
                case Constants.ConditionType.HealthLowerThan:
                    if (subject.health < (int)Value) { return true; }
                    break;
                case Constants.ConditionType.HealthOtherThan:
                    if (subject.health != (int)Value) { return true; }
                    break;
                case Constants.ConditionType.PlayerState:
                    if (player.state == (Constants.CharacterState)Value) { return true; }
                    break;
                case Constants.ConditionType.VelocityEqualTo:
                    if (subject.velocity.X == (float)Value) { return true; }
                    break;
                case Constants.ConditionType.VelocityHigherThan:
                    if (subject.velocity.X > (float)Value) { return true; }
                    break;
                case Constants.ConditionType.VelocityLowerThan:
                    if (subject.velocity.X < (float)Value) { return true; }
                    break;
                case Constants.ConditionType.VelocityOtherThan:
                    if (subject.velocity.X != (float)Value) { return true; }
                    break;
                default:
                    return false;
            }

            return false;
        }

        private int DistanceBetween(int a, int b)
        {
            return a - b > 0 ? a - b : b - a;
        }
    }
}
