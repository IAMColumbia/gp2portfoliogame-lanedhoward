using CommandInputReaderLibrary.Gestures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class BulletTrainAttacks
{
    public static List<GameAttack> GetBulletTrainAttacks()
    {
        GameAttack stance = new GunStance();

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
                new GunDraw(stance),
                new GunHolster(),
                new Gun5Shot(stance),
                new Gun2Shot(stance),
                new Gun4Shot(stance),
                new Gun6Shot(stance),
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

/// <summary>
/// An attack that transitions to gun stance on ending.
/// </summary>
public abstract class GunStanceAttack : GameAttack
{
    protected GameAttack stance;
    public GunStanceAttack(GameAttack _stance) : base()
    {
        stance = _stance;
    }

    public override void OnAnimationEnd(FighterMain fighter)
    {
        base.OnAnimationEnd(fighter);

        fighter.SetCurrentAttack(stance);
    }
}

public class GunStance : GameAttack
{
    public GunStance() : base()
    {
        whiffSoundIndex = -1;

        properties.AnimationName = "GunStance";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        fighter.canAct = true;
    }
}

public class GunDraw : GunStanceAttack
{
    public GunDraw(GameAttack stance) : base(stance)
    {
        conditions.Add(new GestureCondition(this, new QuarterCircleForward()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new GatlingCondition(this));

        whiffSoundIndex = 2;

        properties.AnimationName = "GunDraw";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

    }
}

public class GunHolster : GameAttack
{
    public GunHolster() : base()
    {
        conditions.Add(new GestureCondition(this, new QuarterCircleBack()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, true));
        //conditions.Add(new FollowUpCondition(this, typeof(GunStanceAttack)));
        conditions.Add(new LogicalOrCondition(this, 
            new FollowUpCondition(this, typeof(GunStance)), 
            new FollowUpCondition(this, typeof(GunStanceAttack))));

        whiffSoundIndex = 2;

        properties.AnimationName = "GunHolster";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

    }
}

public class Gun5Shot : GunStanceAttack
{
    public Gun5Shot(GameAttack stance) : base(stance)
    {
        conditions.Add(new GestureCondition(this, new NeutralGesture()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new LogicalOrCondition(this,
            new FollowUpCondition(this, typeof(GunStance)),
            new FollowUpCondition(this, typeof(GunStanceAttack))));

        whiffSoundIndex = 5;
        hitSoundIndex = 2;

        properties.AnimationName = "Gun5Shot";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;


        properties.blockProperties.knockback.Set(-7f, 0);
        properties.blockProperties.airKnockback.Set(-7f, 6f);
        properties.blockProperties.selfKnockback.Set(-9f, 0);
        properties.blockProperties.damage = 50;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel2_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel2_blockstun;

        properties.hitProperties.knockback.Set(-9f, 9f);
        properties.hitProperties.airKnockback.Set(-9f, 11f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 200f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel2_hitstun;

    }
}

public class Gun2Shot : GunStanceAttack
{
    public Gun2Shot(GameAttack stance) : base(stance)
    {
        conditions.Add(new GestureCondition(this, new CrouchGesture()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new LogicalOrCondition(this,
            new FollowUpCondition(this, typeof(GunStance)),
            new FollowUpCondition(this, typeof(GunStanceAttack))));

        whiffSoundIndex = 5;
        hitSoundIndex = 2;

        properties.AnimationName = "Gun2Shot";

        properties.blockType = GameAttackProperties.BlockType.Low;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;


        properties.blockProperties.knockback.Set(-4f, 0);
        properties.blockProperties.airKnockback.Set(-5f, 6f);
        properties.blockProperties.selfKnockback.Set(-4f, 0);
        properties.blockProperties.damage = 50;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel2_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel2_blockstun;

        properties.hitProperties.knockback.Set(-1.5f, 16f);
        properties.hitProperties.airKnockback.Set(-2f, 13f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 200f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel2_hitstun;

    }
}

public class Gun4Shot : GunStanceAttack
{
    public Gun4Shot(GameAttack stance) : base(stance)
    {
        conditions.Add(new GestureCondition(this, new BackGesture()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new LogicalOrCondition(this,
            new FollowUpCondition(this, typeof(GunStance)),
            new FollowUpCondition(this, typeof(GunStanceAttack))));

        whiffSoundIndex = 5;
        hitSoundIndex = 2;

        properties.AnimationName = "Gun4Shot";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;


        properties.blockProperties.knockback.Set(-9f, 0);
        properties.blockProperties.airKnockback.Set(-9f, 6f);
        properties.blockProperties.selfKnockback.Set(-6f, 0);
        properties.blockProperties.damage = 50;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel2_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel2_blockstun;

        properties.hitProperties.knockback.Set(-11f, 0f);
        properties.hitProperties.airKnockback.Set(-11f, 15f);
        properties.hitProperties.selfKnockback.Set(-5f, 0);
        properties.hitProperties.damage = 200f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel2_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel2_hitstun;

    }
}

public class Gun6Shot : GunStanceAttack
{
    public Gun6Shot(GameAttack stance) : base(stance)
    {
        conditions.Add(new GestureCondition(this, new ForwardGesture()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new LogicalOrCondition(this,
            new FollowUpCondition(this, typeof(GunStance)),
            new FollowUpCondition(this, typeof(GunStanceAttack))));

        whiffSoundIndex = 5;
        hitSoundIndex = 2;

        properties.AnimationName = "Gun6Shot";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;


        properties.blockProperties.knockback.Set(-7f, 0);
        properties.blockProperties.airKnockback.Set(-7f, 6f);
        properties.blockProperties.selfKnockback.Set(-9f, 0);
        properties.blockProperties.damage = 50;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-9f, 9f);
        properties.hitProperties.airKnockback.Set(-9f, 11f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 200f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel3_hitstun;

    }
}