using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Bearventure
{
    public class Behaviour
    {
        #region Members

        private Character subject;
        private Character target;
        private Constants.BehaviourType behaviourType;
        public StrategyPlanner strategyPlanner;
        private float waitTimer = 0f;

        #region Patrolling members

        private int pointA = 0;
        private int pointB = 0;
        private int previousPoint = 0;
        private int nextPoint = 0;

        #endregion

        #endregion

        #region Initialization
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="subject">Enemy -type object to which the behaviour is applied to. One of the "Init..." -methods should be called after this.</param>
        /// <param name="startDirection">The starting direction of the subject.</param>
        public Behaviour(Enemy subject, Player player)
        {
            this.subject = subject;
            subject.state = Constants.CharacterState.Stopped;
            target = player;
            strategyPlanner = new StrategyPlanner(subject, target);
        }
        /// <summary>
        /// Set up passive behaviour. A passive subject stands still until the player reaches its line of sight. This method can be called upon at any time to switch from a previously initialized behaviour type.
        /// </summary>
        public void InitPassive()
        {
            behaviourType = Constants.BehaviourType.Passive;
        }
        /// <summary>
        /// Set up patrol behaviour with no fixed points. The subject starts patrolling to its direction and turns back when a collision happens. This method can be called upon at any time to switch from a previously initialized behaviour type.
        /// </summary>
        public void InitFreePatrol()
        {
            behaviourType = Constants.BehaviourType.FreePatrol;
        }
        /// <summary>
        /// Set up patrol behaviour with fixed points. The subject continuously patrols from point_A to point_B. Starting direction does not matter, the subject starts heading towards point_A automaticly. This method can be called upon at any time to switch from a previously initialized behaviour type.
        /// </summary>
        /// <param name="point_A">First patrolling point in the X-axis. Give it a value of 0 to assing the subjects starting position to point_A</param>
        /// <param name="point_B">Second patrolling point in the X-axis</param>
        public void InitFixedPatrol(int point_A, int point_B)
        {
            behaviourType = Constants.BehaviourType.FixedPatrol;
            this.pointA = point_A;
            this.pointB = point_B;

            nextPoint = NearestPoint;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Should be called in the Update method of the subject. Applies the chosen behaviour type to it.
        /// </summary>
        /// <param name="gameTime">GameTime should be passed for timers</param>
        public void Apply(GameTime gameTime)
        {
            strategyPlanner.Plan();

            switch (behaviourType)
            {
                case Constants.BehaviourType.FixedPatrol:

                    switch (strategyPlanner.CurrentAction())
                    {
                        case Constants.ActionType.Default:
                            UpdateFixedPatrol(gameTime);
                            break;
                        case Constants.ActionType.Chase:
                            GoTo((int)target.position.X);
                            break;
                        case Constants.ActionType.Attack:
                            SetState(Constants.CharacterState.Attacking);
                            break;
                        case Constants.ActionType.Stop:
                            SetState(Constants.CharacterState.Stopped);
                            break;
                    }
                    break;
                default:
                    Debug.Assert(true, "Behaviour class Apply method switch case behaviour type default");
                    break;
            }
        }
        /// <summary>
        /// Set wait time for character in milliseconds. The use of wait time depends on the type of the subjects behaviour. 
        /// FixedPatrol: Subject stops for a time equal to Wait_time at each patrol point.
        /// </summary>
        public float WaitTime
        {
            get;
            set;
        }

        #endregion

        #region Private methods

        private void UpdateFreePatrol()
        {

        }

        private void UpdateFixedPatrol(GameTime gameTime)
        {
            waitTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            float brakepoint = (subject.walkSpeed / subject.deacceleration) * 2;

            if (DistanceToPoint(pointA) <= brakepoint)
            {
                if (previousPoint != pointA)
                {
                    previousPoint = pointA;
                    waitTimer = 0;
                }

                nextPoint = pointB;
                SetState(Constants.CharacterState.Stopped);
            }

            else if (DistanceToPoint(pointB) <= brakepoint)
            {
                if (previousPoint != pointB)
                {
                    previousPoint = pointB;
                    waitTimer = 0;
                }

                nextPoint = pointA;
                SetState(Constants.CharacterState.Stopped);
            }

            if (waitTimer >= WaitTime)
            {
                GoTo(nextPoint);
            }

        }

        private int NearestPoint
        {
            get
            {
                return DistanceToPoint(pointA) < DistanceToPoint(pointB) ? pointA : pointB;
            }
        }

        private int DistanceToPoint(int point)
        {
            int distance = (int)subject.position.X - point;
            return (distance < 0) ? distance * -1 : distance;
        }

        private void GoTo(int point)
        {
            int position = (int)subject.position.X;

            if (position < point)
            {
                ChangeDirection(Constants.Direction.Right);
                SetState(Constants.CharacterState.Walking);
            }
            else if (position > point)
            {
                ChangeDirection(Constants.Direction.Left);
                SetState(Constants.CharacterState.Walking);
            }
        }

        private void ChangeDirection(Constants.Direction newDirection)
        {
            subject.direction = newDirection;
        }

        private void SetState(Constants.CharacterState newState)
        {
            if(subject.state != Constants.CharacterState.UsingSkill && subject.state != Constants.CharacterState.Disabled)
                subject.state = newState;
        }

        #endregion
    }
}
