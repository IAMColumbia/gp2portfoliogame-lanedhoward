using CommandInputReaderLibrary.Gestures;
using CommandInputReaderLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommandInputReaderLibrary.Directions;
using System.Runtime.CompilerServices;

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
                new TwoC(),
                new FiveC(),
                new JumpC(),
                new PeoplesUppercut(),
                new Overhead(),
                //new StageDive(),
                new FireballStomp(),
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


public class PeoplesUppercut : GameAttack
{
    private float baseLandingLagTime = 0.25f;
    private float onHitLandingLagTime = 0f;
    public PeoplesUppercut() : base()
    {
        conditions.Add(new GestureCondition(this, new DragonPunch()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[1];
        //hitSound = fighter.hitSounds[4];
        whiffSoundIndex = 4;
        hitSoundIndex = 3;

        properties.AnimationName = "Dragonpunch";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;
        properties.landCancelStartup = false;
        properties.landCancelActive = false;
        properties.landingLagTime = baseLandingLagTime;

        properties.blockProperties.knockback.Set(-2f, 0);
        properties.blockProperties.airKnockback.Set(-2f, 7f);
        properties.blockProperties.selfKnockback.Set(-4f, 0);
        properties.blockProperties.damage = 100f;
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-5f, 16f);
        properties.hitProperties.airKnockback.Set(-5f, 14f);
        properties.hitProperties.selfKnockback.Set(-2f, 0);
        properties.hitProperties.damage = 500f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel3_hitstun;
        properties.hitProperties.hardKD = true;
    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        fighter.isStrikeInvulnerable = true;
        properties.landingLagTime = baseLandingLagTime;
    }

    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);
        fighter.isStrikeInvulnerable = false;
    }

    public override void OnHit(FighterMain fighter, FighterMain otherFighter)
    {
        base.OnHit(fighter, otherFighter);

        properties.landingLagTime = onHitLandingLagTime;
    }
}

public class Overhead : GameAttack
{
    public Overhead() : base()
    {
        conditions.Add(new GestureCondition(this, new QuarterCircleBack()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[3];
        whiffSoundIndex = 2;
        hitSoundIndex = 3;

        properties.AnimationName = "Overhead";

        properties.blockType = GameAttackProperties.BlockType.High;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

        properties.blockProperties.knockback.Set(-3f, 0);
        properties.blockProperties.airKnockback.Set(-3f, 5f);
        properties.blockProperties.selfKnockback.Set(-8f, 0);
        properties.blockProperties.damage = 50f;
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-1f, 15f);
        properties.hitProperties.airKnockback.Set(-1f, -7f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 300f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel3_hitstun;
        properties.hitProperties.hardKD = false;
        properties.hitProperties.groundBounceOnAirHit = true;
        
    }
}

public class StageDive : GameAttack
{
    Vector2 airdashVelocity;
    public StageDive() : base()
    {
        conditions.Add(new GestureCondition(this, new QuarterCircleForward()));
        conditions.Add(new ButtonCondition(this, new AttackA()));
        conditions.Add(new GroundedCondition(this, false));
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[3];
        whiffSoundIndex = 0;
        hitSoundIndex = 1;

        properties.AnimationName = "Airdash";

        properties.blockType = GameAttackProperties.BlockType.High;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Air;

        properties.blockProperties.knockback.Set(-3f, 0);
        properties.blockProperties.airKnockback.Set(-3f, 5);
        properties.blockProperties.selfKnockback.Set(0, 0);
        properties.blockProperties.damage = 25f;
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel1_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel1_blockstun;

        properties.hitProperties.knockback.Set(9f, 9f);
        properties.hitProperties.airKnockback.Set(9f, 9f);
        properties.hitProperties.selfKnockback.Set(0, 0);
        properties.hitProperties.damage = 100f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel1_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel1_hitstun;
        properties.hitProperties.hardKD = false;

        properties.landCancelStartup = false;
        properties.landCancelActive = false;

        airdashVelocity = new Vector2(7f, 6f);
    }

    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);

        fighter.OnHaltAllVelocity();
        fighter.OnVelocityImpulseRelativeToSelf(airdashVelocity);
    }
}

