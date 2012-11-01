using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Characters
{
    public class CharacterSkill
    {
        private float cd_timer;

        public float Cooldown
        {
            private set;
            get;
        }

        public int Cost
        {
            private set;
            get;
        }
    }
}
