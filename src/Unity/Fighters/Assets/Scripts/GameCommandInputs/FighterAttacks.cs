using CommandInputReaderLibrary.Gestures;
using CommandInputReaderLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommandInputReaderLibrary.Directions;

public static class FighterAttacks
{
    public static List<GameAttack> GetFighterAttacks()
    {
        List<GameAttack> attacks = new List<GameAttack>()
            {
                new FiveA(),
                new TwoA(),
                new JumpA(),
                new SixTwoThreeA()
            };
        return attacks;
    }
}

public class FiveA : GameAttack
{
    public FiveA() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackA()));
        conditions.Add(new StanceCondition(this, FighterStance.Standing));

        properties.AnimationName = "Jab";

        properties.knockback.Set(-4.5f, 0);
    }
}

public class TwoA : GameAttack
{
    public TwoA() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackA()));
        conditions.Add(new StanceCondition(this, FighterStance.Crouching));

        properties.AnimationName = "Crouchjab";

        properties.knockback.Set(-4, 0);
    }
}

public class JumpA : GameAttack
{
    public JumpA() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackA()));
        conditions.Add(new StanceCondition(this, FighterStance.Air));

        properties.AnimationName = "Jumpknee";

        properties.knockback.Set(-3.5f, 0);
    }
}

public class SixTwoThreeA : GameAttack
{
    public SixTwoThreeA() : base()
    {
        conditions.Add(new GestureCondition(this, new QuarterCircleForward()));
        conditions.Add(new ButtonCondition(this, new AttackA()));
        conditions.Add(new StanceCondition(this, FighterStance.Standing | FighterStance.Crouching | FighterStance.Air));

        properties.AnimationName = "Dragonpunch";

        properties.knockback.Set(-5, 13);
    }
}