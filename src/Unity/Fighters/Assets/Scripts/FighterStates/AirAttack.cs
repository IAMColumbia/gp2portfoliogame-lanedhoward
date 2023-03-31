using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirAttack : FighterState
{
    public AirAttack(FighterMain fighterMain) : base(fighterMain)
    {
        jumpsEnabled = false;
    }

    public override void EnterState()
    {
        base.EnterState();

        fighter.currentStance = FighterStance.Air;

        GameAttack attack = fighter.currentAttack;

        fighter.canAct = false;

        fighter.fighterAnimator.StartAnimation(attack.properties.AnimationName);
    }

    public override void DoState()
    {
        base.DoState();

        if (stateTimer > 0.1f)
        {
            AllowLanding();
        }

        AnimationEndTransitionToNextState(fighter.air);
    }
}
