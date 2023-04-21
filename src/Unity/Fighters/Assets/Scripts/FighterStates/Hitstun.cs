using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitstun : FighterState, IStunState
{

    float hitstun;
    private bool wasEverAirborne;
    private bool hardKD;

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

        fighter.isThrowInvulnerable = true;

        wasEverAirborne = false;
        hardKD = false;

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
            wasEverAirborne = true;
            if (stateTimer > 0.1f)
            {
                if (fighter.isGrounded)
                {
                    fighter.SwitchState(fighter.knockdown);
                    float kdTime = hardKD ? fighter.hardKnockdownTime : fighter.softKnockdownTime;
                    ((IStunState)fighter.currentState).SetStun(kdTime);
                }
            }
        }
        else
        {
            if (wasEverAirborne)
            {
                if (stateTimer > 0.1f)
                {
                    if (fighter.isGrounded)
                    {
                        fighter.SwitchState(fighter.knockdown);
                        float kdTime = hardKD ? fighter.hardKnockdownTime : fighter.softKnockdownTime;
                        ((IStunState)fighter.currentState).SetStun(kdTime);
                    }
                }
            }
            DoFriction(fighter.groundFriction);
        }

        UpdateStance();

        if (!wasEverAirborne)
        {
            TimeTransitionToNextState(hitstun, NeutralOrAir());
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        fighter.isThrowInvulnerable = false;
        fighter.ExitHitstun();
        
    }

    public void SetStun(float stun)
    {
        hitstun = stun;
    }

    public void SetHardKD(bool _hardKD)
    {
        hardKD = _hardKD;
    }
}
