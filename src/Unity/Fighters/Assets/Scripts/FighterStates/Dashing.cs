using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dashing : FighterState
{

    protected float dashTime;
    protected float invulnTime;
    protected float actionableDelay;
    protected float jumpCancelTime;

    protected Vector2 dashVelocity;

    public Dashing(FighterMain fighterMain) : base(fighterMain)
    {
        jumpsEnabled = false;
        invulnTime = 0f;
        dashTime = fighterMain.forwardDashTime;
        dashVelocity = fighterMain.forwardDashVelocity;
        actionableDelay = fighterMain.forwardDashActionableDelay;
        jumpCancelTime = fighterMain.dashJumpCancelWindow;
    }

    public override void EnterState()
    {
        base.EnterState();

        fighter.canAct = false;
        fighter.canBlock = false;

        fighter.isStrikeInvulnerable = true;

        fighter.OnVelocityImpulseRelativeToSelf(dashVelocity, fighter.dashMomentumMultiplier);

        bool isFalling = UpdateFallingAnimationBool();

        if (isFalling)
        {
            fighter.fighterAnimator.StartAnimation("Air_Falling");
        }
        else
        {
            fighter.fighterAnimator.StartAnimation("Air_Rising");
        }


    }

    public override void DoState()
    {
        base.DoState();

        if (!fighter.canAct && stateTimer > actionableDelay)
        {
            fighter.canAct = true;
        }

        if (fighter.isStrikeInvulnerable && stateTimer > invulnTime)
        {
            fighter.isStrikeInvulnerable = false;
        }

        if (stateTimer < jumpCancelTime)
        {
            if (fighter.hasJumpInput)
            {
                fighter.SwitchState(fighter.air);        
                // jump;
                fighter.fighterRigidbody.velocity = new Vector2((fighter.fighterRigidbody.velocity.x * fighter.dashMomentumMultiplier) + (fighter.jumpVelocityHorizontal * fighter.inputReceiver.LeftRight), fighter.jumpVelocityVertical);
                
            }
        }

        if (fighter.currentStance == FighterStance.Air)
        {
            UpdateFallingAnimationBool();
            if (fighter.isGrounded)
            {
                // just landed, switch animation
                fighter.fighterAnimator.StartAnimation("Idle");
                fighter.fighterAnimator.AnimationUpdateCrouchingBool(false);
            }
        }
        else
        {
            DoFriction(fighter.groundFriction);
        }

        UpdateStance();

        TimeTransitionToNextState(dashTime, NeutralOrAir());
    }

    public override void ExitState()
    {
        base.ExitState();
        fighter.isStrikeInvulnerable = false;
    }
}
