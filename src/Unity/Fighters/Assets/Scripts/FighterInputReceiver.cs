using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandInputReaderLibrary;
using System;

public class FighterInputReceiver : IInputReceiver
{
    protected FighterInputHost inputHost;
    protected InputReader inputReader;

    public int UpDown;
    public int LeftRight;

    public FighterInputReceiver(FighterInputHost _inputHost, InputReader _inputReader)
    {
        inputHost = _inputHost;
        inputReader = _inputReader;

        UpDown = 0;
        LeftRight = 0;
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

        return true;
    }
}
