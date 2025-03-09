using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blockstun : FighterState, IStunState
{

    float blockstun;

    public Blockstun(FighterMain fighterMain) : base(fighterMain)
    {
        jumpsEnabled = false;

        blockstun = 0.25f;
    }

    public override void EnterState()
    {
        base.EnterState();

        fighter.canAct = false;
        fighter.canBlock = true;

        if (!fighter.blockEverything)
        {
            UpdateStance();
        }

        switch(fighter.currentStance)
        {
            case FighterStance.Standing:
                fighter.fighterAnimator.StartAnimation("StandBlock");
                break;

            case FighterStance.Crouching:
                fighter.fighterAnimator.StartAnimation("CrouchBlock");
                break;

            case FighterStance.Air:
                fighter.fighterAnimator.StartAnimation("AirBlock");
                break;
        }

        
    }

    public override void DoState()
    {
        base.DoState();

        if (fighter.currentStance == FighterStance.Air)
        {
            AllowLanding();
        }
        else
        {
            DoFriction(fighter.groundFriction);
        }

        UpdateStance();

        if (TimeTransitionToNextState(blockstun, NeutralOrAir()))
        {
            CheckForReversal();
        }
    }

    public void SetStun(float stun)
    {
        blockstun = stun;
    }

    public override void ExitState()
    {
        base.ExitState();
        fighter.ExitBlockstun();
    }
}
