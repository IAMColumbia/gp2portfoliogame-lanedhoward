using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neutral : FighterState
{
    public Neutral(FighterMain fighterMain) : base(fighterMain)
    {
        jumpsEnabled = true;
    }

    public override void EnterState()
    {
        base.EnterState();

        fighter.currentStance = FighterStance.Standing;

        fighter.canAct = true;
        fighter.canBlock = true;

        UpdateStance();

        if (!fighter.isGrounded)
        {
            fighter.SwitchState(fighter.air);
            return;
        }


        if (fighter.currentStance == FighterStance.Standing)
        {
            fighter.fighterAnimator.StartAnimation("Idle");
            fighter.fighterAnimator.AnimationUpdateCrouchingBool(false);
        }
        else
        {
            fighter.fighterAnimator.StartAnimation("Crouchidle");
            fighter.fighterAnimator.AnimationUpdateCrouchingBool(true);
        }
        
    }

    public override void DoState()
    {
        base.DoState();

        UpdateStance();

        AllowAutoTurnaround();


        if (fighter.currentStance == FighterStance.Standing)
        {
            AllowHorizontalMovement();
            fighter.fighterAnimator.AnimationUpdateCrouchingBool(false);
        }
        else
        {
            DoFriction(fighter.groundFriction);
            fighter.fighterAnimator.AnimationUpdateCrouchingBool(true);
        }

        AllowJumping();
    }
}
