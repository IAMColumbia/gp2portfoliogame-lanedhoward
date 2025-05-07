using CommandInputReaderLibrary.Gestures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class FormerKingAttacks
{
    public static List<GameAttack> GetFormerKingAttacks()
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
                new Scepter1(),
                new Scepter2(),
                new Scepter3(),
                new JumpSlice(),
                new Vault(),
                new VaultKick(),
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
                new BackWavedashCancel(),
                new Burst(new ForwardDiveRoll(), new BackDiveRoll()),
                new ForwardDiveRoll(),
                new BackDiveRoll(),
            };
        return attacks;
    }

}

public class Scepter1 : GameAttack
{
    public Scepter1() : base()
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

        properties.AnimationName = "Scepter1";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Heavy;
        properties.attackStance = FighterStance.Standing;

        properties.blockProperties.knockback.Set(-3f, 0);
        properties.blockProperties.airKnockback.Set(-3f, 5);
        properties.blockProperties.selfKnockback.Set(-6f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel2_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel2_blockstun;

        properties.hitProperties.knockback.Set(-5f, 0);
        properties.hitProperties.airKnockback.Set(-1.75f, 12f);
        properties.hitProperties.selfKnockback.Set(-5f, 0);
        properties.hitProperties.damage = 200f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel2_hitstun;
    }
}

public class Scepter2 : GameAttack
{
    public Scepter2() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, true));
        //conditions.Add(new StanceCondition(this, FighterStance.Standing));
        conditions.Add(new FollowUpCondition(this, typeof(Scepter1)));

        //whiffSound = fighter.whiffSounds[0];
        // hitSound = fighter.hitSounds[2];
        whiffSoundIndex = 1;
        hitSoundIndex = 2;

        properties.AnimationName = "Scepter2";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Heavy;
        properties.attackStance = FighterStance.Standing;

        properties.blockProperties.knockback.Set(-3f, 0);
        properties.blockProperties.airKnockback.Set(-3f, 5);
        properties.blockProperties.selfKnockback.Set(-6f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel2_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel2_blockstun;

        properties.hitProperties.knockback.Set(-5f, 0);
        properties.hitProperties.airKnockback.Set(-1.75f, 12f);
        properties.hitProperties.selfKnockback.Set(-5f, 0);
        properties.hitProperties.damage = 175f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel2_hitstun;
    }
}

public class Scepter3 : GameAttack
{
    public Scepter3() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, true));
        //conditions.Add(new StanceCondition(this, FighterStance.Standing));
        conditions.Add(new FollowUpCondition(this, typeof(Scepter2)));

        //whiffSound = fighter.whiffSounds[0];
        // hitSound = fighter.hitSounds[2];
        whiffSoundIndex = 1;
        hitSoundIndex = 2;

        properties.AnimationName = "Scepter3";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Heavy;
        properties.attackStance = FighterStance.Standing;

        properties.blockProperties.knockback.Set(-3f, 0);
        properties.blockProperties.airKnockback.Set(-3f, 5);
        properties.blockProperties.selfKnockback.Set(-6f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel1_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel1_blockstun;

        properties.hitProperties.knockback.Set(-1.5f, 10f);
        properties.hitProperties.airKnockback.Set(-1.25f, 9f);
        properties.hitProperties.selfKnockback.Set(-2f, 0);
        properties.hitProperties.damage = 175f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel1_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel1_hitstun;
    }
}

public class Vault : GameAttack
{
    Vector2 airdashVelocity;
    public Vault() : base()
    {
        conditions.Add(new GestureCondition(this, new BackForwardCharge()));
        conditions.Add(new ButtonCondition(this, new AttackA()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[3];
        whiffSoundIndex = 0;
        hitSoundIndex = 1;

        properties.AnimationName = "Vault";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

        airdashVelocity = new Vector2(18f, 0);
    }

    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);

        fighter.OnVelocityImpulseRelativeToSelf(airdashVelocity);
    }

    public override void OnRecovery(FighterMain fighter)
    {
        base.OnRecovery(fighter);

        fighter.canAct = true;
    }
}

public class VaultKick : GameAttack
{
    Vector2 airdashVelocity;
    public VaultKick() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackA()));
        conditions.Add(new GroundedCondition(this, true));
        //conditions.Add(new StanceCondition(this, FighterStance.Standing));
        conditions.Add(new FollowUpCondition(this, typeof(Vault)));

        //whiffSound = fighter.whiffSounds[0];
        // hitSound = fighter.hitSounds[2];
        whiffSoundIndex = 1;
        hitSoundIndex = 2;

        properties.AnimationName = "VaultKick";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

        properties.blockProperties.knockback.Set(-5f, 0);
        properties.blockProperties.airKnockback.Set(-5f, 5);
        properties.blockProperties.selfKnockback.Set(-8f, 0);
        properties.blockProperties.damage = 0;
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-9f, 18f);
        properties.hitProperties.airKnockback.Set(-9f, 18f);
        properties.hitProperties.selfKnockback.Set(-6f, 0);
        properties.hitProperties.damage = 350f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel3_hitstun;
        properties.hitProperties.wallBounce = true;

        airdashVelocity = new Vector2(9f, 0);
    }

    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);

        fighter.OnVelocityImpulseRelativeToSelf(airdashVelocity);
    }
}