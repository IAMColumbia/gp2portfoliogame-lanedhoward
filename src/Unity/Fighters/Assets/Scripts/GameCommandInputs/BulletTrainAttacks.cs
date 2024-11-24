using CommandInputReaderLibrary;
using CommandInputReaderLibrary.Gestures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public static class BulletTrainAttacks
{
    public static List<GameAttack> GetBulletTrainAttacks()
    {
        GameAttack stance = new GunStance();
        SuperGunHolster superGunHolster = new SuperGunHolster();
        GameAttack superGunStance = new SuperGunStance(superGunHolster);


        List<GameAttack> attacks = new List<GameAttack>()
            {
                new CrouchJab(),
                new Jab(),
                new JumpKnee(),
                new CrouchKick(),
                new FullPunch(),
                new JumpDust(),
                new CrouchSweep(),
                new Roundhouse(),
                new JumpStomp(),
                new GunDraw(stance),
                new GunHolster(),
                new Gun5Shot(stance),
                new Gun2Shot(stance),
                new Gun4Shot(stance),
                new Gun6Shot(stance),
                new GunEmpty(stance),
                new GunReload(stance),
                new GunWhip(stance),
                new GunSpinForward(stance),
                new GunSpinBackward(stance),
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
                new SuperGunDraw(superGunStance),
                superGunHolster,
                new SuperGunSpinForward(superGunStance),
                new SuperGunSpinBackward(superGunStance),
                new SuperGunShot(superGunStance),


            };
        return attacks;
    }

    public static List<IReadableGesture> GetGestures()
    {
        List<IReadableGesture> gestures = new List<IReadableGesture>()
            {
                new QuarterCircleBack(),
                new QuarterCircleForward(),
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

public class HasStockCondition : GameAttackCondition
{
    public HasStockCondition(GameAttack _parent) : base(_parent)
    {
    }

    public override bool CanExecute(GameMoveInput moveInput, FighterMain fighter)
    {
        return fighter.GetStocks() > 0;
    }
}

public class AmmoNotFullCondition : GameAttackCondition
{
    public AmmoNotFullCondition(GameAttack _parent) : base(_parent)
    {
    }

    public override bool CanExecute(GameMoveInput moveInput, FighterMain fighter)
    {
        return fighter.GetStocks() < 6;
    }
}

public class AmmoEmptyCondition : GameAttackCondition
{
    public AmmoEmptyCondition(GameAttack _parent) : base(_parent)
    {
    }

    public override bool CanExecute(GameMoveInput moveInput, FighterMain fighter)
    {
        return fighter.GetStocks() == 0;
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

        properties.landCancelStartup = false;
        properties.landCancelActive = false;
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
        conditions.Add(new LogicalOrCondition(this,
            // normal input
            new LogicalAndCondition(this,
                new GestureCondition(this, new QuarterCircleForward()),
                new ButtonCondition(this, new AttackC())),
            // simple input
            new LogicalAndCondition(this,
                new GestureCondition(this, new NoGesture()),
                new ButtonCondition(this, new SpecialButton()))
            ));
        //conditions.Add(new GestureCondition(this, new QuarterCircleForward()));
        //conditions.Add(new ButtonCondition(this, new AttackC()));
        //conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new GatlingCondition(this));

        whiffSoundIndex = 2;

        properties.AnimationName = "GunDraw";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

        properties.landCancelRecovery = false;
        properties.landCancelStartup = false;
        properties.landCancelActive = false;
    }
}

public class GunHolster : GameAttack
{
    public GunHolster() : base()
    {
        conditions.Add(new LogicalOrCondition(this,
            // normal input
            new LogicalAndCondition(this,
                new GestureCondition(this, new QuarterCircleBack()),
                new ButtonCondition(this, new AttackC())),
            // simple input
            new LogicalAndCondition(this,
                new GestureCondition(this, new NoGesture()),
                new ButtonCondition(this, new SpecialButton()))
            ));
        //conditions.Add(new GestureCondition(this, new QuarterCircleBack()));
        //conditions.Add(new ButtonCondition(this, new AttackC()));
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
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new LogicalOrCondition(this,
            new FollowUpCondition(this, typeof(GunStance)),
            new FollowUpCondition(this, typeof(GunStanceAttack))));
        conditions.Add(new HasStockCondition(this));

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
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-9f, 9f);
        properties.hitProperties.airKnockback.Set(-8f, 9f);
        properties.hitProperties.selfKnockback.Set(-3f, 0);
        properties.hitProperties.damage = 325;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel3_hitstun;

    }

    public override void OnStartup(FighterMain fighter)
    {
    }

    public override void OnActive(FighterMain fighter)
    {
        // play sound (gunshot) when active
        base.OnStartup(fighter);
        base.OnActive(fighter);

        fighter.SetStocks(fighter.GetStocks() - 1);
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
        conditions.Add(new HasStockCondition(this));


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
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-1.5f, 14f);
        properties.hitProperties.airKnockback.Set(-2f, 13f);
        properties.hitProperties.selfKnockback.Set(-3f, 0);
        properties.hitProperties.damage = 325;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel3_hitstun;

    }

    public override void OnStartup(FighterMain fighter)
    {
    }

    public override void OnActive(FighterMain fighter)
    {
        // play sound (gunshot) when active
        base.OnStartup(fighter);
        base.OnActive(fighter);

        fighter.SetStocks(fighter.GetStocks() - 1);
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
        conditions.Add(new HasStockCondition(this));


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
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-10f, 0f);
        properties.hitProperties.airKnockback.Set(-6f, 10f);
        properties.hitProperties.selfKnockback.Set(-3f, 0);
        properties.hitProperties.damage = 325;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel3_hitstun;

    }
    public override void OnStartup(FighterMain fighter)
    {
    }

    public override void OnActive(FighterMain fighter)
    {
        // play sound (gunshot) when active
        base.OnStartup(fighter);
        base.OnActive(fighter);

        fighter.SetStocks(fighter.GetStocks() - 1);
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
        conditions.Add(new HasStockCondition(this));


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
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-6f, 14f);
        properties.hitProperties.airKnockback.Set(-6f, 12f);
        properties.hitProperties.selfKnockback.Set(-3f, 0);
        properties.hitProperties.damage = 325;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel3_hitstun;

    }

    public override void OnStartup(FighterMain fighter)
    {
    }

    public override void OnActive(FighterMain fighter)
    {
        // play sound (gunshot) when active
        base.OnStartup(fighter);
        base.OnActive(fighter);

        fighter.SetStocks(fighter.GetStocks() - 1);
    }
}

