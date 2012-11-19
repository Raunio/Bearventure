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
        private Animation animation;
        private Character subject;
        private Character target;
        private bool mobile;

        private List<VisualEffect> effects = new List<VisualEffect>();
        private List<Vector2> effectPositions = new List<Vector2>();
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

        public CharacterSkill(Character subject, Animation animation, int cooldown, int damage)
        {
            this.subject = subject;
            this.animation = animation;
            this.Cooldown = cooldown;
            this.Damage = damage;
            this.IsReady = true;
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
                    if (animation.CurrentFrame == DamagingFrames[i])
                    {
                        HitBox = new Rectangle((int)subject.position.X + (int)HitBoxPositions[i].X, (int)subject.position.Y + (int)HitBoxPositions[i].Y, HitBoxWidth, HitBoxHeight);
                    }
                    else
                        HitBox = new Rectangle(-1, -1, 1, 1);
                }

                if (animation.HasFinished)
                {
                    IsActive = false;
                    subject.state = Constants.CharacterState.Stopped;
                }
            }
        }

        public void Activate()
        {
            cdTimer = 0;
            IsReady = false;
            IsActive = true;
            animation.Reset();
            subject.currentAnimation = animation;
            subject.velocity = StartVelocity;
            foreach (VisualEffect e in effects)
                VisualEffectManager.Instance.CreateEffect(e, subject.position);

            if(SoundEffectAsset != null)
                SoundEffectManager.Instance.PlaySound(SoundEffectAsset);
        }

        public void AddEffect(VisualEffect effect, Vector2 positionOffset)
        {
            effect.PositionOffset = positionOffset;
            effects.Add(effect);          
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
    }
}
