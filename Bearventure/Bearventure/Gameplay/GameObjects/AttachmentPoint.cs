using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bearventure.Gameplay.GameObjects
{
    public class AttachmentPoint
    {
        private Vector2 location;
        private GameplayObject subject;

        public bool IsOccupied
        {
            get
            {
                if (subject != null)
                    return true;
                return false;
            }
        }

        public Vector2 Location
        {
            get
            {
                return location;
            }
        }

        public GameplayObject Subject
        {
            get
            {
                return subject;
            }
        }

        public AttachmentPoint(Vector2 location)
        {
            this.location = location;
        }

        public void Attach(GameplayObject subject)
        {
            this.subject = subject;
        }

        public void Detach()
        {
            this.subject = null;
        }
    }
}