public class GunEmpty : GunStanceAttack
{
    public GunEmpty(GameAttack stance) : base(stance)
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new LogicalOrCondition(this,
            new FollowUpCondition(this, typeof(GunStance)),
            new FollowUpCondition(this, typeof(GunStanceAttack))));
        conditions.Add(new AmmoEmptyCondition(this));

        whiffSoundIndex = 6;
        hitSoundIndex = 2;

        properties.AnimationName = "GunEmpty";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;


    }

    public override void OnStartup(FighterMain fighter)
    {
    }

    public override void OnActive(FighterMain fighter)
    {
        // play sound (misfire) when active
        base.OnStartup(fighter);
        base.OnActive(fighter);

        //fighter.SetStocks(fighter.GetStocks() - 1);
    }
}

public class GunReload : GunStanceAttack
{
    public GunReload(GameAttack stance) : base(stance)
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackA()));
        conditions.Add(new GroundedCondition(this, true));
        //conditions.Add(new FollowUpCondition(this, typeof(GunStanceAttack)));
        conditions.Add(new LogicalOrCondition(this,
            new FollowUpCondition(this, typeof(GunStance)),
            new FollowUpCondition(this, typeof(GunStanceAttack))));

        whiffSoundIndex = 2;

        properties.AnimationName = "GunReload";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

    }
    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);
        // not maxed out on ammo
        if (fighter.GetStocks() < 6)
        {
            // reload a bullet
            fighter.SetStocks(fighter.GetStocks() + 1);
            // play reload sound
            fighter.PlaySound(fighter.whiffSounds[7]);

        }
    }
    public override void OnRecovery(FighterMain fighter)
    {
        base.OnRecovery(fighter);
        fighter.canAct = true;
    }
}

