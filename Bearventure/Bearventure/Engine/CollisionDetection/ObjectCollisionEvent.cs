using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bearventure.Gameplay;

namespace Bearventure.Engine.CollisionDetection
{
    public class ObjectCollisionEvent
    {
        private GameplayObject a, b;
        private int collision_location;

        /// <summary>
        /// Returns the bounding box location of the collision.
        /// </summary>
        public int CollisionLocation
        {
            get
            {
                return collision_location;
            }
        }

        /// <summary>
        /// The object that starts the collision event.
        /// </summary>
        public GameplayObject A
        {
            get
            {
                return a;
            }
        }

        /// <summary>
        /// The object that A collides into.
        /// </summary>
        public GameplayObject B
        {
            get
            {
                return b;
            }
        }

        public ObjectCollisionEvent(GameplayObject a, GameplayObject b, int collision_location)
        {
            this.a = a;
            this.b = b;
            this.collision_location = collision_location;
        }

        public void ApplyPushForce()
        {

        }
    }
}
