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
                new CrouchJab(),
                new Jab(),
                new JumpKnee(),
                new CrouchKick(),
                new FullPunch(),
                new JumpDust(),
                new Launcher(),
                new ShoulderBash(),
                new JumpSlice(),
                new PeoplesUppercut(),
                new Overhead(),
                //new StageDive(),
                new FireballStomp(),
                new GriddyForward(),
                new GriddyBack(),
                new EnhancedPeoplesUppercut(),
                new EnhancedOverhead(),
                new EnhancedFireballStomp(),
                new BackThrowWhiff(new GrabSuccess()),
                new GrabWhiff(new GrabSuccess()),
                new AirBackThrowWhiff(new AirGrabSuccess()),
                new AirGrabWhiff(new AirGrabSuccess()),
                new Parry(),
                new BackHop(),
                new ForwardHop(),
                new NeutralHop(),
                new ForwardWavedash(),
                new BackWavedash(),
                new ForwardWavedashCancel(),
                new BackWavedashCancel()
            };
        return attacks;
    }

    public static List<IReadableGesture> GetGestures()
    {
        List<IReadableGesture> gestures = new List<IReadableGesture>()
            {
                new QuarterCircleBack(),
                new QuarterCircleForward(),
                new DragonPunch(),
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
        properties.hitProperties.playGroundBounceParticlesOnGroundedHit = true;

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
    protected GameAttackProperties fireballProperties;
    protected Vector2 fireballVelocity;
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

        fireballVelocity = new Vector2(5f, 0f);

    }

    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);

        if (!fighter.fireball.projectileActive)
        {
            fighter.fireball.projectileProperties = fireballProperties;
            fighter.fireball.velocity = fireballVelocity;
            fighter.fireball.StartProjectile();
        }

    }
}

public class GriddyAttack : GameAttack
{
    protected Vector2 velocity;

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        fighter.OnHaltHorizontalVelocity();
        fighter.OnVelocityImpulseRelativeToSelf(velocity);
    }

    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);
        fighter.canAct = true;
    }
}

public class GriddyBack : GriddyAttack
{
    public GriddyBack() : base()
    {
        conditions.Add(new LogicalOrCondition(this, 
            new GestureCondition(this, new QuarterCircleBack()),
            new LogicalAndCondition(this,
                new FollowUpCondition(this, typeof(GriddyForward)),
                new GestureCondition(this, new BackGesture())
                )
            )
            );
        conditions.Add(new ButtonCondition(this, new AttackA()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new LogicalOrCondition(this, 
            new GatlingCondition(this),
            new FollowUpCondition(this, typeof(GriddyForward))
            )
            );

        whiffSoundIndex = 2;

        properties.AnimationName = "GriddyBack";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

        velocity = new Vector2(-13f, 0f);
    }
}

public class GriddyForward : GriddyAttack
{
    public GriddyForward() : base()
    {
        conditions.Add(new LogicalOrCondition(this,
            new GestureCondition(this, new QuarterCircleForward()),
            new LogicalAndCondition(this,
                new FollowUpCondition(this, typeof(GriddyBack)),
                new GestureCondition(this, new ForwardGesture())
                )
            )
            );
        conditions.Add(new ButtonCondition(this, new AttackA()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new LogicalOrCondition(this,
            new GatlingCondition(this),
            new FollowUpCondition(this, typeof(GriddyBack))
            )
            );

        whiffSoundIndex = 2;

        properties.AnimationName = "GriddyForward";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

        velocity = new Vector2(13f, 0f);
    }
}

public class EnhancedPeoplesUppercut : PeoplesUppercut
{
    private Vector2 velocity;
    public EnhancedPeoplesUppercut() : base()
    {
        conditions.Clear();
        conditions.Add(new LogicalOrCondition(this,
            new GestureCondition(this, new DragonPunch()),
            new GestureCondition(this, new ForwardGesture())
            )
            );
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new FollowUpCondition(this, typeof(GriddyAttack)));

