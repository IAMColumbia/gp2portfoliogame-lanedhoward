using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : FighterState
{
    public Crouch(FighterMain fighterMain) : base(fighterMain)
    {
        jumpsEnabled = true;
    }

    public override void EnterState()
    {
        base.EnterState();


        fighter.fighterAnimator.StartAnimation("crouchidle_default");
    }

    public override void DoState()
    {
        base.DoState();


        DoFriction(fighter.groundFriction);
        if (!fighter.hasCrouchInput)
        {
            fighter.SwitchState(fighter.neutral);
        }

        AllowJumping();
    }
}