public class FireballStomp : GameAttack
{
    GameAttackProperties fireballProperties;
    public FireballStomp() : base()
    {
        conditions.Add(new GestureCondition(this, new QuarterCircleForward()));
        conditions.Add(new ButtonCondition(this, new AttackB()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[3];
        whiffSoundIndex = 2;
        hitSoundIndex = 3;

        properties.AnimationName = "FireballStomp";

        properties.blockType = GameAttackProperties.BlockType.Low;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

        properties.blockProperties.knockback.Set(-5f, 0);
        properties.blockProperties.airKnockback.Set(-3f, 8f);
        properties.blockProperties.selfKnockback.Set(-7f, 0);
        properties.blockProperties.damage = 50f;
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel2_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel2_blockstun;

        properties.hitProperties.knockback.Set(-3f, 12f);
        properties.hitProperties.airKnockback.Set(-3f, 12f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 125f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel2_hitstun;
        properties.hitProperties.hardKD = false;

        fireballProperties = new GameAttackProperties(this);

        fireballProperties.blockType = GameAttackProperties.BlockType.Mid;
        fireballProperties.attackType = GameAttackProperties.AttackType.Special;
        fireballProperties.attackStance = FighterStance.Standing;

        fireballProperties.blockProperties.knockback.Set(-5f, 0);
        fireballProperties.blockProperties.airKnockback.Set(-3f, 9f);
        fireballProperties.blockProperties.damage = 75f;
        fireballProperties.blockProperties.hitstopTime = AttackSettings.attackLevel1_blockhitstop;
        fireballProperties.blockProperties.stunTime = AttackSettings.attackLevel2_blockstun;

        fireballProperties.hitProperties.knockback.Set(-6f, 0f);
        fireballProperties.hitProperties.airKnockback.Set(-2f, 13f);
        fireballProperties.hitProperties.damage = 200f;
        fireballProperties.hitProperties.hitstopTime = AttackSettings.attackLevel1_hithitstop;
        fireballProperties.hitProperties.stunTime = AttackSettings.attackLevel2_hitstun;
        fireballProperties.hitProperties.hardKD = false;

    }

    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);

        if (!fighter.fireball.projectileActive)
        {
            fighter.fireball.projectileProperties = fireballProperties;
            fighter.fireball.StartProjectile();
        }

    }
}

public class GrabWhiff : ThrowAttack
{
    public GrabWhiff(ThrowAttackSuccess _success) : base(_success)
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackD()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new NoGatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[0];
        whiffSoundIndex = 0;
        hitSoundIndex = 0;

        properties.AnimationName = "ThrowWhiff";
    }
}

public class BackThrowWhiff : BackThrowAttack
{
    public BackThrowWhiff(ThrowAttackSuccess _success) : base(_success)
    {
        conditions.Add(new GestureCondition(this, new BackGesture()));
        conditions.Add(new ButtonCondition(this, new AttackD()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new NoGatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[0];
        whiffSoundIndex = 0;
        hitSoundIndex = 0;

        properties.AnimationName = "ThrowWhiff";
    }

    
}

public class GrabSuccess : ThrowAttackSuccess
{
    public GrabSuccess() : base()
    {
        properties.AnimationName = "ThrowSuccess";

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[2];
        whiffSoundIndex = -1;
        hitSoundIndex = 2;

        properties.hitProperties.knockback.Set(-5f, 10f);
        properties.hitProperties.selfKnockback.Set(0f, 0);
        properties.hitProperties.damage = 500f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel2_hitstun;
        properties.hitProperties.hardKD = true;
    }
}

public class AirGrabWhiff : ThrowAttack
{
    public AirGrabWhiff(ThrowAttackSuccess _success) : base(_success)
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackD()));
        conditions.Add(new GroundedCondition(this, false));
        conditions.Add(new StanceCondition(this, FighterStance.Air));
        conditions.Add(new NoGatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[0];
        whiffSoundIndex = 0;
        hitSoundIndex = 0;

        properties.AnimationName = "ThrowWhiff";

        properties.attackStance = FighterStance.Air;

        properties.landingLagTime = 8f / 60f;

        
    }
}

public class AirBackThrowWhiff : BackThrowAttack
{
    public AirBackThrowWhiff(ThrowAttackSuccess _success) : base(_success)
    {
        conditions.Add(new GestureCondition(this, new BackGesture()));
        conditions.Add(new ButtonCondition(this, new AttackD()));
        conditions.Add(new GroundedCondition(this, false));
        conditions.Add(new StanceCondition(this, FighterStance.Air));
        conditions.Add(new NoGatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[0];
        whiffSoundIndex = 0;
        hitSoundIndex = 0;

        properties.AnimationName = "ThrowWhiff";

        properties.attackStance = FighterStance.Air;

        properties.landingLagTime = 8f / 60f;


    }

}

public class AirGrabSuccess : ThrowAttackSuccess
{
    public AirGrabSuccess() : base()
    {
        properties.AnimationName = "ThrowSuccess";

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[2];
        whiffSoundIndex = -1;
        hitSoundIndex = 2;

        properties.hitProperties.airKnockback.Set(-2f, 10f);
        properties.hitProperties.selfKnockback.Set(0f, 0);
        properties.hitProperties.damage = 500f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel2_hitstun;
        properties.hitProperties.hardKD = true;
    }
}

public class BackHop : GameAttack
{
    public BackHop() : base()
    {
        conditions.Add(new GestureCondition(this, new BackGesture()));
        conditions.Add(new ButtonCondition(this, new DashMacro()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new NoGatlingCondition(this));

        whiffSoundIndex = 0;
        hitSoundIndex = 0;

        properties.AnimationName = "Air_Rising";
    }
    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);

