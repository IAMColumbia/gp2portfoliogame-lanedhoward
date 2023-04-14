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
                new TwoA(),
                new FiveA(),
                new JumpA(),
                new TwoB(),
                new FiveB(),
                new JumpB(),
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
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new StanceCondition(this, FighterStance.Standing));

        properties.AnimationName = "Jab";

        properties.knockback.Set(-4.5f, 0);

        properties.blockType = GameAttackProperties.BlockType.Mid;
    }
}

public class TwoA : GameAttack
{
    public TwoA() : base()
    {
        //conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new GestureCondition(this, new CrouchGesture()));
        conditions.Add(new ButtonCondition(this, new AttackA()));
        conditions.Add(new GroundedCondition(this, true));
        
        properties.AnimationName = "Crouchjab";

        properties.knockback.Set(-4, 0);

        properties.blockType = GameAttackProperties.BlockType.Mid;
    }
}

public class JumpA : GameAttack
{
    public JumpA() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackA()));
        conditions.Add(new GroundedCondition(this, false));
        conditions.Add(new StanceCondition(this, FighterStance.Air));

        properties.AnimationName = "Jumpknee";

        properties.knockback.Set(-3.5f, 0);

        properties.blockType = GameAttackProperties.BlockType.High;
    }
}

public class FiveB : GameAttack
{
    public FiveB() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackB()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new StanceCondition(this, FighterStance.Standing));

        properties.AnimationName = "Fullpunch";

        properties.knockback.Set(-6f, 2);

        properties.blockType = GameAttackProperties.BlockType.Mid;
    }
}

public class TwoB : GameAttack
{
    public TwoB() : base()
    {
        //conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new GestureCondition(this, new CrouchGesture()));
        conditions.Add(new ButtonCondition(this, new AttackB()));
        conditions.Add(new GroundedCondition(this, true));
        
        properties.AnimationName = "Crouchkick";

        properties.knockback.Set(-5, 3);

        properties.blockType = GameAttackProperties.BlockType.Low;
    }
}

public class JumpB : GameAttack
{
    public JumpB() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackB()));
        conditions.Add(new GroundedCondition(this, false));
        conditions.Add(new StanceCondition(this, FighterStance.Air));

        properties.AnimationName = "Jumpslice";

        properties.knockback.Set(-5f, 0);

        properties.blockType = GameAttackProperties.BlockType.High;
    }
}

public class SixTwoThreeA : GameAttack
{
    public SixTwoThreeA() : base()
    {
        conditions.Add(new GestureCondition(this, new DragonPunch()));
        conditions.Add(new ButtonCondition(this, new AttackA()));

        properties.AnimationName = "Dragonpunch";

        properties.knockback.Set(-5, 15);

        properties.blockType = GameAttackProperties.BlockType.Mid;
    }
}