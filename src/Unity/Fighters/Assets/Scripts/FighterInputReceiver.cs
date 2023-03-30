using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandInputReaderLibrary;
using System;
using System.Linq;

public class FighterInputReceiver : IInputReceiver
{
    protected FighterInputHost inputHost;
    protected InputReader inputReader;

    protected IButton lastButton;
    protected float lastButtonTime;
    protected float buttonBufferTimeMax;

    public int UpDown;
    public int LeftRight;

    public FighterInputReceiver(FighterInputHost _inputHost, InputReader _inputReader)
    {
        inputHost = _inputHost;
        inputReader = _inputReader;

        UpDown = 0;
        LeftRight = 0;

        lastButton = null;
        lastButtonTime = 0;
        buttonBufferTimeMax = InputReader.TimeBetweenSequentialInputs; // might need its own value later
    }

    public bool CheckForInputs()
    {
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

        return true;
    }
}
