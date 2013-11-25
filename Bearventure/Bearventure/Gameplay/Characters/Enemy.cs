using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bearventure.Engine.Effects;
using System.Collections.Generic;
using Bearventure.Gameplay.Characters.Skills;

namespace Bearventure.Gameplay.Characters
{
    //Initialization, members, gets & sets
    public partial class Enemy : Character
    {
        #region Members

        private Character player;
        private Constants.EnemyType type;

        #endregion

        #region Gets and Sets
        /// <summary>
        /// Gets the Reaction time of the enemy in milliseconds.
        /// </summary>
        public float ReactSpeed
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the list of skills the enemy has.
        /// </summary>
        public List<EnemySkill> Skills
        {
            get;
            private set;
        }
        /// <summary>
        /// Enemy combat behaviour
        /// </summary>
        public Constants.CombatBehaviour combatBehaviour
        {
            private set;
            get;
        }
        /// <summary>
        /// Enemy behaviour
        /// </summary>
        public Behaviour behaviour
        {
            private set;
            get;
        }

        /// <summary>
        /// Range of vision.
        /// </summary>
        public int Vision
        {
            get;
            private set;
        }

        /// <summary>
        /// Enemy attack range.
        /// </summary>
        public int AttackRange
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the basic attack skill of the enemy.
        /// </summary>
        public EnemySkill Attack
        {
            get;
            private set;
        }

        #region Animations

        public Animation WalkLeft
        {
            private set;
            get;
        }
        public Animation WalkRight
        {
            private set;
            get;
        }
        public Animation Stopped
        {
            private set;
            get;
        }
        public Animation Jumping
        {
            private set;
            get;
        }
        public Animation RunLeft
        {
            private set;
            get;
        }
        public Animation RunRight
        {
            private set;
            get;
        }
        /// <summary>
        /// Must not be a looping animation.
        /// </summary>
        public Animation Dying
        {
            get;
            private set;
        }
        #endregion

        #endregion

        #region Initialization

        public Enemy(Constants.EnemyType type, Vector2 position, Texture2D spriteSheet, Player player)
        {
            TAG = "Enemy";
            this.type = type;
            this.position = position;
            this.spriteSheet = spriteSheet;

            InitAnimations();
            InitStats();
            InitSkills();
            this.player = player;
            InitBehaviour(player, 0, 0);
        }

