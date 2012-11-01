using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bearventure
{
    public class Enemy : Character
    {

        // TODO: Fix warnings! -Huemac

        #region Enumerations

        public enum EnemyType
        {
            BlackMetalBadger,
            DelayOwl,
            WahCat,
        };
        public enum CombatBehaviour
        {
            Default,
            AttackAndFlee,

        };

        #endregion
        #region Members

        private EnemyType type;
        private float attackTimer = 0;

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
        public Animation Attacking
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
        #endregion

        #endregion

        #region Gets and Sets
        /// <summary>
        /// Enemy combat behaviour
        /// </summary>
        public CombatBehaviour combatBehaviour
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

        #endregion

        #region Methods
        public Enemy(EnemyType type, Vector2 position, Texture2D spriteSheet, Player player)
        {
            this.type = type;
            this.position = position;
            this.spriteSheet = spriteSheet;

            InitAnimations();
            InitStats();
            InitBehavirour(player);

            attackTimer = attackSpeed;
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
        }

        public override void Update(GameTime gameTime)
        {
            CharacterPhysics.Apply(this, gameTime);

            behaviour.Apply(gameTime);

            HandleAnimations(gameTime);

            HandleAttacking(gameTime);

        }

        private void HandleAnimations(GameTime gameTime)
        {
            switch (state)
            {
                case State.Walking:
                    if (direction == Direction.Left) { ChangeAnimation(WalkLeft); }
                    else { ChangeAnimation(WalkRight); }
                    break;

                case State.Running:
                    if (direction == Direction.Left) { ChangeAnimation(RunLeft); }
                    else { ChangeAnimation(RunRight); }
                    break;

                case State.Stopped:
                    ChangeAnimation(Stopped);
                    break;

                case State.Jumping:
                    ChangeAnimation(Jumping);
                    break;

                case State.Attacking:
                    ChangeAnimation(Attacking);
                    break;
            }

            currentAnimation.Animate(gameTime);
        }

        private void ChangeAnimation(Animation animation)
        {
            if (currentAnimation != animation)
            {
                currentAnimation.Reset();
                currentAnimation = animation;
            }
        }

        private void InitAnimations()
        {
            switch (type)
            {
                case EnemyType.BlackMetalBadger:
                    WalkRight = new Animation(spriteSheet, 0, 93, 103, 9, 15, 60);
                    WalkLeft = new Animation(spriteSheet, 0, 93, 103, 0, 6, 60);
                    RunRight = new Animation(spriteSheet, 0, 93, 103, 9, 15, 40);
                    RunLeft = new Animation(spriteSheet, 0, 93, 103, 0, 6, 40);
                    Stopped = new Animation(spriteSheet, 0, 93, 103, 7, 7, 60);
                    Attacking = new Animation(spriteSheet, 1, 93, 103, 0, 4, 100, false, false);
                    Jumping = new Animation(spriteSheet, 0, 93, 103, 5, 6, 100);
                    break;
                case EnemyType.DelayOwl:
                    WalkRight = new Animation(spriteSheet, 0, 91, 59, 0, 0, 100);
                    WalkLeft = new Animation(spriteSheet, 0, 91, 59, 0, 0, 100);
                    RunRight = new Animation(spriteSheet, 0, 91, 59, 0, 0, 100);
                    RunLeft = new Animation(spriteSheet, 0, 91, 59, 0, 0, 100);
                    Stopped = new Animation(spriteSheet, 0, 91, 59, 0, 0, 100);
                    Attacking = new Animation(spriteSheet, 1, 91, 59, 0, 2, 100, false, false);
                    Jumping = new Animation(spriteSheet, 0, 91, 59, 0, 0, 100);
                    break;
            }

            currentAnimation = Stopped;
        }

        private void InitBehavirour(Player player)
        {
            switch (type)
            {
                case EnemyType.BlackMetalBadger:
                    combatBehaviour = CombatBehaviour.Default;
                    behaviour = new Behaviour(this, player);
                    behaviour.InitFixedPatrol(200, 600);
                    behaviour.WaitTime = 2000;
                    break;

                case EnemyType.DelayOwl:
                    combatBehaviour = CombatBehaviour.AttackAndFlee;
                    behaviour = new Behaviour(this, player);
                    behaviour.InitPassive();
                    break;
            }
        }

        private void InitStats()
        {
            switch (type)
            {
                case EnemyType.BlackMetalBadger:
                    walkSpeed = 2f;
                    runSpeed = 4.5f;
                    acceleration = 1f;
                    deacceleration = 0.2f;
                    jumpStrenght = 5;
                    orientation = Orientation.Ground;
                    Vision = 300;
                    AttackRange = 120;
                    health = 50;
                    max_health = 50;
                    health_regen = 10;
                    attackSpeed = 1000;
                    break;

                case EnemyType.DelayOwl:
                    walkSpeed = 3f;
                    runSpeed = 5f;
                    acceleration = 1f;
                    deacceleration = 0.1f;
                    jumpStrenght = 1;
                    orientation = Orientation.Air;
                    Vision = 400;
                    AttackRange = 80;
                    health = 1;
                    max_health = 1;
                    health_regen = 1;
                    attackSpeed = 7000;
                    break;
            }
        }

        private void HandleAttacking(GameTime gameTime)
        {
            attackTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (currentAnimation == Attacking)
            {
                if (currentAnimation.HasFinished)
                    attackTimer = 0;
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

        public bool AttackReady
        {
            get
            {
                return attackTimer >= attackSpeed ? true : false;
            }
        }
        #endregion
    }
}
