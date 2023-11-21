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
        return conditions.All(c => c.CanExecute(moveInput, fighter));
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

    public bool landCancelStartup;
    public bool landCancelActive;
    public bool landCancelRecovery;

    public float landingLagTime;

    public bool cancelIntoAnyAction;

    public GameAttackPropertiesProperties blockProperties;
    public GameAttackPropertiesProperties hitProperties;

    public GameAttackProperties(GameAttack _parent)
    {
        this.parent = _parent;
        AnimationName = string.Empty;
        blockType = BlockType.Mid;
        attackType = AttackType.Light;
        attackStance = FighterStance.Standing;

        landCancelStartup = true;
        landCancelActive = true;
        landCancelRecovery = true;
        landingLagTime = 0f;
        cancelIntoAnyAction = false;

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
    }
}

