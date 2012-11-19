using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bearventure.Gameplay.Characters.Skills
{
    public class CombatManager
    {
        public static CombatManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CombatManager();
                }

                return instance;
            }
        }

        private static CombatManager instance;

        public List<String> CombatLog
        {
            get;
            private set;
        }

        private List<Enemy> enemies;
        private Player player;

        public void Initialize(Player player, List<Enemy> enemies)
        {
            this.player = player;
            this.enemies = enemies;
        }

        public void Update()
        {

        }
    }
}
