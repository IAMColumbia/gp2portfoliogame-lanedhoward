using CommandInputReaderLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CpuMove : GameMoveInput
{
    public new IReadableGesture gesture;
    /// <summary>
    /// does this move need to connect (hit or block) in order to continue the combo
    /// use for projectiles, or status (reload)
    /// </summary>
    public bool needsToConnect;

    public CpuMove(IReadableGesture _gesture, IButton _button, bool _needsToConnect) : base(_gesture, _button)
    {
        gesture = _gesture;
        button = _button;
        needsToConnect = _needsToConnect;
    }
}

public class CpuCombo
{
    public virtual bool CanExecute(FighterMain fighter)
    {
        if (conditions == null)
        {
            return true;
        }
        return conditions.All(c => c.CanExecute(fighter));
    }
    public List<CpuMove> moves;
    public List<GameAttackCondition> conditions;
    public int weight = 100;
}

public class FarCondition : GameAttackCondition
{
    public FarCondition(GameAttack _parent) : base(_parent)
    {
    }

    public override bool CanExecute(FighterMain fighter)
    {
        return Mathf.Abs(fighter.otherFighter.transform.position.x - fighter.transform.position.x) >= 3;
    }
}