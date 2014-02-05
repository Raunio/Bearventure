using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bearventure.Gameplay.Characters.Skills
{
    public class EnemySkill : CharacterSkill
    {
        public List<Condition> Conditions
        {
            get;
            private set;
        }

        public EnemySkill(Character subject, CharacterAnimation animation, int cooldown, int damage, Constants.DamageType damageType) : base(subject, animation, cooldown, damage, damageType) 
        {
            Conditions = new List<Condition>();
        }

        public EnemySkill(Character subject, CharacterAnimation right, CharacterAnimation left, int cooldown, int damage, Constants.DamageType damageType)
            : base(subject, right, left, cooldown, damage, damageType)
        {
            Conditions = new List<Condition>();
        }
    }
}
