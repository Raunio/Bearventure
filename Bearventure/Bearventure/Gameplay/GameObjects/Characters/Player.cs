using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Bearventure.Engine.Effects;
using Bearventure.Gameplay.Characters.Skills;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Bearventure.Gameplay.GameObjects;
using Microsoft.Xna.Framework;

namespace Bearventure.Gameplay.Characters
{
    public class Player : Character
    {
        CharacterAnimation stoppedRight;
        CharacterAnimation stoppedLeft;
        CharacterAnimation walkRight;
        CharacterAnimation walkLeft;
        CharacterAnimation jumpingRight;
        CharacterAnimation jumpingLeft;
        CharacterAnimation climbing;
        CharacterAnimation climbingStopped;
        CharacterAnimation runRight;
        CharacterAnimation runLeft;

        Texture2D comboSheet;
        Texture2D jumpSheet;
        Texture2D climbSheet;
        Texture2D puukkoSheet;

        #region InputActions

        InputAction moveLeft;
        InputAction moveRight;
        InputAction jump;
        InputAction playerAttack;
        InputAction moveDown;
        InputAction moveUp;
        InputAction run;
        InputAction stab;

        #endregion



        float jumpTimer = 0;
        float jumpFrequency = 150;

        CharacterSkillCombo combo1 = new CharacterSkillCombo();
        CharacterSkill puukotus;

        public Player(ContentManager content)
        {
            TAG = "Player";
            Name = "You";
            scale = 1f;
            this.spriteSheet = content.Load<Texture2D>("Sprites/kavelyfixed");
            comboSheet = content.Load<Texture2D>("Sprites/karhukombo1");
            jumpSheet = content.Load<Texture2D>("Sprites/hyppyfix");
            climbSheet = content.Load<Texture2D>("Sprites/karhuclimb2");
            puukkoSheet = content.Load<Texture2D>("Sprites/puukkorage");
            IsActive = true;

            this.position = Position;
            directionX = Constants.DirectionX.Right;

            AttachmentPoints = new AttachmentPoint[]
            {
                new AttachmentPoint(new Vector2(20, 20)),
                new AttachmentPoint(new Vector2(20, 30)),
                new AttachmentPoint(new Vector2(20, 40)),
                new AttachmentPoint(new Vector2(20, 50)),
                new AttachmentPoint(new Vector2(20, 60)),
            };


            InitControls(); 
            InitAnimations();
            InitStats();
            InitSkills();

            BoundingBoxSize = new Point(stoppedRight.FrameWidth - 15, stoppedLeft.FrameHeight);
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
        new Buttons[] { Buttons.B },
        new Keys[] { Keys.Q },
        true);
            moveDown = new InputAction(
        new Buttons[] { Buttons.DPadDown, Buttons.LeftThumbstickDown },
        new Keys[] { Keys.Down },
        false);
            run = new InputAction(
        new Buttons[] { Buttons.X },
        new Keys[] { Keys.LeftShift },
        false);
            stab = new InputAction(
         new Buttons[] { Buttons.LeftShoulder },
         new Keys[] { Keys.W },
         true);
            #endregion

        }
        #endregion
        private void InitStats()
        {
            acceleration = 1.75f;
            decceleration = 1.75f;
            walkSpeed = 12f;
            runSpeed = 20f;
            jumpStrenght = 25;

            maxHealth = 50;
            health = 50;

            mass = 150;

            //BoundingBoxOffset = 8;

            BoundingBoxSize = new Point(stoppedRight.FrameWidth, stoppedLeft.FrameHeight);

            MaxSkillResource = 100;

            ArmorType = Constants.ArmorType.Fur;
        }

