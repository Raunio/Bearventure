using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bearventure.Engine.Effects;
using System.Collections.Generic;
using Bearventure.Gameplay.Characters.Skills;

namespace Bearventure.Gameplay.Characters
{
    public class Enemy : Character
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
            this.type = type;
            this.position = position;
            this.spriteSheet = spriteSheet;

            InitAnimations();
            InitStats();
            InitBehavirour(player, 0, 0);
            this.player = player;
        }

        public Enemy(Constants.EnemyType type, int x, int y, Texture2D spriteSheet, Player player, int patrol_A, int patrol_B)
        {
            this.type = type;
            this.position = new Vector2(x, y);
            this.spriteSheet = spriteSheet;
            this.player = player;
            InitAnimations();
            InitStats();
            InitBehavirour(player, patrol_A, patrol_B);
            InitSkills();
        }

        private void InitAnimations()
        {
            switch (type)
            {
                case Constants.EnemyType.BlackMetalBadger:
                    WalkRight = new Animation(spriteSheet, 0, 93, 103, 9, 15, 60);
                    WalkLeft = new Animation(spriteSheet, 0, 93, 103, 0, 6, 60);
                    RunRight = new Animation(spriteSheet, 0, 93, 103, 9, 15, 40);
                    RunLeft = new Animation(spriteSheet, 0, 93, 103, 0, 6, 40);
                    Stopped = new Animation(spriteSheet, 0, 93, 103, 7, 7, 60);
                    Jumping = new Animation(spriteSheet, 0, 93, 103, 5, 6, 100);
                    Dying = new Animation(spriteSheet, 0, 93, 103, 1, 8, 80, false, false);
                    break;
                case Constants.EnemyType.DelayOwl:
                    WalkRight = new Animation(spriteSheet, 0, 91, 59, 0, 0, 100);
                    WalkLeft = new Animation(spriteSheet, 0, 91, 59, 0, 0, 100);
                    RunRight = new Animation(spriteSheet, 0, 91, 59, 0, 0, 100);
                    RunLeft = new Animation(spriteSheet, 0, 91, 59, 0, 0, 100);
                    Stopped = new Animation(spriteSheet, 0, 91, 59, 0, 0, 100);
                    Jumping = new Animation(spriteSheet, 0, 91, 59, 0, 0, 100);
                    break;
            }

            currentAnimation = Stopped;
        }

        private void InitBehavirour(Player player, int pointA, int pointB)
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

        private void InitStats()
        {
            switch (type)
            {
                case Constants.EnemyType.BlackMetalBadger:
                    walkSpeed = 6f;
                    runSpeed = 9f;
                    acceleration = 1f;
                    deacceleration = 0.5f;
                    jumpStrenght = 5;
                    orientation = Constants.CharacterOrientation.Ground;
                    Vision = 300;
                    AttackRange = 120;
                    health = 50;
                    maxHealth = 50;
                    healthRegen = 2;
                    BoundingBoxOffset = 10;
                    ReactSpeed = 250f;
                    mass = 100;
                    break;

                case Constants.EnemyType.DelayOwl:
                    walkSpeed = 3f;
                    runSpeed = 5f;
                    acceleration = 1f;
                    deacceleration = 0.1f;
                    jumpStrenght = 1;
                    orientation = Constants.CharacterOrientation.Air;
                    Vision = 400;
                    AttackRange = 80;
                    health = 1;
                    maxHealth = 1;
                    healthRegen = 1;
                    break;
            }
        }

        private void InitSkills()
        {
            switch (type)
            {
                case Constants.EnemyType.BlackMetalBadger:
                    #region TESTING
                    EnemySkill testSkill = new EnemySkill(this, new Animation(spriteSheet, 0, 93, 103, 0, 15, 20, false, false), 8000, 2);
                    testSkill.Acceleration = 0.25f;
                    testSkill.StartVelocity = new Vector2(15, -15);
                    testSkill.UltimateVelocityX = 0;
                    testSkill.AddEffect(Constants.TestEffect, Vector2.Zero);
                    testSkill.SoundEffectAsset = Constants.BadgerSkill;
                    testSkill.Conditions.Add(new Condition(Constants.ConditionType.DistanceToPlayerLowerThan, AttackRange));
                    testSkill.DamagingFrames = new List<int>
                    {
                        15,
                        16,
                    };

                    testSkill.HitBoxPositions[0] = position;
                    testSkill.HitBoxPositions[1] = position;
                    testSkill.HitBoxHeight = 100;
                    testSkill.HitBoxWidth = 200;

                    EnemySkill testAttack = new EnemySkill(this, new Animation(spriteSheet, 0, 93, 103, 1, 4, 80, false, false), 900, 2);
                    testAttack.StartVelocity = new Vector2(0, -8);
                    testAttack.Conditions.Add(new Condition(Constants.ConditionType.DistanceToPlayerLowerThan, AttackRange));
                    testAttack.SoundEffectAsset = Constants.BadgerAttack;
                    testAttack.DamagingFrames = new List<int>
                    {
                        1,
                        2,
                    };

                    testAttack.HitBoxPositions[0] = position;
                    testAttack.HitBoxPositions[1] = position;
                    testAttack.HitBoxHeight = 100;
                    testAttack.HitBoxWidth = 200;

                    Skills = new List<EnemySkill>();
                    Skills.Add(testSkill);
                    Skills.Add(testAttack);
                    #endregion
                    break;
            }
        }

        #endregion

        #region Updates

        private void UpdateSkills(GameTime gameTime)
        {
            foreach (EnemySkill s in Skills)
                s.Update(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (state != Constants.CharacterState.Dead)
            {
                behaviour.Apply(gameTime);

                HandleAnimations(gameTime);

                UpdateSkills(gameTime);

                RegenerateHealth(gameTime);

                CleanActiveSkill();

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
                    }
                }
            }

            CharacterPhysics.Apply(this, gameTime);
        }

        private void HandleAnimations(GameTime gameTime)
        {
            switch (state)
            {
                case Constants.CharacterState.Walking:
                    if (direction == Constants.Direction.Left) { ChangeAnimation(WalkLeft); }
                    else { ChangeAnimation(WalkRight); }
                    break;

                case Constants.CharacterState.Running:
                    if (direction == Constants.Direction.Left) { ChangeAnimation(RunLeft); }
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

        public void Kill()
        {
            state = Constants.CharacterState.Disabled;
            ChangeAnimation(Dying);
        }

    }
}
