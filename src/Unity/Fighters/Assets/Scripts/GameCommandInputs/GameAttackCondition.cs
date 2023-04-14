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