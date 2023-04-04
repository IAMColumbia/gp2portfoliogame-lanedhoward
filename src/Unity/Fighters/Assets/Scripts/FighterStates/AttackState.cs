using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : FighterState
{
    public AttackState(FighterMain fighterMain) : base(fighterMain)
    {
        jumpsEnabled = false;
    }

    public override void EnterState()
    {
        base.EnterState();

        GameAttack attack = fighter.currentAttack;

        fighter.canAct = false;

        //fighter.currentStance = FighterStance.Crouching;

        fighter.fighterAnimator.StartAnimation(attack.properties.AnimationName);
    }

    public override void DoState()
    {
        base.DoState();


        if (fighter.currentStance == FighterStance.Air)
        {
            if (stateTimer > 0.1f)
            {
                AllowLanding();
            }
        }
        else
        {
            DoFriction(fighter.groundFriction);
        }

        UpdateStance();

        AnimationEndTransitionToNextState((fighter.currentStance == FighterStance.Air) ? fighter.air : fighter.neutral);
    }
}
