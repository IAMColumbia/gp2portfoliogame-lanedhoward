using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandInputReaderLibrary;
using System;
using System.Linq;

public class FighterInputReceiver : IInputReceiver
{
    protected FighterMain fighter;
    protected FighterInputHost inputHost;
    protected InputReader inputReader;

    protected IButton lastButton;
    protected float lastButtonTime;
    protected float buttonBufferTimeMax;

    public int UpDown;
    public int LeftRight;

    public IReadPackage bufferedInput;
    public float bufferedAttackTime;
    public float bufferedAttackTimeMax;

    public FighterInputReceiver(FighterMain _fighter, FighterInputHost _inputHost, InputReader _inputReader)
    {
        fighter = _fighter;
        inputHost = _inputHost;
        inputReader = _inputReader;

        UpDown = 0;
        LeftRight = 0;

        lastButton = null;
        lastButtonTime = 0;
        buttonBufferTimeMax = InputReader.TimeBetweenNonSequentialInputs;

        bufferedAttackTime = 0f;
        bufferedAttackTimeMax = 0.2f; // TODO: Convert this into "frames" like the input reader uses
    }

    public bool CheckForInputs()
    {
        ManageBuffer();

        IReadPackage package = inputReader.Tick(TimeSpan.FromSeconds(Time.deltaTime));

        if (package == null)
        {
            return false;
        }

        UpDown = package.mostRecentInputs.UpDown;
        LeftRight = package.mostRecentInputs.LeftRight;
        
        // then we'll handle gestures and buttons and such

        if (package.buttons.Count == 0)
        {
            // no buttons currently pressed, so we check if there were any buttons pressed in the buffered input
            // we do this so that you can press your button a little bit before completing a gesture
            // and have it register still

            // might still need to rework this to have proper kara canceling
            // and also if i decide to have 2 button attacks
            /*
            if (bufferedInput != null)
            {
                if (package.TimeReceived - bufferedInput.TimeReceived <= buttonBufferTimeMax)
                {
                    foreach (var button in package.buttons)
                    {
                        button.State = IButton.ButtonState.Held;
                    }
                    package.buttons.AddRange(bufferedInput.buttons);
                }
            }
            */
        }


        if (package.buttons.Count > 0)
        {
            bufferedInput = package;
            bufferedAttackTime = 0;
        }

        return true;
    }

    protected void ManageBuffer()
    {
        if (bufferedInput != null)
        {
            bufferedAttackTime += Time.deltaTime;

            if (bufferedAttackTime >= bufferedAttackTimeMax)
            {
                bufferedInput = null;
            }
        }
    }

    public GameAttack ParseAttack(IReadPackage package)
    {
        NoGesture noGesture = new NoGesture();

        IGesture currentGesture = noGesture;

        GameMoveInput currentMoveInput = new GameMoveInput(currentGesture, package.buttons[0]);


        for (int i = 0; i < package.gestures.Count; i++)
        {
            if (package.gestures.Count > 0)
            {
                currentGesture = package.gestures[i];
            }

            currentMoveInput.gesture = currentGesture;

            foreach (GameAttack attack in fighter.fighterAttacks)
            {
                if (attack.CanExecute(currentMoveInput, fighter))
                {
                    // found our attack!
                    return attack;
                }
            }
        }
        return null;
    }

    public void UpdateFacingDirection()
    {
        inputReader.ChangeFacingDirection(fighter.facingDirection);
    }
}
