using CommandInputReaderLibrary;
using CommandInputReaderLibrary.Gestures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class KnightAttacks
{
    public static List<GameAttack> GetKnightAttacks()
    {

        List<GameAttack> attacks = new List<GameAttack>()
            {
                new CrouchJab(),
                new Jab(),
                new JumpKnee(),
                new CrouchKick(),
                new FullPunch(),
                new JumpDust(),
                new CrouchSweep(),
                new Roundhouse(),
                new JumpStomp(),
                new BackThrowWhiff(new GrabSuccess()),
                new GrabWhiff(new GrabSuccess()),
                new AirBackThrowWhiff(new AirGrabSuccess()),
                new AirGrabWhiff(new AirGrabSuccess()),
                new Parry(),
                new BackHop(),
                new ForwardHop(),
                new NeutralHop(),
                new KnightForwardDiveRoll(),
                new KnightBackDiveRoll(),
                new Burst(new ForwardDiveRoll(), new BackDiveRoll()),
                new ForwardDiveRoll(),
                new BackDiveRoll(),
            };
        return attacks;
    }

    public static List<IReadableGesture> GetGestures()
    {
        List<IReadableGesture> gestures = new List<IReadableGesture>()
            {
                new QuarterCircleBack(),
                new QuarterCircleForward(),
                new CrouchGesture(),
                new BackGesture(),
                new ForwardGesture(),
                new NeutralGesture(),
                new ForwardOrNeutralGesture(),
                new DownForwardGesture(),
                new DownBackGesture(),
                new NoGesture()
            };
        return gestures;
    }
}

public class KnightForwardDiveRoll : GameAttack
{
    protected Vector2 wavedashVelocity;
    public KnightForwardDiveRoll() : base()
    {
        conditions.Add(new GestureCondition(this, new DownForwardGesture()));
        conditions.Add(new ButtonCondition(this, new DashMacro()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new GatlingCondition(this));

        whiffSoundIndex = 0;
        hitSoundIndex = 0;

        wavedashVelocity = new Vector2(12f, 8f);

        properties.AnimationName = "DiveRollForward";

        properties.attackType = GameAttackProperties.AttackType.Special;

        properties.landCancelActive = false;
        properties.landCancelStartup = false;
        properties.landCancelRecovery = false;
    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);

        fighter.isStrikeInvulnerable = true;
        fighter.isThrowInvulnerable = true;
    }

    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);

        fighter.OnHaltAllVelocity();
        fighter.OnVelocityImpulseRelativeToSelf(wavedashVelocity);
    }

    public override void OnRecovery(FighterMain fighter)
    {
        base.OnRecovery(fighter);

        fighter.isStrikeInvulnerable = false;
        fighter.isThrowInvulnerable = false;

    }

    
}

public class KnightBackDiveRoll : KnightForwardDiveRoll
{
    public KnightBackDiveRoll() : base()
    {
        conditions.Clear();
        conditions.Add(new GestureCondition(this, new DownBackGesture()));
        conditions.Add(new ButtonCondition(this, new DashMacro()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new GatlingCondition(this));

        whiffSoundIndex = 0;
        hitSoundIndex = 0;

        wavedashVelocity = new Vector2(-12f, 8f);

        properties.AnimationName = "DiveRollBack";

        properties.attackType = GameAttackProperties.AttackType.Special;
    }
}