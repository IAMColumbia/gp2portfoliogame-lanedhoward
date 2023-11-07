using CommandInputReaderLibrary.Gestures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public static class MrsHiveAttacks
{
    public static List<GameAttack> GetMrsHiveAttacks()
    {
        List<GameAttack> attacks = new List<GameAttack>()
            {
                new TwoA(),
                new FiveA(),
                new JumpA(),
                new TwoB(),
                new FiveB(),
                new JumpB(),
                new TwoC(),
                new FiveC(),
                new JumpC(),
                new Pogo(),
                new Thrust(),
                new Fly(),
                new BackThrowWhiff(new GrabSuccess()),
                new GrabWhiff(new GrabSuccess()),
                new AirBackThrowWhiff(new AirGrabSuccess()),
                new AirGrabWhiff(new AirGrabSuccess()),
                new Parry(),
                new BackHop(),
                new ForwardHop(),
                new NeutralHop(),
                new ForwardWavedash(),
                new BackWavedash()
            };
        return attacks;
    }

}

public class Pogo : GameAttack
{
    private float baseLandingLagTime = 0.25f;
    private float onHitLandingLagTime = 0f;
    Vector2 airdashVelocity;

    public Pogo() : base()
    {
        conditions.Add(new GestureCondition(this, new DownDownGesture()));
        conditions.Add(new ButtonCondition(this, new AttackB()));
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[1];
        //hitSound = fighter.hitSounds[4];
        whiffSoundIndex = 2;
        hitSoundIndex = 3;

        properties.AnimationName = "Pogo";

        properties.blockType = GameAttackProperties.BlockType.Low;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;
        properties.landCancelStartup = false;
        properties.landingLagTime = baseLandingLagTime;

        properties.blockProperties.knockback.Set(-2f, 0);
        properties.blockProperties.airKnockback.Set(-2f, 7f);
        properties.blockProperties.selfKnockback.Set(-1f, 0);
        properties.blockProperties.damage = 50f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-3f, 16f);
        properties.hitProperties.airKnockback.Set(-3f, 15f);
        properties.hitProperties.selfKnockback.Set(-1f, 0);
        properties.hitProperties.damage = 300f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel3_hitstun;
        properties.hitProperties.hardKD = false;

        airdashVelocity = new Vector2(0, 17f);
    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        fighter.isStrikeInvulnerable = true;
        properties.cancelIntoAnyAction = false;
        properties.landingLagTime = baseLandingLagTime;
    }

    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);
        fighter.isStrikeInvulnerable = false;
        fighter.OnHaltAllVelocity();
        fighter.OnVelocityImpulseRelativeToSelf(airdashVelocity);
    }

    public override void OnHit(FighterMain fighter, FighterMain otherFighter)
    {
        base.OnHit(fighter, otherFighter);

        properties.landingLagTime = onHitLandingLagTime;
        properties.cancelIntoAnyAction = true;
    }
}

public class Thrust : GameAttack
{
    public Thrust() : base()
    {
        conditions.Add(new GestureCondition(this, new BackForwardGesture()));
        conditions.Add(new ButtonCondition(this, new AttackB()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        // hitSound = fighter.hitSounds[2];
        whiffSoundIndex = 1;
        hitSoundIndex = 2;

        properties.AnimationName = "Thrust";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

        properties.blockProperties.knockback.Set(-3f, 0);
        properties.blockProperties.airKnockback.Set(-3f, 5);
        properties.blockProperties.selfKnockback.Set(-6f, 0);
        properties.blockProperties.damage = 75f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-5f, 10f);
        properties.hitProperties.airKnockback.Set(-4f, 12f);
        properties.hitProperties.selfKnockback.Set(-3f, 0);
        properties.hitProperties.damage = 400f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel3_hitstun;
    }

    public override void OnHit(FighterMain fighter, FighterMain otherFighter)
    {
        base.OnHit(fighter, otherFighter);
        if (fighter.currentState is AttackState a)
        {
            a.allowJumping = true;
        }
    }

    public override void OnBlock(FighterMain fighter, FighterMain otherFighter)
    {
        base.OnBlock(fighter, otherFighter);
        if (fighter.currentState is AttackState a)
        {
            a.allowJumping = true;
        }
    }
}

public class Fly : GameAttack
{
    Vector2 airdashDirection;
    float airdashMagnitude = 9f;
    float airdashForceUpVelocity = 10f;
    float airdashUpMultiplier = 0.75f;

    int upDown;
    int leftRight;
    public Fly() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new DashMacro()));
        conditions.Add(new GroundedCondition(this, false));
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[3];
        whiffSoundIndex = 0;
        hitSoundIndex = 1;

        properties.AnimationName = "Fly";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Air;

        properties.landCancelStartup = false;

        properties.cancelIntoAnyAction = true;
    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        //fighter.OnHaltAllVelocity();
        //fighter.OnHaltVerticalVelocity();

        SetInputDirections(fighter);
    }

    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);

        fighter.OnHaltAllVelocity();

        if (fighter.inputReceiver.UpDown != 0
            || fighter.inputReceiver.LeftRight != 0)
        {
            SetInputDirections(fighter);
        }
        else
        {
            // use whatever the input was at startup
        }

        float upVel = upDown != -1 ? airdashForceUpVelocity : 0;

        Vector2 vel = new Vector2(leftRight * airdashMagnitude, upDown * airdashMagnitude * airdashUpMultiplier + upVel);

        if (fighter.facingDirection == CommandInputReaderLibrary.Directions.FacingDirection.LEFT)
        {
            vel.x *= -1;
        }

        fighter.OnVelocityImpulseRelativeToSelf(vel);
    }

    public override void OnRecovery(FighterMain fighter)
    {
        base.OnRecovery(fighter);
        fighter.canAct = true;
        
    }

    public override void OnCancel(FighterMain fighter)
    {
        base.OnCancel(fighter);
        fighter.AutoTurnaround();
    }

    public override void OnAnimationEnd(FighterMain fighter)
    {
        base.OnAnimationEnd(fighter);
        fighter.AutoTurnaround();
    }

    private void SetInputDirections(FighterMain fighter)
    {
        upDown = fighter.inputReceiver.UpDown;
        leftRight = fighter.inputReceiver.LeftRight;
    }
}