public class GunSpinForward : GunStanceAttack
{
    Vector2 velocity;
    public GunSpinForward(GameAttack stance) : base(stance)
    {
        conditions.Add(new GestureCondition(this, new ForwardOrNeutralGesture()));
        conditions.Add(new ButtonCondition(this, new DashMacro()));
        conditions.Add(new GroundedCondition(this, true));
        //conditions.Add(new FollowUpCondition(this, typeof(GunStanceAttack)));
        conditions.Add(new LogicalOrCondition(this,
            new FollowUpCondition(this, typeof(GunStance)),
            new FollowUpCondition(this, typeof(GunStanceAttack))));

        whiffSoundIndex = 2;

        properties.AnimationName = "GunSpinForward";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

        velocity = new Vector2(12f, 0f);
    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        fighter.PlayWavedashVFX();
        fighter.OnHaltHorizontalVelocity();
        fighter.OnVelocityImpulseRelativeToSelf(velocity);
    }

    public override void OnRecovery(FighterMain fighter)
    {
        base.OnRecovery(fighter);
        fighter.canAct = true;
    }
}

public class GunSpinBackward : GunStanceAttack
{
    Vector2 velocity;
    public GunSpinBackward(GameAttack stance) : base(stance)
    {
        conditions.Add(new GestureCondition(this, new BackGesture()));
        conditions.Add(new ButtonCondition(this, new DashMacro()));
        conditions.Add(new GroundedCondition(this, true));
        //conditions.Add(new FollowUpCondition(this, typeof(GunStanceAttack)));
        conditions.Add(new LogicalOrCondition(this,
            new FollowUpCondition(this, typeof(GunStance)),
            new FollowUpCondition(this, typeof(GunStanceAttack))));

        whiffSoundIndex = 2;

        properties.AnimationName = "GunSpinBackward";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

        velocity = new Vector2(-12f, 0f);
    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        fighter.PlayWavedashVFX();
        fighter.OnHaltHorizontalVelocity();
        fighter.OnVelocityImpulseRelativeToSelf(velocity);
    }

    public override void OnRecovery(FighterMain fighter)
    {
        base.OnRecovery(fighter);
        fighter.canAct = true;
    }
}

public class GunWhip : GunStanceAttack
{
    public GunWhip(GameAttack stance) : base(stance)
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackB()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new LogicalOrCondition(this,
            new FollowUpCondition(this, typeof(GunStance)),
            new FollowUpCondition(this, typeof(GunStanceAttack))));

        whiffSoundIndex = 2;
        hitSoundIndex = 3;

        properties.AnimationName = "GunWhip";

        properties.blockType = GameAttackProperties.BlockType.High;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;


        properties.blockProperties.knockback.Set(-3f, 0);
        properties.blockProperties.airKnockback.Set(-3f, -3f);
        properties.blockProperties.selfKnockback.Set(-8f, 0);
        properties.blockProperties.damage = 50f;
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-1f, 9f);
        properties.hitProperties.airKnockback.Set(-1f, -3f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 300f;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel3_hitstun;
        properties.hitProperties.hardKD = true;

    }

}

