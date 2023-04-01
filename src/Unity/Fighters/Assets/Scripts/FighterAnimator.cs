using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAnimator
{
    private Animator animator;
    private FighterMain fighter;

    [Header("Animator Values")]
    private int animatorMoveBool;
    private int animatorMoveSpeedFloat;
    private int animatorGroundedBool;
    private int animatorFallingBool;

    public float velocityToStopMovingAnim;

    public FighterAnimator(FighterMain main)
    {
        fighter = main;
        animator = fighter.GetComponentInChildren<Animator>();
        animatorMoveBool = Animator.StringToHash("IsMoving");
        animatorMoveSpeedFloat = Animator.StringToHash("WalkSpeed");
        animatorGroundedBool = Animator.StringToHash("IsGrounded");
        animatorFallingBool = Animator.StringToHash("IsFalling");
    }

    public void AnimationUpdateMoveBool(float moveVelocity, float maxVelocity)
    {
        bool isMoving = true;
        float moveSpeed = Mathf.Abs(moveVelocity);

        if ((moveVelocity < velocityToStopMovingAnim && moveVelocity >= -velocityToStopMovingAnim))
        {
            isMoving = false;
        }

        float animSpeed = moveVelocity / maxVelocity;

        animator.SetBool(animatorMoveBool, isMoving);
        animator.SetFloat(animatorMoveSpeedFloat, animSpeed);
    }

    public void AnimationUpdateGroundedBool(bool isGrounded)
    {
        animator.SetBool(animatorGroundedBool, isGrounded);
    }

    public void AnimationUpdateFallingBool(bool isFalling)
    {
        animator.SetBool(animatorFallingBool, isFalling);
    }

    /// <summary>
    /// starts the animation
    /// </summary>
    /// <param name="animationStateName"></param>
    public void StartAnimation(string animationStateName)
    {
        StartAnimation(animationStateName, 0.0f);
    }

    /// <summary>
    /// Starts the animation at a given starting point of the animation.
    /// </summary>
    /// <param name="animationStateName"></param>
    /// <param name="normalizedTime"> Starts the animation part way through. 0.0 is begining, 1.0 is end, 0.5 is 50% through etc.</param>
    public void StartAnimation(string animationStateName, float normalizedTime)
    {
        animator.Play(animationStateName, -1, normalizedTime);
    }


    public bool CheckIfAnimationEnded()
    {
        return (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
    }
}
