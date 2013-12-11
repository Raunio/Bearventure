using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Bearventure.Engine.Effects;
using Bearventure.Gameplay.Characters;
using Bearventure.Gameplay.Characters.Skills;

namespace Bearventure.Gameplay
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
                if (player.ActiveSkill != null && player.state == Constants.CharacterState.UsingSkill && !player.ActiveSkill.HasDamaged && enemies[i].health > 0)
                {
                    if (player.ActiveSkill.HitsCharacter(enemies[i]))
                    {
                        InflictDamage(enemies[i], player.ActiveSkill);
                        InflictDebuffs(enemies[i], player.ActiveSkill, player.directionX);

                        PlayHitEffects(new Vector2(player.ActiveSkill.HitBox.X + player.ActiveSkill.HitBox.Width / 2,
                                player.ActiveSkill.HitBox.Y + player.ActiveSkill.HitBox.Height / 2), player.ActiveSkill.DamageType, enemies[i].ArmorType);               


                        if(enemies[i].health <= 0)
                            CombatLog.Add(enemies[i].Name + " has died.");
                    }

                }

                if (enemies[i].ActiveSkill != null && enemies[i].state == Constants.CharacterState.UsingSkill && !enemies[i].ActiveSkill.HasDamaged && player.health > 0)
                {
                    if(enemies[i].ActiveSkill.HitsCharacter(player))
                    {
                        InflictDamage(player, enemies[i].ActiveSkill);
                        InflictDebuffs(player, enemies[i].ActiveSkill, enemies[i].directionX);

                        PlayHitEffects(new Vector2(enemies[i].ActiveSkill.HitBox.X + enemies[i].ActiveSkill.HitBox.Width / 2,
                                enemies[i].ActiveSkill.HitBox.Y + enemies[i].ActiveSkill.HitBox.Height / 2), enemies[i].ActiveSkill.DamageType, player.ArmorType); 
                    }
                }

            }

            if(player.ActiveSkill != null)
                if (player.ActiveSkill.HitsCharacter(player))
                {
                    InflictDamage(player, player.ActiveSkill);
                    InflictDebuffs(player, player.ActiveSkill, player.directionX);

                    PlayHitEffects(new Vector2(player.ActiveSkill.HitBox.X + player.ActiveSkill.HitBox.Width / 2,
                            player.ActiveSkill.HitBox.Y + player.ActiveSkill.HitBox.Height / 2), player.ActiveSkill.DamageType, player.ArmorType);
                }
        }

        private void PlayHitEffects(Vector2 position, Constants.DamageType damageType, Constants.ArmorType armorType)
        {
            //VisualEffectManager.Instance.CreateEffect("VisualEffects/hitTest", position, 100);

            if (Globals.GoreEnabled)
            {
                if (armorType == Constants.ArmorType.Fur || armorType == Constants.ArmorType.Feathers || armorType == Constants.ArmorType.Skin)
                {
                    VisualEffectManager.Instance.CreateEffect("VisualEffects/blood2", position, 200);
                    SoundEffectManager.Instance.PlaySound(Constants.Splat);
                }
            }

            if (damageType == Constants.DamageType.Crushing)
            {
                if (armorType == Constants.ArmorType.Fur)
                {
                    SoundEffectManager.Instance.PlaySound(Constants.Crush);
                }
            }
        }

        private void InflictDamage(Character subject, CharacterSkill skill)
        {
            float damage = skill.Damage;

            float weakness = 1.5f * (float)Globals.DifficultyFactor;
            float minorWeakness = 1.25f * (float)Globals.DifficultyFactor;
            float resistance = 0.5f / (float)Globals.DifficultyFactor;
            float minorResistance = 0.75f / (float)Globals.DifficultyFactor;

            switch (subject.ArmorType)
            {
                case Constants.ArmorType.Energy:
                    if (skill.DamageType != Constants.DamageType.NegativeEnergy)
                        damage = 0;
                    break;

                case Constants.ArmorType.Feathers:
                    if (skill.DamageType == Constants.DamageType.Fire)
                        damage *= minorWeakness;
                    break;

                case Constants.ArmorType.Force:
                    if (skill.DamageType == Constants.DamageType.Energy || skill.DamageType == Constants.DamageType.NegativeEnergy)
                        damage *= minorWeakness;
                    else if (skill.DamageType != Constants.DamageType.Force)
                        damage *= minorResistance;
                    break;

                case Constants.ArmorType.Fur:
                    if (skill.DamageType == Constants.DamageType.Fire)
                        damage *= weakness;
                    else if (skill.DamageType == Constants.DamageType.Cold)
                        damage *= minorResistance;
                    break;
                case Constants.ArmorType.Leather:
                    if (skill.DamageType == Constants.DamageType.Crushing || skill.DamageType == Constants.DamageType.Piercing || skill.DamageType == Constants.DamageType.Slashing)
                        damage *= minorResistance;
                    break;
                case Constants.ArmorType.Metal:
                    if (skill.DamageType == Constants.DamageType.Crushing || skill.DamageType == Constants.DamageType.Piercing || skill.DamageType == Constants.DamageType.Slashing)
                        damage *= resistance;
                    break;
                case Constants.ArmorType.NegativeEnergy:
                    if (skill.DamageType != Constants.DamageType.Energy)
                        damage = 0;
                    break;
                case Constants.ArmorType.Rock:
                    if (skill.DamageType == Constants.DamageType.Piercing || skill.DamageType == Constants.DamageType.Slashing || skill.DamageType == Constants.DamageType.Fire || skill.DamageType == Constants.DamageType.Cold)
                        damage *= resistance;
                    break;
            }
            if(damage < skill.Damage)
                CombatLog.Add(subject.Name + " resisted " + (int)(skill.Damage - damage) + " damage.");
            else if(damage > skill.Damage)
                CombatLog.Add("Critical hit!");
            subject.TakeDamage(damage);
        }

        private void InflictDebuffs(Character subject, CharacterSkill skill, Constants.DirectionX direction)
        {
            if (skill.InflictForce != Vector2.Zero)
            {
                Vector2 mass = new Vector2(skill.InflictForce.X > 0 ? subject.Mass * 0.033f : 0, skill.InflictForce.Y != 0 ? subject.Mass * 0.033f : 0);
                Vector2 force = new Vector2(direction == Constants.DirectionX.Left ? -skill.InflictForce.X + mass.X : skill.InflictForce.X - mass.X, skill.InflictForce.Y + mass.Y);
                subject.velocity = force;
                subject.position += force;


                if (subject.ActiveSkill != null && force.Length() > subject.ActiveSkill.ForceInterruptTreshold && subject.ActiveSkill.ForceInterruptTreshold != 0)
                {
                    subject.ActiveSkill.Cancel();
                    CombatLog.Add(subject.Name + " was interrupted!");
                }

            }
        }
    }
}
