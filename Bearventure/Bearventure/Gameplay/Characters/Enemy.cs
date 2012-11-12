using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bearventure
{
    public class Enemy : Character
    {
        #region Members

        private Constants.EnemyType type;
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

        #endregion

        #region Methods
        public Enemy(Constants.EnemyType type, Vector2 position, Texture2D spriteSheet, Player player)
        {
            this.type = type;
            this.position = position;
            this.spriteSheet = spriteSheet;

            InitAnimations();
            InitStats();
            InitBehavirour(player, 0, 0);

            attackTimer = attackSpeed;
        }

        public Enemy(Constants.EnemyType type, int x, int y, Texture2D spriteSheet, Player player, int patrol_A, int patrol_B)
        {
            this.type = type;
            this.position = new Vector2(x, y);
            this.spriteSheet = spriteSheet;

            InitAnimations();
            InitStats();
            InitBehavirour(player, patrol_A, patrol_B);

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

                case Constants.CharacterState.Attacking:
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
                case Constants.EnemyType.BlackMetalBadger:
                    WalkRight = new Animation(spriteSheet, 0, 93, 103, 9, 15, 60);
                    WalkLeft = new Animation(spriteSheet, 0, 93, 103, 0, 6, 60);
                    RunRight = new Animation(spriteSheet, 0, 93, 103, 9, 15, 40);
                    RunLeft = new Animation(spriteSheet, 0, 93, 103, 0, 6, 40);
                    Stopped = new Animation(spriteSheet, 0, 93, 103, 7, 7, 60);
                    Attacking = new Animation(spriteSheet, 0, 93, 103, 7, 7, 100, false, false);
                    Jumping = new Animation(spriteSheet, 0, 93, 103, 5, 6, 100);
                    break;
                case Constants.EnemyType.DelayOwl:
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
                    max_health = 50;
                    health_regen = 10;
                    attackSpeed = 1000;
                    BoundingBox_Offset = 5;
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
