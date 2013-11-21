using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Bearventure.Engine.Effects;
using Bearventure.Gameplay.Characters.Skills;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Bearventure.Gameplay.Characters
{
    public class Player : Character
    {
        Animation stoppedRight;
        Animation stoppedLeft;
        Animation walkRight;
        Animation walkLeft;
        Animation jumpingRight;
        Animation jumpingLeft;

        Texture2D comboSheet;
        Texture2D jumpSheet;

        CharacterSkillCombo combo1 = new CharacterSkillCombo();

        public Player(ContentManager content)
        {
            TAG = "Player";
            Name = "You";
            scale = 1f;
            this.spriteSheet = content.Load<Texture2D>("Sprites/kavelyfixed");
            comboSheet = content.Load<Texture2D>("Sprites/karhukombo1");
            jumpSheet = content.Load<Texture2D>("Sprites/hyppyfix");
            IsActive = true;

            this.position = position;
            directionX = Constants.DirectionX.Right;

            InitStats();
            InitAnimations();
            InitSkills();
        }

        private void InitStats()
        {
            acceleration = 1.75f;
            decceleration = 1.75f;
            walkSpeed = 12f;
            runSpeed = 17f;
            jumpStrenght = 20;

            maxHealth = 50;
            health = 50;

            mass = 150;

            BoundingBoxOffset = 15;

            ArmorType = Constants.ArmorType.Fur;
        }

        private void InitAnimations()
        {
            stoppedRight = new Animation(spriteSheet, 0, 88, 121, 12, 12, 50);
            stoppedLeft = new Animation(spriteSheet, 0, 88, 121, 9, 9, 50);
            walkRight = new Animation(spriteSheet, 0, 88, 121, 13, 21, 50);
            walkLeft = new Animation(spriteSheet, 0, 88, 121, 0, 8, 50, true);

            jumpingRight = new Animation(jumpSheet, 0, 106, 120, 0, 3, 40, false, false);
            jumpingLeft = new Animation(jumpSheet, 0, 106, 120, 0, 3, 40, false, false);
            jumpingLeft.Effects = SpriteEffects.FlipHorizontally;

            currentAnimation = stoppedRight;
        }

        private void InitSkills()
        {
            #region Combo1 Initialization
            #region ComboSkill 1 Initialization

            Animation StraightPunch_R = new Animation(comboSheet, 0, 118, 120, 0, 3, 30, false, false);
            StraightPunch_R.FreezeFrames = new Animation.FrameFreezer
            {
                Frames = new List<int>
                {
                    2,
                },

                Amount = 150,
            };

            Animation StraightPunch_L = new Animation(comboSheet, 0, 118, 120, 0, 3, 30, SpriteEffects.FlipHorizontally, 0f, 0f, false, false);
            StraightPunch_L.FreezeFrames = new Animation.FrameFreezer
            {
                Frames = new List<int>
                {
                    2,
                },

                Amount = 150,
            };

            CharacterSkill skill1 = new CharacterSkill(this, StraightPunch_R, StraightPunch_L, 300, 3, Constants.DamageType.Crushing);

            skill1.SoundEffectAsset = Constants.KarhuHit1;
            skill1.Acceleration = 0.25f;
            skill1.StartVelocity = new Vector2(4, 0);
            skill1.UltimateVelocityX = 0;
            skill1.InflictForce = new Vector2(7, -5);
            skill1.DamagingFrames = new List<int>
            {
                2,
            };
            skill1.HitBoxPositions[0] = new Vector2(30, -10);
            skill1.HitBoxHeight = 25;
            skill1.HitBoxWidth = 25;

            #endregion
            #region ComboSkill 2 Initialization
            Animation SweepingPunch_R = new Animation(comboSheet, 1, 118, 120, 0, 5, 30, false, false);
            SweepingPunch_R.FreezeFrames = new Animation.FrameFreezer
            {
                Frames = new List<int>
                {
                    3,
                    4,
                },

                Amount = 70,
            };
            Animation SweepingPunch_L = new Animation(comboSheet, 1, 118, 120, 0, 5, 30, SpriteEffects.FlipHorizontally, 0f, 0f, false, false);
            SweepingPunch_L.FreezeFrames = new Animation.FrameFreezer
            {
                Frames = new List<int>
                {
                    3,
                    4,
                },

                Amount = 70,
            };
            CharacterSkill skill2 = new CharacterSkill(this, SweepingPunch_R, SweepingPunch_L, 300, 4, Constants.DamageType.Crushing);

            skill2.SoundEffectAsset = Constants.KarhuHit2;
            skill2.Acceleration = 0.25f;
            skill2.StartVelocity = new Vector2(3, 0);
            skill2.InflictForce = new Vector2(7, -5);
            skill2.UltimateVelocityX = 0;
            skill2.DamagingFrames = new List<int>
            {
                3,
                4,
            };
            skill2.HitBoxPositions[0] = new Vector2(30, -5);
            skill2.HitBoxPositions[1] = new Vector2(30, 0);
            skill2.HitBoxHeight = 25;
            skill2.HitBoxWidth = 25;

            #endregion
            #region ComboSkill 3 Initialization

            Animation UpperCut_R = new Animation(comboSheet, 2, 118, 120, 3, 7, 25, false, false);
            UpperCut_R.FreezeFrames = new Animation.FrameFreezer
            {
                Frames = new List<int>
                {
                    5,
                },

                Amount = 150,
            };
            Animation UpperCut_L = new Animation(comboSheet, 2, 118, 120, 2, 7, 25, SpriteEffects.FlipHorizontally, 0f, 0f, false, false);
            UpperCut_L.FreezeFrames = new Animation.FrameFreezer
            {
                Frames = new List<int>
                {
                    5,
                },

                Amount = 150,
            };
            CharacterSkill skill3 = new CharacterSkill(this, UpperCut_R, UpperCut_L, 300, 10, Constants.DamageType.Crushing);

            skill3.SoundEffectAsset = Constants.KarhuHit3;
            skill3.Acceleration = 0.25f;
            skill3.StartVelocity = new Vector2(2, -8);
            skill3.UltimateVelocityX = 0;

            skill3.InflictForce = new Vector2(30, -20);

            skill3.DamagingFrames = new List<int>
            {
                5,
            };
            skill3.HitBoxPositions[0] = new Vector2(30, -25);
            skill3.HitBoxHeight = 25;
            skill3.HitBoxWidth = 25;

            #endregion

            combo1.SkillArray = new List<CharacterSkill>
            {
                skill1,
                skill2,
                skill3,
            };

            combo1.ResetTime = 1500;

            combo1.Name = "Flying Diarrhea Drops";
            combo1.Description = "A basic 3-strike combo with the final strike causing a medium force knockback";

            #endregion
        }
        public override void Update(GameTime gameTime)
        {
            CharacterPhysics.Apply(this, gameTime);

            HandleAnimations();

            currentAnimation.Animate(gameTime);

            // TODO: This can be done more smartly. See MenuScreen.cs
            if (!IsDisabled)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    SetState(Constants.CharacterState.Walking);

                    directionX = Constants.DirectionX.Left;
                }

                else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    SetState(Constants.CharacterState.Walking);

                    directionX = Constants.DirectionX.Right;
                }
                else
                {
                    SetState(Constants.CharacterState.Stopped);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    if(CharacterPhysics.OnGround(this))
                        SetState(Constants.CharacterState.Jumping);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    if (CharacterPhysics.OnLadder(this))
                        SetState(Constants.CharacterState.ClimbingDown);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Q))
                {
                    combo1.SetNextSkill();
                    UseSkill(combo1.ActiveSkill);
                }
            }
            combo1.Update(gameTime); 

            RegenerateHealth(gameTime);

            CleanActiveSkill();
        }

        private void HandleAnimations()
        {
            switch (state)
            {
                case Constants.CharacterState.Stopped:
                    jumpingRight.Reset();
                    jumpingLeft.Reset();
                    if (directionX == Constants.DirectionX.Left) { currentAnimation = stoppedLeft; }
                    else { currentAnimation = stoppedRight; }
                    break;
                case Constants.CharacterState.Walking:
                    jumpingRight.Reset();
                    jumpingLeft.Reset();
                    if (directionX == Constants.DirectionX.Left) { currentAnimation = walkLeft; }
                    else { currentAnimation = walkRight; }
                    break;
                case Constants.CharacterState.Jumping:
                    if (directionX == Constants.DirectionX.Left) { currentAnimation = jumpingLeft; }
                    else { currentAnimation = jumpingRight; }
                    break;
                case Constants.CharacterState.Falling:
                    if (directionX == Constants.DirectionX.Left) { currentAnimation = jumpingLeft; }
                    else { currentAnimation = jumpingRight; }
                    break;
            }
        }
    }
}
