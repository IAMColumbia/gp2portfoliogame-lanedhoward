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
                new TwoC(),
                new FiveC(),
                new JumpC(),
                new SixTwoThreeA(),
                new TwoOneFourB(),
                new TwoThreeSixC(),
                new BackThrowWhiff(new GrabSuccess()),
                new GrabWhiff(new GrabSuccess()),
                new BackDash(),
                new ForwardDash(),
                new NeutralDash()
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
    static FighterAttacks()
    {
        attackLevel1_hithitstop = 7f / 60f;
        attackLevel1_blockhitstop = 5f / 60f;
        attackLevel1_hitstun = 14f / 60f;
        attackLevel1_blockstun = 12f / 60f;

        attackLevel2_hithitstop = 9f / 60f;
        attackLevel2_blockhitstop = 7f / 60f;
        attackLevel2_hitstun = 19f / 60f;
        attackLevel2_blockstun = 16f / 60f;

        attackLevel3_hithitstop = 11f / 60f;
        attackLevel3_blockhitstop = 9f / 60f;
        attackLevel3_hitstun = 25f / 60f;
        attackLevel3_blockstun = 20f / 60f;
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
        properties.blockProperties.airKnockback.Set(-2.5f, 3);
        properties.blockProperties.selfKnockback.Set(-5f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel1_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel1_blockstun;

        properties.hitProperties.knockback.Set(-3f, 0);
        properties.hitProperties.airKnockback.Set(-3f, 6f);
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
        properties.blockProperties.airKnockback.Set(-2.5f, 3);
        properties.blockProperties.selfKnockback.Set(-5f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel1_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel1_blockstun;

        properties.hitProperties.knockback.Set(-3f, 0);
        properties.hitProperties.airKnockback.Set(-3f, 6f);
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
        properties.blockProperties.airKnockback.Set(-2.5f, 3);
        properties.blockProperties.selfKnockback.Set(-1f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel1_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel1_blockstun;

        properties.hitProperties.knockback.Set(-3f, 0);
        properties.hitProperties.airKnockback.Set(-3f, 6f);
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
        properties.blockProperties.airKnockback.Set(-3f, 3);
        properties.blockProperties.selfKnockback.Set(-6f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel2_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel2_blockstun;

        properties.hitProperties.knockback.Set(-5f, 0);
        properties.hitProperties.airKnockback.Set(-5f, 7f);
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
        properties.blockProperties.airKnockback.Set(-3, 3);
        properties.blockProperties.selfKnockback.Set(-6f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel2_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel2_blockstun;

        properties.hitProperties.knockback.Set(-6f, 5f);
        properties.hitProperties.airKnockback.Set(-5f, 9f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 250f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel2_hitstun;
        properties.hitProperties.hardKD = true;
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
        properties.blockProperties.airKnockback.Set(-3.5f, 3);
        properties.blockProperties.selfKnockback.Set(-2f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel2_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel2_blockstun;

        properties.hitProperties.knockback.Set(-7f, 10f);
        properties.hitProperties.airKnockback.Set(-7f, 9f);
        properties.hitProperties.selfKnockback.Set(-0.5f, 0);
        properties.hitProperties.damage = 200f;
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
        properties.blockProperties.airKnockback.Set(-5f, 3);
        properties.blockProperties.selfKnockback.Set(-8f, 0);
        properties.blockProperties.damage = 0;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-7f, 5f);
        properties.hitProperties.airKnockback.Set(-11f, 10f);
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
        properties.blockProperties.airKnockback.Set(-5, 5);
        properties.blockProperties.selfKnockback.Set(-8f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-3f, 13.5f);
        properties.hitProperties.airKnockback.Set(-3f, 9f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
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
        properties.blockProperties.airKnockback.Set(-4, 3);
        properties.blockProperties.selfKnockback.Set(-6f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-6f, 0);
        properties.hitProperties.airKnockback.Set(-6f, 5f);
        properties.hitProperties.selfKnockback.Set(-5f, 0);
        properties.hitProperties.damage = 300f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel3_hitstun;
    }
}

public class SixTwoThreeA : GameAttack
{
    public SixTwoThreeA() : base()
    {
        conditions.Add(new GestureCondition(this, new DragonPunch()));
        conditions.Add(new ButtonCondition(this, new AttackA()));
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

        properties.blockProperties.knockback.Set(-2f, 0);
        properties.blockProperties.airKnockback.Set(-2f, 3);
        properties.blockProperties.selfKnockback.Set(-4f, 0);
        properties.blockProperties.damage = 50f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-5f, 13f);
        properties.hitProperties.airKnockback.Set(-5f, 10f);
        properties.hitProperties.selfKnockback.Set(-2f, 0);
        properties.hitProperties.damage = 350f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel3_hitstun;
        properties.hitProperties.hardKD = true;
    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        fighter.isStrikeInvulnerable = true;
    }

    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);
        fighter.isStrikeInvulnerable = false;
    }
}

public class TwoOneFourB : GameAttack
{
    public TwoOneFourB() : base()
    {
        conditions.Add(new GestureCondition(this, new QuarterCircleBack()));
        conditions.Add(new ButtonCondition(this, new AttackB()));
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
        properties.blockProperties.airKnockback.Set(-3f, 3);
        properties.blockProperties.selfKnockback.Set(-8f, 0);
        properties.blockProperties.damage = 50f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-4f, 13.5f);
        properties.hitProperties.airKnockback.Set(-4f, 6f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 300f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel3_hitstun;
        properties.hitProperties.hardKD = false;
    }
}

public class TwoThreeSixC : GameAttack
{
    public TwoThreeSixC() : base()
    {
        conditions.Add(new GestureCondition(this, new QuarterCircleForward()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[3];
        whiffSoundIndex = 2;
        hitSoundIndex = 3;

        properties.AnimationName = "FireballStomp";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

        properties.blockProperties.knockback.Set(-5f, 0);
        properties.blockProperties.airKnockback.Set(-3f, 5f);
        properties.blockProperties.selfKnockback.Set(-7f, 0);
        properties.blockProperties.damage = 50f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel2_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel2_blockstun;

        properties.hitProperties.knockback.Set(-3f, 6f);
        properties.hitProperties.airKnockback.Set(-3f, 6f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 125f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel2_hitstun;
        properties.hitProperties.hardKD = false;
    }
}

public class GrabWhiff : ThrowAttack
{
    public GrabWhiff(ThrowAttackSuccess _success) : base(_success)
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackD()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new StanceCondition(this, FighterStance.Standing));
        conditions.Add(new NoGatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[0];
        whiffSoundIndex = 0;
        hitSoundIndex = 0;

        properties.AnimationName = "ThrowWhiff";
    }
}

public class BackThrowWhiff : ThrowAttack
{
    public BackThrowWhiff(ThrowAttackSuccess _success) : base(_success)
    {
        conditions.Add(new GestureCondition(this, new BackGesture()));
        conditions.Add(new ButtonCondition(this, new AttackD()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new StanceCondition(this, FighterStance.Standing));
        conditions.Add(new NoGatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[0];
        whiffSoundIndex = 0;
        hitSoundIndex = 0;

        properties.AnimationName = "ThrowWhiff";
    }

    public override void OnHit(FighterMain fighter, FighterMain otherFighter)
    {
        Vector3 originalPos = fighter.transform.position;
        fighter.transform.position = fighter.throwPivot.position;
        otherFighter.transform.position = originalPos;
        fighter.AutoTurnaround();

        base.OnHit(fighter, otherFighter);
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

        properties.hitProperties.knockback.Set(-5f, 7f);
        properties.hitProperties.selfKnockback.Set(0f, 0);
        properties.hitProperties.damage = 250f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel2_hitstun;
        properties.hitProperties.hardKD = true;
    }
}

public class BackDash : GameAttack
{
    public BackDash() : base()
    {
        conditions.Add(new GestureCondition(this, new BackGesture()));
        conditions.Add(new ButtonCondition(this, new DashMacro()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new StanceCondition(this, FighterStance.Standing));
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
public class ForwardDash : GameAttack
{
    public ForwardDash() : base()
    {
        conditions.Add(new GestureCondition(this, new ForwardGesture()));
        conditions.Add(new ButtonCondition(this, new DashMacro()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new StanceCondition(this, FighterStance.Standing));
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

public class NeutralDash : GameAttack
{
    public NeutralDash() : base()
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