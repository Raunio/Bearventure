using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bearventure.Gameplay.GameObjects.Characters.Skills.Modifiers
{
    public class SkillModifier
    {
        private Vector2 forceModifier;
        public Vector2 ForceModifier
        {
            get
            {
                return forceModifier == null ? Vector2.Zero : forceModifier;
            }
            set
            {
                forceModifier = value;
            }
        }

        public int DamageModifier
        {
            get;
            set;
        }

        public String Description
        {
            get;
            set;
        }

        public String Name
        {
            get;
            set;
        }

        public SkillModifier(String name, String description)
        {
            Name = name;
            Description = description;
        }
    }
}
