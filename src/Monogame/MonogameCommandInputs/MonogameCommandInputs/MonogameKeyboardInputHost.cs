using LaneLibrary;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandInputReaderLibrary;

namespace MonogameCommandInputs
{
    public class MonogameKeyboardInputHost : MonogameInputHost
    {
        KeyboardHandler kb;
        public MonogameKeyboardInputHost(Game game) : base(game) 
        {
            kb = input.KeyboardState;
        }

        public override IHostPackage GetCurrentInputs()
        {
            IHostPackage package = base.GetCurrentInputs();

            bool left = kb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A);
            bool right = kb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D);

            int leftright = right.ToInt() - left.ToInt();

            bool up = kb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) || kb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space);
            bool down = kb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S);

            int updown = up.ToInt() - down.ToInt();

            package.UpDown = updown;
            package.LeftRight = leftright;

            if (kb.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.J))
            {
                Punch p = new Punch();
                p.State = IButton.ButtonState.Pressed;
                if (kb.IsHoldingKey(Microsoft.Xna.Framework.Input.Keys.J))
                {
                    p.State = IButton.ButtonState.Held;
                }
                package.Buttons.Add(p);
            }
            if (kb.HasReleasedKey(Microsoft.Xna.Framework.Input.Keys.J))
            {
                Punch p = new Punch();
                p.State = IButton.ButtonState.Released;
                package.Buttons.Add(p);
            }

            return package;
        }
    }
}