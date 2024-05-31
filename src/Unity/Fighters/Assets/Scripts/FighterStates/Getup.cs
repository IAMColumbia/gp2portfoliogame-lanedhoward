using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Getup : FighterState, IAnimationEndState
{


    public Getup(FighterMain fighterMain) : base(fighterMain)
    {
        jumpsEnabled = false;
    }

    public override void EnterState()
    {
        base.EnterState();

        fighter.canAct = false;
        fighter.canBlock = true;

        fighter.isStrikeInvulnerable = true;
        fighter.isThrowInvulnerable = true;

        fighter.fighterAnimator.StartAnimation("Getup");

    }

    public override void DoState()
    {
        base.DoState();

        DoFriction(fighter.groundFriction);

        if (AnimationEndTransitionToNextState(fighter.neutral))
        {
            CheckForReversal();
        }
    }

    public override void ExitState()
    {
        base.ExitState();

        fighter.isStrikeInvulnerable = false;
        fighter.isThrowInvulnerable = false;
    }

    public void OnForceAnimationEnded()
    {
        if (!CheckForReversal())
        {
            fighter.SwitchState(fighter.neutral);
        }
    }
}
