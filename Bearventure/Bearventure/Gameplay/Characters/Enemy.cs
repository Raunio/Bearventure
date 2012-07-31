using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bearventure
{
    public class Enemy : Character
    {
        #region enumerations

        public enum EnemyType
        {
            BlackMetalBadger,
            DelayOwl,
            WahCat,
        };

        #endregion
        #region Members
        /// <summary>
        /// Range of vision.
        /// </summary>
        private int vision;
        /// <summary>
        /// Enemy attack range.
        /// </summary>
        private int attackRange;
        /// <summary>
        /// Enemy behaviour
        /// </summary>
        public Behaviour behaviour;
        /// <summary>
        /// Enemy orientation. Air or Ground.
        /// </summary>
        public Orientation orientation;
        private EnemyType type;

        #region Animations
        private Animation walkLeft;
        private Animation walkRight;
        private Animation stopped;
        private Animation attacking;
        private Animation jumping;
        private Animation runLeft;
        private Animation runRight;
        #endregion

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

            
        }

        public override void Update(GameTime gameTime)
        {
            CharacterPhysics.Apply(this, gameTime);

            behaviour.Apply(gameTime);

            HandleAnimations(gameTime);

            //TEMPORARY!!! :

            if (position.Y > 430)
            {
                position.Y = 430;
            }

        }

        private void HandleAnimations(GameTime gameTime)
        {
            switch (state)
            {
                case State.Walking:
                    if (direction == Direction.Left) { currentAnimation = walkLeft; }
                    else { currentAnimation = walkRight; }
                    break;

                case State.Running:
                    if (direction == Direction.Left) { currentAnimation = runLeft; }
                    else { currentAnimation = runRight; }
                    break;

                case State.Stopped:
                    currentAnimation = stopped;
                    break;

                case State.Jumping:
                    currentAnimation = jumping;
                    break;
                    
                case State.Attacking:
                    //currentAnimation = attacking;
                    break;
            }

            currentAnimation.Animate(gameTime);
        }

        private void InitAnimations()
        {
            switch (type)
            {
                case EnemyType.BlackMetalBadger:
                    walkRight = new Animation(spriteSheet, 0, 93, 103, 9, 15, 60);
                    walkLeft = new Animation(spriteSheet, 0, 93, 103, 0, 6, 60);
                    runRight = new Animation(spriteSheet, 0, 93, 103, 9, 15, 40);
                    runLeft = new Animation(spriteSheet, 0, 93, 103, 0, 6, 40);
                    stopped = new Animation(spriteSheet, 0, 93, 103, 7, 7, 60);
                    break;
            }

            //currentAnimation = stopped;
        }
        private void InitBehavirour(Player player)
        {
            switch (type)
            {
                case EnemyType.BlackMetalBadger:
                    behaviour = new Behaviour(this, player);
                    behaviour.InitFixedPatrol(200, 600);
                    behaviour.Wait_time = 2000;
                    break;
            }
        }
        private void InitStats()
        {
            switch (type)
            {
                case EnemyType.BlackMetalBadger:
                    walkSpeed = 4f;
                    acceleration = 1f;
                    deacceleration = 0.2f;
                    orientation = Orientation.Ground;
                    vision = 300;
                    attackRange = 120;
                    health = 50;
                    max_health = 50;
                    break;
            }
        }

        public int Vision
        {
            get
            {
                return vision;
            }
        }
        public int AttackRange
        {
            get
            {
                return attackRange;
            }
        }
        #endregion
    }
}
