using Bearventure;
using Bearventure.Engine.Effects;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Bearventure.Engine.Audio;
using Bearventure.Gameplay.GameObjects.Characters.Skills.Modifiers;

namespace Bearventure.Gameplay.Characters.Skills
{
    public class CharacterSkill
    {
        #region Members
        // TODO: Invent a more descriptive name
        private float cdTimer;
        private CharacterAnimation rightAnimation;
        private CharacterAnimation leftAnimation;
        private CharacterAnimation currentAnimation;
        private Character subject;

        private List<CharacterSkillEffect> Effects = new List<CharacterSkillEffect>();
        private VisualEffect[] effectPointers;

        private struct CharacterSkillEffect
        {
            public string asset;
            public Vector2 positionOffset;
            public Constants.CharacterBodyPart bodyPartTarget;
            public int creationFrame;
        };


        private int hitBoxWidth;
        private int hitBoxHeight;
        private Rectangle hitBox;

        private List<int> damagingFrames;

        private bool effectsHaveActivated;
        private int frameOfActivation;

        #endregion
        #region Gets & Sets
        public float CooldownTimer
        {
            get
            {
                return cdTimer;
            }
        }
        /// <summary>
        /// Gets or sets the icon for the skill.
        /// </summary>
        public Texture2D Icon
        {
            get;
            set;
        }
        /// <summary>
        /// Gets the cooldown time of the skill in seconds.
        /// </summary>
        public float Cooldown
        {
            private set;
            get;
        }
        /// <summary>
        /// Gets the cost of the skill.
        /// </summary>
        public int Cost
        {
            set;
            get;
        }
        /// <summary>
        /// Gets the damage of the skill.
        /// </summary>
        public int Damage
        {
            set;
            get;
        }
        /// <summary>
        /// Gets the healing amount of the skill.
        /// </summary>
        public int Healing
        {
            private set;
            get;
        }
        /// <summary>
        /// Gets wether the skill is ready to use.
        /// </summary>
        public bool IsReady
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets wether the skill is currently active.
        /// </summary>
        public bool IsActive
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets or sets the initial velocity of the character who activates this skill.
        /// </summary>
        public Vector2 StartVelocity
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the initial rotation of the character who activates this skill.
        /// </summary>
        public float StartRotation
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the ultimate rotation of the character who activates this skill.
        /// </summary>
        public float UltimateRotation
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the speed used for rotating the character who activates this skill.
        /// </summary>
        public float RotationVelocity
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the ultimate velocity of the character who activates this skill.
        /// </summary>
        public float UltimateVelocityX
        {
            get;
            set;
        }
        /// <summary>
        /// Means of use depends on wether StartVelocity.X is greater than UltimateVelocityX or the other way around.
        /// </summary>
        public float Acceleration
        {
            get;
            set;
        }
        /// <summary>
        /// Do not initialize if no sound effect should be played upon activation of this skill.
        /// </summary>
        public SoundEffect ActivationSoundEffect
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the name of the skill.
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the lifetime of the visual effects activated by this skill (milliseconds).
        /// </summary>
        public int VisualEffectLifetime
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the description of the skill.
        /// </summary>
        public string Description
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the frames of this skills animation that CombatManager use;
        /// </summary>
        public List<int> DamagingFrames
        {
            get
            {
                return damagingFrames;
            }
            set
            {
                damagingFrames = value;
                HitBoxPositions = new Vector2[damagingFrames.Count];
            }
        }
        /// <summary>
        /// Gets or sets the frames of the animtaion of this skill that create given effects.
        /// </summary>
        public List<int> EffectActivationFrames
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the targeting type of this skill.
        /// </summary>
        public Constants.SkillTarget TargetingType
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets the force inflicted to the target of this skill upon hit.
        /// </summary>
        public Vector2 InflictForce
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the width of the skills hitbox.
        /// </summary>
        public int HitBoxWidth
        {
            get
            {
                return hitBoxWidth;
            }
            set
            {
                HitBox = new Rectangle(HitBox.X, HitBox.Y, value, HitBox.Height);
                hitBoxWidth = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the height of the skills hitbox.
        /// </summary>
        public int HitBoxHeight
        {
            get
            {
                return hitBoxHeight;
            }
            set
            {
                HitBox = new Rectangle(HitBox.X, HitBox.Y, HitBox.Width, value);
                hitBoxHeight = value;
            }
        }
        
        /// <summary>
        /// Gets the hitbox of the skill.
        /// </summary>
        public Rectangle HitBox
        {
            get
            {
                return hitBox;
            }
            private set
            {
                hitBox = value;
            }
        }
        /// <summary>
        /// Gets and sets the positions for the hitbox for each damaging frame.
        /// </summary>
        public Vector2[] HitBoxPositions
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets wether the skill has already damaged someone.
        /// </summary>
        public bool HasDamaged
        {
            get;
            private set;
        }

        public Constants.DamageType DamageType
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets and sets the treshold for the needed force to interrupt this skill.
        /// </summary>
        public int ForceInterruptTreshold
        {
            get;
            set;
        }
        /// <summary>
        /// Sets up a sound effect randomizer for random activation sound effects.
        /// </summary>
        public SoundEffectRandomizer ActivationSoundEffectRand
        {
            get;
            set;
        }
        /// <summary>
        /// Sound effect that is played when the skill "hits" succesfully
        /// </summary>
        public SoundEffect HitSoundEffect
        {
            get;
            set;
        }
        /// <summary>
        /// Gets a list of modifiers for the skill.
        /// </summary>
        public List<SkillModifier> Modifiers
        {
            get;
            private set;
        }
        #endregion
        /// <summary>
        /// Setups the skill with 1 animation.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="animation"></param>
        /// <param name="cooldown"></param>
        /// <param name="damage"></param>
        /// <param name="DamageType"></param>
        public CharacterSkill(Character subject, CharacterAnimation animation, int cooldown, int damage, Constants.DamageType DamageType)
        {
            this.subject = subject;
            this.rightAnimation = animation;
            this.Cooldown = cooldown;
            this.Damage = damage;
            this.IsReady = true;
            this.DamageType = DamageType;
            hitBox = new Rectangle(-1, -1, 1, 1);
            ForceInterruptTreshold = -1;
        }
        /// <summary>
        /// Setups the skill with different animations for different directions.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="animation"></param>
        /// <param name="cooldown"></param>
        /// <param name="damage"></param>
        /// <param name="DamageType"></param>
        public CharacterSkill(Character subject, CharacterAnimation right, CharacterAnimation left, int cooldown, int damage, Constants.DamageType DamageType)
        {
            this.subject = subject;
            this.rightAnimation = right;
            this.leftAnimation = left;
            this.Cooldown = cooldown;
            this.cdTimer = cooldown;
            this.Damage = damage;
            this.IsReady = true;
            this.DamageType = DamageType;
            hitBox = new Rectangle(-1, -1, 1, 1);
            ForceInterruptTreshold = -1;
        }
        public CharacterSkill(Character subject, CharacterAnimation right, CharacterAnimation left, int cooldown, int healing)
        {
            this.subject = subject;
            this.rightAnimation = right;
            this.leftAnimation = left;
            this.Cooldown = cooldown;
            this.cdTimer = cooldown;
            this.Healing = healing;
            this.IsReady = true;
            this.DamageType = DamageType;
            hitBox = new Rectangle(-1, -1, 1, 1);
            ForceInterruptTreshold = -1;
        }
        /// <summary>
        /// Returns true if the skills hitbox hits the characters BoundingBox.
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public bool HitsCharacter(Character character)
        {
            if (character != subject && (TargetingType == Constants.SkillTarget.Enemy || TargetingType == Constants.SkillTarget.Both))
            {
                if (HitBox.Intersects(character.BoundingBox))
                    HasDamaged = true;

                return HitBox.Intersects(character.BoundingBox);
            }
            else if (character == subject && (TargetingType == Constants.SkillTarget.Self || TargetingType == Constants.SkillTarget.Both))
            {
                if(DamagingFrames != null)
                    for (int i = 0; i < DamagingFrames.Count; i++)
                    {
                        if (currentAnimation.CurrentFrame == DamagingFrames[i])
                        {
                            HasDamaged = true;

                            return true;
                        }
                    }
            }

            return false;
        }

        public void Update(GameTime gameTime)
        {
            cdTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (cdTimer > Cooldown)
                IsReady = true;

            if (IsActive)
            {
                if (subject.velocity.X < UltimateVelocityX - Acceleration)
                    subject.velocity.X += Acceleration;
                else if (subject.velocity.X > UltimateVelocityX + Acceleration)
                    subject.velocity.X -= Acceleration;
                else
                    subject.velocity.X = UltimateVelocityX;

                float targetRotation = subject.directionX == Constants.DirectionX.Left ? -UltimateRotation : UltimateRotation;

                if (subject.SpriteRotation < targetRotation + RotationVelocity)
                    subject.SpriteRotation += RotationVelocity;
                else if (subject.SpriteRotation > targetRotation - RotationVelocity)
                    subject.SpriteRotation -= RotationVelocity;
                else
                    subject.SpriteRotation = UltimateRotation;

                if(DamagingFrames != null)
                    for (int i = 0; i < DamagingFrames.Count; i++)
                    {
                        if (currentAnimation.CurrentFrame == DamagingFrames[i])
                        {
                            if (subject.directionX == Constants.DirectionX.Left)
                            {
                                hitBox.X = (int)subject.Position.X - ((int)HitBoxPositions[i].X + HitBoxWidth);
                                hitBox.Y = (int)subject.Position.Y + (int)HitBoxPositions[i].Y;
                                hitBox.Width = HitBoxWidth;
                                hitBox.Height = HitBoxHeight;
                            }
                            else
                            {
                                hitBox.X = (int)subject.Position.X + (int)HitBoxPositions[i].X;
                                hitBox.Y = (int)subject.Position.Y + (int)HitBoxPositions[i].Y;
                                hitBox.Width = HitBoxWidth;
                                hitBox.Height = HitBoxHeight;
                            }

                            CombatManager.Instance.Update();

                            break;
                        }

                        else
                        {
                            hitBox.X = -1;
                            hitBox.Y = -1;
                            hitBox.Width = 1;
                            hitBox.Height = 1;
                        }
                    }

                if (currentAnimation.CurrentFrame != frameOfActivation)
                    effectsHaveActivated = false;

                if (!effectsHaveActivated)
                {
                    for (int i = 0; i < Effects.Count; i++)
                        if (currentAnimation.CurrentFrame == Effects[i].creationFrame)
                        {
                            Vector2 origin = new Vector2(subject.BoundingBox.X, subject.BoundingBox.Y);
                            Vector2 offset = Effects[i].positionOffset;

                            if (subject.directionX == Constants.DirectionX.Right)
                            {
                                VisualEffectManager.Instance.CreateEffect(Effects[i].asset, subject.Position + offset, VisualEffectLifetime == 0 ? 500 : VisualEffectLifetime);
                                effectPointers[i] = VisualEffectManager.Instance.GetTopEffect();
                            }
                            else
                            {
                                VisualEffectManager.Instance.CreateEffect(Effects[i].asset, subject.Position + new Vector2(-offset.X, offset.Y), VisualEffectLifetime == 0 ? 500 : VisualEffectLifetime);
                                effectPointers[i] = VisualEffectManager.Instance.GetTopEffect();
                            }

                            frameOfActivation = currentAnimation.CurrentFrame;


                            effectsHaveActivated = true;
                        }
                }

                if (currentAnimation.HasFinished)
                {
                    IsActive = false;
                    subject.state = Constants.CharacterState.Stopped;
                }

                UpdateEffectOffsets();
                
            }
        }
        /// <summary>
        /// Activates the skill.
        /// </summary>
        public void Activate()
        {
            if (!IsReady)
                return;

            cdTimer = 0;
            IsReady = false;
            IsActive = true;
            rightAnimation.Reset();

            if (effectPointers == null)
                effectPointers = new VisualEffect[Effects.Count];

            if(leftAnimation != null)
                leftAnimation.Reset();

            if (subject.directionX == Constants.DirectionX.Left)
            {  
                if (subject.velocity.X >= -subject.walkSpeed)
                    subject.velocity.X -= StartVelocity.X;
            }
            else if(subject.directionX == Constants.DirectionX.Right)
            {
                if (subject.velocity.X <= subject.walkSpeed)
                    subject.velocity.X += StartVelocity.X;
            }

            subject.velocity.Y += StartVelocity.Y;
            

            HasDamaged = false;

            if (subject.directionX == Constants.DirectionX.Left)
            {
                if (leftAnimation != null)
                {
                    currentAnimation = leftAnimation;
                }
                else
                    currentAnimation = rightAnimation;
            }
            else
                currentAnimation = rightAnimation;

            subject.ChangeAnimation(currentAnimation);

            subject.SpriteRotation = subject.directionX == Constants.DirectionX.Left ? -StartRotation : StartRotation;

            if (ActivationSoundEffect != null)
            {
                SoundEffectManager.Instance.PlaySoundFromPosition(subject.Position, ActivationSoundEffect);
            }

            if (ActivationSoundEffectRand != null)
            {
                SoundEffect s = ActivationSoundEffectRand.GetRandomSound(true);
                if (s != null)
                    SoundEffectManager.Instance.PlaySoundFromPosition(subject.Position, s);
            }
        }

        public void AddEffect(string asset, Vector2 positionOffset, int creationFrame)
        {
            CharacterSkillEffect effect = new CharacterSkillEffect();
            effect.asset = asset;
            effect.positionOffset = positionOffset;
            effect.creationFrame = creationFrame;

            Effects.Add(effect);

        }
        public void AddEffect(string asset, Constants.CharacterBodyPart bodyPart, int creationFrame)
        {
            CharacterSkillEffect effect = new CharacterSkillEffect();
            effect.asset = asset;
            effect.bodyPartTarget = bodyPart;
            effect.creationFrame = creationFrame;

            Effects.Add(effect);
        }

        private void UpdateEffectOffsets()
        {
            for (int i = 0; i < effectPointers.Length; i++)
            {
                Vector2 target = Vector2.Zero;
                if(effectPointers[i] != null)
                    switch (Effects[i].bodyPartTarget)
                    {
                        case Constants.CharacterBodyPart.Belly:
                            target = currentAnimation.GetAnatomicInfo().Belly;
                            break;
                        case Constants.CharacterBodyPart.Groin:
                            target = currentAnimation.GetAnatomicInfo().Groin;
                            break;
                        case Constants.CharacterBodyPart.LeftEye:
                            target = currentAnimation.GetAnatomicInfo().LeftEye;
                            break;
                        case Constants.CharacterBodyPart.LeftFoot:
                            target = currentAnimation.GetAnatomicInfo().LeftFoot;
                            break;
                        case Constants.CharacterBodyPart.LeftHand:
                            target = currentAnimation.GetAnatomicInfo().LeftHand;
                            break;
                        case Constants.CharacterBodyPart.Mouth:
                            target = currentAnimation.GetAnatomicInfo().Mouth;
                            break;
                        case Constants.CharacterBodyPart.RightEye:
                            target = currentAnimation.GetAnatomicInfo().RightEye;
                            break;
                        case Constants.CharacterBodyPart.RightFoot:
                            target = currentAnimation.GetAnatomicInfo().RightFoot;
                            break;
                        case Constants.CharacterBodyPart.RightHand:
                            target = currentAnimation.GetAnatomicInfo().RightHand;
                            break;
                    }

                if (target != Vector2.Zero)
                {
                    effectPointers[i].Position = new Vector2(subject.Position.X - currentAnimation.Origin.X + target.X, subject.Position.Y - currentAnimation.Origin.Y + target.Y);
                }
            }
        }
        /// <summary>
        /// Draw hitbox. Used for testing.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="color"></param>
        public void DrawHitBox(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, HitBox, Color.White);
        }

        public void Cancel()
        {
            IsActive = false;
            subject.state = Constants.CharacterState.Stopped;
        }

        public void AddModifier(SkillModifier mod)
        {
            Modifiers.Add(mod);

            InflictForce += mod.ForceModifier;

            Damage += mod.DamageModifier;
        }
    }
}
