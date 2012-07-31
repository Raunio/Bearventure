using Microsoft.Xna.Framework;

namespace Bearventure
{
    public class Behaviour
    {
        #region Making things easier until I come up with a better way to do this.
        private Character.State Stopped;
        private Character.State Walking;
        // What the balls are these? :D -Huemac
        private const Character.State Attacking = Character.State.Attacking;
        private const Character.State Disabled = Character.State.Disabled;
        private const Character.State Falling = Character.State.Falling;
        private const Character.State Flying = Character.State.Flying;
        private const Character.State Jumping = Character.State.Jumping;
        private Character.State Running;
        #endregion

        #region Enumerations

        private enum BehaviourType
        {
            Passive,
            FreePatrol,
            FixedPatrol,
        };

        #endregion

        #region Members

        private Character subject;
        private Character target;
        private BehaviourType behaviourType;
        public StrategyPlanner strategyPlanner;
        private float waitTimer = 0f;
        private float waitTime;

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
            subject.state = Stopped;
            target = player;
            strategyPlanner = new StrategyPlanner(subject, target);

            if (subject.orientation == Character.Orientation.Air)
            {
                Walking = Character.State.Flying;
                Running = Character.State.Flying;
                Stopped = Character.State.Hovering;
            }
            else
            {
                Walking = Character.State.Walking;
                Running = Character.State.Running;
                Stopped = Character.State.Stopped;
            }
        }
        /// <summary>
        /// Set up passive behaviour. A passive subject stands still until the player reaches its line of sight. This method can be called upon at any time to switch from a previously initialized behaviour type.
        /// </summary>
        public void InitPassive()
        {
            behaviourType = BehaviourType.Passive;
        }
        /// <summary>
        /// Set up patrol behaviour with no fixed points. The subject starts patrolling to its direction and turns back when a collision happens. This method can be called upon at any time to switch from a previously initialized behaviour type.
        /// </summary>
        public void InitFreePatrol()
        {
            behaviourType = BehaviourType.FreePatrol;
        }
        /// <summary>
        /// Set up patrol behaviour with fixed points. The subject continuously patrols from point_A to point_B. Starting direction does not matter, the subject starts heading towards point_A automaticly. This method can be called upon at any time to switch from a previously initialized behaviour type.
        /// </summary>
        /// <param name="point_A">First patrolling point in the X-axis. Give it a value of 0 to assing the subjects starting position to point_A</param>
        /// <param name="point_B">Second patrolling point in the X-axis</param>
        public void InitFixedPatrol(int point_A, int point_B)
        {
            behaviourType = BehaviourType.FixedPatrol;
            this.pointA = point_A;
            this.pointB = point_B;

            nextPoint = NearestPoint;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Should be called once when a collision happens. Temporary!
        /// </summary>
        public void CollisionHappened(Character.Direction dir)
        {

        }
        /// <summary>
        /// Should be called in the Update method of the subject. Applies the chosen behaviour type to it.
        /// </summary>
        /// <param name="gameTime">GameTime should be passed for timers</param>
        public void Apply(GameTime gameTime)
        {
            strategyPlanner.Plan();

            switch (behaviourType)
            {
                case BehaviourType.FixedPatrol:
                    if (strategyPlanner.CurrentAction() == Action.ActionType.Default)
                        UpdateFixedPatrol(gameTime);
                    else if (strategyPlanner.CurrentAction() == Action.ActionType.Chase)
                        GoTo((int)target.position.X);
                    else if (strategyPlanner.CurrentAction() == Action.ActionType.Attack)
                        SetState(Attacking);
                    else if (strategyPlanner.CurrentAction() == Action.ActionType.Stop)
                        SetState(Stopped);
                    break;
            }
        }
        /// <summary>
        /// Set wait time for character in milliseconds. The use of wait time depends on the type of the subjects behaviour. FixedPatrol: Subject stops for a time equal to Wait_time at each patrol point.
        /// </summary>
        public float WaitTime
        {
            set { waitTime = value; }
            get { return waitTime; }
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
                SetState(Stopped);
            }

            else if (DistanceToPoint(pointB) <= brakepoint)
            {
                if (previousPoint != pointB)
                {
                    previousPoint = pointB;
                    waitTimer = 0;
                }

                nextPoint = pointA;
                SetState(Stopped);
            }

            if (waitTimer >= waitTime)
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
            //TODO: What if point and position are equal? -Huemac
            if (position < point)
            {
                ChangeDirection(Character.Direction.Right);
                SetState(Walking);
            }
            else if (position > point)
            {
                ChangeDirection(Character.Direction.Left);
                SetState(Walking);
            }
        }

        private void ChangeDirection(Character.Direction newDirection)
        {
            subject.direction = newDirection;
        }

        private void SetState(Character.State newState)
        {
            subject.state = newState;
        }

        #endregion
    }
}
