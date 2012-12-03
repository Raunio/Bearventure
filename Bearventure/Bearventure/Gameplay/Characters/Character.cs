using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bearventure.Gameplay.Characters.Skills;

namespace Bearventure.Gameplay.Characters
{
    public abstract class Character
    {
        #region Members
        protected Texture2D spriteSheet;
        /// <summary>
        /// Character position.
        /// </summary>
        public Vector2 position;
        /// <summary>
        /// Character velocity.
        /// </summary>
        public Vector2 velocity;
        /// <summary>
        /// Current animation of character
        /// </summary>
        public Animation currentAnimation;
        protected float scale = 1f;
        /// <summary>
        /// Character walking speed.
        /// </summary>
        public float walkSpeed;
        /// <summary>
        /// Character running speed.
        /// </summary>
        public float runSpeed;
        /// <summary>
        /// Character acceleration.
        /// </summary>
        public float acceleration;
        /// <summary>
        /// Character brake strenght.
        /// </summary>
        public float decceleration;
        /// <summary>
        /// Character mass.
        /// </summary>
        public int mass;
        /// <summary>
        /// Represents the state of the character
        /// </summary>
        public Constants.CharacterState state;
        /// <summary>
        /// Left or right.
        /// </summary>
        public Constants.DirectionX directionX;
        /// <summary>
        /// Up or down
        /// </summary>
        public Constants.DirectionY directionY;
        /// <summary>
        /// Character orientation. Air or Ground.
        /// </summary>
        public Constants.CharacterOrientation Orientation
        {
            protected set;
            get;
        }
        /// <summary>
        /// Armor type of the character.
        /// </summary>
        public Constants.ArmorType ArmorType
        {
            protected set;
            get;
        }
        /// <summary>
        /// Character current health.
        /// </summary>
        public int health;
        /// <summary>
        /// Character maximun health.
        /// </summary>
        public int maxHealth;
        /// <summary>
        /// Character jump strenght.
        /// </summary>
        public int jumpStrenght;
        /// <summary>
        /// Character health regeneration per five seconds.
        /// </summary>
        public int healthRegen;
        private float regenTimer;
        private float damageTimer;
        protected int BoundingBoxOffset;
        /// <summary>
        /// Gets or sets the name of the character.
        /// </summary>
        public string Name
        {
            get
            {
                if (name == null)
                    return "Character";

                return name;
            }
            set
            {
                name = value;
            }
        }
        private string name;
        /// <summary>
        /// Color data used for collision
        /// </summary>
        public Color[] textureData
        {
            get
            {
                Color[] td =  new Color[currentAnimation.FrameWidth * currentAnimation.FrameHeight];
                currentAnimation.FrameTexture.GetData(td);

                return td;
            }
        }
        /// <summary>
        /// Gets the currently active skill of the character.
        /// </summary>
        public CharacterSkill ActiveSkill
        {
            get;
            protected set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Rectangle used primarily for collision detection.
        /// </summary>
        public Rectangle BoundingBox
        {
            get
            {
                int x = (int)(position.X - (currentAnimation.Origin.X * scale)) + BoundingBoxOffset;
                int y = (int)(position.Y - (currentAnimation.Origin.Y * scale)) + BoundingBoxOffset;
                int width = (int)(currentAnimation.FrameWidth * scale) - BoundingBoxOffset * 2;
                int height = (int)(currentAnimation.FrameHeight * scale) - BoundingBoxOffset;

                return new Rectangle(x, y, width, height);
            }
        }

        public bool IsDisabled
        {
            get
            {
                if (state == Constants.CharacterState.Disabled || state == Constants.CharacterState.Knocked || state == Constants.CharacterState.Stunned)
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // TODO: Chop this too -Huemac
            spriteBatch.Draw(currentAnimation.spriteSheet, position, currentAnimation.FrameRectangle, Color.White, currentAnimation.Rotation, currentAnimation.Origin, scale, currentAnimation.Effects, currentAnimation.LayerDepth);
        }

        public abstract void Update(GameTime gameTime);

        protected void RegenerateHealth(GameTime gameTime)
        {
            regenTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            damageTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (regenTimer >= 5f && health < maxHealth)
            {
                health += healthRegen;
                regenTimer = 0;

                if (health > maxHealth)
                    health = maxHealth;
            }
        }

        public void TakeDamage(float damage)
        {
            if (damageTimer > 300)
            {
                health -= (int)damage;
                damageTimer = 0;

                CombatManager.Instance.CombatLog.Add(this.Name + " took " + damage + " damage.");
            }
        }

        public void SetState(Constants.CharacterState newState)
        {
            if (state != Constants.CharacterState.UsingSkill && !IsDisabled)
            {
                switch (newState)
                {
                    case Constants.CharacterState.Walking:
                        if (CharacterPhysics.OnGround(this) || Orientation == Constants.CharacterOrientation.Air)
                            state = newState;
                        else
                            return;
                        break;
                    case Constants.CharacterState.Stopped:
                        if (CharacterPhysics.OnGround(this) || Orientation == Constants.CharacterOrientation.Air)
                            state = newState;
                        else
                            return;
                        break;
                    case Constants.CharacterState.Jumping:
                        state = newState;
                        break;
                    case Constants.CharacterState.UsingSkill:
                        state = newState;
                        break;
                }
            }
        }

        public void UseSkill(CharacterSkill skill)
        {
            if (skill.IsReady && !skill.IsActive)
            {
                state = Constants.CharacterState.UsingSkill;
                skill.Activate();
                ActiveSkill = skill; 
            }
        }

        protected void CleanActiveSkill()
        {
            if(ActiveSkill != null)
                if (!ActiveSkill.IsActive || state != Constants.CharacterState.UsingSkill)
                {
                    ActiveSkill.Cancel();
                    ActiveSkill = null;
                }
        }

        #endregion
    }
}
