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

        fighter.fighterAnimator.StartAnimation("Idle");
    }

    public override void DoState()
    {
        base.DoState();


        AllowHorizontalMovement();
        
        if (fighter.hasCrouchInput)
        {
            fighter.SwitchState(fighter.crouch);
        }

        AllowJumping();
    }
}
