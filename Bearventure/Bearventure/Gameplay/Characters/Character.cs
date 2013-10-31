using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bearventure.Gameplay.Characters.Skills;

namespace Bearventure.Gameplay.Characters
{
    public abstract class Character : GameplayObject
    {
        #region Members

        /// <summary>
        /// Character mass.
        /// </summary>
        public int mass;
        /// <summary>
        /// Represents the state of the character
        /// </summary>
        public Constants.CharacterState state;

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
        /// Returns true if character is in any way disabled.
        /// </summary>
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
        /// <summary>
        /// Apply damage to character.
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(float damage)
        {
            if (damageTimer > 300)
            {
                health -= (int)damage;
                damageTimer = 0;

                CombatManager.Instance.CombatLog.Add(this.Name + " took " + damage + " damage.");
            }
        }
        /// <summary>
        /// Change the characters state.
        /// </summary>
        /// <param name="newState"></param>
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
        /// <summary>
        /// Try to activate a skill. Will not activate if the skill is not ready or it is already active.
        /// </summary>
        /// <param name="skill"></param>
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
