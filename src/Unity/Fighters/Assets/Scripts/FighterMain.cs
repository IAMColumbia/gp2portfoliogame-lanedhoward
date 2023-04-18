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
public class FighterMain : SoundPlayer, IHitboxResponder
{
    public FighterInputReceiver inputReceiver;
    public FighterAnimator fighterAnimator;
    protected FighterAnimationEvents fighterAnimationEvents;

    public Rigidbody2D fighterRigidbody;
    public BoxCollider2D fighterCollider;

    public GameObject otherFighter;
    public FighterMain otherFighterMain;
    public Transform throwPivot;

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
    public Grabbing grabbing;
    public GetGrabbed getGrabbed;
    public Dashing dashing;
    public Backdashing backdashing;
    public Neutraldashing neutraldashing;

    [Header("Health Values")]
    public float MaxHealth = 1000;
    public float CurrentHealth = 0;

    [Header("KD Values")]
    public float softKnockdownTime = 0.25f;
    public float hardKnockdownTime = 1.25f;

    [Header("Throw Tech Values")]
    public float throwTechWindow = 10f / 60f;
    public float throwTechHitstun = 0.3f;
    public Vector2 throwTechKnockback;
    public float throwTechHitstop = 15f / 60f;

    [Header("Movement Values")]
    public float walkAccel;
    public float walkMaxSpeed;
    public float groundFriction;
    public float velocityToStopMoveAnimation;

    [Header("Dashing Values")]
    public float forwardDashTime = 0.3f;
    public float forwardDashActionableDelay = 0.1f;
    public Vector2 forwardDashVelocity;

    public float backDashTime = 0.3f;
    public float backDashActionableDelay = 0.1f;
    public float backDashStrikeInvulnTime = 0.1f;
    public Vector2 backDashVelocity;
    
    public float neutralDashTime = 0.3f;
    public float neutralDashActionableDelay = 0.1f;
    public Vector2 neutralDashVelocity;

    public float dashJumpCancelWindow = 0.1f;
    public float dashMomentumMultiplier;


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

    [Header("Sounds")]
    public AudioClip[] whiffSounds;
    public AudioClip[] hitSounds;
    public AudioClip[] blockSounds;
    public AudioClip throwTechSound;

    void Awake()
    {
        timeManager = FindObjectOfType<TimeManager>();

        var inputHost = new FighterInputHost(GetComponent<PlayerInput>());
        var inputReader = new InputReader(inputHost);
        inputReader.SetPossibleGestures(FighterGestures.GetDefaultGestures());
        inputReceiver = new FighterInputReceiver(this, inputHost, inputReader);

        fighterRigidbody = GetComponent<Rigidbody2D>();
        fighterCollider = GetComponent<BoxCollider2D>();

        if (otherFighter != null)
        {
            otherFighterMain = otherFighter.GetComponent<FighterMain>();
        }

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
        grabbing = new Grabbing(this);
        getGrabbed = new GetGrabbed(this);
        dashing = new Dashing(this);
        backdashing = new Backdashing(this);
        neutraldashing = new Neutraldashing(this);

        currentAttack = null;
        fighterAttacks = FighterAttacks.GetFighterAttacks();

        CurrentHealth = MaxHealth;

        InitializeFacingDirection();
        inputReceiver.UpdateFacingDirection();

        SwitchState(neutral);
    }

    void Update()
    {
        CheckForInputs();
    }

