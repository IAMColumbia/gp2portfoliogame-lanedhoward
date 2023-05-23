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
                new TwoA(),
                new FiveA(),
                new JumpA(),
                new TwoB(),
                new FiveB(),
                new JumpB(),
                new TwoC(),
                new FiveC(),
                new JumpC(),
                new SharkCall(),
                new WaveCall(),
                new Pegleg(),
                new CannonGrabWhiff(new CannonGrabSuccess()),
                new BackThrowWhiff(new GrabSuccess()),
                new GrabWhiff(new GrabSuccess()),
                new AirBackThrowWhiff(new AirGrabSuccess()),
                new AirGrabWhiff(new AirGrabSuccess()),
                new BackDash(),
                new ForwardDash(),
                new NeutralDash()
            };
        return attacks;
    }
}

// blackhand
public class CannonGrabWhiff : ThrowAttack
{
    public CannonGrabWhiff(ThrowAttackSuccess _success) : base(_success)
    {
        conditions.Add(new GestureCondition(this, new QuarterCircleForward()));
        conditions.Add(new ButtonCondition(this, new AttackD()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new NoGatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[0];
        whiffSoundIndex = 0;
        hitSoundIndex = 1;

        properties.AnimationName = "CannonGrab";

        canBeTeched = false;
        canTech = false;

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

        properties.hitProperties.knockback.Set(-20f, 8f);
        properties.hitProperties.selfKnockback.Set(0f, 0);
        properties.hitProperties.damage = 1300f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel4_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel4_hitstun;
        properties.hitProperties.hardKD = true;

        properties.hitProperties.wallBounce = true;
    }
}

public class SharkCall : GameAttack
{
    public SharkCall() : base()
    {
        conditions.Add(new GestureCondition(this, new DragonPunch()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
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
        properties.blockProperties.airKnockback.Set(-2f, -2);
        properties.blockProperties.selfKnockback.Set(-7f, 0);
        properties.blockProperties.damage = 75f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-1f, 6f);
        properties.hitProperties.airKnockback.Set(-1f, 10.5f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 300f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel3_hitstun;
        properties.hitProperties.hardKD = false;
    }
}

public class WaveCall : GameAttack
{
    GameAttackProperties fireballProperties;
    Vector3 spawnOffset;
    public WaveCall() : base()
    {
        conditions.Add(new GestureCondition(this, new QuarterCircleBack()));
        conditions.Add(new ButtonCondition(this, new AttackB()));
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
        fireballProperties.blockProperties.airKnockback.Set(4f, 4f);
        fireballProperties.blockProperties.damage = 25f;
        fireballProperties.blockProperties.hitstopTime = FighterAttacks.attackLevel1_blockhitstop;
        fireballProperties.blockProperties.stunTime = FighterAttacks.attackLevel2_blockstun;

        fireballProperties.hitProperties.knockback.Set(6f, 6f);
        fireballProperties.hitProperties.airKnockback.Set(6f, 6f);
        fireballProperties.hitProperties.damage = 100f;
        fireballProperties.hitProperties.hitstopTime = FighterAttacks.attackLevel1_hithitstop;
        fireballProperties.hitProperties.stunTime = FighterAttacks.attackLevel2_hitstun;
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
        conditions.Add(new GestureCondition(this, new QuarterCircleForward()));
        conditions.Add(new ButtonCondition(this, new AttackB()));
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
        properties.blockProperties.airKnockback.Set(-6f, 3f);
        properties.blockProperties.selfKnockback.Set(-9f, 0);
        properties.blockProperties.damage = 75f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel2_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel2_blockstun;

        properties.hitProperties.knockback.Set(-10f, 6f);
        properties.hitProperties.airKnockback.Set(-10f, 6f);
        properties.hitProperties.selfKnockback.Set(-3f, 0);
        properties.hitProperties.damage = 350f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel2_hitstun;
        properties.hitProperties.hardKD = true;
    }
}