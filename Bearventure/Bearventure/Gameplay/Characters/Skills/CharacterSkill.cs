using Bearventure;
using Bearventure.Engine.Effects;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bearventure.Gameplay.Characters.Skills
{
    public class CharacterSkill
    {
        // TODO: Invent a more descriptive name
        private float cdTimer;
        private Animation rightAnimation;
        private Animation leftAnimation;
        private Character subject;

        private List<string> effects = new List<string>();
        private List<Vector2> effectPositions = new List<Vector2>();
        private List<Vector2> effectPositionOffsets = new List<Vector2>();
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
            private set;
            get;
        }
        /// <summary>
        /// Gets the damage of the skill.
        /// </summary>
        public int Damage
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
        public string SoundEffectAsset
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
        private List<int> damagingFrames;
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
        private int hitBoxWidth;
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
        private int hitBoxHeight;
        /// <summary>
        /// Gets the hitbox of the skill.
        /// </summary>
        public Rectangle HitBox
        {
            get;
            private set;
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
        /// Setups the skill with 1 animation.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="animation"></param>
        /// <param name="cooldown"></param>
        /// <param name="damage"></param>
        /// <param name="DamageType"></param>
        public CharacterSkill(Character subject, Animation animation, int cooldown, int damage, Constants.DamageType DamageType)
        {
            this.subject = subject;
            this.rightAnimation = animation;
            this.Cooldown = cooldown;
            this.Damage = damage;
            this.IsReady = true;
            this.DamageType = DamageType;
        }
        /// <summary>
        /// Setups the skill with different animations for different directions.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="animation"></param>
        /// <param name="cooldown"></param>
        /// <param name="damage"></param>
        /// <param name="DamageType"></param>
        public CharacterSkill(Character subject, Animation right, Animation left, int cooldown, int damage, Constants.DamageType DamageType)
        {
            this.subject = subject;
            this.rightAnimation = right;
            this.leftAnimation = left;
            this.Cooldown = cooldown;
            this.Damage = damage;
            this.IsReady = true;
            this.DamageType = DamageType;
        }
        /// <summary>
        /// Returns true if the skills hitbox hits the characters BoundingBox.
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public bool HitsCharacter(Character character)
        {
            if (HitBox.Intersects(character.BoundingBox))
                HasDamaged = true;

            return HitBox.Intersects(character.BoundingBox);
        }

        public void Update(GameTime gameTime)
        {
            cdTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (cdTimer > Cooldown)
                IsReady = true;

            if (IsActive)
            {
                if (subject.velocity.X < UltimateVelocityX + Acceleration)
                    subject.velocity.X += Acceleration;
                else if (subject.velocity.X > UltimateVelocityX - Acceleration)
                    subject.velocity.X -= Acceleration;
                else
                    subject.velocity.X = UltimateVelocityX;

                for (int i = 0; i < DamagingFrames.Count; i++)
                {
                    if (rightAnimation.CurrentFrame == DamagingFrames[i] || (leftAnimation != null && leftAnimation.CurrentFrame == DamagingFrames[i]))
                    {
                        if (subject.directionX == Constants.DirectionX.Left)
                        {
                            HitBox = new Rectangle((int)subject.position.X - ((int)HitBoxPositions[i].X + HitBoxWidth),
                                (int)subject.position.Y + (int)HitBoxPositions[i].Y, HitBoxWidth, HitBoxHeight);
                        }
                        else
                        {
                            HitBox = new Rectangle((int)subject.position.X + (int)HitBoxPositions[i].X,
                                (int)subject.position.Y + (int)HitBoxPositions[i].Y, HitBoxWidth, HitBoxHeight);
                        }

                        break;
                    }

                    else
                        HitBox = new Rectangle(-1, -1, 1, 1);
                }

                if (rightAnimation.HasFinished)
                {
                    IsActive = false;
                    subject.state = Constants.CharacterState.Stopped;
                }
                if (leftAnimation != null && leftAnimation.HasFinished)
                {
                    IsActive = false;
                    subject.state = Constants.CharacterState.Stopped;
                }

                CombatManager.Instance.Update();
            }
        }
        /// <summary>
        /// Activates the skill.
        /// </summary>
        public void Activate()
        {
            cdTimer = 0;
            IsReady = false;
            IsActive = true;
            rightAnimation.Reset();

            if(leftAnimation != null)
                leftAnimation.Reset();

            
            subject.velocity += new Vector2(subject.directionX == Constants.DirectionX.Left ? -StartVelocity.X : StartVelocity.X, StartVelocity.Y);
            HasDamaged = false;

            if (subject.directionX == Constants.DirectionX.Left)
            {
                if (leftAnimation != null)
                {
                    subject.currentAnimation = leftAnimation;
                }
                else
                    subject.currentAnimation = rightAnimation;
            }
            else
                subject.currentAnimation = rightAnimation;


            for (int i = 0; i < effects.Count; i++)
            {
                VisualEffectManager.Instance.CreateEffect(effects[i], subject.position + effectPositionOffsets[i], 500);
            }

            if(SoundEffectAsset != null)
                SoundEffectManager.Instance.PlaySound(SoundEffectAsset);
        }

        public void AddEffect(string asset, Vector2 positionOffset)
        {
            effectPositionOffsets.Add(positionOffset);
            effects.Add(asset);          
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
        }
    }
}
