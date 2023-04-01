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
        buttonBufferTimeMax = InputReader.TimeBetweenSequentialInputs; // might need its own value later

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

        if (package.buttons.Count > 0)
        {
            lastButton = package.buttons.Dequeue();
            // probably gotta do something with this buffer later
            //lastButtonTime = ((ReadablePackage)package.mostRecentInputs).TimeReceived;
        }
        else
        {
            lastButton = null;
        }

        if (lastButton != null)
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

        GameMoveInput currentMoveInput = new GameMoveInput(currentGesture, lastButton);

        for (int i = 0; i < package.gestures.Count + 1; i++)
        {
            //for each gesture ( and once more for no gesture )

            if (package.gestures.Count > 0)
            {
                currentGesture = package.gestures.Dequeue();
            }
            else
            {
                currentGesture = noGesture;
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
}
