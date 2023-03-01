using Microsoft.Xna.Framework;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandInputReaderLibrary;

namespace MonogameCommandInputs
{
    public class MonogameInputHost : InputHost
    {
        protected InputHandler input;

        public MonogameInputHost(Game game)
        {
            input = (InputHandler)game.Services.GetService<IInputHandler>();
            if (input == null)
            {
                input = new InputHandler(game);
                game.Components.Add(input);
            }
        }

        public override IHostPackage GetCurrentInputs()
        {
            IHostPackage package = base.GetCurrentInputs();

            return package;
        }
    }
}