﻿using CommandInputReaderLibrary.Gestures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public static class MrsHiveAttacks
{
    public static List<GameAttack> GetMrsHiveAttacks()
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
                new Pogo(),
                new Thrust(),
                new Fly(),
                new HiveSummon(),
                new HiveAttack(),
                new Teleport(),
                new Stinger(),
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

public class Pogo : GameAttack
{
    private float baseLandingLagTime = 0.25f;
    private float onHitLandingLagTime = 0f;
    Vector2 airdashVelocity;

    public Pogo() : base()
    {
        conditions.Add(new GestureCondition(this, new DownDownGesture()));
        conditions.Add(new ButtonCondition(this, new AttackB()));
        conditions.Add(new GatlingCondition(this));
        conditions.Add(new GroundedCondition(this, true));

        //whiffSound = fighter.whiffSounds[1];
        //hitSound = fighter.hitSounds[4];
        whiffSoundIndex = 2;
        hitSoundIndex = 3;

        properties.AnimationName = "Pogo";

        properties.blockType = GameAttackProperties.BlockType.Low;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;
        properties.landingLagTime = baseLandingLagTime;

        properties.blockProperties.knockback.Set(-2f, 0);
        properties.blockProperties.airKnockback.Set(-2f, 7f);
        properties.blockProperties.selfKnockback.Set(-1f, 0);
        properties.blockProperties.damage = 50f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-3f, 16f);
        properties.hitProperties.airKnockback.Set(-3f, 15f);
        properties.hitProperties.selfKnockback.Set(-1f, 0);
        properties.hitProperties.damage = 300f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel3_hitstun;
        properties.hitProperties.hardKD = false;

        airdashVelocity = new Vector2(0, 18.5f);
    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        fighter.isStrikeInvulnerable = true;
        properties.cancelIntoAnyAction = false;
        properties.landingLagTime = baseLandingLagTime;
    }

    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);
        fighter.isStrikeInvulnerable = false;
        fighter.OnHaltAllVelocity();
        fighter.OnVelocityImpulseRelativeToSelf(airdashVelocity);
    }

    public override void OnHit(FighterMain fighter, FighterMain otherFighter)
    {
        base.OnHit(fighter, otherFighter);

        properties.landingLagTime = onHitLandingLagTime;
        properties.cancelIntoAnyAction = true;
    }
}

