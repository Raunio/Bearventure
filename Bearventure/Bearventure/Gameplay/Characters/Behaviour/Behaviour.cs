using System.Diagnostics;
using Microsoft.Xna.Framework;
using Bearventure.Gameplay.Characters;
using Bearventure.Gameplay.Characters.Skills;

namespace Bearventure
{
    public class Behaviour
    {
        #region Members

        private Enemy subject;
        private Character target;
        private Constants.BehaviourType behaviourType;
        private StrategyPlanner strategyPlanner;
        private float waitTimer = 0f;

        private float blockCheckFrequency = 50f;
        private float blockCheckTimer = 0f;
        private bool TimeOut
        {
            get
            {
                return timeOut;
            }
            set
            {
                if(value == true)
                    subject.SetState(Constants.CharacterState.Stopped);

                timeOut = value;
            }
        }
        private bool timeOut;

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
            PointY = subject.Orientation == Constants.CharacterOrientation.Air ? (int)subject.position.Y : 0;
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

        #region Gets & Sets
        /// <summary>
        /// Set wait time for character in milliseconds. The use of wait time depends on the type of the subjects behaviour. 
        /// FixedPatrol: Subject stops for a time equal to Wait_time at each patrol point.
        /// </summary>
        public float WaitTime
        {
            get;
            set;
        }
        /// <summary>
        /// Gets and sets the Y-scale patrolling point. This is needed for Air-oriented characters.
        /// </summary>
        public int PointY
        {
            get;
            set;
        }
        
        #endregion

        #region Public methods
        /// <summary>
        /// Should be called in the Update method of the subject. Applies the chosen behaviour type to it.
        /// </summary>
        /// <param name="gameTime">GameTime should be passed for timers</param>
        public void Apply(GameTime gameTime)
        {
            blockCheckTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            strategyPlanner.Plan(gameTime);

            switch (strategyPlanner.CurrentAction())
            {
                case Constants.ActionType.Default:
                    if (behaviourType == Constants.BehaviourType.FixedPatrol)
                        if (CharacterPhysics.OnGround(subject) || subject.Orientation == Constants.CharacterOrientation.Air)
                            UpdateFixedPatrol(gameTime);
                    if (PointY > 0)
                        UpdateAltitude(PointY);
                    break;
                case Constants.ActionType.Chase:
                    if (CharacterPhysics.OnGround(subject) || subject.Orientation == Constants.CharacterOrientation.Air)
                        GoTo((int)target.position.X);
                    if (PointY > 0)
                        UpdateAltitude((int)target.BoundingBox.Y);
                    break;
                case Constants.ActionType.Attack:
                    if(CharacterPhysics.OnGround(subject) || subject.Orientation == Constants.CharacterOrientation.Air)
                        subject.SetState(Constants.CharacterState.Stopped);
                    break;
                case Constants.ActionType.Stop:
                    if (CharacterPhysics.OnGround(subject) || subject.Orientation == Constants.CharacterOrientation.Air)
                        subject.SetState(Constants.CharacterState.Stopped);
                    if (PointY > 0)
                        UpdateAltitude(PointY);
                    break;
            }

            ManageSkills();
            
        }

        #endregion

        #region Private methods

        private void ManageSkills()
        {
            if (subject.ActiveSkill == null || !subject.ActiveSkill.IsActive)
            {
                foreach (EnemySkill s in subject.Skills)
                {
                    int fulfilledConditions = 0;

                    foreach (Condition c in s.Conditions)
                    {
                        if (c.Fulfilled(subject, target))
                        {
                            fulfilledConditions++;
                        }
                    }

                    if (fulfilledConditions == s.Conditions.Count)
                    {
                        if (s.IsReady && !s.IsActive)
                        {
                            subject.UseSkill(s);
                            return;
                        }
                    }
                }

            }
        }
        private void UpdateFreePatrol()
        {

        }
        private void UpdateFixedPatrol(GameTime gameTime)
        {
            waitTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            float brakepoint = (subject.walkSpeed / subject.decceleration) * 2;

            if (DistanceBetween((int)subject.position.X, pointA) <= brakepoint)
            {
                if (previousPoint != pointA)
                {
                    previousPoint = pointA;
                    waitTimer = 0;
                }

                nextPoint = pointB;
                subject.SetState(Constants.CharacterState.Stopped);
            }

            else if (DistanceBetween((int)subject.position.X, pointB) <= brakepoint)
            {
                if (previousPoint != pointB)
                {
                    previousPoint = pointB;
                    waitTimer = 0;
                }

                nextPoint = pointA;
                subject.SetState(Constants.CharacterState.Stopped);
            }

            if (blockCheckTimer >= blockCheckFrequency)
            {
                blockCheckTimer = 0;

                if (CharacterPhysics.Blocked(subject))
                {
                    if (nextPoint == pointA)
                    {
                        previousPoint = pointA;
                        nextPoint = pointB;
                    }
                    else if(nextPoint == pointB)
                    {
                        previousPoint = pointB;
                        nextPoint = pointA;
                    }
                }
            }

            if (waitTimer >= WaitTime)
            {
                GoTo(nextPoint);
            }

        }
        private void UpdateAltitude(int point)
        {
            float brakepoint = (subject.walkSpeed / subject.decceleration) * 2;

            int position = (int)subject.position.Y;

            if (DistanceBetween(position, point) > brakepoint)
            {
                if (position > point)
                    subject.directionY = Constants.DirectionY.Up;
                else
                    subject.directionY = Constants.DirectionY.Down;
            }
            else
                subject.directionY = Constants.DirectionY.Hold;

        }
        private int NearestPoint
        {
            get
            {
                return DistanceBetween((int)subject.position.X, pointA) < DistanceBetween((int)subject.position.X, pointB) ? pointA : pointB;
            }
        }
        private int DistanceBetween(int a, int b)
        {
            int distance = a - b;
            return (distance < 0) ? distance * -1 : distance;
        }
        private void GoTo(int point)
        {
            int position = (int)subject.position.X;

            if (position < point)
            {
                ChangeDirection(Constants.DirectionX.Right);
                subject.SetState(Constants.CharacterState.Walking);
            }
            else if (position > point)
            {
                ChangeDirection(Constants.DirectionX.Left);
                subject.SetState(Constants.CharacterState.Walking);
            }
        }
        private void ChangeDirection(Constants.DirectionX newDirection)
        {
            subject.directionX = newDirection;
        }

        #endregion
    }
}
