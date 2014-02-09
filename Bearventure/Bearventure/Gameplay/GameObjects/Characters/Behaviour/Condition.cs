
using Bearventure.Gameplay.Characters;
using Microsoft.Xna.Framework;
using Bearventure.Engine.CollisionDetection;
using Bearventure.Gameplay.Characters.Skills;
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
                    if (Vector2.Distance(subject.Position, player.Position) == (int)Value) { return true; }
                    break;
                case Constants.ConditionType.DistanceToPlayerLowerThan:
                    if (Vector2.Distance(subject.Position, player.Position) < (int)Value) { return true; }
                    break;
                case Constants.ConditionType.DistanceToPlayerGreaterThan:
                    if (Vector2.Distance(subject.Position, player.Position) > (int)Value) { return true; }
                    break;
                case Constants.ConditionType.DistanceToPlayerOtherThan:
                    if (Vector2.Distance(subject.Position, player.Position) != (int)Value) { return true; }
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
                case Constants.ConditionType.SubjectState:
                    if (subject.state == (Constants.CharacterState)Value) { return true; }
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
                case Constants.ConditionType.AttackReady:
                    if (subject.Attack.IsReady == (bool)Value) { return true; }
                    break;
                case Constants.ConditionType.Blocked:
                    if(CharacterPhysics.Blocked(subject) == (bool)Value) { return true; }
                    break;
                case Constants.ConditionType.FacingPlayer:
                    if (player.Position.X < subject.Position.X)
                    {
                        return subject.directionX == Constants.DirectionX.Left ? true : false;
                    }
                    else if (player.Position.X > subject.Position.X)
                    {
                        return subject.directionX == Constants.DirectionX.Right ? true : false;
                    }
                    break;
                case Constants.ConditionType.CollidesWithPlayer:
                    if(subject.BoundingBox.Intersects(player.BoundingBox) == (bool)Value) 
                    { 
                        return true; 
                    }
                    break;
                case Constants.ConditionType.OnGround:
                    if (CharacterPhysics.OnGround(subject) == (bool)Value)
                        return true;
                    break;
                default:
                    return false;
                case Constants.ConditionType.HasActiveSkill:
                    if (subject.ActiveSkill != null) { if (subject.ActiveSkill.IsActive == (bool)Value) return true; }
                    break;
                case Constants.ConditionType.IsUsingSkill:
                    if (subject.ActiveSkill == (CharacterSkill)Value) { return true; }
                    break;
                case Constants.ConditionType.IsNotUsingSkill:
                    if (subject.ActiveSkill != (CharacterSkill)Value) { return true; }
                    break;
            }

            return false;
        }
    }
}
