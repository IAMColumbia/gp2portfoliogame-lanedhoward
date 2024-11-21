using LaneLibrary;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandInputReaderLibrary;
using Microsoft.Xna.Framework.Input;

namespace MonogameCommandInputs
{
    public class MonogameControllerInputHost : MonogameInputHost
    {
        
        public MonogameControllerInputHost(Game game) : base(game) 
        {
            
        }

        public override IHostPackage GetCurrentInputs()
        {
            IHostPackage package = base.GetCurrentInputs();

            GamePadState gp = input.ButtonHandler.GamePadStates[0];

            float deadzone = 0.4f;

            bool left = gp.DPad.Left == ButtonState.Pressed || gp.ThumbSticks.Left.X < deadzone * -1;
            bool right = gp.DPad.Right == ButtonState.Pressed || gp.ThumbSticks.Left.X > deadzone * 1;


            int leftright = right.ToInt() - left.ToInt();

            bool up = gp.DPad.Up == ButtonState.Pressed || gp.ThumbSticks.Left.Y > deadzone * 1;
            bool down = gp.DPad.Down == ButtonState.Pressed || gp.ThumbSticks.Left.Y < deadzone * -1;

            int updown = up.ToInt() - down.ToInt();

            package.UpDown = updown;
            package.LeftRight = leftright;

            if (input.WasButtonPressed(0, InputHandler.ButtonType.X))
            {
                Punch p = new Punch();
                p.State = IButton.ButtonState.Pressed;
                /*
                if (input.WasButtonHeld(0, InputHandler.ButtonType.X))
                {
                    p.State = IButton.ButtonState.Held;
                }
                */
                package.Buttons.Add(p);
            }
            /*
            if (input.WasButtonReleased(0, InputHandler.ButtonType.X))
            {
                Punch p = new Punch();
                p.State = IButton.ButtonState.Pressed;
                package.Buttons.Add(p);
            }
            */

            return package;
        }
    }
}