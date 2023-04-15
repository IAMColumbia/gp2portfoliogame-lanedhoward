using CommandInputReaderLibrary;
using JetBrains.Annotations;
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

public enum HitReport
{
    Whiff,
    Hit,
    Block
}

[RequireComponent(typeof(PlayerInput))]
public class FighterMain : MonoBehaviour, IHitboxResponder
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
    public Hitstun hitstun;
    public Blockstun blockstun;
    public Knockdown knockdown;
    public Getup getup;


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
    public bool canBlock;
    public bool isCurrentlyAttacking;
    public FighterStance currentStance;
    public GameAttack currentAttack;
    public Directions.FacingDirection facingDirection;
    public bool isStrikeInvulnerable = false;
    public bool isThrowInvulnerable = false;
    public TimeManager timeManager;

    [Header("Test/Training Values")]
    public bool blockEverything;

    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();

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
        hitstun = new Hitstun(this);
        blockstun = new Blockstun(this);
        knockdown = new Knockdown(this);
        getup = new Getup(this);

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
            UpdateStance();
            var foundAttack = inputReceiver.ParseAttack(inputReceiver.bufferedInput);
            inputReceiver.bufferedInput = null;

            if (foundAttack != null)
            {
                currentAttack = foundAttack;
                if (currentStance == FighterStance.Standing || currentStance == FighterStance.Crouching)
                {
                    AutoTurnaround();
                }
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


    public void UpdateStance()
    {
        if (!isGrounded)
        {
            currentStance = FighterStance.Air;
            return;
        }
        currentStance = hasCrouchInput ? FighterStance.Crouching : FighterStance.Standing;
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

    bool IHitboxResponder.CollidedWith(Collider2D collider)
    {
        if (currentAttack == null) throw new Exception("Hitbox hit without a current attack");

        
        Hurtbox hurtbox = collider.GetComponent<Hurtbox>();

        if (hurtbox != null)
        {
            if (hurtbox.fighterParent == this) return false;

            HitReport report = hurtbox.fighterParent.GetHitWith(currentAttack.properties);

            //react to the hit report ???????

            bool successfulHit = report != HitReport.Whiff;

            if (successfulHit)
            {
                // allow for cancels on hit or block
                canAct = true;
            }

            return successfulHit;
        }
        return false;
    }

    public HitReport GetHitWith(GameAttackProperties properties)
    {
        if (isStrikeInvulnerable) return HitReport.Whiff;

        AutoTurnaround();

        // decide if we blocked
        bool blocked = DidWeBlock(properties);
        

        //knockback
        // probably will need to separate this into hit kb and block kb, or apply modifiers or somthing
        Vector2 kb = properties.knockback;
        OnVelocityImpulse(kb);

        timeManager.DoHitStop(properties.hitstopTime);
        if (blocked)
        {
            SwitchState(blockstun);
            return HitReport.Block;
        }

        SwitchState(hitstun);
        return HitReport.Hit;
    }

    protected bool DidWeBlock(GameAttackProperties properties)
    {
        if (canBlock)
        {
            if (blockEverything)
            {
                if (currentStance != FighterStance.Air)
                {
                    if (properties.blockType == GameAttackProperties.BlockType.Low)
                    {
                        currentStance = FighterStance.Crouching;
                    }
                    if (properties.blockType == GameAttackProperties.BlockType.High)
                    {
                        currentStance = FighterStance.Standing;
                    }
                }
                return true;
            }
            Directions.Direction dir = inputReceiver.GetDirection();

            if (currentStance == FighterStance.Air)
            {
                if (properties.blockType == GameAttackProperties.BlockType.Throw)
                {
                    // do throw stuff
                }
                else
                {
                    if (dir == Directions.Direction.Back || dir == Directions.Direction.DownBack || dir == Directions.Direction.UpBack)
                    {
                        // air block / chicken block
                        return true;

                    }
                }
                return false;
            }

            switch (properties.blockType)
            {
                // cannot upback to block on ground, to punish trying to jump out of pressure all the time

                case GameAttackProperties.BlockType.Low:
                    if (dir == Directions.Direction.DownBack)
                    {
                        // crouch block
                        return true;

                    }
                    break;
                case GameAttackProperties.BlockType.High:
                    if (dir == Directions.Direction.Back)
                    {
                        // high block
                        return true;

                    }
                    break;
                case GameAttackProperties.BlockType.Mid:
                    if (dir == Directions.Direction.Back || dir == Directions.Direction.DownBack)
                    {
                        // any block
                        return true;

                    }
                    break;
                case GameAttackProperties.BlockType.Throw:
                    // do throw stuff
                    break;
            }
        }
        return false;
    }

    public Directions.FacingDirection ShouldFaceDirection()
    {
        if (this.transform.position.x > otherFighter.transform.position.x)
        {
            // should face left
            return Directions.FacingDirection.LEFT;
        }
        return Directions.FacingDirection.RIGHT;
    }

    public void AutoTurnaround()
    {
        if (otherFighter == null) return;

        Directions.FacingDirection shouldFaceDirection = ShouldFaceDirection();


        if (facingDirection != shouldFaceDirection)
        {
            FaceDirection(shouldFaceDirection);
        }
    }

    
}
