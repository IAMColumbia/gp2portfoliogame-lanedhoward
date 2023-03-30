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
        fighter.fighterAnimator.StartAnimation("idle_default");
    }

    public override void DoState()
    {
        AllowHorizontalMovement();
        
        if (fighter.hasCrouchInput)
        {
            fighter.SwitchState(fighter.crouch);
        }
    }
}
