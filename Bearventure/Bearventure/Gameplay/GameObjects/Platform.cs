using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Bearventure.Gameplay.Characters;
using Microsoft.Xna.Framework.Content;

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

        private float speed = 5f;

        public Platform(ContentManager content, Constants.PlatformType type, Vector2 position)
        {
            TAG = "Platform";
            this.type = type;
            this.position = position;
            IsActive = true;

            Initialize(content);
        }

        public void InitPatrol(int pointA, int pointB, float waitTime)
        {
            isPatrolling = true;

            patrolPointA = pointA;
            patrolPointB = pointB;

            nextPoint = pointA;

            acceleration = 0.5f;

            mass = -1;

            this.waitTime = waitTime;
        }

        private void Initialize(ContentManager content)
        {
            switch (type)
            {
                case Constants.PlatformType.Basic:
                    currentAnimation = new Animation(content.Load<Texture2D>(Constants.BasicPlatform), 0, 100, 25, 0, 1, 25);
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            currentAnimation.Animate(gameTime);

            this.position += velocity;

            if (isPatrolling)
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
    }
}