        fighter.SwitchState(fighter.backdashing);

    }
}
public class ForwardHop : GameAttack
{
    public ForwardHop() : base()
    {
        conditions.Add(new GestureCondition(this, new ForwardGesture()));
        conditions.Add(new ButtonCondition(this, new DashMacro()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new NoGatlingCondition(this));

        whiffSoundIndex = 0;
        hitSoundIndex = 0;

        properties.AnimationName = "Air_Rising";
    }
    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);

        fighter.SwitchState(fighter.dashing);

    }
}

public class NeutralHop : GameAttack
{
    public NeutralHop() : base()
    {
        conditions.Add(new GestureCondition(this, new NeutralGesture()));
        conditions.Add(new ButtonCondition(this, new DashMacro()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new NoGatlingCondition(this));

        whiffSoundIndex = 0;
        hitSoundIndex = 0;

        properties.AnimationName = "Air_Rising";
    }
    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);

        fighter.SwitchState(fighter.neutraldashing);

    }
}

public class Parry : GameAttack
{
    private float baseLandingLagTime = 0.25f;
    private float onHitLandingLagTime = 0f;

    private bool parriedDuringStartup;

    public Parry() : base()
    {
        conditions.Add(new GestureCondition(this, new CrouchGesture()));
        conditions.Add(new ButtonCondition(this, new AttackD()));
        conditions.Add(new NoGatlingCondition(this));
        //conditions.Add(new GroundedCondition(this, true));

        whiffSoundIndex = 0;

        properties.AnimationName = "Parry";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Crouching;
        properties.landCancelStartup = false;
        properties.landCancelActive = false;
        properties.landingLagTime = baseLandingLagTime;
    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        properties.landingLagTime = baseLandingLagTime;
        properties.cancelIntoAnyAction = false;
        parriedDuringStartup = false;
    }

    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);
        // if caught an attack during startup, do the parry now
        if (parriedDuringStartup)
        {
            DoParry(fighter);
        }
    }

    public override HitReport? OnGetHitDuring(FighterMain fighter, GameAttackProperties properties)
    {
        // we already know its not a throw

        switch (fighter.currentAttackState)
        {
            case CurrentAttackState.Startup:
                {
                    parriedDuringStartup = true;

                    return HitReport.Parried;
                }
            case CurrentAttackState.Active:
                {
                    DoParry(fighter);

                    return HitReport.Parried;
                }
            default:
            case CurrentAttackState.Recovery:
                {
                    return null;
                }
        }
    }

    public override void OnRecovery(FighterMain fighter)
    {
        base.OnRecovery(fighter);
    }

    private void DoParry(FighterMain fighter)
    {
        properties.cancelIntoAnyAction = true;
        properties.landingLagTime = onHitLandingLagTime;

        fighter.DoParry();
    }
}

public class ForwardWavedash : GameAttack
{
    Vector2 wavedashVelocity;
    public ForwardWavedash() : base()
    {
        conditions.Add(new GestureCondition(this, new DownForwardGesture()));
        conditions.Add(new ButtonCondition(this, new DashMacro()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new NoGatlingCondition(this));

        whiffSoundIndex = 0;
        hitSoundIndex = 0;

        wavedashVelocity = new Vector2(14f, 0f);

        properties.AnimationName = "WavedashForward";
    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        properties.cancelIntoAnyAction = false;

        fighter.PlayWavedashVFX();
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

        fighter.canAct = true;
        properties.cancelIntoAnyAction = true;
        if (fighter.currentState is AttackState a)
        {
            a.allowJumping = true;
        }

    }
}

public class BackWavedash : GameAttack
{
    Vector2 wavedashVelocity;
    public BackWavedash() : base()
    {
        conditions.Add(new GestureCondition(this, new DownBackGesture()));
        conditions.Add(new ButtonCondition(this, new DashMacro()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new NoGatlingCondition(this));

        whiffSoundIndex = 0;
        hitSoundIndex = 0;

        wavedashVelocity = new Vector2(-14f, 0f);

        properties.AnimationName = "WavedashBack";
    }
    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        properties.cancelIntoAnyAction = false;

        fighter.PlayWavedashVFX();
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

        fighter.canAct = true;
        properties.cancelIntoAnyAction = true;
        if (fighter.currentState is AttackState a)
        {
            a.allowJumping = true;
        }
    }
}