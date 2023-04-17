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
                new DashGesture(),
                new ForwardHalfCircleForward(),
                new CrouchGesture(),
                new BackGesture(),
                new ForwardGesture(),
                new NeutralGesture(),
                new DashGesture(),
                new BackDashGesture(),
                new NeutralDashGesture(),
                new NoGesture()
            };
        return gestures;
    }
}

public interface IStandaloneGesture
{

}

public class DashGesture : ReadableGesture, IStandaloneGesture
{
    public DashGesture()
    {
        Priority = 200; // arbitrary
    }

    protected override void ResetRequiredInputs()
    {
        base.ResetRequiredInputs();

        requiredInputs.Push(new GestureComponent(Direction.Neutral, InputReader.TimeBetweenSequentialInputs));
        requiredInputs.Push(new GestureComponent(Direction.Forward, InputReader.TimeBetweenSequentialInputs));
        requiredInputs.Push(new GestureComponent(Direction.Neutral, InputReader.TimeBetweenSequentialInputs));
        requiredInputs.Push(new GestureComponent(Direction.Forward, 0));

        disallowedInputs.Add(new GestureComponent(Direction.Back, 0));
        disallowedInputs.Add(new GestureComponent(Direction.DownBack, 0));
        disallowedInputs.Add(new GestureComponent(Direction.UpBack, 0));
        disallowedInputs.Add(new GestureComponent(Direction.Down, 0));
    }
}

public class BackDashGesture : ReadableGesture, IStandaloneGesture
{
    public BackDashGesture()
    {
        Priority = 200; // arbitrary
    }

    protected override void ResetRequiredInputs()
    {
        base.ResetRequiredInputs();

        requiredInputs.Push(new GestureComponent(Direction.Neutral, InputReader.TimeBetweenSequentialInputs));
        requiredInputs.Push(new GestureComponent(Direction.Back, InputReader.TimeBetweenSequentialInputs));
        requiredInputs.Push(new GestureComponent(Direction.Neutral, InputReader.TimeBetweenSequentialInputs));
        requiredInputs.Push(new GestureComponent(Direction.Back, 0));

        disallowedInputs.Add(new GestureComponent(Direction.Forward, 0));
        disallowedInputs.Add(new GestureComponent(Direction.DownForward, 0));
        disallowedInputs.Add(new GestureComponent(Direction.UpForward, 0));
        disallowedInputs.Add(new GestureComponent(Direction.Down, 0));

    }
}

public class NeutralDashGesture : ReadableGesture, IStandaloneGesture
{
    public NeutralDashGesture()
    {
        Priority = 200; // arbitrary
    }

    protected override void ResetRequiredInputs()
    {
        base.ResetRequiredInputs();

        requiredInputs.Push(new GestureComponent(Direction.Neutral, InputReader.TimeBetweenSequentialInputs));
        requiredInputs.Push(new GestureComponent(Direction.Down, InputReader.TimeBetweenSequentialInputs));
        requiredInputs.Push(new GestureComponent(Direction.Neutral, InputReader.TimeBetweenSequentialInputs));
        requiredInputs.Push(new GestureComponent(Direction.Down, 0));

        disallowedInputs.Add(new GestureComponent(Direction.Forward, 0));
        disallowedInputs.Add(new GestureComponent(Direction.DownForward, 0));
        disallowedInputs.Add(new GestureComponent(Direction.UpForward, 0));

        disallowedInputs.Add(new GestureComponent(Direction.Back, 0));
        disallowedInputs.Add(new GestureComponent(Direction.DownBack, 0));
        disallowedInputs.Add(new GestureComponent(Direction.UpBack, 0));
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

public class MultipleSingleDirectionGesture : ReadableGesture
{
    public List<Direction> directions;
    public MultipleSingleDirectionGesture() : base()
    {
        Priority = 500;
        directions = new List<Direction>();
    }

    public override bool Read(List<ReadablePackage> inputs, float currentTime, FacingDirection facingDirection)
    {
        if (inputs.Count > 0)
        {
            Direction dir = inputs.Last().GetDirectionFacingForward(facingDirection);
            if (directions.Any(d => d == dir))
            {
                return true;
            }
        }
        return false;
    }
}

public class CrouchGesture : MultipleSingleDirectionGesture
{
    public CrouchGesture() : base()
    {
        Priority = 500;
        directions.Add(Direction.Down);
        directions.Add(Direction.DownForward);
        directions.Add(Direction.DownBack);
    }
}

public class ForwardOrNeutralGesture : MultipleSingleDirectionGesture
{
    public ForwardOrNeutralGesture() : base()
    {
        Priority = 500;
        directions.Add(Direction.Neutral);
        directions.Add(Direction.Forward);

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

public class NeutralGesture : SingleDirectionGesture
{
    public NeutralGesture() : base(Direction.Neutral)
    {

    }
}