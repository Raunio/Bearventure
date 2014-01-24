using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bearventure.Gameplay.Characters.Skills;
using Microsoft.Xna.Framework.Audio;

namespace Bearventure.Gameplay.Characters
{
    public abstract class Character : GameplayObject
    {
        #region Members
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
        /// Gets the distance to player.
        /// </summary>
        public Vector2 DistanceToPlayer
        {
            get;
            set;
        }
        /// <summary>
        /// Sets and gets the amount of force needed to knockback the character.
        /// </summary>
        public int KnockBackTreshold
        {
            get;
            protected set;
        }
        /// <summary>
        /// The maximum distance from where the characters sound effects can be heard relative to player.
        /// </summary>
        public float MaxSoundEffectDistance
        {
            get;
            set;
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

        public int CurrentSkillResource
        {
            get;
            set;
        }
        public int MaxSkillResource
        {
            get;
            set;
        }
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
                    case Constants.CharacterState.Climbing:
                        state = newState;
                        break;
                    case Constants.CharacterState.Running:
                        if (CharacterPhysics.OnGround(this) || Orientation == Constants.CharacterOrientation.Air)
                            state = newState;
                        else
                            return;
                        break;
                    case Constants.CharacterState.DoubleJump:
                        state = newState;
                        break;
                    case Constants.CharacterState.LatchedToObject:
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

        /// <summary>
        /// Plays a sound effect and calculates pan & volume values for it relative to player. 
        /// If the character is the player, then calculations will be ignored and volume and pan are set to 1.
        /// </summary>
        /// <param name="sound">The soundeffect</param>
        /// <param name="distance">distance to player</param>
        /// <param name="maxDistance">the maximum distance from where the sound can be heard</param>
        public void PlaySound(SoundEffect sound)
        {
            //float normDistance = DistanceToPlayer < 0 ? -DistanceToPlayer : DistanceToPlayer;
            float dist = DistanceToPlayer.Length() < 0 ? -DistanceToPlayer.Length() : DistanceToPlayer.Length();

            if (this.TAG != "Player" && dist < MaxSoundEffectDistance)
            {
                float pan = DistanceToPlayer.X / MaxSoundEffectDistance;
                float volume = 1 - dist / MaxSoundEffectDistance;

                SoundEffectInstance soundInstance = sound.CreateInstance();
                soundInstance.Pan = pan;
                soundInstance.Volume = volume;

                SoundEffectManager.Instance.PlaySound(soundInstance);
            }
            else if(this.TAG == "Player")
            {
                SoundEffectManager.Instance.PlaySound(sound);
            }
        }

        #endregion
        
        public PlayerIndex? ControllingPlayer
        {
            get { return controllingPlayer; }
            internal set { controllingPlayer = value; }
        }

        PlayerIndex? controllingPlayer;

        public virtual void HandleInput(GameTime gameTime, InputState input) { }
    }
}