        private void InitAnimations()
        {
            stoppedRight = new CharacterAnimation(spriteSheet, 0, 88, 121, 12, 12, 50, SpriteEffects.None, 0f, 0f, false, true);
            stoppedLeft = new CharacterAnimation(spriteSheet, 0, 88, 121, 9, 9, 50, SpriteEffects.None, 0f, 0f, false, true);
            stoppedRight.SetFixedBoundingBoxOffset(10);       
            stoppedLeft.SetFixedBoundingBoxOffset(3);
            walkRight = new CharacterAnimation(spriteSheet, 0, 88, 121, 13, 21, 50, SpriteEffects.None, 0f, 0f, false, true);
            walkRight.SetFixedBoundingBoxOffset(10);
            walkLeft = new CharacterAnimation(spriteSheet, 0, 88, 121, 0, 8, 50, SpriteEffects.None, 0f, 0f, true, true);
            walkLeft.SetFixedBoundingBoxOffset(3);
            runRight = new CharacterAnimation(spriteSheet, 0, 88, 121, 13, 21, 25, SpriteEffects.None, 0f, 0f, false, true);
            runRight.SetFixedBoundingBoxOffset(10);
            runLeft = new CharacterAnimation(spriteSheet, 0, 88, 121, 0, 8, 25, SpriteEffects.None, 0f, 0f, true, true);
            runLeft.SetFixedBoundingBoxOffset(3);

            jumpingRight = new CharacterAnimation(jumpSheet, 0, 106, 120, 0, 3, 40, SpriteEffects.None, 0f, 0f, false, false);
            jumpingRight.SetFixedBoundingBoxOffset(10);
            jumpingLeft = new CharacterAnimation(jumpSheet, 0, 106, 120, 0, 3, 40, SpriteEffects.FlipHorizontally, 0f, 0f, false, false);
            jumpingLeft.SetFixedBoundingBoxOffset(20);

            climbing = new CharacterAnimation(climbSheet, 0, 90, 135, 0, 4, 70, SpriteEffects.None, 0f, 0f, false, true);
            climbingStopped = new CharacterAnimation(climbSheet, 0, 90, 135, 0, 4, 70, SpriteEffects.None, 0f, 0f, false, false);

            ChangeAnimation(stoppedRight);
        }

