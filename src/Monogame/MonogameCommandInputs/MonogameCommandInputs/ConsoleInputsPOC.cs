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
        InputHandler input;

        MonogameKeyboardInputHost kbInputHost;
        MonogameControllerInputHost gpInputHost;
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

            input = (InputHandler)game.Services.GetService<IInputHandler>();
            if (input == null)
            {
                input = new InputHandler(game);
                game.Components.Add(input);
            }

            kbInputHost = new MonogameKeyboardInputHost(game);
            gpInputHost = new MonogameControllerInputHost(game);
            
            inputReader = new InputReader(gpInputHost);

            tickRate = 1d / 60d;
            tickTimer = 0;
            totalTicks = 0;

            console.DebugTextOutput.Add("LeftRight", "0");
            console.DebugTextOutput.Add("UpDown", "0");
        }

        public override void Initialize()
        {
            base.Initialize();

            inputReader.SetPossibleGestures(CommandInputReaderLibrary.Gestures.DefaultGestures.GetDefaultGestures());
            console.DebugTextOutput["FacingDirection"] = inputReader.GetFacingDirection().ToString();
            console.DebugTextOutput["ControlType"] = inputReader.GetInputHost().GetType().Name;

        }

        public override void Update(GameTime gameTime)
        {
            if (input.WasPressed(0, InputHandler.ButtonType.B, Microsoft.Xna.Framework.Input.Keys.F))
            {
                inputReader.ChangeFacingDirection();
                console.DebugTextOutput["FacingDirection"] = inputReader.GetFacingDirection().ToString();
            }
            if (input.WasPressed(0, InputHandler.ButtonType.RightShoulder, Microsoft.Xna.Framework.Input.Keys.G))
            {
                if (inputReader.GetInputHost() == kbInputHost)
                {
                    inputReader.SetInputHost(gpInputHost);
                }
                else
                {
                    inputReader.SetInputHost(kbInputHost);
                }
                console.DebugTextOutput["ControlType"] = inputReader.GetInputHost().GetType().Name;
            }

            /*
            tickTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (tickTimer >= tickRate)
            {
                tickTimer -= tickRate;
                totalTicks++;
                Tick();
            }
            */

            Tick(gameTime);

            base.Update(gameTime);
        }

        private void Tick(GameTime gameTime)
        {
            IReadPackage package = inputReader.Tick(gameTime.ElapsedGameTime);

            if (package != null)
            {
                string output = ((ReadablePackage)package.mostRecentInputs).TimeReceived + ":    ";
                if (package.gestures.Count > 0)
                {
                    IReadableGesture gesture = package.gestures.First();
                    output += gesture.GetType().Name;
                }
                else
                {
                    output += "No Gesture";
                }

                output += "  /  ";
                
                if (package.buttons.Count > 0)
                {
                    var b = package.buttons.First();

                    output += b.GetType().Name;

                }
                else
                {
                    output += "No Button";
                }

                console.GameConsoleWrite(output);
                console.DebugTextOutput["LeftRight"] = package.mostRecentInputs.LeftRight.ToString();
                console.DebugTextOutput["UpDown"] = package.mostRecentInputs.UpDown.ToString();
                
            }

        }

    }
}
