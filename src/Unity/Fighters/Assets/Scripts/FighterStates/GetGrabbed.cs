using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGrabbed : FighterState
{
    public GetGrabbed(FighterMain fighterMain) : base(fighterMain)
    {
        jumpsEnabled = false;
    }

    public override void EnterState()
    {
        base.EnterState();

        fighter.canAct = false;
        fighter.canBlock = false;

        UpdateStance();
        fighter.fighterAnimator.StartAnimation("GetGrabbed");

        fighter.OnHaltAllVelocity();

        fighter.fighterRigidbody.simulated = false;
        
    }

    public override void DoState()
    {
        base.DoState();

    }

    public override void ExitState()
    {
        base.ExitState();
        fighter.fighterRigidbody.simulated = true;
    }
}