        properties.blockProperties.knockback.Set(-2f, 0);
        properties.blockProperties.airKnockback.Set(-2f, 7f);
        properties.blockProperties.selfKnockback.Set(-4f, 0);
        properties.blockProperties.damage = 200f;
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel4_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel4_blockstun;

        properties.hitProperties.knockback.Set(-4f, 18f);
        properties.hitProperties.airKnockback.Set(-4f, 16f);
        properties.hitProperties.selfKnockback.Set(-2f, 0);
        properties.hitProperties.damage = 700f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel4_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel4_hitstun;
        properties.hitProperties.hardKD = true;

        velocity = new Vector2(6f, 0);
    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        fighter.PlayGriddyVFX();

        fighter.OnVelocityImpulseRelativeToSelf(velocity);
    }
}

public class EnhancedOverhead : Overhead
{
    private Vector2 velocity;

    public EnhancedOverhead() : base()
    {
        conditions.Clear();
        conditions.Add(new LogicalOrCondition(this,
            new GestureCondition(this, new QuarterCircleBack()),
            new GestureCondition(this, new BackGesture())
            )
            );
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new FollowUpCondition(this, typeof(GriddyAttack)));

        properties.blockProperties.knockback.Set(-3f, 0);
        properties.blockProperties.airKnockback.Set(-3f, 5f);
        properties.blockProperties.selfKnockback.Set(-8f, 0);
        properties.blockProperties.damage = 75f;
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel4_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel4_blockstun;

        properties.hitProperties.knockback.Set(-1f, 17f);
        properties.hitProperties.airKnockback.Set(-1f, -9f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 400f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel4_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel4_hitstun;
        properties.hitProperties.hardKD = false;
        properties.hitProperties.groundBounceOnAirHit = true;
        properties.hitProperties.playGroundBounceParticlesOnGroundedHit = true;

        velocity = new Vector2(9f, 0);
    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        fighter.PlayGriddyVFX();

        fighter.OnVelocityImpulseRelativeToSelf(velocity);
    }
}

public class EnhancedFireballStomp : FireballStomp
{
    private Vector2 velocity;
    public EnhancedFireballStomp() : base()
    {
        conditions.Clear();
        conditions.Add(new LogicalOrCondition(this,
            new GestureCondition(this, new QuarterCircleForward()),
            new GestureCondition(this, new ForwardGesture())
            )
            );
        conditions.Add(new ButtonCondition(this, new AttackB()));
        conditions.Add(new FollowUpCondition(this, typeof(GriddyAttack)));

        properties.blockProperties.knockback.Set(-5f, 0);
        properties.blockProperties.airKnockback.Set(-3f, 8f);
        properties.blockProperties.selfKnockback.Set(-7f, 0);
        properties.blockProperties.damage = 75f;
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-4f, 10f);
        properties.hitProperties.airKnockback.Set(-4f, 10f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 200f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel3_hitstun;
        properties.hitProperties.hardKD = false;

        fireballProperties.blockProperties.knockback.Set(-5f, 0);
        fireballProperties.blockProperties.airKnockback.Set(-3f, 11f);
        fireballProperties.blockProperties.damage = 75f;
        fireballProperties.blockProperties.hitstopTime = AttackSettings.attackLevel2_blockhitstop;
        fireballProperties.blockProperties.stunTime = AttackSettings.attackLevel3_blockstun;

        fireballProperties.hitProperties.knockback.Set(-6f, 0f);
        fireballProperties.hitProperties.airKnockback.Set(-2f, 15f);
        fireballProperties.hitProperties.damage = 300f;
        fireballProperties.hitProperties.hitstopTime = AttackSettings.attackLevel2_hithitstop;
        fireballProperties.hitProperties.stunTime = AttackSettings.attackLevel3_hitstun;
        fireballProperties.hitProperties.hardKD = false;

        fireballVelocity = new Vector2(7f, 0f);

        velocity = new Vector2(5f, 0);
    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        fighter.PlayGriddyVFX();

        fighter.OnVelocityImpulseRelativeToSelf(velocity);
    }
}