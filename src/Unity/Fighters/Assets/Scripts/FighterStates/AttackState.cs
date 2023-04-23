using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : FighterState
{
    private bool wasEverAirborne;

    public bool allowJumping;

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

        allowJumping = false;

        fighter.isCurrentlyAttacking = true;

        //fighter.currentStance = FighterStance.Crouching;

        fighter.fighterAnimator.StartAnimation(attack.properties.AnimationName);
    }

    public override void DoState()
    {
        base.DoState();


        if (fighter.currentStance == FighterStance.Air)
        {
            wasEverAirborne = true;
            if (stateTimer > 0.1f && (fighter.currentAttackState == CurrentAttackState.Recovery || fighter.currentAttack.properties.landCancelStartup))
            {
                AllowLanding();
            }
        }
        else
        {
            if (wasEverAirborne)
            {
                if (stateTimer > 0.1f && (fighter.currentAttackState == CurrentAttackState.Recovery || fighter.currentAttack.properties.landCancelStartup))
                {
                    AllowLanding();
                }
            }

            if (allowJumping)
            {
                AllowJumping();
            }

            DoFriction(fighter.groundFriction);
        }

        UpdateStance();

        AnimationEndTransitionToNextState(NeutralOrAir());
    }

    public override void ExitState()
    {
        base.ExitState();
        fighter.isCurrentlyAttacking = false;
    }

    protected void ResetCurrentAttack()
    {
        fighter.currentAttack = null;
    }
}
