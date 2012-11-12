using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bearventure
{
    public class Player : Character
    {
        Animation stoppedRight;
        Animation stoppedLeft;
        Animation walkRight;
        Animation walkLeft;

        public Player(Texture2D spriteSheet)
        {
            scale = 1f;
            this.spriteSheet = spriteSheet;
            stoppedRight = new Animation(spriteSheet, 0, 88, 121, 12, 12, 70);
            stoppedLeft = new Animation(spriteSheet, 0, 88, 121, 9, 9, 70);
            walkRight = new Animation(spriteSheet, 0, 88, 121, 13, 21, 70);
            walkLeft = new Animation(spriteSheet, 0, 88, 121, 0, 8, 70, true);

            direction = Constants.Direction.Right;
            currentAnimation = stoppedRight;

            this.position = new Vector2(100, 100);

            acceleration = 1f;
            deacceleration = 1f;
            walkSpeed = 10f;
            jumpStrenght = 18;

            BoundingBox_Offset = 15;
        }

        public override void Update(GameTime gameTime)
        { 
            currentAnimation.Animate(gameTime);
            CharacterPhysics.Apply(this, gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                state = Constants.CharacterState.Walking;

                direction = Constants.Direction.Left;
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                state = Constants.CharacterState.Walking;

                direction = Constants.Direction.Right;
            }
            else
            {
                state = Constants.CharacterState.Stopped;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                state = Constants.CharacterState.Jumping;

            

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
    }
}