    void FixedUpdate()
    {
        CheckForGroundedness();

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
                SetCurrentAttack(foundAttack);
            }
            
        }
    }

    public void SetCurrentAttack(GameAttack newAttack)
    {
        if (currentAttack != null)
        {
            currentAttack.OnExit(this);
        }

        currentAttack = newAttack;
        if (currentStance == FighterStance.Standing || currentStance == FighterStance.Crouching)
        {
            AutoTurnaround();
        }
        SwitchState(attacking);
        currentAttack.OnStartup(this);
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

    public void TurnAroundVisually()
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
        if (currentAttack != null)
        {
            currentAttack.OnActive(this);
        }
    }

    protected void OnAttackRecovery()
    {
        if (currentAttack != null)
        {
            currentAttack.OnRecovery(this);
        }
    }

    public void OnVelocityImpulse(Vector2 v)
    {
        if (ShouldFaceDirection() == Directions.FacingDirection.LEFT)
        {
            v.x = -v.x;
        }
        fighterRigidbody.velocity = new Vector2(fighterRigidbody.velocity.x + v.x, fighterRigidbody.velocity.y + v.y);
    }

    public void OnVelocityImpulse(Vector2 v, float momentumMultiplier)
    {
        if (ShouldFaceDirection() == Directions.FacingDirection.LEFT)
        {
            v.x = -v.x;
        }
        fighterRigidbody.velocity = new Vector2((fighterRigidbody.velocity.x * momentumMultiplier) + (v.x), v.y);
    }

    public void OnHaltVerticalVelocity()
    {
        fighterRigidbody.velocity = new Vector2(fighterRigidbody.velocity.x, 0);
    }

    public void OnHaltHorizontalVelocity()
    {
        fighterRigidbody.velocity = new Vector2(0, fighterRigidbody.velocity.y);
    }

    public void OnHaltAllVelocity()
    {
        fighterRigidbody.velocity = Vector2.zero;
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
                GameAttackPropertiesProperties pp;
                // allow for cancels on hit or block
                canAct = true;
                if (report == HitReport.Hit)
                {
                    currentAttack.OnHit(this, hurtbox.fighterParent);
                    pp = currentAttack.properties.hitProperties;
                }
                else
                {
                    //blocked 
                    currentAttack.OnBlock(this, hurtbox.fighterParent);
                    pp = currentAttack.properties.blockProperties;
                }

                Vector2 kb = pp.selfKnockback;
                OnVelocityImpulse(kb);
            }

            return successfulHit;
        }
        return false;
    }

    public HitReport GetHitWith(GameAttackProperties properties)
    {
        if (properties.blockType == GameAttackProperties.BlockType.Throw) return GetThrownWith(properties);
        
        if (isStrikeInvulnerable) return HitReport.Whiff;

        AutoTurnaround();

        // decide if we blocked
        bool blocked = DidWeBlock(properties);

        GameAttackPropertiesProperties pp = blocked ? properties.blockProperties : properties.hitProperties;

        //knockback
        // probably will need to separate this into hit kb and block kb, or apply modifiers or somthing
        Vector2 kb = pp.knockback;
        OnVelocityImpulse(kb);

        CurrentHealth -= pp.damage;

        timeManager.DoHitStop(pp.hitstopTime);

        if (blocked)
        {
            PlaySound(blockSounds[0]);
            SwitchState(blockstun);
            ((IStunState)currentState).SetStun(pp.stunTime);
            return HitReport.Block;
        }

        SwitchState(hitstun);
        Hitstun hs = (Hitstun)currentState;

        hs.SetStun(pp.stunTime);
        hs.SetHardKD(pp.hardKD);

        return HitReport.Hit;
    }

    private HitReport GetThrownWith(GameAttackProperties properties)
    {
        if (isThrowInvulnerable) return HitReport.Whiff;
        if (isGrounded && currentState.jumpsEnabled && hasJumpInput) return HitReport.Whiff; 
        if ((properties.attackStance == FighterStance.Air) != (currentStance == FighterStance.Air)) return HitReport.Whiff;

        if (isCurrentlyAttacking && currentAttack is ThrowAttack currentThrow)
        {
            if (currentThrow.canTech)
            {
                if (properties.parent is ThrowAttack enemyThrow)
                {
                    if (enemyThrow.canBeTeched)
                    {
                        InitializeThrowTech();
                        return HitReport.Whiff;
                    }
                }
            }

        }

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
                if (dir == Directions.Direction.Back || dir == Directions.Direction.DownBack || dir == Directions.Direction.UpBack)
                {
                    // air block / chicken block
                    return true;

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
    /// <summary>
    /// Only the fighter who would otherwise be getting thrown should initialize the throw tech
    /// </summary>
    public void InitializeThrowTech()
    {
        timeManager.DoHitStop(throwTechHitstop);
        PlaySound(throwTechSound);
        TechThrow();
        otherFighterMain.TechThrow();
    }

    public void TechThrow()
    {
        SwitchState(hitstun);
        Hitstun hs = (Hitstun)currentState;
        hs.SetStun(throwTechHitstun);
        OnVelocityImpulse(throwTechKnockback);
    }
}
