using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Bearventure.Gameplay.Characters;

namespace Bearventure.Gameplay
{
    /// <summary>
    /// The camera controller class is used for maneuvering the camera of the game.
    /// </summary>
    class CameraController
    {
        /// <summary>
        /// Gets the position of the camera controller object
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return position;   
            }
        }

        private Vector2 position;
        private Vector2 velocity;
        private Vector2 speed_multiplier;
        private Vector2 minimum_speed;
        private Vector2 speed_margin;

        private Character target;

        private Vector2 offset;

        public CameraController()
        {

        }
        /// <summary>
        /// Assings the camera controller object to follow a character type object.
        /// </summary>
        /// <param name="subject"></param>
        public void AssingTo(Character subject)
        {
            target = subject;
            position = subject.position;
            velocity = target.velocity;

            offset.X = 300;
            offset.Y = 200;

            speed_margin = new Vector2(0, subject.jumpStrenght + 1);

            speed_multiplier = new Vector2(0.75f, 0.75f);
            minimum_speed = new Vector2(0, 5);
        }

        public void Update(GameTime gameTime)
        {
            Move();
            position += velocity;
        }

        private void Move()
        {
            Vector2 speed;

            speed.X = target.velocity.X * speed_multiplier.X;
            speed.Y = target.velocity.Y * speed_multiplier.Y;

            if (speed.X < 0)
                speed.X *= -1;
            if (speed.Y < 0)
                speed.Y *= -1;

            if (speed.X < minimum_speed.X)
                speed.X = minimum_speed.X;
            if (speed.Y < minimum_speed.Y)
                speed.Y = minimum_speed.Y;

            if (target.direction == Constants.Direction.Left)
            {
                if (position.X > target.position.X - offset.X)
                {
                    velocity.X = target.velocity.X - speed.X;
                }
                else
                    velocity.X = target.velocity.X;
            }
            else if (target.direction == Constants.Direction.Right)
            {
                if (position.X < target.position.X + offset.X)
                {
                    velocity.X = target.velocity.X + speed.X;
                }
                else
                    velocity.X = target.velocity.X;
            }

            if (target.velocity.Y < -speed_margin.Y)
            {
                if (position.Y > target.position.Y - offset.Y)
                {
                    velocity.Y = target.velocity.Y - speed.Y;
                }
                else
                    velocity.Y = target.velocity.Y;
            }

            else if (target.velocity.Y > speed_margin.Y)
            {
                if (position.Y < target.position.Y + offset.Y)
                {
                    velocity.Y = target.velocity.Y + speed.Y;
                }
                else
                    velocity.Y = target.velocity.Y;
            }

            else
            {
                if (position.Y < target.position.Y - speed.Y)
                {
                    velocity.Y = speed.Y;
                }
                else if (position.Y > target.position.Y + speed.Y)
                {
                    velocity.Y = -speed.Y;
                }
                else
                {
                    velocity.Y = 0;
                }
            }
        }

    }
}