        public Enemy(Constants.EnemyType type, int x, int y, Texture2D spriteSheet, Player player, int patrol_A, int patrol_B)
        {
            TAG = "Enemy";
            this.type = type;
            this.position = new Vector2(x, y);
            this.spriteSheet = spriteSheet;
            this.player = player;
            InitAnimations();
            InitStats();
            InitSkills();
            InitBehaviour(player, patrol_A, patrol_B);
        }
        /// <summary>
        /// Initialize all the animations.
        /// </summary>
        private void InitAnimations()
        {
            switch (type)
            {
                case Constants.EnemyType.BlackMetalBadger:
                    WalkRight = new Animation(spriteSheet, 0, 93, 103, 0, 7, 60);
                    WalkLeft = new Animation(spriteSheet, 0, 93, 103, 0, 7, 60, true, true);
                    RunRight = new Animation(spriteSheet, 0, 93, 103, 0, 7, 40);
                    RunLeft = new Animation(spriteSheet, 0, 93, 103, 0, 7, 40);
                    Stopped = new Animation(spriteSheet, 0, 93, 103, 4, 4, 60);
                    Jumping = new Animation(spriteSheet, 0, 93, 103, 5, 6, 100);
                    Dying = new Animation(spriteSheet, 0, 93, 103, 7, 7, 80, false, false);
                    break;
                case Constants.EnemyType.DelayOwl:
                    WalkRight = new Animation(spriteSheet, 0, 128, 90, 8, 11, 70);
                    WalkLeft = new Animation(spriteSheet, 0, 128, 90, 0, 3, 70, true, true);
                    RunRight = new Animation(spriteSheet, 0, 128, 90, 8, 11, 50);
                    RunLeft = new Animation(spriteSheet, 0, 128, 90, 0, 3, 50, true, true);
                    Stopped = new Animation(spriteSheet, 0, 128, 90, 5, 5, 70);
                    Jumping = new Animation(spriteSheet, 0, 128, 90, 0, 0, 70);
                    Dying = new Animation(spriteSheet, 0, 128, 90, 5, 5, 100, false, false);
                    break;
            }

            currentAnimation = Stopped;
        }
        /// <summary>
        /// Initialize "AI"
        /// </summary>
        /// <param name="player">Pointer to player required for AI Initialization</param>
        /// <param name="pointA">Patrol point A</param>
        /// <param name="pointB">PAtrol point B</param>
        private void InitBehaviour(Player player, int pointA, int pointB)
        {
            switch (type)
            {
                case Constants.EnemyType.BlackMetalBadger:
                    combatBehaviour = Constants.CombatBehaviour.Default;
                    behaviour = new Behaviour(this, player);
                    behaviour.InitFixedPatrol(pointA, pointB);
                    behaviour.WaitTime = 2000;
                    break;

                case Constants.EnemyType.DelayOwl:
                    combatBehaviour = Constants.CombatBehaviour.AttackAndFlee;
                    behaviour = new Behaviour(this, player);
                    behaviour.InitPassive();
                    break;
            }
        }
        /// <summary>
        /// Initialize the stats (health etc.)
        /// </summary>
        private void InitStats()
        {
            IsActive = true;

            switch (type)
            {
                case Constants.EnemyType.BlackMetalBadger:
                    walkSpeed = 5f;
                    runSpeed = 7f;
                    acceleration = 1f;
                    decceleration = 0.5f;
                    jumpStrenght = 15;
                    Orientation = Constants.CharacterOrientation.Ground;
                    Vision = 300;
                    AttackRange = 120;
                    health = 50;
                    maxHealth = 50;
                    healthRegen = 2;
                    BoundingBoxOffset = 10;
                    ReactSpeed = 250f;
                    mass = 100;
                    ArmorType = Constants.ArmorType.Fur;
                    break;

                case Constants.EnemyType.DelayOwl:
                    walkSpeed = 7f;
                    runSpeed = 9f;
                    acceleration = 0.25f;
                    decceleration = 0.25f;
                    jumpStrenght = 1;
                    Orientation = Constants.CharacterOrientation.Air;
                    Vision = 700;
                    AttackRange = 110;
                    health = 1;
                    maxHealth = 1;
                    healthRegen = 1;
                    mass = 0;
                    ArmorType = Constants.ArmorType.Feathers;
                    break;
            }
        }
        /// <summary>
        /// Initialize the skills.
        /// </summary>
        private void InitSkills()
        {
            switch (type)
            {
                case Constants.EnemyType.BlackMetalBadger:
                    #region TESTING
                    Animation testSkillAnimation = new Animation(spriteSheet, 0, 93, 103, 0, 4, 20, false, false);
                    testSkillAnimation.FreezeFrames = new Animation.FrameFreezer
                    {
                        Frames = new List<int> { 3, 4 },
                        Amount = 200
                    };

                    EnemySkill testSkill = new EnemySkill(this, testSkillAnimation, 8000, 2, Constants.DamageType.Crushing);
                    testSkill.Acceleration = 0.25f;
                    testSkill.StartVelocity = new Vector2(25, 0);
                    testSkill.UltimateVelocityX = 90;
                    testSkill.SoundEffectAsset = Constants.BadgerSkill;
                    testSkill.Conditions.Add(new Condition(Constants.ConditionType.DistanceToPlayerLowerThan, Vision));
                    testSkill.Conditions.Add(new Condition(Constants.ConditionType.DistanceToPlayerGreaterThan, AttackRange));
                    testSkill.Conditions.Add(new Condition(Constants.ConditionType.FacingPlayer, true));
                    testSkill.DamagingFrames = new List<int>
                    {
                        0,
                        1,
                        2,
                        3,
                        4,
                    };

                    for(int i = 0; i < 4; i++)
                        testSkill.HitBoxPositions[i] = new Vector2(40, -BoundingBox.Height / 2);
                   
                    testSkill.HitBoxHeight = BoundingBox.Height;
                    testSkill.HitBoxWidth = 20;
                    testSkill.InflictForce = new Vector2(5, 0);

                    Animation attack_right = new Animation(spriteSheet, 1, 149, 105, 0, 5, 30, false, false);
                    Animation attack_left = new Animation(spriteSheet, 1, 149, 105, 0, 5, 30, SpriteEffects.FlipHorizontally, 0f, 0f, false, false);

                    Attack = new EnemySkill(this, attack_right, attack_left, 1400, 4, Constants.DamageType.Crushing);
                    Attack.StartVelocity = new Vector2(5, 0);
                    Attack.Conditions.Add(new Condition(Constants.ConditionType.DistanceToPlayerLowerThan, AttackRange));
                    Attack.Conditions.Add(new Condition(Constants.ConditionType.FacingPlayer, true));
                    Attack.SoundEffectAsset = Constants.BadgerAttack;
                    Attack.DamagingFrames = new List<int>
                    {
                        2,
                        3,
                        4,
                    };

                    Attack.HitBoxPositions[0] = new Vector2(55, -10);
                    Attack.HitBoxPositions[1] = new Vector2(55, -10);
                    Attack.HitBoxHeight = 25;
                    Attack.HitBoxWidth = 25;

                    Attack.InflictForce = new Vector2(15, 0);
                    Attack.ForceInterruptTreshold = 5;

                    Skills = new List<EnemySkill>();
                    Skills.Add(testSkill);
                    Skills.Add(Attack);
                    #endregion
                    break;
                case Constants.EnemyType.DelayOwl:
                    #region TESTING
                    Attack = new EnemySkill(this, new Animation(spriteSheet, 0, 126, 88, 8, 9, 100, false, false),
                        5000, 5, Constants.DamageType.Piercing);
                    Attack.Conditions.Add(new Condition(Constants.ConditionType.DistanceToPlayerLowerThan, AttackRange));
                    Attack.DamagingFrames = new List<int>();

                    Skills = new List<EnemySkill>();
                    Skills.Add(Attack);
                    break;
                    #endregion
            }
        }

