using CommandInputReaderLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[Flags]
public enum FighterStance
{
    Standing = 1,
    Crouching = 2,
    Air = 4
}

[RequireComponent(typeof(PlayerInput))]
public class FighterMain : MonoBehaviour
{
    public FighterInputReceiver inputReceiver;
    public FighterAnimator fighterAnimator;
    protected FighterAnimationEvents fighterAnimationEvents;

    public Rigidbody2D fighterRigidbody;
    public BoxCollider2D fighterCollider;

    public GameObject otherFighter;

    public LayerMask groundMask;
    public float groundCheckXDistance;
    public float groundCheckYDistance;

    public List<GameAttack> fighterAttacks;

    public FighterState currentState;

    public Neutral neutral;
    public Prejump prejump;
    public Air air;
    public AttackState attacking;


    [Header("Movement Values")]
    public float walkAccel;
    public float walkMaxSpeed;
    public float groundFriction;
    public float velocityToStopMoveAnimation;

    [Header("Jump Values")]
    public float jumpVelocityHorizontal;
    public float jumpVelocityVertical;
    [Tooltip("Multiplier applied to current x velocity before the jump.")]
    public float jumpMomentumMultiplier;

    [Header("Internal Values")]
    public bool hasCrouchInput;
    public bool hasJumpInput;
    public bool isGrounded;
    public bool canAct;
    public FighterStance currentStance;
    public GameAttack currentAttack;
    public Directions.FacingDirection facingDirection;

    void Start()
    {
        var inputHost = new FighterInputHost(GetComponent<PlayerInput>());
        var inputReader = new InputReader(inputHost);
        inputReader.SetPossibleGestures(FighterGestures.GetDefaultGestures());
        inputReceiver = new FighterInputReceiver(this, inputHost, inputReader);

        fighterRigidbody = GetComponent<Rigidbody2D>();
        fighterCollider = GetComponent<BoxCollider2D>();

        fighterAnimationEvents = GetComponentInChildren<FighterAnimationEvents>();
        fighterAnimationEvents.FighterAnimationHaltVerticalVelocity += OnHaltVerticalVelocity;
        fighterAnimationEvents.FighterAnimationVelocityImpulse += OnVelocityImpulse;
        fighterAnimationEvents.FighterAttackActiveStarted += OnAttackActive;
        fighterAnimationEvents.FighterAttackRecoveryStarted += OnAttackRecovery;

        groundMask = LayerMask.GetMask("Ground");

        fighterAnimator = new FighterAnimator(this);
        fighterAnimator.velocityToStopMovingAnim = velocityToStopMoveAnimation;

        neutral = new Neutral(this);
        prejump = new Prejump(this);
        air = new Air(this);
        attacking = new AttackState(this);

        currentAttack = null;
        fighterAttacks = FighterAttacks.GetFighterAttacks();

        InitializeFacingDirection();

        SwitchState(neutral);
    }


    void FixedUpdate()
    {
        CheckForGroundedness();
        
        CheckForInputs();

        HandleInputs();

        DoCurrentState();
    }

    private void CheckForInputs()
    {
        inputReceiver.CheckForInputs();
    }

    private void HandleInputs()
    {
        switch (inputReceiver.UpDown)
        {
            case -1:
                hasCrouchInput = true;
                hasJumpInput = false;
                break;
            case 1:
                hasCrouchInput = false;
                hasJumpInput = true;
                break;
            default:
            case 0:
                hasCrouchInput = false;
                hasJumpInput = false;
                break;
        }

        if (canAct && inputReceiver.bufferedInput != null)
        {
            currentAttack = inputReceiver.ParseAttack(inputReceiver.bufferedInput);
            inputReceiver.bufferedInput = null;

            if (currentAttack != null)
            {
                SwitchState(attacking);
            }
            
        }
    }

    public void CheckForGroundedness()
    {
        isGrounded = Physics2D.OverlapArea(new Vector2(fighterCollider.bounds.min.x + groundCheckXDistance, fighterCollider.bounds.min.y), new Vector2(fighterCollider.bounds.max.x - groundCheckXDistance, fighterCollider.bounds.min.y - groundCheckYDistance), groundMask);
        
    }

    public void SwitchState(FighterState newState)
    {
        currentState?.ExitState();

        currentState = newState;

        currentState.EnterState();
    }

    public void DoCurrentState()
    {
        currentState.DoState();
    }

    protected void InitializeFacingDirection()
    {
        if (this.transform.localScale.x < 0)
        {
            facingDirection = Directions.FacingDirection.LEFT;
            return;
        }
        facingDirection = Directions.FacingDirection.RIGHT;
    }

    protected void TurnAroundVisually()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void FaceDirection(Directions.FacingDirection newDirection)
    {
        if (newDirection == facingDirection) return;

        facingDirection = newDirection;
        inputReceiver.UpdateFacingDirection();

        TurnAroundVisually();
    }


    protected void OnAttackActive()
    {

    }

    protected void OnAttackRecovery()
    {

    }

    protected void OnVelocityImpulse(Vector2 v)
    {
        if (facingDirection == Directions.FacingDirection.LEFT)
        {
            v.x = -v.x;
        }
        fighterRigidbody.velocity = new Vector2(fighterRigidbody.velocity.x + v.x, fighterRigidbody.velocity.y + v.y);
    }

    protected void OnHaltVerticalVelocity()
    {
        fighterRigidbody.velocity = new Vector2(fighterRigidbody.velocity.x, 0);
    }
}
