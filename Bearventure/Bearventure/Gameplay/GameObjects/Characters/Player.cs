using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Bearventure.Engine.Effects;
using Bearventure.Gameplay.Characters.Skills;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Bearventure.Gameplay.GameObjects;
using Microsoft.Xna.Framework;
using Bearventure.Engine.Audio;
using Bearventure.Gameplay.GameObjects.Characters.Skills;

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

        private CharacterSkillCombo activeCombo = new CharacterSkillCombo();
        private CharacterSkill[] usableSkills;

        public CharacterSkillCombo ActiveCombo
        {
            get
            {
                return activeCombo;
            }
        }

        public CharacterSkill[] UsableSkills
        {
            get
            {
                return usableSkills;
            }
        }

        public Player(ContentManager content)
        {
            TAG = "Player";
            Name = "You";
            scale = 1f;

            spriteSheet = content.Load<Texture2D>(Constants.PlayerSpriteSheet);

            IsActive = true;

            this.position = Position;
            directionX = Constants.DirectionX.Right;

            AttachmentPoints = new AttachmentPoint[]
            {
                new AttachmentPoint(new Vector2(-50, -70)),
                new AttachmentPoint(new Vector2(-50, -30)),
                new AttachmentPoint(new Vector2(-50, -40)),
                new AttachmentPoint(new Vector2(-50, -50)),
                new AttachmentPoint(new Vector2(-50, -60)),
            };


            InitControls(); 
            InitAnimations();
            InitStats();
            

            BoundingBoxSize = new Point(stoppedRight.FrameWidth - 15, stoppedLeft.FrameHeight);

            usableSkills = new CharacterSkill[3];

            PlayerSkills.InitSelectedSkills(content, this);

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
        new Buttons[] { Buttons.B },
        new Keys[] { Keys.Q, Keys.LeftControl },
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
         new Keys[] { Keys.P },
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
            jumpStrenght = 30;

            maxHealth = 50;
            health = 50;

            mass = 150;

            BoundingBoxOffset = 8;

            BoundingBoxSize = new Point(stoppedRight.FrameWidth, stoppedLeft.FrameHeight);

            MaxSkillResource = 100;

            ArmorType = Constants.ArmorType.Fur;
        }

        private void InitAnimations()
        {
            stoppedRight = new CharacterAnimation(spriteSheet, 0, 134, 190, 0, 0, 50, SpriteEffects.None, 0f, 0f, false, true);
            stoppedLeft = new CharacterAnimation(spriteSheet, 0, 134, 190, 0, 0, 50, SpriteEffects.FlipHorizontally, 0f, 0f, false, true);
            stoppedRight.SetFixedBoundingBoxOffset(10);       
            stoppedLeft.SetFixedBoundingBoxOffset(3);
            walkRight = new CharacterAnimation(spriteSheet, 1, 134, 190, 0, 5, 100, SpriteEffects.None, 0f, 0f, false, true);
            walkRight.SetFixedBoundingBoxOffset(10);
            walkLeft = new CharacterAnimation(spriteSheet, 1, 134, 190, 0, 5, 100, SpriteEffects.FlipHorizontally, 0f, 0f, false, true);
            walkLeft.SetFixedBoundingBoxOffset(3);
            runRight = new CharacterAnimation(spriteSheet, 1, 134, 190, 0, 5, 65, SpriteEffects.None, 0f, 0f, false, true);
            runRight.SetFixedBoundingBoxOffset(10);
            runLeft = new CharacterAnimation(spriteSheet, 1, 134, 190, 0, 5, 65, SpriteEffects.FlipHorizontally, 0f, 0f, false, true);
            runLeft.SetFixedBoundingBoxOffset(3);

            jumpingRight = new CharacterAnimation(spriteSheet, 5, 167, 190, 0, 2, 40, SpriteEffects.None, 0f, 0f, false, false);
            jumpingRight.SetFixedBoundingBoxOffset(10);
            jumpingLeft = new CharacterAnimation(spriteSheet, 5, 167, 190, 0, 2, 40, SpriteEffects.FlipHorizontally, 0f, 0f, false, false);
            jumpingLeft.SetFixedBoundingBoxOffset(20);

            climbing = new CharacterAnimation(spriteSheet, 0, 90, 134, 0, 4, 70, SpriteEffects.None, 0f, 0f, false, true);
            climbingStopped = new CharacterAnimation(spriteSheet, 0, 90, 134, 0, 4, 70, SpriteEffects.None, 0f, 0f, false, false);

            ChangeAnimation(stoppedRight);
        }

        private void InitSkills()
        {
            activeCombo = PlayerSkills.SevenDragons;
            usableSkills[0] = PlayerSkills.TestSkill;
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
                    activeCombo.SetNextSkill();
                    UseSkill(activeCombo.ActiveSkill);
                }
                else if (stab.Evaluate(input, ControllingPlayer, out playerIndex))
                {
                    //UseSkill(puukotus);
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

            activeCombo.Update(gameTime);

            for (int i = 0; i < usableSkills.Length; i++)
            {
                if(usableSkills[i] != null)
                    usableSkills[i].Update(gameTime);
            }

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
                if (CurrentAnimation.CurrentFrame == 2 || CurrentAnimation.CurrentFrame == 5)
                {
                    if (CurrentAnimation.IsNewFrame)
                        SoundEffectManager.Instance.PlayStep();
                }
            }
            else if (CurrentAnimation == walkRight || CurrentAnimation == runRight)
            {
                if (CurrentAnimation.CurrentFrame == 2 || CurrentAnimation.CurrentFrame == 5)
                {
                    if (CurrentAnimation.IsNewFrame)
                        SoundEffectManager.Instance.PlayStep();
                }
            }
        }
    }
}
