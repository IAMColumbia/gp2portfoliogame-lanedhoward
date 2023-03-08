using CommandInputReaderLibrary;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameCommandInputs
{
    public class ConsoleInputsPOC : GameComponent
    {
        GameConsole console;

        MonogameKeyboardInputHost inputHost;
        InputReader inputReader;

        double tickRate;
        double tickTimer;
        int totalTicks;

        public ConsoleInputsPOC(Game game) : base(game)
        {
            console = (GameConsole)game.Services.GetService<IGameConsole>();
            if (console == null)
            {
                console = new GameConsole(game);
                game.Components.Add(console);
            }

            inputHost = new MonogameKeyboardInputHost(game);
            inputReader = new InputReader(inputHost);

            tickRate = 1d / 60d;
            tickTimer = 0;
            totalTicks = 0;
        }

        public override void Initialize()
        {
            base.Initialize();

            inputReader.SetPossibleGestures(CommandInputReaderLibrary.Gestures.DefaultGestures.GetDefaultGestures());

        }

        public override void Update(GameTime gameTime)
        {
            tickTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (tickTimer >= tickRate)
            {
                tickTimer -= tickRate;
                totalTicks++;
                Tick();
            }

            base.Update(gameTime);
        }

        private void Tick()
        {
            IReadPackage package = inputReader.Tick();

            if (package != null)
            {
                
                if (package.gestures.Count > 0)
                {
                    IReadableGesture gesture = package.gestures.Dequeue();
                    console.GameConsoleWrite(gesture.GetType().Name + " / " + totalTicks);
                }
                else
                {
                    console.GameConsoleWrite("no gesture" + " / " + totalTicks);
                }

            }

        }

    }
}
