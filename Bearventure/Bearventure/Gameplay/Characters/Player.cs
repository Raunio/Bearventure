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
        Animation climbing;
        Animation climbingStopped;
        Animation runRight;
        Animation runLeft;

        Texture2D comboSheet;
        Texture2D jumpSheet;
        Texture2D climbSheet;

        #region InputActions

        InputAction moveLeft;
        InputAction moveRight;
        InputAction jump;
        InputAction playerAttack;
        InputAction moveDown;
        InputAction moveUp;
        InputAction run;

        #endregion



        float jumpTimer = 0;
        float jumpFrequency = 500;

        CharacterSkillCombo combo1 = new CharacterSkillCombo();

        public Player(ContentManager content)
        {
            TAG = "Player";
            Name = "You";
            scale = 1f;
            this.spriteSheet = content.Load<Texture2D>("Sprites/kavelyfixed");
            comboSheet = content.Load<Texture2D>("Sprites/karhukombo1");
            jumpSheet = content.Load<Texture2D>("Sprites/hyppyfix");
            climbSheet = content.Load<Texture2D>("Sprites/karhuclimb2");
            IsActive = true;

            this.position = position;
            directionX = Constants.DirectionX.Right;


            InitControls();
            InitStats();
            InitAnimations();
            InitSkills();
        }

        #region ControlInitialization

        private void InitControls()
        {
            #region InputAction
            moveLeft = new InputAction(
        new Buttons[] { Buttons.DPadLeft, Buttons.LeftThumbstickLeft },
        new Keys[] { Keys.Left },
        false);
            moveRight = new InputAction(
        new Buttons[] { Buttons.DPadRight, Buttons.LeftThumbstickRight },
        new Keys[] { Keys.Right },
        false);
            jump = new InputAction(
        new Buttons[] { Buttons.A },
        new Keys[] { Keys.Up },
        true);
            moveUp = new InputAction(
        new Buttons[] { Buttons.DPadUp, Buttons.LeftThumbstickUp },
        new Keys[] { Keys.Up },
        false);
            playerAttack = new InputAction(
        new Buttons[] { Buttons.X },
        new Keys[] { Keys.Q },
        true);
            moveDown = new InputAction(
        new Buttons[] { Buttons.DPadDown, Buttons.LeftThumbstickDown },
        new Keys[] { Keys.Down },
        false);
            run = new InputAction(
        new Buttons[] { Buttons.B },
        new Keys[] { Keys.LeftShift },
        false);
            #endregion

        }
        #endregion
        private void InitStats()
        {
            acceleration = 1.75f;
            decceleration = 1.75f;
            walkSpeed = 12f;
            runSpeed = 20f;
            jumpStrenght = 20;

            maxHealth = 50;
            health = 50;

            mass = 150;

            BoundingBoxOffset = 12;

            ArmorType = Constants.ArmorType.Fur;
        }

        private void InitAnimations()
        {
            stoppedRight = new Animation(spriteSheet, 0, 88, 121, 12, 12, 50);
            stoppedLeft = new Animation(spriteSheet, 0, 88, 121, 9, 9, 50);
            walkRight = new Animation(spriteSheet, 0, 88, 121, 13, 21, 50);
            walkLeft = new Animation(spriteSheet, 0, 88, 121, 0, 8, 50, true);
            runRight = new Animation(spriteSheet, 0, 88, 121, 13, 21, 25);
            runLeft = new Animation(spriteSheet, 0, 88, 121, 0, 8, 25, true);

            jumpingRight = new Animation(jumpSheet, 0, 106, 120, 0, 3, 40, false, false);
            jumpingLeft = new Animation(jumpSheet, 0, 106, 120, 0, 3, 40, false, false);
            jumpingLeft.Effects = SpriteEffects.FlipHorizontally;

            climbing = new Animation(climbSheet, 0, 90, 135, 0, 4, 70);
            climbingStopped = new Animation(climbSheet, 0, 90, 135, 0, 4, 70, false, false);

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

            skill1.SkillSoundEffect = SoundEffectManager.Instance.KarhuCombo1;
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

            skill2.SkillSoundEffect = SoundEffectManager.Instance.KarhuCombo2;
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

            skill3.SkillSoundEffect = SoundEffectManager.Instance.KarhuCombo3;
            skill3.Acceleration = 0.25f;
            skill3.StartVelocity = new Vector2(8, -9);
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
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex playerIndex;

            if (state == Constants.CharacterState.Climbing && CharacterPhysics.OnLadder(this))
            {
                if (moveUp.Evaluate(input, ControllingPlayer, out playerIndex))
                {
                    velocity.Y = -walkSpeed;
                }
                else if (moveDown.Evaluate(input, ControllingPlayer, out playerIndex))
                {
                    velocity.Y = walkSpeed;
                }
                else
                    velocity = Vector2.Zero;
            }
            else if (state == Constants.CharacterState.Climbing && !CharacterPhysics.OnLadder(this))
            {
                if (jump.Evaluate(input, ControllingPlayer, out playerIndex))
                {
                    velocity.Y = -jumpStrenght;
                    SetState(Constants.CharacterState.Jumping);
                }
            }

            if (!IsDisabled && !CharacterPhysics.OnLadder(this))
            {
                if (moveLeft.Evaluate(input, ControllingPlayer, out playerIndex))
                {
                    if (run.Evaluate(input, ControllingPlayer, out playerIndex) && moveLeft.Evaluate(input, ControllingPlayer, out playerIndex))
                    {
                        SetState(Constants.CharacterState.Running);
                        directionX = Constants.DirectionX.Left;
                    }
                    else
                    {
                        SetState(Constants.CharacterState.Walking);
                        directionX = Constants.DirectionX.Left;
                    }
                }
                else if (moveRight.Evaluate(input, ControllingPlayer, out playerIndex))
                {
                    if (run.Evaluate(input, ControllingPlayer, out playerIndex) && moveRight.Evaluate(input, ControllingPlayer, out playerIndex))
                    {
                        SetState(Constants.CharacterState.Running);
                        directionX = Constants.DirectionX.Right;
                    }
                    else
                    {
                        SetState(Constants.CharacterState.Walking);
                        directionX = Constants.DirectionX.Right;
                    }
                }
                else
                {
                    SetState(Constants.CharacterState.Stopped);
                }
                if (jump.Evaluate(input, ControllingPlayer, out playerIndex))
                {
                    if (CharacterPhysics.OnGround(this) && jumpTimer >= jumpFrequency)
                    {
                        SetState(Constants.CharacterState.Jumping);
                        jumpTimer = 0;

                        SoundEffectManager.Instance.PlayKarhuJump();
                    }
                    else
                        SetState(Constants.CharacterState.Falling);
                }
                if (!CharacterPhysics.OnGround(this) && jump.Evaluate(input, ControllingPlayer, out playerIndex) && state == Constants.CharacterState.Jumping)
                {
                    SetState(Constants.CharacterState.DoubleJump);
                    jumpTimer = 0;
                    SoundEffectManager.Instance.PlayKarhuJump();
                    
                }
                else
                {
                    SetState(Constants.CharacterState.Falling);
                }
                if (playerAttack.Evaluate(input, ControllingPlayer, out playerIndex))
                {
                    combo1.SetNextSkill();
                    UseSkill(combo1.ActiveSkill);
                }
            }
            else
            {
                if (moveLeft.Evaluate(input, ControllingPlayer, out playerIndex) || moveRight.Evaluate(input, ControllingPlayer, out playerIndex) || jump.Evaluate(input, ControllingPlayer, out playerIndex) || moveDown.Evaluate(input, ControllingPlayer, out playerIndex))
                {
                    SetState(Constants.CharacterState.Climbing);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            CharacterPhysics.Apply(this, gameTime);

            jumpTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            HandleAnimations();

            currentAnimation.Animate(gameTime);

            PlayStepSoundEffects();

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
                case Constants.CharacterState.Running:
                    jumpingRight.Reset();
                    jumpingLeft.Reset();
                    if (directionX == Constants.DirectionX.Left) { currentAnimation = runLeft; }
                    else { currentAnimation = runRight; }
                    break;
                case Constants.CharacterState.Jumping:
                    if (directionX == Constants.DirectionX.Left) { currentAnimation = jumpingLeft; }
                    else { currentAnimation = jumpingRight; }
                    break;
                case Constants.CharacterState.Falling:
                    if (directionX == Constants.DirectionX.Left) { currentAnimation = jumpingLeft; }
                    else { currentAnimation = jumpingRight; }
                    break;
                case Constants.CharacterState.Climbing:
                    if (velocity.Y == 0)
                    {
                        currentAnimation = climbingStopped;
                        climbingStopped.GoToFrame(climbing.CurrentFrame);
                    }
                    else
                    {
                        currentAnimation = climbing;
                    }
                    break;
            }
        }

        private void PlayStepSoundEffects()
        {
            if (currentAnimation == walkLeft || currentAnimation == runLeft)
            {
                if (currentAnimation.CurrentFrame == 2 || currentAnimation.CurrentFrame == 6)
                {
                    if (currentAnimation.IsNewFrame)
                        SoundEffectManager.Instance.PlayStep();
                }
            }
            else if (currentAnimation == walkRight || currentAnimation == runRight)
            {
                if (currentAnimation.CurrentFrame == 15 || currentAnimation.CurrentFrame == 19)
                {
                    if (currentAnimation.IsNewFrame)
                        SoundEffectManager.Instance.PlayStep();
                }
            }
        }
    }
}