public class Thrust : GameAttack
{
    public Thrust() : base()
    {
        conditions.Add(new GestureCondition(this, new HalfCircleForward()));
        conditions.Add(new ButtonCondition(this, new AttackB()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        // hitSound = fighter.hitSounds[2];
        whiffSoundIndex = 1;
        hitSoundIndex = 2;

        properties.AnimationName = "Thrust";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

        properties.blockProperties.knockback.Set(-3f, 0);
        properties.blockProperties.airKnockback.Set(-3f, 5);
        properties.blockProperties.selfKnockback.Set(-6f, 0);
        properties.blockProperties.damage = 75f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-5f, 10f);
        properties.hitProperties.airKnockback.Set(-4f, 12f);
        properties.hitProperties.selfKnockback.Set(-3f, 0);
        properties.hitProperties.damage = 400f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel3_hitstun;
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

public class Fly : GameAttack
{
    Vector2 airdashDirection;
    float airdashMagnitude = 9f;
    float airdashForceUpVelocity = 10f;
    float airdashUpMultiplier = 0.75f;

    int upDown;
    int leftRight;
    public Fly() : base()
    {
        conditions.Add(new GestureCondition(this, new NoGesture()));
        conditions.Add(new ButtonCondition(this, new DashMacro()));
        conditions.Add(new GroundedCondition(this, false));
        conditions.Add(new GatlingCondition(this));
        conditions.Add(new HasStockCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[3];
        whiffSoundIndex = 0;
        hitSoundIndex = 1;

        properties.AnimationName = "Fly";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Air;

        properties.landCancelStartup = false;

        properties.cancelIntoAnyAction = true;
    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        //fighter.OnHaltAllVelocity();
        //fighter.OnHaltVerticalVelocity();

        SetInputDirections(fighter);
    }

    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);

        fighter.OnHaltAllVelocity();

        if (fighter.inputReceiver.UpDown != 0
            || fighter.inputReceiver.LeftRight != 0)
        {
            SetInputDirections(fighter);
        }
        else
        {
            // use whatever the input was at startup
        }

        float upVel = upDown != -1 ? airdashForceUpVelocity : 0;

        Vector2 vel = new Vector2(leftRight * airdashMagnitude, upDown * airdashMagnitude * airdashUpMultiplier + upVel);

        if (fighter.facingDirection == CommandInputReaderLibrary.Directions.FacingDirection.LEFT)
        {
            vel.x *= -1;
        }

        fighter.OnVelocityImpulseRelativeToSelf(vel);

        fighter.SetStocks(fighter.GetStocks() - 1);
    }

    public override void OnRecovery(FighterMain fighter)
    {
        base.OnRecovery(fighter);
        fighter.canAct = true;
        
    }

    public override void OnCancel(FighterMain fighter)
    {
        base.OnCancel(fighter);
        fighter.AutoTurnaround();
    }

    public override void OnAnimationEnd(FighterMain fighter)
    {
        base.OnAnimationEnd(fighter);
        fighter.AutoTurnaround();
    }

    private void SetInputDirections(FighterMain fighter)
    {
        upDown = fighter.inputReceiver.UpDown;
        leftRight = fighter.inputReceiver.LeftRight;
    }
}

public class HiveSummon : GameAttack
{
    int honeyStocksAmount = 4;

    public HiveSummon() : base()
    {
        conditions.Add(new GestureCondition(this, new QuarterCircleForward()));
        conditions.Add(new ButtonCondition(this, new AttackA()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new GatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[3];
        whiffSoundIndex = 2;
        hitSoundIndex = 3;

        properties.AnimationName = "HiveSummon";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

    }

    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);

        int forwardBack = fighter.inputReceiver.LeftRight;
        if (fighter.facingDirection == CommandInputReaderLibrary.Directions.FacingDirection.LEFT)
        {
            forwardBack *= -1;
        }
        if (fighter.fireball is Beehive hive)
        {
            if (!hive.projectileActive)
            {
                hive.StartProjectile(forwardBack);
            }
            else
            {
                hive.EndProjectile();
                hive.StartProjectile(forwardBack);
            }
            fighter.SetStocks(honeyStocksAmount);
        }
    }
}

public class HiveAttack : GameAttack
{
    GameAttackProperties fireballProperties;
    public HiveAttack() : base()
    {
        conditions.Add(new GestureCondition(this, new QuarterCircleBack()));
        conditions.Add(new ButtonCondition(this, new AttackA()));
        conditions.Add(new GatlingCondition(this));
        conditions.Add(new HasStockCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[3];
        whiffSoundIndex = 2;
        hitSoundIndex = 3;

        properties.AnimationName = "HiveAttack";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

        properties.landCancelStartup = false;
        properties.landCancelActive = false;

        fireballProperties = new GameAttackProperties(this);

        fireballProperties.blockType = GameAttackProperties.BlockType.Mid;
        fireballProperties.attackType = GameAttackProperties.AttackType.Special;
        fireballProperties.attackStance = FighterStance.Standing;

        fireballProperties.blockProperties.knockback.Set(-5f, 0);
        fireballProperties.blockProperties.airKnockback.Set(-3f, 6f);
        fireballProperties.blockProperties.damage = 75f;
        fireballProperties.blockProperties.hitstopTime = FighterAttacks.attackLevel2_blockhitstop;
        fireballProperties.blockProperties.stunTime = FighterAttacks.attackLevel3_blockstun;

        fireballProperties.hitProperties.knockback.Set(-6f, 0f);
        fireballProperties.hitProperties.airKnockback.Set(-2f, 16f);
        fireballProperties.hitProperties.damage = 200f;
        fireballProperties.hitProperties.hitstopTime = FighterAttacks.attackLevel2_hithitstop;
        fireballProperties.hitProperties.stunTime = FighterAttacks.attackLevel3_hitstun;
        fireballProperties.hitProperties.hardKD = false;


    }

    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);

        if (fighter.fireball.projectileActive)
        {
            fighter.fireball.projectileProperties = fireballProperties;
            fighter.fireball.breakOnCollision = true;
            fighter.fireball.hitbox.OpenForCollision = true;
            fighter.fireball.hitbox._state = ColliderState.Open;
            fighter.SetStocks(fighter.GetStocks() - 1);
        }

    }

    public override void OnRecovery(FighterMain fighter)
    {
        base.OnRecovery(fighter);
        if (fighter.fireball.projectileActive)
        {
            fighter.fireball.hitbox.OpenForCollision = false;
            fighter.fireball.hitbox._state = ColliderState.Closed;
            fighter.fireball.EndProjectile();
        }
    }

    public override HitReport? OnGetHitDuring(FighterMain fighter, GameAttackProperties properties)
    {
        if (fighter.fireball.projectileActive)
        {
            fighter.fireball.hitbox.OpenForCollision = false;
            fighter.fireball.hitbox._state = ColliderState.Closed;
            fighter.fireball.EndProjectile();
        }
        return base.OnGetHitDuring(fighter, properties);
    }
}

public class Teleport : GameAttack
{

