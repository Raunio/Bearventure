using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bearventure
{
    public class Behaviour
    {
        #region Making things easier until I come up with a better way to do this.
        private const Character.State Stopped = Character.State.Stopped;
        private const Character.State Walking = Character.State.Walking;
        private const Character.State Attacking = Character.State.Attacking;
        private const Character.State Disabled = Character.State.Disabled;
        private const Character.State Falling = Character.State.Falling;
        private const Character.State Flying = Character.State.Flying;
        private const Character.State Jumping = Character.State.Jumping;
        private const Character.State Running = Character.State.Running;
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
        private BehaviourType behaviourType;
        private float wait_timer = 0f;
        private float waitTime;

        #region Patrolling members

        private int point_A = 0;
        private int point_B = 0;
        private int previous_point = 0;
        public int next_point = 0;

        #endregion

        #endregion

        #region Initialization
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="subject">Character -type object to which the behaviour is applied to. One of the "Init..." -methods should be called after this.</param>
        /// <param name="startDirection">The starting direction of the subject.</param>
        public Behaviour(Character subject, Character.Direction startDirection)
        {
            this.subject = subject;
            subject.state = Stopped;
            subject.direction = startDirection;
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
            this.point_A = point_A;
            this.point_B = point_B;

            next_point = NearestPoint;
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
        /// <param name="gameTime"></param>
        public void Apply(GameTime gameTime)
        {
            switch (behaviourType)
            {
                case BehaviourType.FixedPatrol:
                    UpdateFixedPatrol(gameTime);
                    break;
            }

            CharacterPhysics.Apply(subject, gameTime);
        }
        /// <summary>
        /// Set wait time for character in milliseconds. The use of wait time depends on the type of the subjects behaviour. FixedPatrol: Subject stops for a time equal to Wait_time at each patrol point.
        /// </summary>
        public float Wait_time
        {
            set
            {
                waitTime = value;
            }
            get
            {
                return waitTime;
            }
        }

        #endregion

        #region Private methods

        private void UpdateFreePatrol()
        {

        }

        private void UpdateFixedPatrol(GameTime gameTime)
        {
            wait_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (DistanceToPoint(point_A) < subject.walkSpeed)
            {
                if (previous_point != point_A)
                {
                    previous_point = point_A;
                    wait_timer = 0;
                }

                next_point = point_B;
                SetState(Stopped);
            }

            else if (DistanceToPoint(point_B) < subject.walkSpeed)
            {
                if (previous_point != point_B)
                {
                    previous_point = point_B;
                    wait_timer = 0;
                }

                next_point = point_A;
                SetState(Stopped);
            }

            if (wait_timer >= waitTime)
            {
                GoTo(next_point);
            }
      
        }

        public int NearestPoint
        {
            get
            {
                if(DistanceToPoint(point_A) < DistanceToPoint(point_B))
                {
                    return point_A;
                }
                else
                    return point_B;
            }
        }

        public int DistanceToPoint(int point)
        {
            int distance = (int)subject.position.X - point;

            if (distance < 0)
            {
                distance *= -1;
            }

            return distance;
        }

        private void GoTo(int point)
        {
            int position = (int)subject.position.X;

            if (position < point)
            {
                ChangeDirection(Character.Direction.Right);
                subject.state = Walking;
            }
            else if (position > point)
            {
                ChangeDirection(Character.Direction.Left);
                subject.state = Walking;
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