public class SuperGunDraw : SuperGunStanceAttack
{
    float meterCost;
    public SuperGunDraw(GameAttack _stance) : base(_stance)
    {
        meterCost = 200;
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new SuperButton()));
        //conditions.Add(new GatlingCondition(this));
        conditions.Add(new LogicalOrCondition(this, 
            new GatlingCondition(this), //normal gatling
            new LogicalOrCondition(this, // or follow up from gunstance
                new FollowUpCondition(this, typeof(GunStance)),
                new FollowUpCondition(this, typeof(GunStanceAttack)))));
        conditions.Add(new MeterCostCondition(this, meterCost));

        whiffSoundIndex = 2;

        properties.AnimationName = "SuperGunDraw";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Super;
        properties.attackStance = FighterStance.Standing;

        properties.landCancelRecovery = false;
        properties.landCancelStartup = false;
        properties.landCancelActive = false;
    }
    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);
        fighter.SetStocks(4);
    }

    public override void OnSuperFlashStarted(FighterMain fighter)
    {
        fighter.CurrentMeter -= meterCost;
        fighter.StartSuperPortrait("Super Gun!!!");
        base.OnSuperFlashStarted(fighter);
        
    }

    public override HitReport? OnGetHitDuring(FighterMain fighter, GameAttackProperties properties)
    {
        // dont set to full revolver ammo if you get hit before the super takes its meter cost
        if (fighter.currentAttackState == CurrentAttackState.Startup) return null; 
        return base.OnGetHitDuring(fighter, properties);
    }
}

/// <summary>
/// An attack that transitions to gun stance on ending.
/// </summary>
public abstract class SuperGunStanceAttack : GameAttack
{
    protected GameAttack stance;
    public SuperGunStanceAttack(GameAttack _stance) : base()
    {
        stance = _stance;
    }

    public override void OnAnimationEnd(FighterMain fighter)
    {
        base.OnAnimationEnd(fighter);

        fighter.SetCurrentAttack(stance);
    }

    public override HitReport? OnGetHitDuring(FighterMain fighter, GameAttackProperties properties)
    {
        fighter.SetStocks(6);
        return base.OnGetHitDuring(fighter, properties);
    }
}

public class SuperGunStance : GameAttack
{
    SuperGunHolster holster;
    public SuperGunStance(SuperGunHolster holster) : base()
    {
        this.holster = holster;
        whiffSoundIndex = -1;

        properties.AnimationName = "SuperGunStance";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

        properties.landCancelStartup = false;
        properties.landCancelActive = false;
    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        if (fighter.GetStocks() <= 0)
        {
            fighter.SetCurrentAttack(holster);
            return;
        }
        
        fighter.canAct = true;

    }
    public override HitReport? OnGetHitDuring(FighterMain fighter, GameAttackProperties properties)
    {
        fighter.SetStocks(6);
        return base.OnGetHitDuring(fighter, properties);
    }
}


public interface ISuperGunSpinParent { }

