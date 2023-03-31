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

    public Rigidbody2D fighterRigidbody;
    public BoxCollider2D fighterCollider;

    public LayerMask groundMask;
    public float groundCheckXDistance;
    public float groundCheckYDistance;


    public FighterState currentState;

    public Neutral neutral;
    public Crouch crouch;
    public Prejump prejump;
    public Air air;
    


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
    public FighterStance currentStance;

    void Start()
    {
        var inputHost = new FighterInputHost(GetComponent<PlayerInput>());
        var inputReader = new InputReader(inputHost);
        inputReader.SetPossibleGestures(CommandInputReaderLibrary.Gestures.DefaultGestures.GetDefaultGestures());
        inputReceiver = new FighterInputReceiver(inputHost, inputReader);

        fighterRigidbody = GetComponent<Rigidbody2D>();
        fighterCollider = GetComponent<BoxCollider2D>();

        groundMask = LayerMask.GetMask("Ground");

        fighterAnimator = new FighterAnimator(this);
        fighterAnimator.velocityToStopMovingAnim = velocityToStopMoveAnimation;

        neutral = new Neutral(this);
        crouch = new Crouch(this);
        prejump = new Prejump(this);
        air = new Air(this);

        //currentState = neutral;
        SwitchState(neutral);
    }


    void FixedUpdate()
    {
        CheckForGroundedness();
        
        CheckForInputs();

        HandleInputs();

        //UpdateTimers();

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
}
