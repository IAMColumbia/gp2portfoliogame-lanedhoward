using CommandInputReaderLibrary;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UIElements;

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

public enum CurrentAttackState
{
    Startup,
    Active,
    Recovery
}

//[RequireComponent(typeof(PlayerInput))]
public class FighterMain : SoundPlayer, IHitboxResponder
{
    [Header("Character Module!!!")]
    public CharacterModule characterModule;
    protected bool chararacterModuleInitialized = false;

    public FighterInputReceiver inputReceiver;
    public FighterAnimator fighterAnimator;
    protected FighterAnimationEvents fighterAnimationEvents;

    public Rigidbody2D fighterRigidbody;
    public Collider2D fighterCollider;

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
    public LandingLag landingLag;

    [Header("Health Values")]
    public float MaxHealth = 1000;
    public float CurrentHealth = 0;

    [Header("Combo Values")]
    public float JuggleMomentumMultiplier = 0.2f;

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
    public float normalGravity;
    public float comboGravity;

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
    public CurrentAttackState currentAttackState = CurrentAttackState.Startup;
    public Directions.FacingDirection facingDirection;
    public bool isStrikeInvulnerable = false;
    public bool isThrowInvulnerable = false;
    public TimeManager timeManager;
    public Combo currentCombo;
    public Transform fireballSpawnpoint;
    public Projectile fireball;

    [Header("Wall Values")]
    public bool isAtTheWall = false;
    public Directions.FacingDirection wallDirection = Directions.FacingDirection.RIGHT;
    public Vector2 wallKeepoutKnockback;
    public float wallKeepoutMaxHeight;

    [Header("Test/Training Values")]
    public bool blockEverything;

    [Header("Sounds")]
    public AudioClip[] whiffSounds;
    public AudioClip[] hitSounds;
    public AudioClip[] blockSounds;
    public AudioClip throwTechSound;

    [Header("Particles")]
    public ParticleSystem smallHitParticles;

    public event EventHandler GotHit;
    public event EventHandler LeftHitstun;

    public override void Awake()
    {
        base.Awake();

        timeManager = FindObjectOfType<TimeManager>();
        
        //PlayerInput pi = GetComponent<PlayerInput>();

        

        fighterRigidbody = GetComponent<Rigidbody2D>();
        fighterCollider = GetComponent<Collider2D>();

        if (otherFighter != null)
        {
            otherFighterMain = otherFighter.GetComponent<FighterMain>();
        }

        fighterAnimationEvents = GetComponentInChildren<FighterAnimationEvents>();
        fighterAnimationEvents.FighterAnimationHaltVerticalVelocity += OnHaltVerticalVelocity;
        fighterAnimationEvents.FighterAnimationHaltHorizontalVelocity += OnHaltHorizontalVelocity;
        fighterAnimationEvents.FighterAnimationVelocityImpulse += OnVelocityImpulseRelativeToSelf;
        fighterAnimationEvents.FighterAttackActiveStarted += OnAttackActive;
        fighterAnimationEvents.FighterAttackRecoveryStarted += OnAttackRecovery;

        groundMask = LayerMask.GetMask("Ground");

        fighterAnimator = new FighterAnimator(this);

        currentAttack = null;
        
        currentCombo = new Combo();

        InitializeFacingDirection();
        
        
        
        
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
        landingLag = new LandingLag(this);

        
        
        SwitchState(neutral);
    }

    public void Start()
    {

        if (!chararacterModuleInitialized)
        {
            InitializeCharacterModule();
        }


        inputReceiver.UpdateFacingDirection();

        CurrentHealth = MaxHealth;
        fighterAnimator.velocityToStopMovingAnim = velocityToStopMoveAnimation;

        dashing = new Dashing(this);
        backdashing = new Backdashing(this);
        neutraldashing = new Neutraldashing(this);

        

        AutoTurnaround();
    }

    private void OnDestroy()
    {
        if (fireball != null)
        {
            Destroy(fireball.gameObject);
        }
    }

    public void InitializeCharacterModule()
    {
        MaxHealth = characterModule.MaxHealth;
        JuggleMomentumMultiplier = characterModule.JuggleMomentumMultiplier;

        softKnockdownTime = characterModule.softKnockdownTime;
        hardKnockdownTime = characterModule.hardKnockdownTime;

        walkAccel = characterModule.walkAccel;
        walkMaxSpeed = characterModule.walkMaxSpeed;
        groundFriction = characterModule.groundFriction;
        velocityToStopMoveAnimation = characterModule.velocityToStopMoveAnimation;

        forwardDashTime = characterModule.forwardDashTime;
        forwardDashActionableDelay = characterModule.forwardDashActionableDelay;
        forwardDashVelocity = characterModule.forwardDashVelocity;

        backDashTime = characterModule.backDashTime;
        backDashActionableDelay = characterModule.backDashActionableDelay;
        backDashStrikeInvulnTime = characterModule.backDashStrikeInvulnTime;
        backDashVelocity = characterModule.backDashVelocity;

        neutralDashTime = characterModule.neutralDashTime;
        neutralDashActionableDelay = characterModule.neutralDashActionableDelay;
        neutralDashVelocity = characterModule.neutralDashVelocity;

        dashJumpCancelWindow = characterModule.dashJumpCancelWindow;
        dashMomentumMultiplier = characterModule.dashMomentumMultiplier;

        jumpVelocityHorizontal = characterModule.jumpVelocityHorizontal;
        jumpVelocityVertical = characterModule.jumpVelocityVertical;
        jumpMomentumMultiplier = characterModule.jumpMomentumMultiplier;

        fighterAttacks = characterModule.GetGameAttacks();
        inputReceiver.SetPossibleGestures(characterModule.GetPossibleGestures());

        if (characterModule.fireballPrefab != null)
        {
            GameObject fb = Instantiate(characterModule.fireballPrefab, fireballSpawnpoint);
            fireball = fb.GetComponent<Projectile>();
            fireball.fighterParent = this;
            fireball.gameObject.SetActive(false);
        }

        chararacterModuleInitialized = true;
    }

