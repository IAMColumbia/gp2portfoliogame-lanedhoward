using CommandInputReaderLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameAttackCondition
{
    protected GameAttack parent;
    protected FighterMain fighter;

    public GameAttackCondition(GameAttack _parent, FighterMain _fighter)
    {
        parent = _parent;
        fighter = _fighter;
    }

    public virtual bool CanExecute(GameMoveInput moveInput)
    {
        return false;
    }

}

public class StanceCondition : GameAttackCondition
{
    private FighterStance requiredStance;

    public StanceCondition(GameAttack _parent, FighterMain _fighter, FighterStance _requiredStance) : base(_parent, _fighter)
    {
        requiredStance = _requiredStance;
    }

    public override bool CanExecute(GameMoveInput moveInput)
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

    public ButtonCondition(GameAttack _parent, FighterMain _fighter, IButton _requiredButton) : base(_parent, _fighter)
    {
        requiredButton = _requiredButton;
    }

    public override bool CanExecute(GameMoveInput moveInput)
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

    public GestureCondition(GameAttack _parent, FighterMain _fighter, IGesture _requiredGesture) : base(_parent, _fighter)
    {
        requiredGesture = _requiredGesture;
    }

    public override bool CanExecute(GameMoveInput moveInput)
    {
        if (requiredGesture.GetType() == moveInput.button.GetType())
        {
            return true;
        }
        return false;
    }
}