using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bearventure.Gameplay.GameObjects.Characters.Skills.Modifiers
{
    public class PlayerSkillModifiers
    {
        public static SkillComboModifier SevenDragonsTestMod = new SkillComboModifier("A Force to be reckoned with.", "Increases the amount of force inflicted from the final strike of the combo.");

        public static void InitSkillModifiers()
        {
            SevenDragonsTestMod.ForceModifier = new Vector2(65, -15);
            SevenDragonsTestMod.ModifiedSkills = new int[1] { 2, };
        }
    }
}