public class SuperGunSpinForward : SuperGunStanceAttack, ISuperGunSpinParent
{
    Vector2 velocity;
    public SuperGunSpinForward(GameAttack stance) : base(stance)
    {
        conditions.Add(new GestureCondition(this, new ForwardOrNeutralGesture()));
        conditions.Add(new ButtonCondition(this, new DashMacro()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new LogicalOrCondition(this,
            new FollowUpCondition(this, typeof(SuperGunStance)),
            new FollowUpCondition(this, typeof(ISuperGunSpinParent))));

        whiffSoundIndex = 2;

        properties.AnimationName = "SuperGunSpinForward";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

        velocity = new Vector2(12f, 0f);
    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        fighter.PlayWavedashVFX();
        fighter.OnHaltHorizontalVelocity();
        fighter.OnVelocityImpulseRelativeToSelf(velocity);
    }

    public override void OnRecovery(FighterMain fighter)
    {
        base.OnRecovery(fighter);
        fighter.canAct = true;
    }
}

public class SuperGunSpinBackward : SuperGunStanceAttack, ISuperGunSpinParent
{
    Vector2 velocity;
    public SuperGunSpinBackward(GameAttack stance) : base(stance)
    {
        conditions.Add(new GestureCondition(this, new BackGesture()));
        conditions.Add(new ButtonCondition(this, new DashMacro()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new LogicalOrCondition(this,
            new FollowUpCondition(this, typeof(SuperGunStance)),
            new FollowUpCondition(this, typeof(ISuperGunSpinParent))));

        whiffSoundIndex = 2;

        properties.AnimationName = "SuperGunSpinBackward";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

        velocity = new Vector2(-12f, 0f);
    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        fighter.PlayWavedashVFX();
        fighter.OnHaltHorizontalVelocity();
        fighter.OnVelocityImpulseRelativeToSelf(velocity);
    }

    public override void OnRecovery(FighterMain fighter)
    {
        base.OnRecovery(fighter);
        fighter.canAct = true;
    }
}

public class SuperGunHolster : GameAttack
{
    public SuperGunHolster() : base()
    {
        conditions.Add(new LogicalOrCondition(this,
            // normal input
            new LogicalAndCondition(this,
                new GestureCondition(this, new QuarterCircleBack()),
                new ButtonCondition(this, new AttackC())),
            // simple input
            new LogicalAndCondition(this,
                new GestureCondition(this, new NoGesture()),
                new ButtonCondition(this, new SpecialButton()))
            ));
        conditions.Add(new GroundedCondition(this, true));
        //conditions.Add(new LogicalOrCondition(this,
        //    new FollowUpCondition(this, typeof(SuperGunStance)),
        //    new FollowUpCondition(this, typeof(SuperGunStanceAttack))));
        conditions.Add(new FollowUpCondition(this, typeof(SuperGunStance)));

        whiffSoundIndex = 2;

        properties.AnimationName = "SuperGunHolster";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

    }
    public override void OnAnimationEnd(FighterMain fighter)
    {
        base.OnAnimationEnd(fighter);
        fighter.SetStocks(6);
    }
    public override HitReport? OnGetHitDuring(FighterMain fighter, GameAttackProperties properties)
    {
        fighter.SetStocks(6);
        return base.OnGetHitDuring(fighter, properties);
    }
}

public class SuperGunShot : SuperGunStanceAttack
{
    public SuperGunShot(GameAttack stance) : base(stance)
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, true));
        //conditions.Add(new LogicalOrCondition(this,
        //    new FollowUpCondition(this, typeof(SuperGunStance)),
        //    new FollowUpCondition(this, typeof(SuperGunStanceAttack))));
        conditions.Add(new FollowUpCondition(this, typeof(SuperGunStance)));
        conditions.Add(new HasStockCondition(this));

        whiffSoundIndex = 5;
        hitSoundIndex = 2;

        properties.AnimationName = "SuperGunShot";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;


        properties.blockProperties.knockback.Set(-7f, 0);
        properties.blockProperties.airKnockback.Set(-9f, 6f);
        properties.blockProperties.selfKnockback.Set(-2f, 0);
        properties.blockProperties.damage = 75;
        properties.blockProperties.hitstopTime = AttackSettings.attackLevel4_blockhitstop;
        properties.blockProperties.stunTime = AttackSettings.attackLevel4_blockstun;

        properties.hitProperties.knockback.Set(-6f, 14f);
        properties.hitProperties.airKnockback.Set(-6f, 15f);
        properties.hitProperties.selfKnockback.Set(-4f, 0);
        properties.hitProperties.damage = 400;
        properties.hitProperties.hitstopTime = AttackSettings.attackLevel4_hithitstop;
        properties.hitProperties.stunTime = AttackSettings.attackLevel4_hitstun;
    }

    public override void OnStartup(FighterMain fighter)
    {
    }

    public override void OnActive(FighterMain fighter)
    {
        // play sound (gunshot) when active
        base.OnStartup(fighter);
        base.OnActive(fighter);

        fighter.SetStocks(fighter.GetStocks() - 1);
    }
}