using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class FighterAnimator
{
    public Animator animator;
    private FighterMain fighter;

    [Header("Animator Values")]
    private int animatorMoveBool;
    private int animatorMoveSpeedFloat;
    private int animatorGroundedBool;
    private int animatorFallingBool;
    private int animatorCrouchingBool;

    public float velocityToStopMovingAnim;

    public FighterAnimator(FighterMain main)
    {
        fighter = main;
        animator = fighter.GetComponentInChildren<Animator>();
        animatorMoveBool = Animator.StringToHash("IsMoving");
        animatorMoveSpeedFloat = Animator.StringToHash("WalkSpeed");
        animatorGroundedBool = Animator.StringToHash("IsGrounded");
        animatorFallingBool = Animator.StringToHash("IsFalling");
        animatorCrouchingBool = Animator.StringToHash("IsCrouching");
    }

    public void AnimationUpdateMoveBool(float moveVelocity, float maxVelocity)
    {
        bool isMoving = true;
        float moveSpeed = Mathf.Abs(moveVelocity);

        if (moveSpeed < velocityToStopMovingAnim)
        {
            isMoving = false;
        }

        float animSpeed = moveVelocity / maxVelocity * Mathf.Sign(fighter.transform.lossyScale.x);

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

    public void AnimationUpdateCrouchingBool(bool isCrouching)
    {
        animator.SetBool(animatorCrouchingBool, isCrouching);
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

    public AnimatorStateInfo GetAnimatorStateInfo() => animator.GetCurrentAnimatorStateInfo(0);
}