    public void InitializePlayerInput(PlayerInput pi)
    {
        var inputHost = new FighterInputHost(pi);
        var inputReader = new InputReader(inputHost);
        inputReceiver = new FighterInputReceiver(this, inputHost, inputReader);
    }

    public void SetMaterial(Material mat)
    {
        foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.material = mat;
        }

        if (fireball != null)
        {
            fireball.SetMaterial(mat);
        }
        
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

        PushAwayFromWall();

        SetGravity();
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

            if (foundAttack != null)
            {
                inputReceiver.bufferedInput = null;
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
        currentAttackState = CurrentAttackState.Startup;
        SwitchState(attacking);
        currentAttack.OnStartup(this);
    }

    public void CheckForGroundedness()
    {
        isGrounded = Physics2D.OverlapArea(new Vector2(fighterCollider.bounds.min.x + groundCheckXDistance, fighterCollider.bounds.min.y), new Vector2(fighterCollider.bounds.max.x - groundCheckXDistance, fighterCollider.bounds.min.y - groundCheckYDistance), groundMask);
        
    }

    public void SwitchState(FighterState newState)
    {
        if (currentState != null && newState.GetType() != currentState.GetType())
        {
            currentState.ExitState();
        }

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
            currentAttackState = CurrentAttackState.Active;
            currentAttack.OnActive(this);
        }
    }

    protected void OnAttackRecovery()
    {
        if (currentAttack != null)
        {
            currentAttackState = CurrentAttackState.Recovery;
            currentAttack.OnRecovery(this);
        }
    }

    public void OnVelocityImpulseRelativeToSelf(Vector2 v)
    {
        if (facingDirection == Directions.FacingDirection.LEFT)
        {
            v.x = -v.x;
        }
        fighterRigidbody.velocity = new Vector2(fighterRigidbody.velocity.x + v.x, fighterRigidbody.velocity.y + v.y);
    }

    public void OnVelocityImpulse(Vector2 v, float momentumMultiplier)
    {
        if (facingDirection == Directions.FacingDirection.LEFT)
        {
            v.x = -v.x;
        }
        fighterRigidbody.velocity = new Vector2((fighterRigidbody.velocity.x * momentumMultiplier) + (v.x), v.y);
    }

    public void OnVelocityImpulseRelativeToOtherFighter(Vector2 v)
    {
        if (ShouldFaceDirection() == Directions.FacingDirection.LEFT)
        {
            v.x = -v.x;
        }
        fighterRigidbody.velocity = new Vector2(fighterRigidbody.velocity.x + v.x, fighterRigidbody.velocity.y + v.y);
    }

    public void OnVelocityImpulseRelativeToOtherFighter(Vector2 v, float momentumMultiplier)
    {
        if (ShouldFaceDirection() == Directions.FacingDirection.LEFT)
        {
            v.x = -v.x;
        }
        fighterRigidbody.velocity = new Vector2((fighterRigidbody.velocity.x * momentumMultiplier) + (v.x), v.y);
    }

    public void OnVelocityImpulseJuggle(Vector2 v, float momentumMultiplier)
    {
        if (ShouldFaceDirection() == Directions.FacingDirection.LEFT)
        {
            v.x = -v.x;
        }
        fighterRigidbody.velocity = new Vector2((fighterRigidbody.velocity.x * momentumMultiplier) + (v.x), (fighterRigidbody.velocity.y * momentumMultiplier) + v.y);
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

    bool IHitboxResponder.CollidedWith(Collider2D collider, Vector3 hitPosition)
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

                    if (hitPosition != null)
                    {
                        PlayHitVFX(hitPosition);
                    }

                }
                else
                {
                    //blocked 
                    currentAttack.OnBlock(this, hurtbox.fighterParent);
                    pp = currentAttack.properties.blockProperties;

                    if (hitPosition != null)
                    {
                        PlayBlockVFX(hitPosition);
                    }
                }


                Vector2 kb = pp.selfKnockback;
                
                if (hurtbox.fighterParent.isAtTheWall)
                {
                    kb.x += pp.knockback.x / 3;
                }

                OnVelocityImpulseRelativeToOtherFighter(kb);
            }

            return successfulHit;
        }
        return false;
    }

    public HitReport GetHitWith(GameAttackProperties properties)
    {
        if (properties.blockType == GameAttackProperties.BlockType.Throw) return GetThrownWith(properties);
        
        if (isStrikeInvulnerable) return HitReport.Whiff;

        if (isCurrentlyAttacking)
        {
            if (currentAttack != null)
            {
                HitReport? attackReport = currentAttack.OnGetHitDuring(this, properties);
                if (attackReport != null)
                {
                    return (HitReport)attackReport;
                }
            }
        }

        AutoTurnaround();

        // decide if we blocked
        bool blocked = DidWeBlock(properties);

        GameAttackPropertiesProperties pp = blocked ? properties.blockProperties : properties.hitProperties;

        //knockback
        // probably will need to separate this into hit kb and block kb, or apply modifiers or somthing
        Vector2 kb;
        if (isGrounded)
        {
            kb = pp.knockback;
        }
        else
        {
            kb = pp.airKnockback;
        }

        timeManager.DoHitStop(pp.hitstopTime);

        if (blocked)
        {
            OnVelocityImpulseJuggle(kb, JuggleMomentumMultiplier);
            CurrentHealth -= pp.damage;

            PlaySound(blockSounds[0]);
            SwitchState(blockstun);
            ((IStunState)currentState).SetStun(pp.stunTime);
            return HitReport.Block;
        }

        if (!currentCombo.currentlyGettingComboed)
        {
            currentCombo.ResetCombo();
            currentCombo.currentlyGettingComboed = true;
        }
        
        currentCombo.AddHit();

        float hitDamage = Mathf.Ceil(pp.damage * currentCombo.damageScale);
        
        CurrentHealth -= hitDamage;
        currentCombo.totalDamage += hitDamage;

        kb *= currentCombo.knockbackScale;
        
        OnVelocityImpulseJuggle(kb, JuggleMomentumMultiplier * currentCombo.momentumScale);

        GotHit?.Invoke(this, EventArgs.Empty);
        

        SwitchState(hitstun);
        Hitstun hs = (Hitstun)currentState;

        hs.SetStun(pp.stunTime);
        hs.SetHardKD(pp.hardKD);
        hs.SetWallBounce(pp.wallBounce);
        if (pp.wallBounce && isAtTheWall)
        {
            hs.CheckForWallbounce();
        }

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

    public void DoWallBounce()
    {
        //timeManager.DoHitStop(0.1f);
        //PlaySound(hitSounds[0]);
        fighterRigidbody.velocity = new Vector2(-fighterRigidbody.velocity.x, fighterRigidbody.velocity.y);
    }

    public void ExitHitstun()
    {
        currentCombo.currentlyGettingComboed = false;
        LeftHitstun?.Invoke(this, EventArgs.Empty);
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
        // do other fighter first because he might need to unflip a backthrow
        otherFighterMain.TechThrow();
        TechThrow();
    }

    public void TechThrow()
    {
        if (isCurrentlyAttacking && currentAttack is ThrowAttackSuccess currentThrowSuccess)
        {
            currentThrowSuccess.OnThrowTeched(this, otherFighterMain);
        }
        SwitchState(hitstun);
        Hitstun hs = (Hitstun)currentState;
        hs.SetStun(throwTechHitstun);
        OnVelocityImpulseRelativeToOtherFighter(throwTechKnockback);
    }

    public void ThrowFlipPlayers()
    {
        Vector3 originalPos = transform.position;
        transform.position = throwPivot.position;
        otherFighterMain.transform.position = originalPos;
        AutoTurnaround();
        otherFighterMain.AutoTurnaround();
    }

    private void PushAwayFromWall()
    {
        if (isAtTheWall && !isGrounded && otherFighterMain.isAtTheWall && otherFighterMain.isGrounded && wallDirection == otherFighterMain.wallDirection && (transform.position.y - otherFighter.transform.position.y) <= wallKeepoutMaxHeight)
        {
            Vector2 v = wallKeepoutKnockback * Time.deltaTime;
            if (wallDirection != facingDirection)
            {
                v.x = -v.x;
            }
            OnVelocityImpulseRelativeToSelf(v);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WallTrigger"))
        {
            isAtTheWall = true;
            if (collision.gameObject.transform.position.x > transform.position.x)
            {
                wallDirection = Directions.FacingDirection.RIGHT;
            }
            else
            {
                wallDirection = Directions.FacingDirection.LEFT;
            }

            if (currentState is Hitstun h)
            {
                h.CheckForWallbounce();
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("WallTrigger"))
        {
            isAtTheWall = false;
        }
    }

    private void SetGravity()
    {
        if (currentCombo != null)
        {
            if (currentCombo.currentlyGettingComboed)
            {
                fighterRigidbody.gravityScale = comboGravity;
                return;
            }
        }

        fighterRigidbody.gravityScale = normalGravity;
    }

    public void PlayHitVFX(Vector3 hitLocation)
    {
        smallHitParticles.transform.position = hitLocation;
        smallHitParticles.Play();
    }

    public void PlayBlockVFX(Vector3 hitLocation)
    {

    }
}