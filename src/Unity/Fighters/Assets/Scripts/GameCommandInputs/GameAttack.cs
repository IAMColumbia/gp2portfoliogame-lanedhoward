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

    public AudioClip whiffSound;
    public AudioClip hitSound;
    public int whiffSoundIndex;
    public int hitSoundIndex;

    public GameAttack()
    {
        conditions = new List<GameAttackCondition>();
        properties = new GameAttackProperties();
    }

    public virtual bool CanExecute(GameMoveInput moveInput, FighterMain fighter)
    {
        if (conditions.Count == 0) return true;
        // if all conditions pass, this move can go
        return conditions.All(c => c.CanExecute(moveInput, fighter));
    }

    public virtual void OnStartup(FighterMain fighter)
    {
        if (whiffSound != null)
        {
            fighter.PlaySound(whiffSound);
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
        if (hitSound != null)
        {
            fighter.PlaySound(hitSound);
        }
    }

    public virtual void OnBlock(FighterMain fighter, FighterMain otherFighter)
    {

    }

    public virtual void OnExit(FighterMain fighter)
    {

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
        Throw
    }

    public enum BlockType
    {
        Low,
        High,
        Mid,
        Throw
    }

    public string AnimationName;

    public BlockType blockType;
    public AttackType attackType;
    public FighterStance attackStance;

    public GameAttackPropertiesProperties blockProperties;
    public GameAttackPropertiesProperties hitProperties;

    public GameAttackProperties()
    {
        AnimationName = string.Empty;
        blockType = BlockType.Mid;
        attackType = AttackType.Light;
        attackStance = FighterStance.Standing;

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
    public Vector2 selfKnockback;

    public bool hardKD;

    public GameAttackPropertiesProperties()
    {
        hitstopTime = 0f;
        stunTime = 0f;
        damage = 0f;
        knockback = Vector2.zero;
        selfKnockback = Vector2.zero;
        hardKD = false;
    }
}

