using CommandInputReaderLibrary.Gestures;
using CommandInputReaderLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommandInputReaderLibrary.Directions;

public static class FighterAttacks
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
                new NoGesture()
            };
        return gestures;
    }
}

public class FiveA : GameAttack
{
    public FiveA(FighterMain _fighter) : base(_fighter)
    {
        conditions.Add(new GestureCondition(this, fighter, new NoGesture()));
        conditions.Add(new ButtonCondition(this, fighter, new AttackA()));
        conditions.Add(new StanceCondition(this, fighter, FighterStance.Standing));
    }
}

public class TwoA : GameAttack
{
    public TwoA(FighterMain _fighter) : base(_fighter)
    {
        conditions.Add(new GestureCondition(this, fighter, new NoGesture()));
        conditions.Add(new ButtonCondition(this, fighter, new AttackA()));
        conditions.Add(new StanceCondition(this, fighter, FighterStance.Crouching));
    }
}

public class JumpA : GameAttack
{
    public JumpA(FighterMain _fighter) : base(_fighter)
    {
        conditions.Add(new GestureCondition(this, fighter, new NoGesture()));
        conditions.Add(new ButtonCondition(this, fighter, new AttackA()));
        conditions.Add(new StanceCondition(this, fighter, FighterStance.Air));
    }
}

public class SixTwoThreeA : GameAttack
{
    public SixTwoThreeA(FighterMain _fighter) : base(_fighter)
    {
        conditions.Add(new GestureCondition(this, fighter, new DragonPunch()));
        conditions.Add(new ButtonCondition(this, fighter, new AttackA()));
        conditions.Add(new StanceCondition(this, fighter, FighterStance.Standing | FighterStance.Crouching | FighterStance.Air));
    }
}