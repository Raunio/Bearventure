using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Bearventure.Engine.Effects;
using Bearventure.Gameplay.Characters;
using Bearventure.Gameplay.Characters.Skills;
using Bearventure.Engine.CollisionDetection;
using Bearventure.Gameplay.HUD;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bearventure.Gameplay
{
    public class CombatManager
    {
        private Vector2 mass;
        private Vector2 force;

        private static CombatTextManager textManager = new CombatTextManager();

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

        public void Initialize(Player player, List<Enemy> enemies, ContentManager content)
        {
            this.player = player;
            this.enemies = enemies;

            mass = new Vector2();
            force = new Vector2();

            CombatLog = new List<string>();

            textManager.Initialize(content);
        }

        public void UpdateFloatingTexts(GameTime gameTime)
        {
            textManager.Update(gameTime);
        }

        public void DrawFloatingTexts(SpriteBatch spriteBatch)
        {
            textManager.Draw(spriteBatch);
        }

        public void Update()
        {
            for(int i = 0; i < enemies.Count; i++)
            {
                if (player.ActiveSkill != null && player.state == Constants.CharacterState.UsingSkill && !player.ActiveSkill.HasDamaged && enemies[i].health > 0)
                {
                    if (player.ActiveSkill.HitsCharacter(enemies[i]))
                    {
                        
                        InflictDebuffs(enemies[i], player.ActiveSkill, player.directionX);
                        InflictDamage(enemies[i], player.ActiveSkill);

                        PlayHitEffects(new Vector2(player.ActiveSkill.HitBox.X + player.ActiveSkill.HitBox.Width / 2,
                                player.ActiveSkill.HitBox.Y + player.ActiveSkill.HitBox.Height / 2), player.ActiveSkill);


                        if (enemies[i].health <= 0)
                        {
                            CombatLog.Add(enemies[i].Name + " has died.");
                        }
                    }

                }

                if (enemies[i].ActiveSkill != null && enemies[i].state == Constants.CharacterState.UsingSkill && !enemies[i].ActiveSkill.HasDamaged && player.health > 0)
                {
                    if(enemies[i].ActiveSkill.HitsCharacter(player))
                    {
                        
                        InflictDebuffs(player, enemies[i].ActiveSkill, enemies[i].directionX);
                        InflictDamage(player, enemies[i].ActiveSkill);

                        PlayHitEffects(new Vector2(enemies[i].ActiveSkill.HitBox.X + enemies[i].ActiveSkill.HitBox.Width / 2,
                                enemies[i].ActiveSkill.HitBox.Y + enemies[i].ActiveSkill.HitBox.Height / 2), enemies[i].ActiveSkill); 
                    }
                }

            }

            if(player.ActiveSkill != null)
                if (!player.ActiveSkill.HasDamaged && player.ActiveSkill.HitsCharacter(player))
                {
                    AdjustResources(player, player.ActiveSkill);
                    InflictDamage(player, player.ActiveSkill);
                    InflictDebuffs(player, player.ActiveSkill, player.directionX);
                    ApplyHealing(player, player.ActiveSkill);

                    PlayHitEffects(new Vector2(player.ActiveSkill.HitBox.X + player.ActiveSkill.HitBox.Width / 2,
                           player.ActiveSkill.HitBox.Y + player.ActiveSkill.HitBox.Height / 2), player.ActiveSkill);
                }
        }

        private void PlayHitEffects(Vector2 position, CharacterSkill skill)
        {
            /*VisualEffectManager.Instance.CreateEffect("VisualEffects/hitTest", position, 100);

            if (Globals.GoreEnabled)
            {
                VisualEffectManager.Instance.CreateEffect("VisualEffects/blood2", position, 200);
            }*/

            if (skill.HitSoundEffect != null)
            {
                SoundEffectManager.Instance.PlaySoundFromPosition(position, skill.HitSoundEffect);
            }
        }

        private void AdjustResources(Character subject, CharacterSkill skill)
        {
            subject.CurrentSkillResource -= player.ActiveSkill.Cost;
        }

        private void ApplyHealing(Character subject, CharacterSkill skill)
        {
            subject.ApplyHealing(skill.Healing);
            
        }

        private void InflictDamage(Character subject, CharacterSkill skill)
        {
            float damage = skill.Damage;

            if (damage < skill.Damage)
            {
                CombatLog.Add(subject.Name + " resisted " + (int)(skill.Damage - damage) + " damage.");
            }
            else if (damage > skill.Damage)
            {
                CombatLog.Add("Critical hit!");
            }

            subject.TakeDamage(damage);

            textManager.AddText(new FloatingText("-" + damage, Constants.FloatingTextType.Normal, subject.Position - new Vector2(0, subject.BoundingBox.Height / 2)));
        }

        private void InflictDebuffs(Character subject, CharacterSkill skill, Constants.DirectionX direction)
        {
            //if (skill.InflictForce != Vector2.Zero)
            //{
                mass.X = skill.InflictForce.X > 0 ? subject.Mass * 0.033f : 0;
                mass.Y = skill.InflictForce.Y != 0 ? subject.Mass * 0.033f : 0;

                force.X = direction == Constants.DirectionX.Left ? -skill.InflictForce.X + mass.X : skill.InflictForce.X - mass.X;
                force.Y = skill.InflictForce.Y + mass.Y;

                subject.ChangeVelocity(force.X, force.Y, "Character Skill Force");

                //if(CollisionHandler.CollisionOccursWithMap(subject, force) != subject.BoundingBox.Top)
                    //subject.AdjustPosition(new Vector2(0, force.Y));

                if (skill.InflictForce.Length() >= subject.KnockBackTreshold)
                {
                    subject.state = Constants.CharacterState.Knocked;

                    //textManager.AddText(new FloatingText("Knocked!", Constants.FloatingTextType.Emphasised, subject.Position - new Vector2(0, subject.BoundingBox.Height / 2)));
                }

                if (subject.ActiveSkill != null && force.Length() >= subject.ActiveSkill.ForceInterruptTreshold && subject.ActiveSkill.ForceInterruptTreshold != -1)
                {
                    subject.ActiveSkill.Cancel();
                    CombatLog.Add(subject.Name + " was interrupted!");

                    textManager.AddText(new FloatingText("Interrupted!", Constants.FloatingTextType.Emphasised, subject.Position - new Vector2(0, subject.BoundingBox.Height / 2)));
                    subject.state = Constants.CharacterState.Knocked;
                }

            //}
        }
    }
}
