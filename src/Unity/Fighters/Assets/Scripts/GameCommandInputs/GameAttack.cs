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

    protected FighterMain fighter;

    public List<GameAttackCondition> conditions;

    public GameAttackProperties properties;

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

}

public class GameAttackProperties
{
    public enum AttackType
    {
        Light,
        Medium,
        Heavy,
        Special,
        Super
    }

    public enum BlockType
    {
        Low,
        High,
        Mid,
        Throw
    }

    public string AnimationName;

    public Vector2 knockback;

    public BlockType blockType;
    public AttackType attackType;
    public FighterStance attackStance;

    public GameAttackProperties()
    {
        AnimationName = string.Empty;
        knockback = new Vector2(-2,0);
        blockType = BlockType.Mid;
        attackType = AttackType.Light;
        attackStance = FighterStance.Standing;
    }
}
