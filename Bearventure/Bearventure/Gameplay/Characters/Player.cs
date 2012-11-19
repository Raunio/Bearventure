using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Bearventure.Engine.Effects;

namespace Bearventure
{
    public class Player : Character
    {
        Animation stoppedRight;
        Animation stoppedLeft;
        Animation walkRight;
        Animation walkLeft;

        CharacterSkill testSkill;

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

            testSkill = new CharacterSkill(this, new Animation(spriteSheet, 0, 88, 121, 0, 21, 20, false, false), 4000, 2);
            testSkill.Acceleration = 0.25f;
            testSkill.StartVelocity = new Vector2(15, -15);
            testSkill.UltimateVelocityX = 0;
            testSkill.AddEffect(VisualEffects.Test, Vector2.Zero);
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

            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                UseSkill(testSkill);

            if(activeSkill != null)
                activeSkill.Update(gameTime);


            HandleAnimations();
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
                }
            }
        }
    }
}
