using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backdashing : FighterState, IStunState
{

    float backdashTime;
    float invulnTime;

    public Backdashing(FighterMain fighterMain) : base(fighterMain)
    {
        jumpsEnabled = false;

        invulnTime = 0.15f;
        backdashTime = 0.25f;
    }

    public override void EnterState()
    {
        base.EnterState();

        fighter.canAct = false;
        fighter.canBlock = false;

        fighter.isStrikeInvulnerable = true;

        fighter.OnVelocityImpulse(new Vector2(-8, 4));

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

        if (fighter.isStrikeInvulnerable && stateTimer > invulnTime)
        {
            fighter.isStrikeInvulnerable = false;
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

        TimeTransitionToNextState(backdashTime, NeutralOrAir());
    }

    public void SetStun(float stun)
    {
        backdashTime = stun;
    }
}
