using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitstun : FighterState
{

    float hitstun;

    public Hitstun(FighterMain fighterMain) : base(fighterMain)
    {
        jumpsEnabled = false;

        hitstun = 0.5f;
    }

    public override void EnterState()
    {
        base.EnterState();

        fighter.canAct = false;
        fighter.canBlock = false;

        UpdateStance();

        switch(fighter.currentStance)
        {
            case FighterStance.Standing:
                fighter.fighterAnimator.StartAnimation("StandHit");
                break;

            case FighterStance.Crouching:
                fighter.fighterAnimator.StartAnimation("CrouchHit");
                break;

            case FighterStance.Air:
                fighter.fighterAnimator.StartAnimation("AirHit");
                break;
        }

        
    }

    public override void DoState()
    {
        base.DoState();

        if (fighter.currentStance == FighterStance.Air)
        {
            if (stateTimer > 0.1f)
            {
                if (fighter.isGrounded)
                {
                    fighter.SwitchState(fighter.knockdown);
                }
            }
        }
        else
        {
            DoFriction(fighter.groundFriction);
        }

        UpdateStance();

        TimeTransitionToNextState(hitstun, NeutralOrAir());
    }
}
