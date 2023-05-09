using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingLag : FighterState, IStunState
{

    float landingLagTime;

    public LandingLag(FighterMain fighterMain) : base(fighterMain)
    {
        jumpsEnabled = false;

        landingLagTime = 0.5f;
    }

    public override void EnterState()
    {
        base.EnterState();

        fighter.canAct = false;
        fighter.canBlock = false;


        fighter.fighterAnimator.StartAnimation("LandingLag");

    }

    public override void DoState()
    {
        base.DoState();

        if (fighter.isGrounded) DoFriction(fighter.groundFriction);

        TimeTransitionToNextState(landingLagTime, fighter.neutral);
    }

    public void SetStun(float stun)
    {
        landingLagTime = stun;
    }
}