        #endregion

    }
    //Updates and other methods
    public partial class Enemy : Character
    {
        #region Updates

        private void UpdateSkills(GameTime gameTime)
        {
            if (Skills != null)
                foreach (EnemySkill s in Skills)
                    s.Update(gameTime);
        }
        /// <summary>
        /// Main update method.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (state != Constants.CharacterState.Dead)
            {
                if (!IsDisabled)
                {
                    behaviour.Apply(gameTime);
                }

                HandleAnimations(gameTime);
                UpdateSkills(gameTime);
                CleanActiveSkill();
                RegenerateHealth(gameTime);

                if (health <= 0)
                    Kill();
            }

            if (state == Constants.CharacterState.Disabled)
            {
                if (currentAnimation == Dying)
                {
                    if (currentAnimation.HasFinished)
                    {
                        velocity.X = 0;
                        state = Constants.CharacterState.Dead;
                        IsActive = false;
                    }
                }
            }

            CharacterPhysics.Apply(this, gameTime);
        }
        /// <summary>
        /// Should be called in update. Reads the enemys state and changes its animations accordingly.
        /// </summary>
        /// <param name="gameTime"></param>
        private void HandleAnimations(GameTime gameTime)
        {
            switch (state)
            {
                case Constants.CharacterState.Walking:
                    if (directionX == Constants.DirectionX.Left) { ChangeAnimation(WalkLeft); }
                    else { ChangeAnimation(WalkRight); }
                    break;

                case Constants.CharacterState.Running:
                    if (directionX == Constants.DirectionX.Left) { ChangeAnimation(RunLeft); }
                    else { ChangeAnimation(RunRight); }
                    break;

                case Constants.CharacterState.Stopped:
                    ChangeAnimation(Stopped);
                    break;

                case Constants.CharacterState.Jumping:
                    ChangeAnimation(Jumping);
                    break;
            }

            currentAnimation.Animate(gameTime);
        }

        #endregion

        private void ChangeAnimation(Animation animation)
        {
            if (currentAnimation != animation)
            {
                currentAnimation.Reset();
                currentAnimation = animation;
            }
        }
        /// <summary>
        /// Kill the enemy.
        /// </summary>
        public void Kill()
        {
            if (state != Constants.CharacterState.Disabled || state != Constants.CharacterState.Knocked)
                state = Constants.CharacterState.Disabled;

            ChangeAnimation(Dying);
        }
    }
}
