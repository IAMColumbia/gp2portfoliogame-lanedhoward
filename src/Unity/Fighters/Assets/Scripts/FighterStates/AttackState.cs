using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : FighterState, IAnimationEndState
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
            if (stateTimer > 0.1f 
                && ( 
                    (fighter.currentAttackState == CurrentAttackState.Startup && fighter.currentAttack.properties.landCancelStartup)
                    || (fighter.currentAttackState == CurrentAttackState.Active && fighter.currentAttack.properties.landCancelActive)
                    || (fighter.currentAttackState == CurrentAttackState.Recovery && fighter.currentAttack.properties.landCancelRecovery)
                ))
            {
                AttackLanding();
                //AllowLanding();
            }
        }
        else
        {
            if (wasEverAirborne)
            {
                if (stateTimer > 0.1f
                    && (
                        (fighter.currentAttackState == CurrentAttackState.Startup && fighter.currentAttack.properties.landCancelStartup)
                        || (fighter.currentAttackState == CurrentAttackState.Active && fighter.currentAttack.properties.landCancelActive)
                        || (fighter.currentAttackState == CurrentAttackState.Recovery && fighter.currentAttack.properties.landCancelRecovery)
                    ))
                {
                    AttackLanding();
                    //AllowLanding();
                }
            }

            if (allowJumping)
            {
                AllowJumping();
            }

            DoFriction(fighter.groundFriction);
        }

        UpdateStance();

        if (AnimationEndTransitionToNextState(NeutralOrAir()))
        {
            fighter.currentAttack.OnAnimationEnd(fighter);
        }
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

    protected void AttackLanding()
    {
        if (fighter.currentAttack.properties.landingLagTime > 0f)
        {
            if (fighter.isGrounded)
            {
                fighter.SwitchState(fighter.landingLag);
                ((IStunState)fighter.currentState).SetStun(fighter.currentAttack.properties.landingLagTime);
            }
        }
        else
        {
            AllowLanding();
        }
    }

    public void OnForceAnimationEnded()
    {
        fighter.SwitchState(NeutralOrAir());
        fighter.currentAttack.OnAnimationEnd(fighter);
    }
}
