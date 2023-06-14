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
                new TwoA(),
                new FiveA(),
                new JumpA(),
                new TwoB(),
                new FiveB(),
                new JumpB(),
                new TwoC(),
                new Scepter1(),
                new Scepter2(),
                new Scepter3(),
                new JumpC(),
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

public class Scepter1 : GameAttack
{
    public Scepter1() : base()
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

        properties.AnimationName = "Scepter1";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Heavy;
        properties.attackStance = FighterStance.Standing;

        properties.blockProperties.knockback.Set(-3f, 0);
        properties.blockProperties.airKnockback.Set(-3f, 3);
        properties.blockProperties.selfKnockback.Set(-6f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel2_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel2_blockstun;

        properties.hitProperties.knockback.Set(-5f, 0);
        properties.hitProperties.airKnockback.Set(-1.75f, 7f);
        properties.hitProperties.selfKnockback.Set(-5f, 0);
        properties.hitProperties.damage = 200f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel2_hitstun;
    }
}

public class Scepter2 : GameAttack
{
    public Scepter2() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new StanceCondition(this, FighterStance.Standing));
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
        properties.blockProperties.airKnockback.Set(-3f, 3);
        properties.blockProperties.selfKnockback.Set(-6f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel2_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel2_blockstun;

        properties.hitProperties.knockback.Set(-5f, 0);
        properties.hitProperties.airKnockback.Set(-1.75f, 7f);
        properties.hitProperties.selfKnockback.Set(-5f, 0);
        properties.hitProperties.damage = 200f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel2_hitstun;
    }
}

public class Scepter3 : GameAttack
{
    public Scepter3() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new StanceCondition(this, FighterStance.Standing));
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
        properties.blockProperties.airKnockback.Set(-3f, 3);
        properties.blockProperties.selfKnockback.Set(-6f, 0);
        properties.blockProperties.damage = 0f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel2_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel2_blockstun;

        properties.hitProperties.knockback.Set(-5f, 0);
        properties.hitProperties.airKnockback.Set(-1.75f, 7f);
        properties.hitProperties.selfKnockback.Set(-5f, 0);
        properties.hitProperties.damage = 200f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel2_hitstun;
    }
}