using CommandInputReaderLibrary;
using CommandInputReaderLibrary.Gestures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class BlackhandAttacks
{
    public static List<GameAttack> GetBlackhandAttacks()
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
                new ShoulderBash(),
                new JumpStomp(),
                new SharkCall(), // 5s
                new WaveCall(), // 2s
                new Pegleg(), // 6m
                new CannonGrabWhiff(new CannonGrabSuccess()), // 6s
                new FishingGrabWhiff(new FishingAirSuccess()), // 4s
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

// blackhand
public class CannonGrabWhiff : ThrowAttack
{
    public CannonGrabWhiff(ThrowAttackSuccess _success) : base(_success)
    {
        conditions.Add(new LogicalOrCondition(this,
            // normal input
            new LogicalAndCondition(this,
                new GestureCondition(this, new QuarterCircleForward()),
                new ButtonCondition(this, new AttackD())),
            // simple input
            new LogicalAndCondition(this,
                new GestureCondition(this, new ForwardGesture()),
                new ButtonCondition(this, new SpecialButton()))
            ));
        //conditions.Add(new GestureCondition(this, new QuarterCircleForward()));
        //conditions.Add(new ButtonCondition(this, new AttackD()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[0];
        whiffSoundIndex = 0;
        hitSoundIndex = 1;

        properties.AnimationName = "CannonGrab";

        canBeTeched = false;
        canTech = false;
        canChicagoPunish = false;
    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        fighter.isThrowInvulnerable = true;
    }

    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);
        fighter.isThrowInvulnerable = false;
    }
}

public class CannonGrabSuccess : ThrowAttackSuccess
{
    public CannonGrabSuccess() : base()
    {
        properties.AnimationName = "CannonGrabSuccess";

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[2];
        whiffSoundIndex = -1;
        hitSoundIndex = 3;

        properties.attackType = GameAttackProperties.AttackType.Special;

        properties.hitProperties.knockback.Set(-20f, 11f);
        properties.hitProperties.selfKnockback.Set(0f, 0);
        properties.hitProperties.damage = 1300f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel4_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel4_hitstun;
        properties.hitProperties.hardKD = true;

        properties.hitProperties.wallBounce = true;
    }

    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);
        fighter.PlaySound(fighter.whiffSounds[14]);
    }
}

public class SharkCall : GameAttack
{
    public SharkCall() : base()
    {
        conditions.Add(new LogicalOrCondition(this,
            // normal input
            new LogicalAndCondition(this,
                new GestureCondition(this, new DragonPunch()),
                new ButtonCondition(this, new AttackC())),
            // simple input
            new LogicalAndCondition(this,
                new GestureCondition(this, new CrouchGesture()),
                new ButtonCondition(this, new SpecialButton()))
            ));
        //conditions.Add(new GestureCondition(this, new DragonPunch()));
        //conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[3];
        whiffSoundIndex = 2;
        hitSoundIndex = 3;

        properties.AnimationName = "SharkCall";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

        properties.blockProperties.knockback.Set(-2f, 0);
        properties.blockProperties.airKnockback.Set(-2f, -4);
        properties.blockProperties.selfKnockback.Set(-7f, 0);
        properties.blockProperties.damage = 75f;
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-1f, 9f);
        properties.hitProperties.airKnockback.Set(-1f, 16f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 300f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel3_hitstun;
        properties.hitProperties.hardKD = false;
    }

    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);
        fighter.PlaySound(fighter.whiffSounds[17]);
    }
}

