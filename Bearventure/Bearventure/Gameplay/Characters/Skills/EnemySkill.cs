using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bearventure.Gameplay.Characters.Skills
{
    public class EnemySkill : CharacterSkill
    {
        public List<Condition> Conditions = new List<Condition>();

        public EnemySkill(Character subject, Animation animation, int cooldown, int damage) : base(subject, animation, cooldown, damage) { }
    }
}
