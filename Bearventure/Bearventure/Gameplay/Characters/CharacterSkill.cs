
using Bearventure;
using Bearventure.Engine.Effects;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
namespace Bearventure
{
    public class CharacterSkill
    {
        // TODO: Invent a more descriptive name
        private float cdTimer;
        private Animation animation;
        private Character subject;
        private Character target;
        private bool isActive = false;
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

            if (isActive)
            {
                if (subject.velocity.X < UltimateVelocityX + Acceleration)
                    subject.velocity.X += Acceleration;
                else if (subject.velocity.X > UltimateVelocityX - Acceleration)
                    subject.velocity.X -= Acceleration;
                else
                    subject.velocity.X = UltimateVelocityX;

                if (animation.HasFinished)
                {
                    isActive = false;
                    subject.state = Constants.CharacterState.Stopped;
                }
            }
        }

        public void Activate()
        {
            cdTimer = 0;
            IsReady = false;
            isActive = true;
            animation.Reset();
            subject.currentAnimation = animation;
            subject.velocity = StartVelocity;
            foreach (VisualEffect e in effects)
                VisualEffectManager.Instance.CreateEffect(e, subject.position);
        }

        public void AddEffect(VisualEffect effect, Vector2 positionOffset)
        {
            effect.PositionOffset = positionOffset;
            effects.Add(effect);          
        }
    }
}
