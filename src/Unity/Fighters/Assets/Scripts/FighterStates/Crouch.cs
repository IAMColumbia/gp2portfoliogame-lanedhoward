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
        fighter.fighterAnimator.StartAnimation("crouch_default");
    }

    public override void DoState()
    {
        if (!fighter.hasCrouchInput)
        {
            fighter.SwitchState(fighter.neutral);
        }
    }
}
