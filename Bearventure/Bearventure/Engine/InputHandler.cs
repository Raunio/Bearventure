using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Bearventure.Gameplay.Characters;

namespace Bearventure.Engine
{
    public class InputHandler
    {
        InputAction moveLeft;
        InputAction moveRight;
        InputAction jump;
        InputAction moveDown;
        InputAction moveUp;
        InputAction run;

        InputAction useSkillCombo;
        InputAction useSkill0;
        InputAction useSkill1;
        InputAction useSkill2;

        public PlayerIndex? ControllingPlayer
        {
            get { return controllingPlayer; }
            internal set { controllingPlayer = value; }
        }

        PlayerIndex? controllingPlayer;

        public void InitControls()
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
                useSkillCombo = new InputAction(
            new Buttons[] { Buttons.Y },
            new Keys[] { Keys.Q, Keys.LeftControl },
            true);
                moveDown = new InputAction(
            new Buttons[] { Buttons.DPadDown, Buttons.LeftThumbstickDown },
            new Keys[] { Keys.Down },
            false);
                run = new InputAction(
            new Buttons[] { Buttons.LeftTrigger },
            new Keys[] { Keys.LeftShift },
            false);
                useSkill0 = new InputAction(
             new Buttons[] { Buttons.LeftShoulder },
             new Keys[] { Keys.W },
             true);
                #endregion
        }

        public void HandleInput(InputState input, Player player)
        {
            PlayerIndex playerIndex;

            if (moveLeft.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                if (run.Evaluate(input, ControllingPlayer, out playerIndex) && moveLeft.Evaluate(input, ControllingPlayer, out playerIndex))
                {
                    player.RunLeft();
                }
                else
                {
                    player.WalkLeft();
                }
            }
            else if (moveRight.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                if (run.Evaluate(input, ControllingPlayer, out playerIndex) && moveRight.Evaluate(input, ControllingPlayer, out playerIndex))
                {
                    player.RunRight();
                }
                else
                {
                    player.WalkRight();
                }
            }
            else
            {
                player.Stop();
            }
            if (jump.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                player.Jump();
            }
            if (useSkillCombo.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                player.UseSkillCombo();
            }
            else if (useSkill0.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                player.UseSkill(0);
            }
            
        }
    }
}
