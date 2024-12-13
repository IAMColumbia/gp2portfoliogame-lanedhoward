using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameAttack
{
    //gonna need conditions like

    // can this be done in the air, ground, or while crouching?

    // is this a light, medium, heavy, dash, or special move?


    // then after its exectuted, itll need proprties like

    // was this a light, medium, heavy, dash, or special move?

    // what cancels can you do from this move? and when?

    // also,

    // is this a strike or a throw?

    // damage, knockback, hitstun, blockstun

    // strike / throw invuln

    // counterhit state (and maybe more things that come with that ?)

    public FighterMain fighter;

    public List<GameAttackCondition> conditions;

    public GameAttackProperties properties;

    public int whiffSoundIndex;
    public int hitSoundIndex;

    public GameAttack()
    {
        conditions = new List<GameAttackCondition>();
        properties = new GameAttackProperties(this);
    }

    public virtual bool CanExecute(GameMoveInput moveInput, FighterMain fighter)
    {
        if (conditions.Count == 0) return true;
        // if all conditions pass, this move can go
        // todo: reverse this for optimization? or don't use linq? if one condition is false, stop executing the rest of the conditions
        //return conditions.All(c => c.CanExecute(moveInput, fighter));
        //return !conditions.Any(c => !c.CanExecute(moveInput, fighter));
        bool canExecute = true;
        foreach (var c in conditions)
        {
            if (c.CanExecute(moveInput, fighter)) continue;
            canExecute = false;
            break;
        }
        return canExecute;
    }

    public virtual void OnStartup(FighterMain fighter)
    {
        if (whiffSoundIndex >= 0)
        {
            fighter.PlaySound(fighter.whiffSounds[whiffSoundIndex]);
        }
    }

    public virtual void OnActive(FighterMain fighter)
    {

    }

    public virtual void OnRecovery(FighterMain fighter)
    {

    }

    public virtual void OnHit(FighterMain fighter, FighterMain otherFighter)
    {
        if (hitSoundIndex >= 0)
        {
            fighter.PlaySound(fighter.hitSounds[hitSoundIndex]);
        }
    }

    public virtual void OnBlock(FighterMain fighter, FighterMain otherFighter)
    {

    }

    public virtual void OnCancel(FighterMain fighter)
    {

    }

    public virtual void OnAnimationEnd(FighterMain fighter)
    {

    }

    /// <summary>
    /// Override and return anything but null to take over getting hit logic.
    /// Useful for armor, guardpoint, parries, etc
    /// </summary>
    /// <param name="fighter"></param>
    /// <param name="properties"></param>
    /// <returns></returns>
    public virtual HitReport? OnGetHitDuring(FighterMain fighter, GameAttackProperties properties)
    {
        return null;
    }

    public virtual void OnSuperFlashStarted(FighterMain fighter)
    {

    }

    public virtual void OnSuperFlashEnded(FighterMain fighter)
    {

    }
}

public class GameAttackStartupVelocity : GameAttack
{
    protected Vector2 velocity;

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);
        fighter.OnVelocityImpulseRelativeToSelf(velocity, 0.5f);
    }
}

public class GameAttackProperties
{
    public enum AttackType
    {
        Light,
        Medium,
        Heavy,
        Special,
        Super,
        Throw,
        Other
    }

    public enum BlockType
    {
        Low,
        High,
        Mid,
        Throw
    }

    public GameAttack parent;

    public string AnimationName;

    public BlockType blockType;
    public AttackType attackType;
    public FighterStance attackStance;

    // grab-specific properties
    public FighterStance stanceToBeGrabbed;
    public bool canGrabHitstun;

    public bool landCancelStartup;
    public bool landCancelActive;
    public bool landCancelRecovery;

    public float landingLagTime;

    public bool cancelIntoAnyAction;

    /// <summary>
    /// lowers the damage scale value to this value after the move hits, if it is not already lower
    /// </summary>
    public float damageScaleComboProration;
    /// <summary>
    /// MINIMUM damage scale that can be applied to this move. applied to the combo before the move does damage
    /// </summary>
    public float minDamageScale;

    /// <summary>
    /// applied on hit
    /// </summary>
    public float maxMeterScaleOnHit;

    public GameAttackPropertiesProperties blockProperties;
    public GameAttackPropertiesProperties hitProperties;

    public GameAttackProperties(GameAttack _parent)
    {
        this.parent = _parent;
        AnimationName = string.Empty;
        blockType = BlockType.Mid;
        attackType = AttackType.Light;
        attackStance = FighterStance.Standing;

        stanceToBeGrabbed = FighterStance.Standing;
        canGrabHitstun = false;

        landCancelStartup = true;
        landCancelActive = true;
        landCancelRecovery = true;
        landingLagTime = 0f;
        cancelIntoAnyAction = false;

        damageScaleComboProration = 1f;
        minDamageScale = 0f;
        maxMeterScaleOnHit = 1f;

        blockProperties = new GameAttackPropertiesProperties();
        hitProperties = new GameAttackPropertiesProperties();
    }
}

public class GameAttackPropertiesProperties
{
    public float hitstopTime;
    public float stunTime;
    public float damage;
    public Vector2 knockback;
    public Vector2 airKnockback;
    public Vector2 selfKnockback;

    public bool hardKD;
    public bool wallBounce;
    public bool groundBounceOnAirHit;
    public bool groundBounceOnGroundedHit;
    public bool playGroundBounceParticlesOnGroundedHit;

    public GameAttackPropertiesProperties()
    {
        hitstopTime = 0f;
        stunTime = 0f;
        damage = 0f;
        knockback = Vector2.zero;
        airKnockback = Vector2.zero;
        selfKnockback = Vector2.zero;
        hardKD = false;
        wallBounce = false;
        groundBounceOnAirHit = false;
        groundBounceOnGroundedHit = false;
        playGroundBounceParticlesOnGroundedHit = false;
    }
}

