using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bearventure.Gameplay.GameObjects.Characters.Skills.Modifiers
{
    public class SkillComboModifier : SkillModifier
    {
        public SkillComboModifier(String name, String description) : base(name, description) { }

        public int[] ModifiedSkills
        {
            get;
            set;
        }
    }
}
