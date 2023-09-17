using CommandInputReaderLibrary;
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
            return true;
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

        return false;
    }
}