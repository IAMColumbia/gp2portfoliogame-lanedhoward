using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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

public class Jab : GameAttack
{
    public Jab() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackA()));
        conditions.Add(new GroundedCondition(this, true));
        //conditions.Add(new StanceCondition(this, FighterStance.Standing));
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

public class CrouchJab : GameAttack
{
    public CrouchJab() : base()
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

public class JumpKnee : GameAttack
{
    public JumpKnee() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackA()));
        conditions.Add(new GroundedCondition(this, false));
        //conditions.Add(new StanceCondition(this, FighterStance.Air));
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

public class FullPunch : GameAttack
{
    public FullPunch() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackB()));
        conditions.Add(new GroundedCondition(this, true));
        //conditions.Add(new StanceCondition(this, FighterStance.Standing));
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

public class CrouchKick : GameAttack
{
    public CrouchKick() : base()
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
        properties.hitProperties.groundBounceOnGroundedHit = false;
    }
}

public class JumpDust : GameAttack
{
    public JumpDust() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackB()));
        conditions.Add(new GroundedCondition(this, false));
        //conditions.Add(new StanceCondition(this, FighterStance.Air));
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

public class ShoulderBash : GameAttack
{
    public ShoulderBash() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, true));
        //conditions.Add(new StanceCondition(this, FighterStance.Standing));
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

public class Launcher : GameAttack
{
    public Launcher() : base()
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

public class JumpSlice : GameAttack
{
    public JumpSlice() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, false));
        //conditions.Add(new StanceCondition(this, FighterStance.Air));
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
        //conditions.Add(new StanceCondition(this, FighterStance.Air));
        conditions.Add(new NoGatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[0];
        whiffSoundIndex = 0;
        hitSoundIndex = 0;

        properties.AnimationName = "ThrowWhiff";

        properties.attackStance = FighterStance.Air;
        properties.stanceToBeGrabbed = FighterStance.Air;

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
        //conditions.Add(new StanceCondition(this, FighterStance.Air));
        conditions.Add(new NoGatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[0];
        whiffSoundIndex = 0;
        hitSoundIndex = 0;

        properties.AnimationName = "ThrowWhiff";

        properties.attackStance = FighterStance.Air;
        properties.stanceToBeGrabbed = FighterStance.Air;

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

// second normals

public class Roundhouse : GameAttackStartupVelocity
{
    public Roundhouse() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, true));
        //conditions.Add(new StanceCondition(this, FighterStance.Standing));
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        // hitSound = fighter.hitSounds[2];
        whiffSoundIndex = 1;
        hitSoundIndex = 2;

        properties.AnimationName = "Roundhouse";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Heavy;
        properties.attackStance = FighterStance.Standing;

        properties.blockProperties.knockback.Set(-5f, 0);
        properties.blockProperties.airKnockback.Set(-5f, 5f);
        properties.blockProperties.selfKnockback.Set(-8f, 0);
        properties.blockProperties.damage = 0;
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-7f, 9f);
        properties.hitProperties.airKnockback.Set(-7f, 8f);
        properties.hitProperties.selfKnockback.Set(-6f, 0);
        properties.hitProperties.damage = 400f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel3_hitstun;
        properties.hitProperties.wallBounce = true;

        velocity = new Vector2(8.5f, 0f);
    }
}

public class CrouchSweep : GameAttackStartupVelocity
{
    public CrouchSweep() : base()
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

        properties.AnimationName = "Sweep";

        properties.blockType = GameAttackProperties.BlockType.Low;
        properties.attackType = GameAttackProperties.AttackType.Heavy;
        properties.attackStance = FighterStance.Crouching;

        properties.blockProperties.knockback.Set(-5f, 0);
        properties.blockProperties.airKnockback.Set(-5, 8f);
        properties.blockProperties.selfKnockback.Set(-8f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-6f, 10f);
        properties.hitProperties.airKnockback.Set(-3.5f, 11f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 400f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel3_hitstun;
        properties.hitProperties.hardKD = true;
        properties.hitProperties.groundBounceOnGroundedHit = false;

        velocity = new Vector2(9f, 0f);
    }
}

public class JumpStomp : GameAttack
{
    public JumpStomp() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, false));
        //Add(new StanceCondition(this, FighterStance.Air));
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[3];
        whiffSoundIndex = 2;
        hitSoundIndex = 3;

        properties.AnimationName = "Stomp";

        properties.blockType = GameAttackProperties.BlockType.High;
        properties.attackType = GameAttackProperties.AttackType.Heavy;
        properties.attackStance = FighterStance.Air;

        properties.blockProperties.knockback.Set(-4f, 0);
        properties.blockProperties.airKnockback.Set(-4, 5f);
        properties.blockProperties.selfKnockback.Set(0, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel4_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel4_blockstun;

        properties.hitProperties.knockback.Set(-3f, 15f);
        //properties.hitProperties.airKnockback.Set(-2f, 11f);
        properties.hitProperties.airKnockback.Set(-2f, -11f);
        properties.hitProperties.groundBounceOnAirHit = true;
        properties.hitProperties.selfKnockback.Set(0f, 0f);

        //properties.hitProperties.selfKnockback.Set(-3f, 0);
        properties.hitProperties.damage = 450f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel4_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel4_hitstun;

        properties.landingLagTime = 6f / 60f;

        properties.landCancelActive = false;
    }

    public override void OnHit(FighterMain fighter, FighterMain otherFighter)
    {
        base.OnHit(fighter, otherFighter);
        fighter.OnHaltVerticalVelocity();
        fighter.OnVelocityImpulseRelativeToSelf(new Vector2(0, 12));
    }

    public override void OnBlock(FighterMain fighter, FighterMain otherFighter)
    {
        base.OnBlock(fighter, otherFighter);
        fighter.OnHaltVerticalVelocity();
        fighter.OnVelocityImpulseRelativeToSelf(new Vector2(0, 12));
    }
}