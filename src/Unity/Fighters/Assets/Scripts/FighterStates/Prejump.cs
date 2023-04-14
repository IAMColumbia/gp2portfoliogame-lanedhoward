using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prejump : FighterState
{
    private int jumpLeftRight;

    public Prejump(FighterMain fighterMain) : base(fighterMain)
    {
        jumpsEnabled = false;
        jumpLeftRight = 0;
    }

    public override void EnterState()
    {
        base.EnterState();

        fighter.currentStance = FighterStance.Standing;

        jumpLeftRight = fighter.inputReceiver.LeftRight;

        fighter.canAct = false;
        fighter.canBlock = false;

        fighter.fighterAnimator.StartAnimation("Prejump");
    }

    public override void DoState()
    {
        base.DoState();


        DoFriction(fighter.groundFriction);
        
        if (AnimationEndTransitionToNextState(fighter.air))
        {
            // jump;
            fighter.fighterRigidbody.velocity = new Vector2((fighter.fighterRigidbody.velocity.x * fighter.jumpMomentumMultiplier) + (fighter.jumpVelocityHorizontal * jumpLeftRight), fighter.jumpVelocityVertical);
        }
    }

    public override void ExitState()
    {
        base.ExitState();

    }
}
