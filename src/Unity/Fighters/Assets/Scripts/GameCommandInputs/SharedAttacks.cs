using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class AttackSettings
{
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
    static AttackSettings()
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
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel1_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel1_blockstun;

        properties.hitProperties.knockback.Set(-3f, 0);
        properties.hitProperties.airKnockback.Set(-1.75f, 9f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 85f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel1_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel1_hitstun;
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
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel1_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel1_blockstun;

        properties.hitProperties.knockback.Set(-3f, 0);
        properties.hitProperties.airKnockback.Set(-1.75f, 9f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 75f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel1_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel1_hitstun;
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
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel1_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel1_blockstun;

        properties.hitProperties.knockback.Set(-3f, 0);
        properties.hitProperties.airKnockback.Set(-1.5f, 9f);
        properties.hitProperties.selfKnockback.Set(-1f, 0);
        properties.hitProperties.damage = 100f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel1_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel1_hitstun;
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
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel2_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel2_blockstun;

        properties.hitProperties.knockback.Set(-5f, 0);
        properties.hitProperties.airKnockback.Set(-1.75f, 11f);
        properties.hitProperties.selfKnockback.Set(-5f, 0);
        properties.hitProperties.damage = 200f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel2_hitstun;

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
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel2_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel2_blockstun;

        properties.hitProperties.knockback.Set(-3f, 9f);
        properties.hitProperties.airKnockback.Set(-1.5f, 11f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 250f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel2_hitstun;
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
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel2_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel2_blockstun;

        properties.hitProperties.knockback.Set(-5f, 12f);
        properties.hitProperties.airKnockback.Set(-5f, 11f);
        properties.hitProperties.selfKnockback.Set(-0.5f, 0);
        properties.hitProperties.damage = 250f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel2_hitstun;
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
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-7f, 8f);
        properties.hitProperties.airKnockback.Set(-11f, 12f);
        properties.hitProperties.selfKnockback.Set(-6f, 0);
        properties.hitProperties.damage = 400f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel3_hitstun;
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
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-0.5f, 15f);
        properties.hitProperties.airKnockback.Set(-0.5f, 11f);
        properties.hitProperties.selfKnockback.Set(-3f, 0);
        properties.hitProperties.damage = 150f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel3_hitstun;
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
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-6f, 0);
        properties.hitProperties.airKnockback.Set(-2f, 11f);
        //properties.hitProperties.airKnockback.Set(-2f, -11f);
        //properties.hitProperties.groundBounce = true;
        //properties.hitProperties.selfKnockback.Set(-3f, 8f);

        properties.hitProperties.selfKnockback.Set(-3f, 0);
        properties.hitProperties.damage = 450f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel3_hitstun;

        properties.landingLagTime = 4f / 60f;
    }
}