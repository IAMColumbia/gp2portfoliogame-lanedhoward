using CommandInputReaderLibrary.Gestures;
using CommandInputReaderLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommandInputReaderLibrary.Directions;
using System.Linq;

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
                new ForwardHalfCircleForward(),
                new CrouchGesture(),
                new BackGesture(),
                new ForwardGesture(),
                new NoGesture()
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

public class NoGesture : ReadableGesture
{
    public NoGesture() : base()
    {
        Priority = 1000;
    }

    public override bool Read(List<ReadablePackage> inputs, float currentTime, FacingDirection facingDirection)
    {
        return true;
    }
}

public class CrouchGesture : ReadableGesture
{
    public CrouchGesture() : base()
    {
        Priority = 500;
    }

    public override bool Read(List<ReadablePackage> inputs, float currentTime, FacingDirection facingDirection)
    {
        if (inputs.Count > 0)
        {
            Direction dir = inputs.Last().GetDirectionFacingForward(facingDirection);
            if (dir == Direction.DownForward || dir == Direction.Down || dir == Direction.DownBack)
            {
                return true;
            }
        }
        return false;
    }
}

public class SingleDirectionGesture : ReadableGesture
{
    public Direction requiredDirection;
    public SingleDirectionGesture(Direction direction) : base()
    {
        requiredDirection = direction;
        Priority = 500;
    }

    public override bool Read(List<ReadablePackage> inputs, float currentTime, FacingDirection facingDirection)
    {
        if (inputs.Count > 0)
        {
            Direction dir = inputs.Last().GetDirectionFacingForward(facingDirection);
            if (dir == requiredDirection)
            {
                return true;
            }
        }
        return false;
    }
}

public class BackGesture : SingleDirectionGesture
{
    public BackGesture() : base(Direction.Back)
    {

    }
}

public class ForwardGesture : SingleDirectionGesture
{
    public ForwardGesture() : base(Direction.Forward)
    {

    }
}