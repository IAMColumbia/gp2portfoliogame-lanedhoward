using CommandInputReaderLibrary.Gestures;
using CommandInputReaderLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommandInputReaderLibrary.Directions;

public static class FighterGestures
{
    public static List<IReadableGesture> GetDefaultGestures()
    {
        List<IReadableGesture> gestures = new List<IReadableGesture>()
            {
                new QuarterCircleBack(),
                new QuarterCircleForward(),
                new DragonPunch(),
                new Dash(),
                new ForwardHalfCircleForward()
            };
        return gestures;
    }
}

public class Dash : ReadableGesture
{
    public Dash()
    {
        Priority = 200; // arbitrary
    }

    protected override void ResetRequiredInputs()
    {
        base.ResetRequiredInputs();

        requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenSequentialInputs));
        requiredInputs.Push(new GestureComponent(Direction.Neutral, InputReader.TimeBetweenSequentialInputs));
        requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenSequentialInputs));

    }
}

public class ForwardHalfCircleForward : ReadableGestureWithShortcuts
{
    private class Shortcut1 : ReadableGesture
    {
        protected override void ResetRequiredInputs()
        {
            base.ResetRequiredInputs();

            requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenSequentialInputs));
            requiredInputs.Push(new GestureComponent(Direction.Back, InputReader.TimeBetweenNonSequentialInputs));
            requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenNonSequentialInputs));
            requiredInputs.Push(new GestureComponent(Direction.DownForward, InputReader.TimeBetweenSequentialInputs));
            requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenSequentialInputs));

        }
    }

    private class Shortcut2 : ReadableGesture
    {
        protected override void ResetRequiredInputs()
        {
            base.ResetRequiredInputs();

            requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenSequentialInputs));
            requiredInputs.Push(new GestureComponent(Direction.Back, InputReader.TimeBetweenNonSequentialInputs));
            requiredInputs.Push(new GestureComponent(Direction.DownBack, InputReader.TimeBetweenSequentialInputs));
            requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenSequentialInputs));
            requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenNonSequentialInputs));

        }
    }

    public ForwardHalfCircleForward() : base()
    {
        Priority = 30;

        possibleGestures.Add(new Shortcut1());
        possibleGestures.Add(new Shortcut2());
    }
}