    public Teleport() : base()
    {
        conditions.Add(new GestureCondition(this, new DownDownGesture()));
        conditions.Add(new ButtonCondition(this, new AttackA()));
        //conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new LogicalOrCondition(this,
            new GatlingCondition(this),
            new FollowUpCondition(this, typeof(Teleport))
            ));
        conditions.Add(new HasStockCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[3];
        whiffSoundIndex = 2;
        hitSoundIndex = 3;

        properties.AnimationName = "Teleport";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Standing;

    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        fighter.OnHaltVerticalVelocity();
    }

    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);


        Vector2 startPos = fighter.transform.position;

        fighter.transform.position = fighter.fireball.transform.position;

        fighter.fireball.transform.position = startPos;

        fighter.AutoTurnaround();

        fighter.SetStocks(fighter.GetStocks() - 1);
    }

    public override void OnRecovery(FighterMain fighter)
    {
        base.OnRecovery(fighter);
        fighter.canAct = true;
    }
}

public class Stinger : GameAttack
{
    Vector2 backVelocity = new Vector2(5f, -4f);
    Vector2 neutralVelocity = new Vector2(8f, -1f);
    Vector2 forwardVelocity = new Vector2(12f, -0f);

    public Stinger() : base()
    {
        conditions.Add(new GestureCondition(this, new CrouchGesture()));
        conditions.Add(new ButtonCondition(this, new AttackC()));
        conditions.Add(new GroundedCondition(this, false));
        conditions.Add(new GatlingCondition(this));
        conditions.Add(new HasStockCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[3];
        whiffSoundIndex = 2;
        hitSoundIndex = 3;

        properties.AnimationName = "Stinger";

        properties.blockType = GameAttackProperties.BlockType.Mid;
        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.attackStance = FighterStance.Air;

        properties.blockProperties.knockback.Set(-3f, 0);
        properties.blockProperties.airKnockback.Set(-3f, -3f);
        properties.blockProperties.selfKnockback.Set(-6f, 0);
        properties.blockProperties.damage = 50f;
        properties.blockProperties.hitstopTime = FighterAttacks.attackLevel3_blockhitstop;
        properties.blockProperties.stunTime = FighterAttacks.attackLevel3_blockstun;

        properties.hitProperties.knockback.Set(-5f, 0);
        properties.hitProperties.airKnockback.Set(-1.75f, -6f);
        properties.hitProperties.selfKnockback.Set(-5f, 0);
        properties.hitProperties.damage = 300f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel3_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel3_hitstun;
        properties.hitProperties.hardKD = true;

        properties.landCancelStartup = false;
        properties.landingLagTime = 0.1f;

    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        fighter.OnHaltAllVelocity();
        fighter.AutoTurnaround();
    }

    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);
        //fighter.OnHaltAllVelocity();

        int forwardBack = fighter.inputReceiver.LeftRight;
        if (fighter.facingDirection == CommandInputReaderLibrary.Directions.FacingDirection.LEFT)
        {
            forwardBack *= -1;
        }

        Vector2 vel;

        switch (forwardBack)
        {
            case -1:
                vel = backVelocity;
                break;
            default:
            case 0:
                vel = neutralVelocity;
                break;
            case 1:
                vel = forwardVelocity;
                break;
        }

        fighter.OnVelocityImpulseRelativeToSelf(vel);

        fighter.SetStocks(fighter.GetStocks() - 1);
    }
}