using CommandInputReaderLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FighterState
{
    /// <summary>
    /// Uses the Subclass Sandbox pattern from Game Programming Patterns
    /// </summary>
    protected FighterMain fighter;

    public bool jumpsEnabled;

    public float stateTimer;

    public FighterState(FighterMain fighterMain)
    {
        fighter = fighterMain;
        stateTimer = 0f;
    }

    public virtual void EnterState()
    {
        stateTimer = 0f;
    }

    public virtual void DoState()
    {
        stateTimer += Time.deltaTime;
    }

    public virtual void ExitState()
    {

    }

    #region MOVEMENT

    public void AllowHorizontalMovement()
    {
        MoveHorizontal(fighter.walkMaxSpeed, fighter.groundFriction);
        fighter.fighterAnimator.AnimationUpdateMoveBool(fighter.fighterRigidbody.velocity.x, fighter.walkMaxSpeed);
    }

    public void MoveHorizontal(float _maxSpeed, float fricAmount)
    {
        float _inputMovement = fighter.inputReceiver.LeftRight;

        float hsp = fighter.fighterRigidbody.velocity.x;

        //if we only move the left stick a little bit, inari should only accelerate to a walking pace
        float maxSpeed = _maxSpeed * Mathf.Abs(_inputMovement);



        if (_inputMovement != 0)
        {
            // we move!

            float actingHorizontalAccel = fighter.walkAccel;

            /*
            if (hsp != 0 && System.Math.Sign(hsp) != System.Math.Sign(_inputMovement))
            {
                //if we are trying to change directions, give extra traction.
                actingHorizontalAccel += fricAmount;
            }
            */

            if (
                (System.Math.Abs(hsp) <= (maxSpeed - (actingHorizontalAccel * System.Math.Abs(_inputMovement) * Time.deltaTime))) || //if you can accelerate
                (System.Math.Sign(_inputMovement) != System.Math.Sign(hsp)) // or if you are trying to change directions
                )
            {
                hsp += actingHorizontalAccel * Mathf.Sign(_inputMovement) * Time.deltaTime;
            }
            else
            {
                // cap speed
                // TODO: maybe soft cap speed? like if ur going over your max speed, just slow down over time until you get back to max speed
                //hsp = maxSpeed * System.Math.Sign(hsp);
                hsp = ApplyHorizontalFriction(fricAmount, hsp, maxSpeed * System.Math.Sign(hsp));
            }
        }
        else
        {
            // we are not moving.
            hsp = ApplyHorizontalFriction(fricAmount, hsp);
        }

        fighter.fighterRigidbody.velocity = new Vector2(hsp, fighter.fighterRigidbody.velocity.y);
    }

    private float ApplyHorizontalFriction(float fricAmount, float hsp)
    {


        return ApplyHorizontalFriction(fricAmount, hsp, 0f);

    }
    private float ApplyHorizontalFriction(float fricAmount, float hsp, float goalhsp)
    {
        if (System.Math.Abs(hsp) >= System.Math.Abs(goalhsp) + (fricAmount * Time.deltaTime))
        {
            hsp -= System.Math.Sign(hsp) * fricAmount * Time.deltaTime;
        }
        else
        {
            hsp = goalhsp;
        }

        return hsp;
    }

    public void DoFriction(float frictionAmount)
    {
        float hsp = ApplyHorizontalFriction(frictionAmount, fighter.fighterRigidbody.velocity.x);
        fighter.fighterRigidbody.velocity = new Vector2(hsp, fighter.fighterRigidbody.velocity.y);
    }

    public void AllowJumping()
    {
        if (fighter.hasJumpInput)
        {
            fighter.SwitchState(fighter.prejump);
        }
    }

    public bool UpdateFallingAnimationBool()
    {
        bool b = CheckIfFalling();
        fighter.fighterAnimator.AnimationUpdateFallingBool(b);
        return b;
    }

    public bool CheckIfFalling()
    {
        return fighter.fighterRigidbody.velocity.y < 0;
    }

    public void UpdateStance()
    {
        if (!fighter.isGrounded)
        {
            fighter.currentStance = FighterStance.Air;
            return;
        }
        fighter.currentStance = fighter.hasCrouchInput ? FighterStance.Crouching : FighterStance.Standing;

    }

    #endregion

    #region TURNING AROUND

    public void AllowAutoTurnaround()
    {
        if (fighter.otherFighter == null) return;

        Directions.FacingDirection shouldFaceDirection = Directions.FacingDirection.RIGHT;
        if (fighter.transform.position.x > fighter.otherFighter.transform.position.x)
        {
            // should face left
            shouldFaceDirection = Directions.FacingDirection.LEFT;
        }
        else
        {
            shouldFaceDirection = Directions.FacingDirection.RIGHT;
        }

        if (fighter.facingDirection != shouldFaceDirection)
        {
            fighter.FaceDirection(shouldFaceDirection);
        }

    }

    #endregion

    #region STATE TRANSITIONS

    protected bool AnimationEndTransitionToNextState(FighterState nextState)
    {
        if (fighter.fighterAnimator.CheckIfAnimationEnded() && stateTimer >= 0.05f)
        {
            fighter.SwitchState(nextState);
            return true;
        }
        return false;
    }

    protected void TimeTransitionToNextState(float stateTimerMax, FighterState nextState)
    {
        if (CheckStateTimer(stateTimerMax))
        {
            fighter.SwitchState(nextState);
        }
    }

    protected bool CheckStateTimer(float stateTimerMax)
    {
        if (stateTimer >= stateTimerMax)
        {
            return true;
        }
        return false;
    }

    protected void AllowLanding()
    {
        if (fighter.isGrounded)
        {
            fighter.SwitchState(fighter.neutral);
        }
    }

    protected FighterState NeutralOrAir()
    {
        return (fighter.currentStance == FighterStance.Air) ? fighter.air : fighter.neutral;
    }

    #endregion
}
