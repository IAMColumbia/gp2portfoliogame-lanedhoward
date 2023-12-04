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

    public static float attackLevel1_hithitstop;
    public static float attackLevel1_blockhitstop;
    public static float attackLevel1_hitstun;
    public static float attackLevel1_blockstun;

    public static float attackLevel2_hithitstop;
    public static float attackLevel2_blockhitstop;
    public static float attackLevel2_hitstun;
    public static float attackLevel2_blockstun;

    public static float attackLevel3_hithitstop;
    public static float attackLevel3_blockhitstop;
    public static float attackLevel3_hitstun;
    public static float attackLevel3_blockstun;

    public static float attackLevel4_hithitstop;
    public static float attackLevel4_blockhitstop;
    public static float attackLevel4_hitstun;
    public static float attackLevel4_blockstun;
    static FighterAttacks()
    {
        attackLevel1_hithitstop = 7f / 60f;
        attackLevel1_blockhitstop = 5f / 60f;
        attackLevel1_hitstun = 15f / 60f;
        attackLevel1_blockstun = 13f / 60f;

        attackLevel2_hithitstop = 9f / 60f;
        attackLevel2_blockhitstop = 7f / 60f;
        attackLevel2_hitstun = 19f / 60f;
        attackLevel2_blockstun = 16f / 60f;

        attackLevel3_hithitstop = 11f / 60f;
        attackLevel3_blockhitstop = 9f / 60f;
        attackLevel3_hitstun = 25f / 60f;
        attackLevel3_blockstun = 20f / 60f;

        attackLevel4_hithitstop = 15f / 60f;
        attackLevel4_blockhitstop = 12f / 60f;
        attackLevel4_hitstun = 32f / 60f;
        attackLevel4_blockstun = 26f / 60f;
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
        conditions.Add(new GatlingCondition(this));

        whiffSoundIndex = 0;
        hitSoundIndex = 0;

        properties.AnimationName = "Jab";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Light;
        properties.attackStance = FighterStance.Standing;

        properties.blockProperties.knockback.Set(-2.5f, 0);
        properties.blockProperties.airKnockback.Set(-2.5f, 5f);
        properties.blockProperties.selfKnockback.Set(-5f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel1_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel1_blockstun;

        properties.hitProperties.knockback.Set(-3f, 0);
        properties.hitProperties.airKnockback.Set(-1.75f, 9f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 85f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel1_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel1_hitstun;
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
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[0];
        whiffSoundIndex = 0;
        hitSoundIndex = 0;

        properties.AnimationName = "Crouchjab";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Light;
        properties.attackStance = FighterStance.Crouching;


        properties.blockProperties.knockback.Set(-2.5f, 0);
        properties.blockProperties.airKnockback.Set(-2.5f, 5f);
        properties.blockProperties.selfKnockback.Set(-5f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel1_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel1_blockstun;

        properties.hitProperties.knockback.Set(-3f, 0);
        properties.hitProperties.airKnockback.Set(-1.75f, 9f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 75f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel1_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel1_hitstun;
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
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[0];
        whiffSoundIndex = 0;
        hitSoundIndex = 0;

        properties.AnimationName = "Jumpknee";

        properties.blockType = GameAttackProperties.BlockType.High;
        properties.attackType = GameAttackProperties.AttackType.Light;
        properties.attackStance = FighterStance.Air;

        properties.blockProperties.knockback.Set(-2.5f, 0);
        properties.blockProperties.airKnockback.Set(-2.5f, 5f);
        properties.blockProperties.selfKnockback.Set(-1f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel1_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel1_blockstun;

        properties.hitProperties.knockback.Set(-3f, 0);
        properties.hitProperties.airKnockback.Set(-1.5f, 9f);
        properties.hitProperties.selfKnockback.Set(-1f, 0);
        properties.hitProperties.damage = 100f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel1_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel1_hitstun;
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
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        // hitSound = fighter.hitSounds[2];
        whiffSoundIndex = 1;
        hitSoundIndex = 2;

        properties.AnimationName = "Fullpunch";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Medium;
        properties.attackStance = FighterStance.Standing;

        properties.blockProperties.knockback.Set(-3f, 0);
        properties.blockProperties.airKnockback.Set(-3f, 6f);
        properties.blockProperties.selfKnockback.Set(-6f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel2_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel2_blockstun;

        properties.hitProperties.knockback.Set(-5f, 0);
        properties.hitProperties.airKnockback.Set(-1.75f, 11f);
        properties.hitProperties.selfKnockback.Set(-5f, 0);
        properties.hitProperties.damage = 200f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel2_hitstun;
        
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

public class TwoB : GameAttack
{
    public TwoB() : base()
    {
        //conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new GestureCondition(this, new CrouchGesture()));
        conditions.Add(new ButtonCondition(this, new AttackB()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[3];
        whiffSoundIndex = 2;
        hitSoundIndex = 3;

        properties.AnimationName = "Crouchkick";

        properties.blockType = GameAttackProperties.BlockType.Low;
        properties.attackType = GameAttackProperties.AttackType.Medium;
        properties.attackStance = FighterStance.Crouching;

        properties.blockProperties.knockback.Set(-3f, 0);
        properties.blockProperties.airKnockback.Set(-3, 5f);
        properties.blockProperties.selfKnockback.Set(-6f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel2_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel2_blockstun;

        properties.hitProperties.knockback.Set(-3f, 9f);
        properties.hitProperties.airKnockback.Set(-1.5f, 11f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 250f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel2_hitstun;
        properties.hitProperties.hardKD = true;
        properties.hitProperties.groundBounceOnGroundedHit = true;
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
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[0];
        whiffSoundIndex = 0;
        hitSoundIndex = 0;

        properties.AnimationName = "JD";

        properties.blockType = GameAttackProperties.BlockType.High;
        properties.attackType = GameAttackProperties.AttackType.Medium;
        properties.attackStance = FighterStance.Air;

        properties.blockProperties.knockback.Set(-3.5f, 0);
        properties.blockProperties.airKnockback.Set(-3.5f, 5f);
        properties.blockProperties.selfKnockback.Set(-2f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel2_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel2_blockstun;

        properties.hitProperties.knockback.Set(-5f, 12f);
        properties.hitProperties.airKnockback.Set(-5f, 11f);
        properties.hitProperties.selfKnockback.Set(-0.5f, 0);
        properties.hitProperties.damage = 250f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel2_hitstun;
        properties.hitProperties.wallBounce = true;
    }
}

public class FiveC : GameAttack
{
    public FiveC() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new StanceCondition(this, FighterStance.Standing));
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        // hitSound = fighter.hitSounds[2];
        whiffSoundIndex = 1;
        hitSoundIndex = 2;

        properties.AnimationName = "ShoulderBash";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Heavy;
        properties.attackStance = FighterStance.Standing;

        properties.blockProperties.knockback.Set(-5f, 0);
        properties.blockProperties.airKnockback.Set(-5f, 5f);
        properties.blockProperties.selfKnockback.Set(-8f, 0);
        properties.blockProperties.damage = 0;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-7f, 8f);
        properties.hitProperties.airKnockback.Set(-11f, 12f);
        properties.hitProperties.selfKnockback.Set(-6f, 0);
        properties.hitProperties.damage = 400f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel3_hitstun;
        properties.hitProperties.wallBounce = true;
    }
}

public class TwoC : GameAttack
{
    public TwoC() : base()
    {
        //conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new GestureCondition(this, new CrouchGesture()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[3];
        whiffSoundIndex = 2;
        hitSoundIndex = 3;

        properties.AnimationName = "Launcher";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Heavy;
        properties.attackStance = FighterStance.Crouching;

        properties.blockProperties.knockback.Set(-5f, 0);
        properties.blockProperties.airKnockback.Set(-5, 8f);
        properties.blockProperties.selfKnockback.Set(-8f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-0.5f, 15f);
        properties.hitProperties.airKnockback.Set(-0.5f, 11f);
        properties.hitProperties.selfKnockback.Set(-3f, 0);
        properties.hitProperties.damage = 150f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel3_hitstun;
        properties.hitProperties.hardKD = false;
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

public class JumpC : GameAttack
{
    public JumpC() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, false));
        conditions.Add(new StanceCondition(this, FighterStance.Air));
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[3];
        whiffSoundIndex = 2;
        hitSoundIndex = 3;

        properties.AnimationName = "Jumpslice";

        properties.blockType = GameAttackProperties.BlockType.High;
        properties.attackType = GameAttackProperties.AttackType.Heavy;
        properties.attackStance = FighterStance.Air;

        properties.blockProperties.knockback.Set(-4f, 0);
        properties.blockProperties.airKnockback.Set(-4, 5f);
        properties.blockProperties.selfKnockback.Set(-3f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-6f, 0);
        properties.hitProperties.airKnockback.Set(-2f, 11f);
        //properties.hitProperties.airKnockback.Set(-2f, -11f);
        //properties.hitProperties.groundBounce = true;
        //properties.hitProperties.selfKnockback.Set(-3f, 8f);

        properties.hitProperties.selfKnockback.Set(-3f, 0);
        properties.hitProperties.damage = 450f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel3_hitstun;

        properties.landingLagTime = 4f / 60f;
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
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-5f, 16f);
        properties.hitProperties.airKnockback.Set(-5f, 14f);
        properties.hitProperties.selfKnockback.Set(-2f, 0);
        properties.hitProperties.damage = 500f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel3_hitstun;
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
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-1f, 15f);
        properties.hitProperties.airKnockback.Set(-1f, -7f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 300f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel3_hitstun;
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
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel1_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel1_blockstun;

        properties.hitProperties.knockback.Set(9f, 9f);
        properties.hitProperties.airKnockback.Set(9f, 9f);
        properties.hitProperties.selfKnockback.Set(0, 0);
        properties.hitProperties.damage = 100f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel1_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel1_hitstun;
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
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel2_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel2_blockstun;

        properties.hitProperties.knockback.Set(-3f, 12f);
        properties.hitProperties.airKnockback.Set(-3f, 12f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 125f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel2_hitstun;
        properties.hitProperties.hardKD = false;

        fireballProperties = new GameAttackProperties(this);

        fireballProperties.blockType = GameAttackProperties.BlockType.Mid;
        fireballProperties.attackType = GameAttackProperties.AttackType.Special;
        fireballProperties.attackStance = FighterStance.Standing;

        fireballProperties.blockProperties.knockback.Set(-5f, 0);
        fireballProperties.blockProperties.airKnockback.Set(-3f, 9f);
        fireballProperties.blockProperties.damage = 75f;
        fireballProperties.blockProperties.hitstopTime = FighterAttacks.attackLevel1_blockhitstop;
        fireballProperties.blockProperties.stunTime = FighterAttacks.attackLevel2_blockstun;

        fireballProperties.hitProperties.knockback.Set(-6f, 0f);
        fireballProperties.hitProperties.airKnockback.Set(-2f, 13f);
        fireballProperties.hitProperties.damage = 200f;
        fireballProperties.hitProperties.hitstopTime = FighterAttacks.attackLevel1_hithitstop;
        fireballProperties.hitProperties.stunTime = FighterAttacks.attackLevel2_hitstun;
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
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel2_hitstun;
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
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel2_hitstun;
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