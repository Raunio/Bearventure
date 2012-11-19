using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Bearventure.Engine.Effects;
using Bearventure.Gameplay.Characters.Skills;
using System.Collections.Generic;

namespace Bearventure.Gameplay.Characters
{
    public class Player : Character
    {
        Animation stoppedRight;
        Animation stoppedLeft;
        Animation walkRight;
        Animation walkLeft;

        CharacterSkillCombo combo1 = new CharacterSkillCombo();

        public Player(Texture2D spriteSheet)
        {
            scale = 1f;
            this.spriteSheet = spriteSheet;
            stoppedRight = new Animation(spriteSheet, 0, 88, 121, 12, 12, 70);
            stoppedLeft = new Animation(spriteSheet, 0, 88, 121, 9, 9, 70);
            walkRight = new Animation(spriteSheet, 0, 88, 121, 13, 21, 70);
            walkLeft = new Animation(spriteSheet, 0, 88, 121, 0, 8, 70, true);
            this.position = position;
            direction = Constants.Direction.Right;
            currentAnimation = stoppedRight;

            acceleration = 1f;
            deacceleration = 1f;
            walkSpeed = 10f;
            jumpStrenght = 18;

            BoundingBoxOffset = 15;

            CharacterSkill skill1 = new CharacterSkill(this, new Animation(spriteSheet, 0, 88, 121, 10, 12, 70, false, false), 200, 2);
            skill1.SoundEffectAsset = Constants.KarhuHit1;
            skill1.Acceleration = 0.25f;
            skill1.StartVelocity = new Vector2(-9, -5);
            skill1.UltimateVelocityX = 0;
            skill1.DamagingFrames = new List<int>
            {
                11,
                12,
            };
            skill1.HitBoxPositions[0] = new Vector2(50, 50);
            skill1.HitBoxPositions[1] = new Vector2(50, 50);
            skill1.HitBoxHeight = 100;
            skill1.HitBoxWidth = 200;

            CharacterSkill skill2 = new CharacterSkill(this, new Animation(spriteSheet, 0, 88, 121, 6, 8, 70, false, false), 200, 2);
            skill2.SoundEffectAsset = Constants.KarhuHit2;
            skill2.Acceleration = 0.25f;
            skill2.StartVelocity = new Vector2(9, -1);
            skill2.UltimateVelocityX = 0;
            skill2.DamagingFrames = new List<int>
            {
                11,
                12,
            };
            skill2.HitBoxPositions[0] = new Vector2(50, 50);
            skill2.HitBoxPositions[1] = new Vector2(50, 50);
            skill2.HitBoxHeight = 100;
            skill2.HitBoxWidth = 200;

            CharacterSkill skill3 = new CharacterSkill(this, new Animation(spriteSheet, 0, 88, 121, 12, 14, 70, false, false), 200, 2);
            skill3.SoundEffectAsset = Constants.KarhuHit3;
            skill3.Acceleration = 0.25f;
            skill3.StartVelocity = new Vector2(0, -12);
            skill3.UltimateVelocityX = 0;
            skill3.DamagingFrames = new List<int>
            {
                11,
                12,
            };
            skill3.HitBoxPositions[0] = new Vector2(50, 50);
            skill3.HitBoxPositions[1] = new Vector2(50, 50);
            skill3.HitBoxHeight = 100;
            skill3.HitBoxWidth = 200;

            combo1.SkillArray = new List<CharacterSkill>
            {
                skill1,
                skill2,
                skill3,
            };

            combo1.ResetTime = 500;
        }

        public override void Update(GameTime gameTime)
        {
            currentAnimation.Animate(gameTime);
            CharacterPhysics.Apply(this, gameTime);

            // TODO: This can be done more smartly. See MenuScreen.cs

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                SetState(Constants.CharacterState.Walking);

                direction = Constants.Direction.Left;
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                SetState(Constants.CharacterState.Walking);

                direction = Constants.Direction.Right;
            }
            else
            {
                SetState(Constants.CharacterState.Stopped);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                SetState(Constants.CharacterState.Jumping);

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                combo1.SetNextSkill();
                UseSkill(combo1.ActiveSkill);
            }

            combo1.Update(gameTime);

            HandleAnimations();

            RegenerateHealth(gameTime);

            CleanActiveSkill();
        }

        private void HandleAnimations()
        {
            switch (state)
            {
                case Constants.CharacterState.Stopped:
                    if (direction == Constants.Direction.Left) { currentAnimation = stoppedLeft; }
                    else { currentAnimation = stoppedRight; }
                    break;
                case Constants.CharacterState.Walking:
                    if (direction == Constants.Direction.Left) { currentAnimation = walkLeft; }
                    else { currentAnimation = walkRight; }
                    break;
            }
        }

        private void SetState(Constants.CharacterState newState)
        {
            if (state != Constants.CharacterState.UsingSkill && state != Constants.CharacterState.Disabled)
            {
                switch (newState)
                {
                    case Constants.CharacterState.Walking:
                        if (CharacterPhysics.OnGround(this))
                            state = newState;
                        else
                            return;
                        break;
                    case Constants.CharacterState.Stopped:
                        if (CharacterPhysics.OnGround(this))
                            state = newState;
                        else
                            return;
                        break;
                    case Constants.CharacterState.Jumping:
                        state = newState;
                        break;
                    case Constants.CharacterState.UsingSkill:
                        state = newState;
                        break;
                }
            }
        }
    }
}
