using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Air : FighterState
{
    public Air(FighterMain fighterMain) : base(fighterMain)
    {
        jumpsEnabled = false;
    }

    public override void EnterState()
    {
        base.EnterState();

        fighter.currentStance = FighterStance.Air;

        fighter.canAct = true;

        fighter.fighterAnimator.StartAnimation("Air");
    }

    public override void DoState()
    {
        base.DoState();

        if (stateTimer > 0.1f)
        {
            AllowLanding();
        }

    }
}
