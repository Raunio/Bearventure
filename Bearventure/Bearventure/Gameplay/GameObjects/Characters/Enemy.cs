using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bearventure.Engine.Effects;
using System.Collections.Generic;
using Bearventure.Gameplay.Characters.Skills;
using Microsoft.Xna.Framework.Audio;
using Bearventure.Gameplay.GameObjects;
using Microsoft.Xna.Framework.Content;
using System;

namespace Bearventure.Gameplay.Characters
{
    //Initialization, members, gets & sets
    public partial class Enemy : Character
    {
        #region Members

        private Character player;
        private Constants.EnemyType type;
        private ContentManager content;

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

        public Constants.EnemyType Type
        {
            get
            {
                return type;
            }
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

        public CharacterAnimation WalkLeft
        {
            private set;
            get;
        }
        public CharacterAnimation WalkRight
        {
            private set;
            get;
        }
        public CharacterAnimation Stopped
        {
            private set;
            get;
        }
        public CharacterAnimation Jumping
        {
            private set;
            get;
        }
        public CharacterAnimation RunLeft
        {
            private set;
            get;
        }
        public CharacterAnimation RunRight
        {
            private set;
            get;
        }
        public CharacterAnimation KnockBack
        {
            private set;
            get;
        }
        public CharacterAnimation Damaged
        {
            private set;
            get;
        }
        /// <summary>
        /// Must not be a looping animation.
        /// </summary>
        public CharacterAnimation Dying
        {
            get;
            private set;
        }
        public CharacterAnimation SpawnAnimation
        {
            get;
            private set;
        }
        #endregion

        #endregion

        #region Initialization
        public Enemy(Constants.EnemyType type, Constants.BehaviourType behaviourType, int x, int y, Player player, int patrol_A, int patrol_B, int waitTime, ContentManager content)
        {
            TAG = "Enemy";
            this.type = type;
            this.position = new Vector2(x, y);
            this.content = content;
            this.player = player;
            InitAnimations();
            InitAnatomicInfos();
            InitStats();
            InitSkills();
            if (behaviourType == Constants.BehaviourType.Default)
                InitDefaultBehaviour(player, patrol_A, patrol_B);
            else
                InitCustomBehaviour(behaviourType, player, patrol_A, patrol_B, waitTime);
        }
        /// <summary>
        /// Initialize all the animations.
        /// </summary>
        private void InitAnimations()
        {
            switch (type)
            {
                case Constants.EnemyType.BlackMetalBadger:
                    spriteSheet = content.Load<Texture2D>(Constants.BlackMetalBadger);
                    WalkRight = new CharacterAnimation(spriteSheet, 0, 93, 103, 0, 7, 60, SpriteEffects.None, 0f, 0f, false, true);
                    WalkLeft = new CharacterAnimation(spriteSheet, 0, 93, 103, 0, 7, 60, SpriteEffects.None, 0f, 0f, true, true);
                    RunRight = new CharacterAnimation(spriteSheet, 0, 93, 103, 0, 7, 40, SpriteEffects.None, 0f, 0f, false, true);
                    RunLeft = new CharacterAnimation(spriteSheet, 0, 93, 103, 0, 7, 40, SpriteEffects.None, 0f, 0f, false, true);
                    Stopped = new CharacterAnimation(spriteSheet, 0, 93, 103, 4, 4, 60, SpriteEffects.None, 0f, 0f, false, true);
                    Jumping = new CharacterAnimation(spriteSheet, 0, 93, 103, 5, 6, 100, SpriteEffects.None, 0f, 0f, false, true);
                    Dying = new CharacterAnimation(spriteSheet, 2, 114, 121, 0, 1, 80, SpriteEffects.None, 0f, 0f, false, false);
                    Dying.CustomFrameRowPosition = 208;
                    KnockBack = new CharacterAnimation(spriteSheet, 2, 114, 121, 0, 1, 80, SpriteEffects.None, 0f, 0f, false, false);
                    KnockBack.CustomFrameRowPosition = 208;
                    Damaged = new CharacterAnimation(spriteSheet, 4, 91, 102, 0, 0, 110, SpriteEffects.None, 0f, 0f, false, false);
                    Damaged.CustomFrameRowPosition = 434;
                    SpawnAnimation = new CharacterAnimation(spriteSheet, 3, 154, 107, 0, 3, 50, SpriteEffects.None, 0f, 0f, false, false);
                    SpawnAnimation.CustomFrameRowPosition = 327;
                    SpawnAnimation.ReverseAtEnd = true;
                    break;
                case Constants.EnemyType.DelayOwl:
                    spriteSheet = content.Load<Texture2D>(Constants.DelayOwl);
                    WalkRight = new CharacterAnimation(spriteSheet, 0, 128, 90, 8, 11, 70, SpriteEffects.None, 0f, 0f, false, true);
                    WalkLeft = new CharacterAnimation(spriteSheet, 0, 128, 90, 0, 3, 70, SpriteEffects.None, 0f, 0f, true, true);
                    RunRight = new CharacterAnimation(spriteSheet, 0, 128, 90, 8, 11, 50, SpriteEffects.None, 0f, 0f, false, true);
                    RunLeft = new CharacterAnimation(spriteSheet, 0, 128, 90, 0, 3, 50, SpriteEffects.None, 0f, 0f, true, true);
                    Stopped = new CharacterAnimation(spriteSheet, 0, 128, 90, 5, 5, 70, SpriteEffects.None, 0f, 0f, false, true);
                    Jumping = new CharacterAnimation(spriteSheet, 0, 128, 90, 0, 0, 70, SpriteEffects.None, 0f, 0f, false, true);
                    Dying = new CharacterAnimation(spriteSheet, 0, 128, 90, 5, 5, 100, SpriteEffects.None, 0f, 0f, false, false);
                    break;
                case Constants.EnemyType.OscillatorWorm:
                    spriteSheet = content.Load<Texture2D>(Constants.OscillatorWorm);
                    WalkRight = new CharacterAnimation(spriteSheet, 0, 61, 19, 0, 13, 45, SpriteEffects.None, 0f, 0f, false, true);
                    WalkLeft = new CharacterAnimation(spriteSheet, 0, 61, 19, 0, 13, 45, SpriteEffects.FlipHorizontally, 0f, 0f, false, true);
                    Stopped = new CharacterAnimation(spriteSheet, 0, 61, 19, 5, 5, 45, SpriteEffects.None, 0f, 0f, false, true);
                    Jumping = new CharacterAnimation(spriteSheet, 0, 61, 19, 0, 0, 45, SpriteEffects.None, 0f, 0f, false, true);
                    Dying = new CharacterAnimation(spriteSheet, 0, 61, 19, 5, 5, 45, SpriteEffects.None, 0f, 0f, false, false);
                    break;
            }

            ChangeAnimation(Stopped);
        }
        private void InitAnatomicInfos()
        {
            switch (type)
            {
                case Constants.EnemyType.BlackMetalBadger:

                    break;
            }
        }
        /// <summary>
        /// Initialize "AI"
        /// </summary>
        /// <param name="player">Pointer to player required for AI Initialization</param>
        /// <param name="pointA">Patrol point A</param>
        /// <param name="pointB">PAtrol point B</param>
        private void InitDefaultBehaviour(Player player, int pointA, int pointB)
        {
            InitDefaultCombatBehaviour();
            behaviour = new Behaviour(this, player);

            switch (type)
            {
                case Constants.EnemyType.BlackMetalBadger:
                    if (pointA != 0 && pointB != 0)
                    {
                        behaviour.InitFixedPatrol(pointA, pointB);
                        behaviour.WaitTime = 2000;
                    }
                    else
                    {
                        behaviour.InitFreePatrol();
                    }
                    break;

                case Constants.EnemyType.DelayOwl:
                    behaviour.InitPassive();
                    break;
                case Constants.EnemyType.OscillatorWorm:
                    behaviour.InitPassive();
                    break;
            }
        }

        private void InitDefaultCombatBehaviour()
        {
            switch (type)
            {
                case Constants.EnemyType.BlackMetalBadger:
                    combatBehaviour = Constants.CombatBehaviour.Default;
                    break;

                case Constants.EnemyType.DelayOwl:
                    combatBehaviour = Constants.CombatBehaviour.AttackAndFlee;
                    break;
                case Constants.EnemyType.OscillatorWorm:
                    combatBehaviour = Constants.CombatBehaviour.Default;
                    break;
            }
        }

        private void InitCustomBehaviour(Constants.BehaviourType behaviourType, Player player, int pointA, int pointB, int waitTime)
        {
            InitDefaultCombatBehaviour();
            behaviour = new Behaviour(this, player);

            switch (behaviourType)
            {
                case Constants.BehaviourType.FixedPatrol:
                    behaviour.InitFixedPatrol(pointA, pointB);
                    behaviour.WaitTime = waitTime;
                    break;
                case Constants.BehaviourType.FreePatrol:
                    behaviour.InitFreePatrol();
                    break;
                case Constants.BehaviourType.Passive:
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
                    acceleration = 1.5f;
                    decceleration = 2f;
                    jumpStrenght = 15;
                    Orientation = Constants.CharacterOrientation.Ground;
                    Vision = 600;
                    AttackRange = 120;
                    health = 35;
                    maxHealth = 35;
                    healthRegen = 2;
                    BoundingBoxOffset = 10;
                    ReactSpeed = 250f;
                    mass = 100;
                    ArmorType = Constants.ArmorType.Fur;
                    KnockBackTreshold = 30;
                    BoundingBoxSize = new Point(Stopped.FrameWidth, Stopped.FrameHeight);
                    break;

                case Constants.EnemyType.DelayOwl:
                    walkSpeed = 14f;
                    runSpeed = 9f;
                    acceleration =2f;
                    decceleration = 2f;
                    jumpStrenght = 1;
                    Orientation = Constants.CharacterOrientation.Air;
                    Vision = 300;
                    AttackRange = 110;
                    health = 1;
                    maxHealth = 1;
                    healthRegen = 1;
                    mass = 0;
                    ArmorType = Constants.ArmorType.Feathers;
                    break;

                case Constants.EnemyType.OscillatorWorm:
                    walkSpeed = 7f;
                    runSpeed = 9f;
                    acceleration = 1f;
                    decceleration = 0.5f;
                    jumpStrenght = 17;
                    Orientation = Constants.CharacterOrientation.Ground;
                    Vision = 300;
                    AttackRange = 110;
                    health = 1;
                    maxHealth = 1;
                    healthRegen = 1;
                    mass = 0;
                    ArmorType = Constants.ArmorType.Skin;
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
                    CharacterAnimation testSkillAnimation = new CharacterAnimation(spriteSheet, 3, 154, 107, 0, 3, 30, SpriteEffects.None, 0f, 0f, false, false);
                    testSkillAnimation.CalculateBoundingBoxOffsets(BoundingBoxSize, Constants.DirectionX.None);
                    testSkillAnimation.CustomFrameRowPosition = 327;
                    testSkillAnimation.FreezeFrames = new Animation.FrameFreezer
                    {
                        Frames = new List<int> { 3, 4 },
                        Amount = 200
                    };

                    EnemySkill testSkill = new EnemySkill(this, testSkillAnimation, 8000, 2, Constants.DamageType.Crushing);
                    testSkill.Acceleration = 0.25f;
                    testSkill.StartVelocity = new Vector2(25, 0);
                    testSkill.UltimateVelocityX = 0;
                    testSkill.ActivationSoundEffect = SoundEffectManager.Instance.BadgerSkill;
                    testSkill.Conditions.Add(new Condition(Constants.ConditionType.DistanceToPlayerLowerThan, Vision));
                    testSkill.Conditions.Add(new Condition(Constants.ConditionType.DistanceToPlayerGreaterThan, AttackRange * 2));
                    testSkill.Conditions.Add(new Condition(Constants.ConditionType.FacingPlayer, true));
                    testSkill.Conditions.Add(new Condition(Constants.ConditionType.OnGround, true));
                    testSkill.DamagingFrames = new List<int>
                    {
                        0,
                        1,
                        2,
                        3,
                        4,
                    };

                    for(int i = 0; i < 4; i++)
                        testSkill.HitBoxPositions[i] = new Vector2(70, -BoundingBox.Height / 2);
                   
                    testSkill.HitBoxHeight = BoundingBox.Height;
                    testSkill.HitBoxWidth = 20;
                    testSkill.InflictForce = new Vector2(35, 0);

                    CharacterAnimation attack_right = new CharacterAnimation(spriteSheet, 1, 149, 105, 0, 5, 30, SpriteEffects.None, 0f, 0f, false, false);
                    CharacterAnimation attack_left = new CharacterAnimation(spriteSheet, 1, 149, 105, 0, 5, 30, SpriteEffects.FlipHorizontally, 0f, 0f, false, false);

                    attack_left.CalculateBoundingBoxOffsets(BoundingBoxSize, Constants.DirectionX.Left);
                    attack_right.CalculateBoundingBoxOffsets(BoundingBoxSize, Constants.DirectionX.Right);

                    attack_right.SetAnatomicInfo(Constants.BadgerAttackAnatomy, SpriteEffects.None);
                    attack_left.SetAnatomicInfo(Constants.BadgerAttackAnatomy, SpriteEffects.FlipHorizontally);

                    Attack = new EnemySkill(this, attack_right, attack_left, 1400, 4, Constants.DamageType.Crushing);
                    Attack.StartVelocity = new Vector2(5, 0);
                    Attack.Conditions.Add(new Condition(Constants.ConditionType.DistanceToPlayerLowerThan, AttackRange));
                    Attack.Conditions.Add(new Condition(Constants.ConditionType.FacingPlayer, true));
                    Attack.Conditions.Add(new Condition(Constants.ConditionType.OnGround, true));
                    Attack.Conditions.Add(new Condition(Constants.ConditionType.IsNotUsingSkill, testSkill));
                    Attack.ActivationSoundEffect = SoundEffectManager.Instance.BadgerAttack;
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

                    Attack.InflictForce = new Vector2(10, 0);
                    Attack.ForceInterruptTreshold = 5;

                    Attack.HitSoundEffect = SoundEffectManager.Instance.Crush;

                    Skills = new List<EnemySkill>();
                    Skills.Add(testSkill);
                    Skills.Add(Attack);
                    #endregion
                    break;
                case Constants.EnemyType.DelayOwl:
                    #region TESTING
                    Attack = new EnemySkill(this, new CharacterAnimation(spriteSheet, 0, 126, 88, 8, 9, 100, SpriteEffects.None, 0f, 0f, false, false),
                        5000, 5, Constants.DamageType.Piercing);
                    Attack.Conditions.Add(new Condition(Constants.ConditionType.DistanceToPlayerLowerThan, AttackRange));
                    Attack.DamagingFrames = new List<int>
                    {
                        8,
                        9,
                    };

                    Attack.HitBoxHeight = BoundingBox.Height + 10;
                    Attack.HitBoxWidth = BoundingBox.Width + 10;

                    Attack.HitBoxPositions[0] = new Vector2(0, 0);
                    Attack.HitBoxPositions[1] = new Vector2(0, 0);

                    Skills = new List<EnemySkill>();
                    Skills.Add(Attack);
                    #endregion
                    break;
                case Constants.EnemyType.OscillatorWorm:
                    #region TESTING
                    CharacterAnimation attackRight = new CharacterAnimation(spriteSheet, 0, 61, 19, 0, 13, 5, SpriteEffects.None, 0f, 0f, false, false);
                    CharacterAnimation attackLeft = new CharacterAnimation(spriteSheet, 0, 61, 19, 0, 13, 5, SpriteEffects.None, 0f, 0f, false, false);
                    attackLeft.Effects = SpriteEffects.FlipHorizontally;

                    Attack = new EnemySkill(this, attackRight, attackLeft, 200, 1, Constants.DamageType.Piercing);
                    Attack.Conditions.Add(new Condition(Constants.ConditionType.DistanceToPlayerLowerThan, AttackRange));
                    Attack.DamagingFrames = new List<int>
                    {
                        0,
                        1,
                        2,
                    };
                    Attack.HitBoxPositions[0] = new Vector2(25, -2);
                    Attack.HitBoxPositions[1] = new Vector2(25, -2);
                    Attack.HitBoxPositions[2] = new Vector2(25, -2);

                    Attack.HitBoxHeight = 25;
                    Attack.HitBoxWidth = 25;

                    EnemySkill pounce = new EnemySkill(this, attackRight, attackLeft, 5000, 2, Constants.DamageType.Piercing);
                    pounce.Conditions.Add(new Condition(Constants.ConditionType.DistanceToPlayerLowerThan, Vision));
                    pounce.Conditions.Add(new Condition(Constants.ConditionType.DistanceToPlayerGreaterThan, Vision / 3));

                    pounce.ActivationSoundEffect = SoundEffectManager.Instance.MatoTest;

                    pounce.StartVelocity = new Vector2(jumpStrenght, -jumpStrenght);
                    pounce.UltimateVelocityX = 0;
                    pounce.UltimateRotation = -0.75f;
                    pounce.RotationVelocity = 0.05f;

                    pounce.DamagingFrames = new List<int>
                    {
                        0,
                        1,
                        2,
                    };

                    pounce.HitBoxHeight = 25;
                    pounce.HitBoxWidth = 25;

                    pounce.HitBoxPositions[0] = new Vector2(25, -2);
                    pounce.HitBoxPositions[1] = new Vector2(25, -2);
                    pounce.HitBoxPositions[2] = new Vector2(25, -2);

                    Skills = new List<EnemySkill>();
                    //Skills.Add(Attack);
                    Skills.Add(pounce);
                    #endregion
                    break;
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
            HandleAnimations(gameTime);

            if (state != Constants.CharacterState.Dead && state != Constants.CharacterState.Spawning)
            {
                if (!IsDisabled)
                {
                    behaviour.Apply(gameTime);
                }
  
                UpdateSkills(gameTime);
                CleanActiveSkill();
                RegenerateHealth(gameTime);

                if (health <= 0)
                    Kill();
            }

            if (state == Constants.CharacterState.Disabled || state == Constants.CharacterState.Dead)
            {
                if (CurrentAnimation == Dying)
                {
                    if (CurrentAnimation.HasFinished && CharacterPhysics.OnGround(this) && !IsRotating)
                    {
                        if (velocity.X != 0)
                            CharacterPhysics.Stop(this);
                        else
                        {
                            if (Opacity == 1)
                            {
                                SetOpacity(1f, 0f, 0.05f);
                                state = Constants.CharacterState.Dead;
                            }
                            else if(Opacity == 0)
                            {
                                IsActive = false;
                            }
                        }
                        
                    }
                }
            }
            else if (state == Constants.CharacterState.Spawning)
            {
                if (CurrentAnimation.HasFinished)
                {
                    SetState(Constants.CharacterState.Stopped);
                }
            }
  
            CharacterPhysics.Apply(this, gameTime);
            UpdateScaling();
            UpdateRotating();
            UpdateOpacity();
        }
        /// <summary>
        /// Should be called in update. Reads the enemys state and changes its animations accordingly.
        /// </summary>
        /// <param name="gameTime"></param>
        private void HandleAnimations(GameTime gameTime)
        {
            if (CurrentCharacterAnimation == Damaged && !CurrentCharacterAnimation.HasFinished)
            {

            }
            else
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
                    case Constants.CharacterState.Knocked:
                        if (directionX == Constants.DirectionX.Right)
                            KnockBack.Effects = SpriteEffects.None;
                        else
                            KnockBack.Effects = SpriteEffects.FlipHorizontally;
                    
                        ChangeAnimation(KnockBack);
                        break;
                    case Constants.CharacterState.Spawning:
                        ChangeAnimation(SpawnAnimation);
                        break;
                }

            CurrentAnimation.Animate(gameTime);
            BoundingBoxAnimationOffset = CurrentCharacterAnimation.BoundingBoxOffset;

        }

        #endregion
        /// <summary>
        /// Kill the enemy.
        /// </summary>
        public void Kill()
        {
            if (state != Constants.CharacterState.Disabled)
                state = Constants.CharacterState.Disabled;
            else
                return;

            if (directionX == Constants.DirectionX.Left)
                Dying.Effects = SpriteEffects.FlipHorizontally;
            else
                Dying.Effects = SpriteEffects.None;

            ChangeAnimation(Dying);
            SetRotating(0f, (float)Math.PI / 2, 0.35f, true);

            mass = 0;

            SoundEffectManager.Instance.PlayDeathSound(position, type);
        }

        public void PlayStepSounds()
        {
            switch (type)
            {
                case Constants.EnemyType.BlackMetalBadger:
                    if (CurrentAnimation == WalkLeft)
                    {
                        if (CurrentAnimation.CurrentFrame == 3 || CurrentAnimation.CurrentFrame == 7)
                        {
                            if (CurrentAnimation.IsNewFrame)
                            {
                                SoundEffectManager.Instance.PlaySoundFromPosition(this.Position, SoundEffectManager.Instance.QuietStep);
                            }
                        }
                    }
                    else if (CurrentAnimation == WalkRight)
                    {
                        if (CurrentAnimation.CurrentFrame == 3 || CurrentAnimation.CurrentFrame == 7)
                        {
                            if (CurrentAnimation.IsNewFrame)
                            {
                                SoundEffectManager.Instance.PlaySoundFromPosition(this.Position, SoundEffectManager.Instance.QuietStep);
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Apply damage to character.
        /// </summary>
        /// <param name="damage"></param>
        public override void TakeDamage(float damage)
        {
            if (damageTimer > 300)
            {
                health -= (int)damage;
                damageTimer = 0;

                CombatManager.Instance.CombatLog.Add(this.Name + " took " + damage + " damage.");

                if (directionX == Constants.DirectionX.Left)
                    Damaged.Effects = SpriteEffects.FlipHorizontally;
                else
                    Damaged.Effects = SpriteEffects.None;

                ChangeAnimation(Damaged);
            }
        }

    }
}
