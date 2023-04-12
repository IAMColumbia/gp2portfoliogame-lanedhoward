using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Getup : FighterState
{


    public Getup(FighterMain fighterMain) : base(fighterMain)
    {
        jumpsEnabled = false;
    }

    public override void EnterState()
    {
        base.EnterState();

        fighter.canAct = false;

        fighter.isStrikeInvulnerable = true;
        fighter.isThrowInvulnerable = true;

        fighter.fighterAnimator.StartAnimation("Getup");

    }

    public override void DoState()
    {
        base.DoState();

        DoFriction(fighter.groundFriction);

        AnimationEndTransitionToNextState(fighter.neutral);
    }

    public override void ExitState()
    {
        base.ExitState();

        fighter.isStrikeInvulnerable = false;
        fighter.isThrowInvulnerable = false;
    }
}
