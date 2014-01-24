using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Bearventure.Gameplay.Characters;
using Microsoft.Xna.Framework.Content;
using Bearventure.Engine.CollisionDetection;

namespace Bearventure.Gameplay.GameObjects
{
    public class Platform : GameplayObject
    {
        Constants.PlatformType type;
        Constants.DirectionX direction;
        Constants.PlatformState state;

        private int patrolPointA;
        private int patrolPointB;
        private int previousPoint;
        private int nextPoint;

        private float waitTimer;
        private float waitTime;

        private bool isPatrolling;
        private bool isCollapsing;

        private bool hasTriggered;

        private bool hasBeenTouched;

        private float speed;

        private Vector2 tempVector = new Vector2();

        public Platform(ContentManager content, Constants.PlatformType type, Vector2 position)
        {
            TAG = "Platform";
            this.type = type;
            this.position = position;
            IsActive = true;

            mass = -1;

            Initialize(content);
        }

        public void InitPatrol(float speed, float acceleration, int pointA, int pointB, float waitTime)
        {
            isPatrolling = true;

            patrolPointA = pointA;
            patrolPointB = pointB;

            nextPoint = pointA;

            this.acceleration = acceleration;
            this.speed = speed;

            this.waitTime = waitTime;
        }

        public void InitCollapse(float delay)
        {
            isCollapsing = true;
            waitTime = delay;
        }

        private void Initialize(ContentManager content)
        {
            switch (type)
            {
                case Constants.PlatformType.MovingGrassPlatform:
                    currentAnimation = new Animation(content.Load<Texture2D>(Constants.MovingGrassPlatform), 0, 270, 31, 0, 0, 25);
                    break;
                case Constants.PlatformType.PassiveTreeBranchLeft:
                    currentAnimation = new Animation(content.Load<Texture2D>(Constants.PassiveTreeBranchLeft), 0, 319, 399, 0, 1, 25);
                    break;
                case Constants.PlatformType.PassiveTreeBranchLeft2:
                    currentAnimation = new Animation(content.Load<Texture2D>(Constants.PassiveTreeBranchLeft2), 0, 295, 412, 0, 1, 25);
                    break;
                case Constants.PlatformType.PassiveTreeBranchRight:
                    currentAnimation = new Animation(content.Load<Texture2D>(Constants.PassiveTreeBranchRight), 0, 387, 343, 0, 1, 25);
                    break;
                case Constants.PlatformType.TriggeredTreeBranchLeft:
                    currentAnimation = new Animation(content.Load<Texture2D>(Constants.TriggeredTreeBranchLeft), 0, 290, 402, 0, 1, 25);
                    break;
                case Constants.PlatformType.TriggeredTreeBranchLeft2:
                    currentAnimation = new Animation(content.Load<Texture2D>(Constants.TriggeredTreeBranchLeft2), 0, 248, 446, 0, 1, 25);
                    break;
                case Constants.PlatformType.TriggeredTreeBranchRight:
                    currentAnimation = new Animation(content.Load<Texture2D>(Constants.TriggeredTreeBranchRight), 0, 377, 350, 0, 1, 25);
                    break;
                case Constants.PlatformType.TriggeredTreeBranchRight2:
                    currentAnimation = new Animation(content.Load<Texture2D>(Constants.TriggeredTreeBranchRight2), 0, 312, 389, 0, 1, 25);
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            currentAnimation.Animate(gameTime);

            this.position += velocity;

            CharacterPhysics.HandleObjectCollisions(this);

            if (isPatrolling)
                UpdatePatrols(gameTime);
            if (isCollapsing)
                UpdateCollapsing(gameTime);

            if (state == Constants.PlatformState.Falling)
                CharacterPhysics.ApplyGravity(this);
        }

        private void UpdatePatrols(GameTime gameTime)
        {
            UpdateFixedPatrol(gameTime);

            if (state == Constants.PlatformState.Moving)
            {
                if (direction == Constants.DirectionX.Left)
                {
                    if (velocity.X > -speed)
                        velocity.X -= acceleration;
                    else
                        velocity.X = -speed;
                }
                else
                {
                    if (velocity.X < speed)
                        velocity.X += acceleration;
                    else
                        velocity.X = speed;
                }
            }
            else
            {
                if (velocity.X > acceleration)
                    velocity.X -= acceleration;
                else if (velocity.X < -acceleration)
                    velocity.X += acceleration;
                else
                    velocity.X = 0;
            }
        }
        private void UpdateFixedPatrol(GameTime gameTime)
        {
            waitTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            float brakepoint = (speed / acceleration) * 2;

            if (DistanceBetween((int)position.X, patrolPointA) <= brakepoint)
            {
                if (previousPoint != patrolPointA)
                {
                    previousPoint = patrolPointA;
                    waitTimer = 0;
                }

                nextPoint = patrolPointB;

                state = Constants.PlatformState.Stopped;
            }

            else if (DistanceBetween((int)position.X, patrolPointB) <= brakepoint)
            {
                if (previousPoint != patrolPointB)
                {
                    previousPoint = patrolPointB;
                    waitTimer = 0;
                }

                nextPoint = patrolPointA;

                state = Constants.PlatformState.Stopped;
            }

            if (waitTimer >= waitTime)
            {
                GoTo(nextPoint);
            }

        }
        private void GoTo(int point)
        {
            int position = (int)this.position.X;

            if (position < point)
            {
                direction = Constants.DirectionX.Right;
                state = Constants.PlatformState.Moving;
            }
            else if (position > point)
            {
                direction = Constants.DirectionX.Left;
                state = Constants.PlatformState.Moving;
            }
        }
        private int DistanceBetween(int a, int b)
        {
            int distance = a - b;
            return (distance < 0) ? distance * -1 : distance;
        }

        private void UpdateCollapsing(GameTime gameTime)
        {
            if (hasTriggered && state != Constants.PlatformState.Falling)
            {
                state = Constants.PlatformState.Falling;
            }
            else
            {
                tempVector.X = 0;
                tempVector.Y = -2;

                if (CollisionMap == null)
                {
                    ObjectCollisionEvent e = CollisionHandler.CollisionOccursWithObject(this, tempVector);
                }
                else
                {

                }

                if(hasBeenTouched)
                    waitTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (waitTimer > waitTime)
                    hasTriggered = true;
            }
        }
    }
}