public class WaveCall : GameAttack
{
    GameAttackProperties fireballProperties;
    Vector3 spawnOffset;
    public WaveCall() : base()
    {
        conditions.Add(new LogicalOrCondition(this,
            // normal input
            new LogicalAndCondition(this,
                new GestureCondition(this, new QuarterCircleBack()),
                new ButtonCondition(this, new AttackB())),
            // simple input
            new LogicalAndCondition(this,
                new GestureCondition(this, new NeutralGesture()),
                new ButtonCondition(this, new SpecialButton()))
            ));

        //conditions.Add(new GestureCondition(this, new QuarterCircleBack()));
        //conditions.Add(new ButtonCondition(this, new AttackB()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[3];
        whiffSoundIndex = 2;
        hitSoundIndex = 3;

        properties.AnimationName = "WaveCall";

        properties.blockType = GameAttackProperties.BlockType.Low;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

        fireballProperties = new GameAttackProperties(this);

        fireballProperties.blockType = GameAttackProperties.BlockType.Mid;
        fireballProperties.attackType = GameAttackProperties.AttackType.Special;
        fireballProperties.attackStance = FighterStance.Standing;

        fireballProperties.blockProperties.knockback.Set(6f, 0);
        fireballProperties.blockProperties.airKnockback.Set(4f, 6f);
        fireballProperties.blockProperties.damage = 25f;
        fireballProperties.blockProperties.hitstopTime = AttackSettings.attackLevel1_blockhitstop;
        fireballProperties.blockProperties.stunTime = AttackSettings.attackLevel2_blockstun;

        fireballProperties.hitProperties.knockback.Set(6f, 10f);
        fireballProperties.hitProperties.airKnockback.Set(6f, 10f);
        fireballProperties.hitProperties.damage = 100f;
        fireballProperties.hitProperties.hitstopTime = AttackSettings.attackLevel1_hithitstop;
        fireballProperties.hitProperties.stunTime = AttackSettings.attackLevel2_hitstun;
        fireballProperties.hitProperties.hardKD = false;

        spawnOffset = new Vector3(21, 0, 0);
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

public class Pegleg : GameAttack
{
    public Pegleg() : base()
    {
        conditions.Add(new GestureCondition(this, new ForwardGesture()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[3];
        whiffSoundIndex = 2;
        hitSoundIndex = 3;

        properties.AnimationName = "Pegleg";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

        properties.blockProperties.knockback.Set(-6f, 0);
        properties.blockProperties.airKnockback.Set(-6f, 5f);
        properties.blockProperties.selfKnockback.Set(-4f, 0);
        properties.blockProperties.damage = 75f;
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-10f, 9f);
        properties.hitProperties.airKnockback.Set(-10f, 9f);
        properties.hitProperties.selfKnockback.Set(-1f, 0);
        properties.hitProperties.damage = 350f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel3_hitstun;
        properties.hitProperties.hardKD = true;
        properties.hitProperties.wallBounce = true;
    }
}

public class FishingGrabWhiff : ThrowAttack
{
    public FishingGrabWhiff(ThrowAttackSuccess _success) : base(_success)
    {
        conditions.Add(new LogicalOrCondition(this,
            // normal input
            new LogicalAndCondition(this,
                new GestureCondition(this, new QuarterCircleBack()),
                new ButtonCondition(this, new AttackD())),
            // simple input
            new LogicalAndCondition(this,
                new GestureCondition(this, new BackGesture()),
                new ButtonCondition(this, new SpecialButton()))
            ));
        //conditions.Add(new GestureCondition(this, new QuarterCircleBack()));
        //conditions.Add(new ButtonCondition(this, new AttackD()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[0];
        whiffSoundIndex = 15;
        hitSoundIndex = 1;

        properties.AnimationName = "FishingGrab";

        properties.stanceToBeGrabbed = FighterStance.Air;
        properties.canGrabHitstun = true;

        canBeTeched = false;
        canTech = false;
        canChicagoPunish = false;
    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
    }

    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);
    }

    public override void OnHit(FighterMain fighter, FighterMain otherFighter)
    {
        base.OnHit(fighter, otherFighter);
    }
}

public class FishingAirSuccess : ThrowAttackSuccess
{
    public FishingAirSuccess() : base()
    {
        properties.AnimationName = "FishingAirSuccess";

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[2];
        whiffSoundIndex = 1;
        hitSoundIndex = 3;

        properties.attackType = GameAttackProperties.AttackType.Heavy;

        properties.hitProperties.airKnockback.Set(-2f, -11f);
        properties.hitProperties.selfKnockback.Set(-6f, 0);
        properties.hitProperties.damage = 250f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel3_hitstun;
        properties.hitProperties.hardKD = false;
        properties.hitProperties.groundBounceOnAirHit = true;
        properties.hitProperties.wallBounce = false;

    }

    public override void OnAnimationEnd(FighterMain fighter)
    {
        base.OnAnimationEnd(fighter);

        // manually apply self knockback
        // todo: make it apply combo scaling so you get pushed further later in the combo?

        //Vector2 kb = properties.hitProperties.selfKnockback;

        //if (fighter.otherFighterMain.isAtTheWall)
        //{
        //    kb.x += properties.hitProperties.knockback.x / 1.5f;
        //}

        //fighter.OnVelocityImpulseRelativeToOtherFighter(kb);

    }
}