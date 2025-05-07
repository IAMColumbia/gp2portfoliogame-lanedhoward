using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grabbing : FighterState, IAnimationEndState
{
    public bool grabbing;
    public Grabbing(FighterMain fighterMain) : base(fighterMain)
    {
        jumpsEnabled = false;
    }

    public override void EnterState()
    {
        base.EnterState();

        fighter.canAct = false;
        fighter.canBlock = false;


        fighter.isCurrentlyAttacking = true;

        fighter.OnHaltAllVelocity();

        fighter.fighterRigidbody.simulated = false;
        grabbing = true;
    }

    public override void DoState()
    {
        base.DoState();

        if (grabbing)
        {
            fighter.otherFighter.transform.SetPositionAndRotation(fighter.throwPivot.position, fighter.throwPivot.rotation);
        }


        if (AnimationEndTransitionToNextState(NeutralOrAir()))
        {
            fighter.currentAttack.OnAnimationEnd(fighter);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        fighter.fighterRigidbody.simulated = true;
        fighter.isCurrentlyAttacking = false;
    }

    public void OnForceAnimationEnded()
    {
        fighter.SwitchState(NeutralOrAir());
        fighter.currentAttack.OnAnimationEnd(fighter);
    }
}
