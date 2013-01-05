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
        private Character target;

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
        }

        public void Update(GameTime gameTime)
        {
            Move();
            position += velocity;
        }

        private void Move()
        {
            position = target.position;
        }

    }
}
