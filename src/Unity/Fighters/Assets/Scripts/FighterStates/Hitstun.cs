using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static FighterMain;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Hitstun : FighterState, IStunState
{

    float hitstun;
    private bool wasEverAirborne;
    private bool hardKD;
    private bool wallBounce;
    private bool groundBounce;

    private Vector2 lastVelocity;

    public Hitstun(FighterMain fighterMain) : base(fighterMain)
    {
        jumpsEnabled = false;

        lastVelocity = Vector2.zero;

        hitstun = 0.5f;
    }

    public override void EnterState()
    {
        base.EnterState();

        fighter.canAct = false;
        fighter.canBlock = false;

        //fighter.isThrowInvulnerable = true;

        wasEverAirborne = false;
        hardKD = false;
        wallBounce = false;
        groundBounce = false;

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
            if (!fighter.fighterAnimator.GetAnimatorStateInfo().IsName("AirHit") && !fighter.fighterAnimator.GetAnimatorStateInfo().IsName("AirHitFalling"))
            {
                fighter.fighterAnimator.StartAnimation("AirHit");
            }

            UpdateFallingAnimationBool();

            wasEverAirborne = true;
            if (stateTimer > 0.1f)
            {
                if (fighter.isGrounded)
                {
                    HandleLanding();
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
                        HandleLanding();   
                    }
                }
            }
            DoFriction(fighter.groundFriction);
        }

        UpdateStance();

        if (!wasEverAirborne)
        {
            if (TimeTransitionToNextState(hitstun, NeutralOrAir()))
            {
                if (fighter.isDead)
                {
                    // go to knockdown if you are killed by a hit that leaves you in grounded hitstun
                    fighter.SwitchState(fighter.knockdown);
                    float kdTime = 500000000f;

                    ((IStunState)fighter.currentState).SetStun(kdTime);
                }
            }
        }

        if (fighter.fighterRigidbody.velocity.y != 0)
        {
            lastVelocity = fighter.fighterRigidbody.velocity;
        }

    }

    public override void ExitState()
    {
        base.ExitState();
        //fighter.isThrowInvulnerable = false;
        if (!fighter.isGettingGrabbed)
        {
            fighter.ExitHitstun();
        }
        
    }

    private void HandleLanding()
    {
        if (groundBounce)
        {
            //fighter.fighterRigidbody.velocity = new Vector2(fighter.fighterRigidbody.velocity.x, -fighter.fighterRigidbody.velocity.y);

            //Vector2 groundBounceVelocity = fighter.fighterRigidbody.velocity;
            ////if (Mathf.Abs(groundBounceVelocity.y) <= 1f)
            //{
            //    groundBounceVelocity.y = lastVelocity.y;
            //}

            //groundBounceVelocity.y *= -1;


            // use velocity from the frame before, because current velocity might already be 0 from colliding with ground

            if (lastVelocity.y < fighter.fighterRigidbody.velocity.y)
            {
                fighter.fighterRigidbody.velocity = new Vector2(lastVelocity.x, -lastVelocity.y * MathF.Pow(fighter.currentCombo.knockbackScale.y,2)); //lastVelocity * Vector2.down; //groundBounceVelocity;
            }
            else
            {
                fighter.fighterRigidbody.velocity = new Vector2(fighter.fighterRigidbody.velocity.x, -fighter.fighterRigidbody.velocity.y * MathF.Pow(fighter.currentCombo.knockbackScale.y, 2)); //Vector2.down; //groundBounceVelocity;
            }

            groundBounce = false;

            

            fighter.DoGroundBounceFX();

            //stateTimer = 0f;
        }
        else
        {
            Land();
        }
    }

    private void Land()
    {
        fighter.SwitchState(fighter.knockdown);
        float kdTime = hardKD ? fighter.hardKnockdownTime : fighter.softKnockdownTime;

        if (fighter.isDead)
        {
            kdTime = 500000000f;
        }

        ((IStunState)fighter.currentState).SetStun(kdTime);
    }

    public void SetStun(float stun)
    {
        hitstun = stun;
    }

    public void SetHardKD(bool _hardKD)
    {
        hardKD = _hardKD;
    }

    public void SetWallBounce(bool _wallBounce)
    {
        wallBounce = _wallBounce;
    }

    public void SetGroundBounce(bool _groundBounce)
    {
        groundBounce = _groundBounce;
    }

    public void CheckForWallbounce()
    {
        if (wallBounce)
        {
            wallBounce = false;
            fighter.DoWallBounce();
        }
    }
}
