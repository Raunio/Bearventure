using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Bearventure.Engine.Effects;

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

        public List<string> CombatLog
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

            CombatLog = new List<string>();
        }

        public void Update()
        {
            for(int i = 0; i < enemies.Count; i++)
            {
                if (player.state == Constants.CharacterState.UsingSkill && !player.ActiveSkill.HasDamaged && enemies[i].state != Constants.CharacterState.Dead)
                {
                    if (player.ActiveSkill.HitsCharacter(enemies[i]))
                    {
                        enemies[i].TakeDamage(player.ActiveSkill.Damage);

                        VisualEffectManager.Instance.CreateEffect("VisualEffects/Blood", 
                            new Vector2(player.ActiveSkill.HitBox.X + player.ActiveSkill.HitBox.Width / 2, 
                                player.ActiveSkill.HitBox.Y + player.ActiveSkill.HitBox.Height / 2), 500);

                        SoundEffectManager.Instance.PlaySound(Constants.Splat);

                        if(enemies[i].health <= 0 && enemies[i].state != Constants.CharacterState.Dead)
                            CombatLog.Add(enemies[i].Name + " has died.");
                    }
                }

                if (enemies[i].state == Constants.CharacterState.UsingSkill)
                {

                }

            }
        }
    }
}