        private void InitSkills()
        {
            #region Combo1 Initialization
            #region ComboSkill 1 Initialization

            CharacterAnimation StraightPunch_R = new CharacterAnimation(comboSheet, 0, 118, 120, 0, 3, 30, SpriteEffects.None, 0f, 0f, false, false);
            StraightPunch_R.FreezeFrames = new Animation.FrameFreezer
            {
                Frames = new List<int>
                {
                    2,
                },

                Amount = 150,
            };

            StraightPunch_R.CalculateBoundingBoxOffsets(BoundingBoxSize, Constants.DirectionX.Right);

            CharacterAnimation StraightPunch_L = new CharacterAnimation(comboSheet, 0, 118, 120, 0, 3, 30, SpriteEffects.FlipHorizontally, 0f, 0f, false, false);
            StraightPunch_L.FreezeFrames = new Animation.FrameFreezer
            {
                Frames = new List<int>
                {
                    2,
                },

                Amount = 150,
            };

            StraightPunch_L.CalculateBoundingBoxOffsets(BoundingBoxSize, Constants.DirectionX.Left);

            CharacterSkill skill1 = new CharacterSkill(this, StraightPunch_R, StraightPunch_L, 300, 3, Constants.DamageType.Crushing);

            skill1.SkillSoundEffect = SoundEffectManager.Instance.KarhuCombo1;
            skill1.Acceleration = 0.25f;
            skill1.StartVelocity = new Vector2(4, 0);
            skill1.UltimateVelocityX = 0;
            skill1.InflictForce = new Vector2(9, -7);
            skill1.DamagingFrames = new List<int>
            {
                2,
            };
            skill1.HitBoxPositions[0] = new Vector2(30, -10);
            skill1.HitBoxHeight = 25;
            skill1.HitBoxWidth = 25;

            #endregion
            #region ComboSkill 2 Initialization
            CharacterAnimation SweepingPunch_R = new CharacterAnimation(comboSheet, 1, 118, 120, 0, 5, 30, SpriteEffects.None, 0f, 0f, false, false);
            SweepingPunch_R.FreezeFrames = new Animation.FrameFreezer
            {
                Frames = new List<int>
                {
                    3,
                    4,
                },

                Amount = 70,
            };

            SweepingPunch_R.CalculateBoundingBoxOffsets(BoundingBoxSize, Constants.DirectionX.Right);

            CharacterAnimation SweepingPunch_L = new CharacterAnimation(comboSheet, 1, 118, 120, 0, 5, 30, SpriteEffects.FlipHorizontally, 0f, 0f, false, false);
            SweepingPunch_L.FreezeFrames = new Animation.FrameFreezer
            {
                Frames = new List<int>
                {
                    3,
                    4,
                },

                Amount = 70,
            };

            SweepingPunch_L.CalculateBoundingBoxOffsets(BoundingBoxSize, Constants.DirectionX.Left);

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

            CharacterAnimation UpperCut_R = new CharacterAnimation(comboSheet, 2, 118, 120, 3, 7, 25, SpriteEffects.None, 0f, 0f, false, false);
            UpperCut_R.FreezeFrames = new Animation.FrameFreezer
            {
                Frames = new List<int>
                {
                    5,
                },

                Amount = 150,
            };
            CharacterAnimation UpperCut_L = new CharacterAnimation(comboSheet, 2, 118, 120, 2, 7, 25, SpriteEffects.FlipHorizontally, 0f, 0f, false, false);

            UpperCut_L.CalculateBoundingBoxOffsets(BoundingBoxSize, Constants.DirectionX.Left);

            UpperCut_L.FreezeFrames = new Animation.FrameFreezer
            {
                Frames = new List<int>
                {
                    5,
                },

                Amount = 150,
            };

            UpperCut_R.CalculateBoundingBoxOffsets(BoundingBoxSize, Constants.DirectionX.Right);

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

            #region Puukko initialization

            CharacterAnimation puukko_R = new CharacterAnimation(puukkoSheet, 0, 156, 121, 0, 7, 25, SpriteEffects.None, 0f, 0f, false, false);
            puukko_R.CalculateBoundingBoxOffsets(BoundingBoxSize, Constants.DirectionX.Right);
            puukko_R.SetAnatomicInfo(Constants.KarhuPuukkoAnatomy, SpriteEffects.None);

            CharacterAnimation puukko_L = new CharacterAnimation(puukkoSheet, 0, 156, 121, 0, 7, 25, SpriteEffects.None, 0f, 0f, false, false);
            puukko_L.CalculateBoundingBoxOffsets(BoundingBoxSize, Constants.DirectionX.Left);
            puukko_L.SetAnatomicInfo(Constants.KarhuPuukkoAnatomy, SpriteEffects.FlipHorizontally);

            puukko_L.Effects = SpriteEffects.FlipHorizontally;

            puukko_L.LoopFrames = new Animation.FrameLooper
            {
                startFrame = 4,
                endFrame = 7,
                loopAmount = 4,
            };

            puukko_R.LoopFrames = new Animation.FrameLooper
            {
                startFrame = 4,
                endFrame = 7,
                loopAmount = 4,
            };

            puukko_L.FreezeFrames = new Animation.FrameFreezer()
            {
                Frames = new List<int>()
                {
                    2,
                    3,
                },

                Amount = 120,
            };

            puukko_R.FreezeFrames = new Animation.FrameFreezer()
            {
                Frames = new List<int>()
                {
                    2,
                    3,
                },

                Amount = 120,
            };

            puukko_R.ReverseAtEnd = true;
            puukko_L.ReverseAtEnd = true;

            puukotus = new CharacterSkill(this, puukko_R, puukko_L, 1000, 5, Constants.DamageType.Piercing);
            puukotus.StartVelocity = new Vector2(0, 0);
            puukotus.DamagingFrames = new List<int>()
            {
                4,
            };
            puukotus.HitBoxPositions[0] = new Vector2(17, -37);
            
            //puukotus.AddEffect("VisualEffects/blood2", new Vector2(17, -37), 4);
            puukotus.AddEffect("VisualEffects/blood2", Constants.CharacterBodyPart.LeftEye, 4);
            puukotus.VisualEffectLifetime = 150;

            puukotus.SkillSoundEffect = SoundEffectManager.Instance.Puukotus;

            puukotus.TargetingType = Constants.SkillTarget.Self;

            puukotus.UltimateVelocityX = 0f;
            puukotus.Acceleration = 1f;
            puukotus.Cost = -25;
            

            #endregion
        }
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (state == Constants.CharacterState.Knocked || state == Constants.CharacterState.Disabled)
                return;

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
                if (!CharacterPhysics.OnGround(this) && jump.Evaluate(input, ControllingPlayer, out playerIndex) && state == Constants.CharacterState.Jumping && jumpTimer >= jumpFrequency)
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
                else if (stab.Evaluate(input, ControllingPlayer, out playerIndex))
                {
                    UseSkill(puukotus);
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
            CurrentAnimation.Animate(gameTime);

            HandleAnimations();

            CharacterPhysics.Apply(this, gameTime);

            jumpTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            PlayStepSoundEffects();

            combo1.Update(gameTime);

            puukotus.Update(gameTime);

            RegenerateHealth(gameTime);

            CleanActiveSkill();
        }

