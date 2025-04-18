﻿using CommandInputReaderLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameAttackCondition
{
    protected GameAttack parent;

    public GameAttackCondition(GameAttack _parent)
    {
        parent = _parent;
    }

    public virtual bool CanExecute(GameMoveInput moveInput, FighterMain fighter)
    {
        return false;
    }

    /// <summary>
    /// used only in CPU fighter calculations, not in attack parsing
    /// </summary>
    /// <param name="fighter"></param>
    /// <returns></returns>
    public virtual bool CanExecute(FighterMain fighter)
    {
        return CanExecute(null, fighter);
    }

}

public class StanceCondition : GameAttackCondition
{
    private FighterStance requiredStance;

    public StanceCondition(GameAttack _parent, FighterStance _requiredStance) : base(_parent)
    {
        requiredStance = _requiredStance;
    }

    public override bool CanExecute(GameMoveInput moveInput, FighterMain fighter)
    {
        if (requiredStance.HasFlag(fighter.currentStance))
        {
            return true;
        }
        return false;
    }

}

public class ButtonCondition : GameAttackCondition
{
    private IButton requiredButton;

    public ButtonCondition(GameAttack _parent, IButton _requiredButton) : base(_parent)
    {
        requiredButton = _requiredButton;
    }

    public override bool CanExecute(GameMoveInput moveInput, FighterMain fighter)
    {
        if (requiredButton.GetType() == moveInput.button.GetType())
        {
            return true;
        }
        return false;
    }
}

public class GestureCondition : GameAttackCondition
{
    private IGesture requiredGesture;

    public GestureCondition(GameAttack _parent, IGesture _requiredGesture) : base(_parent)
    {
        requiredGesture = _requiredGesture;
    }

    public override bool CanExecute(GameMoveInput moveInput, FighterMain fighter)
    {
        if (requiredGesture.GetType() == moveInput.gesture.GetType())
        {
            return true;
        }
        return false;
    }
}

public class GroundedCondition : GameAttackCondition
{
    private bool requiredGroundedness;

    public GroundedCondition(GameAttack _parent, bool _requiredGroundedness) : base(_parent)
    {
        requiredGroundedness = _requiredGroundedness;
    }

    public override bool CanExecute(GameMoveInput moveInput, FighterMain fighter)
    {
        return fighter.isGrounded == requiredGroundedness;
    }

}

public class GatlingCondition : GameAttackCondition
{

    public GatlingCondition(GameAttack _parent) : base(_parent)
    {
    }

    public override bool CanExecute(GameMoveInput moveInput, FighterMain fighter)
    {
        if (!fighter.isCurrentlyAttacking) return true;

        if (fighter.currentAttack == null)
        {
            return true;
        }

        if (fighter.currentAttack.properties.cancelIntoAnyAction)
        {
            return true;
        }

        if (fighter.currentAttack.properties.attackType == GameAttackProperties.AttackType.Throw)
        {
            return false;
        }

        if (parent.properties.attackType > fighter.currentAttack.properties.attackType) //gatling into a higher attack level
        { 
            if (fighter.currentAttack.properties.attackType != GameAttackProperties.AttackType.Special 
                && fighter.currentAttack.properties.attackType != GameAttackProperties.AttackType.Super) // specials and supers are end of gatling series
            {
                return true;
            }
        }
        if (parent.properties.attackType == GameAttackProperties.AttackType.Special || parent.properties.attackType == GameAttackProperties.AttackType.Super)
        {
            // dont allow crouch gatlings for special or super
            return false;
        }
        if (parent.properties.attackType == fighter.currentAttack.properties.attackType) //gatling stand -> crouch of same level
        {
            if (fighter.currentAttack.properties.attackStance == FighterStance.Standing && parent.properties.attackStance == FighterStance.Crouching)
            {
                return true;
            }
        }

        return false;
    }
}

public class NoGatlingCondition : GameAttackCondition
{
    public NoGatlingCondition(GameAttack _parent) : base(_parent)
    {
    }

    public override bool CanExecute(GameMoveInput moveInput, FighterMain fighter)
    {
        if (!fighter.isCurrentlyAttacking) return true;

        if (fighter.currentAttack.properties.cancelIntoAnyAction)
        {
            return true;
        }

        return false;
    }
}

public class FollowUpCondition : GameAttackCondition
{
    Type followUpFrom;
    public FollowUpCondition(GameAttack _parent, Type _followUpFrom) : base(_parent)
    {
        followUpFrom = _followUpFrom;
    }

    public override bool CanExecute(GameMoveInput moveInput, FighterMain fighter)
    {
        if (!fighter.isCurrentlyAttacking) return false;

        if (fighter.currentAttack == null)
        {
            return false;
        }


        if (fighter.currentAttack.GetType() == followUpFrom)
        {
            return true;
        }

        if (fighter.currentAttack.GetType().IsSubclassOf(followUpFrom))
        {
            return true;
        }

        return false;
    }
}

public class MeterCostCondition : GameAttackCondition
{
    float MeterCost;
    public MeterCostCondition(GameAttack _parent, float meterCost) : base(_parent)
    {
        MeterCost = meterCost;
    }

    public override bool CanExecute(GameMoveInput moveInput, FighterMain fighter)
    {
        if (fighter.CurrentMeter >= MeterCost) return true;

        return false;
    }
}

public class LogicalAndCondition : GameAttackCondition
{
    GameAttackCondition condition1;
    GameAttackCondition condition2;
    public LogicalAndCondition(GameAttack _parent, GameAttackCondition condition1, GameAttackCondition condition2) : base(_parent)
    {
        this.condition1 = condition1;
        this.condition2 = condition2;
    }

    public override bool CanExecute(GameMoveInput moveInput, FighterMain fighter)
    {
        return (condition1.CanExecute(moveInput, fighter) && condition2.CanExecute(moveInput, fighter));
    }
}

public class LogicalOrCondition : GameAttackCondition
{
    GameAttackCondition condition1;
    GameAttackCondition condition2;
    public LogicalOrCondition(GameAttack _parent, GameAttackCondition condition1, GameAttackCondition condition2) : base(_parent)
    {
        this.condition1 = condition1;
        this.condition2 = condition2;
    }

    public override bool CanExecute(GameMoveInput moveInput, FighterMain fighter)
    {
        return (condition1.CanExecute(moveInput, fighter) || condition2.CanExecute(moveInput, fighter));
    }
}

public class LogicalNotCondition : GameAttackCondition
{
    GameAttackCondition condition;
    public LogicalNotCondition(GameAttack _parent, GameAttackCondition condition) : base(_parent)
    {
        this.condition = condition;
    }

    public override bool CanExecute(GameMoveInput moveInput, FighterMain fighter)
    {
        return (!condition.CanExecute(moveInput, fighter));
    }
}

public class InHitstunCondition : GameAttackCondition
{
    public InHitstunCondition(GameAttack _parent) : base(_parent)
    {
    }

    public override bool CanExecute(GameMoveInput moveInput, FighterMain fighter)
    {
        return (fighter.currentState is Hitstun);
    }
}