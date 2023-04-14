using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : FighterState
{
    private bool wasEverAirborne;
    public AttackState(FighterMain fighterMain) : base(fighterMain)
    {
        jumpsEnabled = false;
    }

    public override void EnterState()
    {
        base.EnterState();

        GameAttack attack = fighter.currentAttack;

        fighter.canAct = false;
        fighter.canBlock = false;

        wasEverAirborne = false;

        //fighter.currentStance = FighterStance.Crouching;

        fighter.fighterAnimator.StartAnimation(attack.properties.AnimationName);
    }

    public override void DoState()
    {
        base.DoState();


        if (fighter.currentStance == FighterStance.Air)
        {
            wasEverAirborne = true;
            if (stateTimer > 0.1f)
            {
                AllowLanding();
            }
        }
        else
        {
            if (wasEverAirborne)
            {
                if (stateTimer > 0.1f)
                {
                    AllowLanding();
                }
            }
            DoFriction(fighter.groundFriction);
        }

        UpdateStance();

        AnimationEndTransitionToNextState(NeutralOrAir());
    }
}