        private void HandleAnimations()
        {
            switch (state)
            {
                case Constants.CharacterState.Stopped:
                    if (directionX == Constants.DirectionX.Left) { ChangeAnimation(stoppedLeft); }
                    else { ChangeAnimation(stoppedRight); }
                    break;
                case Constants.CharacterState.Walking:
                    if (directionX == Constants.DirectionX.Left) { ChangeAnimation(walkLeft); }
                    else { ChangeAnimation(walkRight); }
                    break;
                case Constants.CharacterState.Running:
                    if (directionX == Constants.DirectionX.Left) { ChangeAnimation(runLeft); }
                    else { ChangeAnimation(runRight); }
                    break;
                case Constants.CharacterState.Jumping:
                    if (directionX == Constants.DirectionX.Left) { ChangeAnimation(jumpingLeft); }
                    else { ChangeAnimation(jumpingRight); }
                    break;
                case Constants.CharacterState.Falling:
                    if (directionX == Constants.DirectionX.Left) { ChangeAnimation(jumpingLeft); }
                    else { ChangeAnimation(jumpingRight); }
                    break;
                case Constants.CharacterState.Climbing:
                    if (velocity.Y == 0)
                    {
                        ChangeAnimation(climbingStopped);
                        climbingStopped.GoToFrame(climbing.CurrentFrame);
                    }
                    else
                    {
                        ChangeAnimation(climbing);
                    }
                    break;
            }

            BoundingBoxAnimationOffset = CurrentCharacterAnimation.BoundingBoxOffset;
        }

        private void PlayStepSoundEffects()
        {
            if (CurrentAnimation == walkLeft || CurrentAnimation == runLeft)
            {
                if (CurrentAnimation.CurrentFrame == 2 || CurrentAnimation.CurrentFrame == 6)
                {
                    if (CurrentAnimation.IsNewFrame)
                        SoundEffectManager.Instance.PlayStep();
                }
            }
            else if (CurrentAnimation == walkRight || CurrentAnimation == runRight)
            {
                if (CurrentAnimation.CurrentFrame == 15 || CurrentAnimation.CurrentFrame == 19)
                {
                    if (CurrentAnimation.IsNewFrame)
                        SoundEffectManager.Instance.PlayStep();
                }
            }
        }
    }
}
