using CommandInputReaderLibrary;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

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
    Block,
    Parried
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

    public Vector2 centerOffset;

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

    [Header("Super Values")]
    public float MaxMeter = 400;
    public float CurrentMeter
    {
        get => m_currentMeter;
        set
        {
            m_currentMeter = MathF.Min(MaxMeter, value);
        }
        
    }
    private float m_currentMeter;

    public float MeterPerDamage = 0.1f;

    [Header("Combo Values")]
    public float JuggleMomentumMultiplier = 0.2f;
    public float bounceHitstop = 0f / 60f;

    [Header("KD Values")]
    public float softKnockdownTime = 0.25f;
    public float hardKnockdownTime = 1.25f;

    [Header("Throw Tech Values")]
    public float throwTechWindow = 10f / 60f;
    public float throwTechHitstun = 0.3f;
    public Vector2 throwTechKnockback;
    public float throwTechHitstop = 15f / 60f;

    [Header("Parry Values")]
    public float parryHitstop = 15f / 60f;
    public float parryMeterGain = 50f;

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
    /// <summary>
    /// Hack! set just before the state transition to GetGrabbed, so the current state knows they are getting grabbed
    /// used to keep a combo going instead of "exiting hitstun" on comboable grabs
    /// </summary>
    public bool isGettingGrabbed;
    public bool isDead;

    [Header("Wall Values")]
    public bool isAtTheWall = false;
    public Directions.FacingDirection wallDirection = Directions.FacingDirection.RIGHT;
    public Vector2 wallKeepoutKnockback;
    public float wallKeepoutMaxHeight;
    /// <summary>
    /// max distance from walls to still be counted as isAtTheWall;
    /// </summary>
    public float maxDistanceFromWall;
    /// <summary>
    /// Array of walls. set by gamemanager
    /// </summary>
    public GameObject[] Walls;

    [Header("Stocks")]
    public bool displayStocks = false;
    private int currentStocks = 0;
    

    [Header("Test/Training Values")]
    public bool blockEverything;

    [Header("Sounds")]
    public AudioClip[] whiffSounds;
    public AudioClip[] hitSounds;
    public AudioClip[] blockSounds;
    public AudioClip throwTechSound;
    public AudioClip parrySound;
    public AudioClip bounceSound;
    public AudioClip griddyEnhancedSound;

    [Header("Particles")]
    public ParticleSystem smallHitParticles;
    public ParticleSystem midHitParticles;
    public ParticleSystem heavyHitParticles;
    public ParticleSystem blockParticles;
    public ParticleSystem parryParticles;
    public ParticleSystem wavedashParticles;
    public ParticleSystem grabHitParticles;
    public ParticleSystem throwTechParticles;
    public ParticleSystem griddyParticles;
    public ParticleSystem deathHitParticles;

    public event EventHandler GotHit;
    public event EventHandler LeftHitstun;
    public event EventHandler LeftBlockstun;
    public event EventHandler<int> StocksUpdated;
    public event Action<int,int> PausePressed;
    public event EventHandler<string> SentNotification;
    public event EventHandler HitConnected;
    public event EventHandler AttackInRecovery;
    public event EventHandler AttackActive;
    public event EventHandler Blocked;
    public event EventHandler Parried;
    public event EventHandler ThrowTeched;



    public static event EventHandler<Vector2> GroundBounced;
    public class WallBounceEventArgs : EventArgs
    {
        public Vector2 position;
        public Directions.FacingDirection wallDirection;
    }
    public static event EventHandler<WallBounceEventArgs> WallBounced;

    public override void Awake()
    {
        base.Awake();

        timeManager = FindObjectOfType<TimeManager>();

        //PlayerInput pi = GetComponent<PlayerInput>();

        isDead = false;

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
        fighterAnimationEvents.FighterForceAnimationEnded += OnForceAnimationEnded;
        fighterAnimationEvents.FighterAttackSuperFlashStarted += OnSuperFlashStarted;
        fighterAnimationEvents.FighterAttackSuperFlashEnded += OnSuperFlashEnded;

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

        displayStocks = characterModule.displayStocks;
        currentStocks = characterModule.startingStocks;

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

        inputHost.PausePressed += InputHost_PausePressed;
    }

    public void InjectInputReceiver(FighterInputReceiver receiver)
    {
        inputReceiver = receiver;
    }

    private void InputHost_PausePressed(object sender, EventArgs e)
    {
        PausePressed?.Invoke(inputReceiver.LeftRight, inputReceiver.UpDown);
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

        if (griddyParticles != null)
        {
            griddyParticles.GetComponent<ParticleSystemRenderer>().material = mat;
        }
        
    }

    void Update()
    {
        CheckForInputs();
    }

    void FixedUpdate()
    {
        CheckForGroundedness();

        CheckForWalledness();

        HandleInputs();

        DoCurrentState();

        PushAwayFromWall();

        AdjustCollisionInAir();

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
            TryExecuteBufferedInput();
            
        }
    }

    public bool TryExecuteBufferedInput()
    {
        UpdateStance();

        var foundAttack = inputReceiver.ParseAttack(inputReceiver.bufferedInput);

        if (foundAttack != null)
        {
            //inputReceiver.bufferedInput = null;
            SetCurrentAttack(foundAttack);
            return true;
        }
        return false;
    }

    public void SetCurrentAttack(GameAttack newAttack)
    {
        if (currentAttack != null)
        {
            currentAttack.OnCancel(this);
        }

        currentAttack = newAttack;
        if (currentStance != FighterStance.Air && inputReceiver.bufferedInput != null)
            //bufferedInput might be null here if the attack was set outside of normal input timing (e.g. as a followup from an attack)
        {
            bool turnedAround = AutoTurnaround();
            
            if (turnedAround)
            {
                // inputReceiver.bufferedInput will have changed due to the turn around.
                // need to re-parse the attack
                var foundAttack = inputReceiver.ParseAttack(inputReceiver.bufferedInput);
                if (foundAttack != null)
                {
                    currentAttack = foundAttack;
                }
            }
        }
        inputReceiver.bufferedInput = null;
        currentAttackState = CurrentAttackState.Startup;
        SwitchState(attacking);
        currentAttack.OnStartup(this);

    }

    public void CheckForGroundedness()
    {
        isGrounded = Physics2D.OverlapArea(new Vector2(fighterCollider.bounds.min.x + groundCheckXDistance, fighterCollider.bounds.min.y), new Vector2(fighterCollider.bounds.max.x - groundCheckXDistance, fighterCollider.bounds.min.y - groundCheckYDistance), groundMask);
        
    }

    public void CheckForWalledness()
    {
        bool shouldBeWalled = false;
        foreach (GameObject wall in Walls)
        {
            if (Mathf.Abs(wall.transform.position.x - transform.position.x) <= maxDistanceFromWall)
            {
                shouldBeWalled = true;
                
                if (isAtTheWall == false)
                {
                    // first frame of being walled

                    if (wall.transform.position.x > transform.position.x)
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
            else
            {
                //isAtTheWall = false;
            }
        }

        isAtTheWall = shouldBeWalled;
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
            AttackActive?.Invoke(this, EventArgs.Empty);
        }
    }

    protected void OnAttackRecovery()
    {
        if (currentAttack != null)
        {
            currentAttackState = CurrentAttackState.Recovery;
            currentAttack.OnRecovery(this);
            AttackInRecovery?.Invoke(this, EventArgs.Empty);
        }
    }
    protected void OnSuperFlashStarted()
    {
        fighterAnimator.animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        timeManager.StartSuperFlash();
    }
    protected void OnSuperFlashEnded()
    {
        fighterAnimator.animator.updateMode = AnimatorUpdateMode.Normal;
        timeManager.EndSuperFlash();
    }

    protected void OnForceAnimationEnded()
    {
        if (currentState is IAnimationEndState state)
        {
            state.OnForceAnimationEnded();
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

    public void OnVelocityImpulseRelativeToSelf(Vector2 v, float momentumMultiplier)
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

            float enemyStartHealth = hurtbox.fighterParent.CurrentHealth;

            HitReport report = hurtbox.fighterParent.GetHitWith(currentAttack.properties);

            //react to the hit report ???????

            if (report == HitReport.Parried)
            {
                // do nothing, stop hitbox from colliding
                return true;
            }

            bool successfulHit = report != HitReport.Whiff;

            if (successfulHit)
            {
                GameAttackPropertiesProperties pp;
                // allow for cancels on hit or block
                if ((currentState is not IStunState))
                {
                    canAct = true;

                }
                if (report == HitReport.Hit)
                {
                    pp = currentAttack.properties.hitProperties;

                    if (hitPosition != null)
                    {
                        PlayHitVFX(hitPosition, currentAttack.properties);
                        if (hurtbox.fighterParent.CurrentHealth <= 0 && enemyStartHealth > 0)
                        {
                            // died from this attack
                            PlayKillHitVFX(hitPosition, currentAttack.properties);
                        }
                    }
                    currentAttack.OnHit(this, hurtbox.fighterParent);

                }
                else
                {
                    //blocked 
                    currentAttack.OnBlock(this, hurtbox.fighterParent);
                    pp = currentAttack.properties.blockProperties;

                    if (hitPosition != null)
                    {
                        PlayBlockVFX(hitPosition, currentAttack.properties);
                    }
                }

                //meter
                var cc = hurtbox.fighterParent.currentCombo;
                float meterScale = cc.currentlyGettingComboed ? cc.damageScale : 1;
                float meterGain = meterScale * pp.damage * MeterPerDamage;
                CurrentMeter += meterGain;

                // self knockback
                Vector2 kb = pp.selfKnockback;
                
                if (hurtbox.fighterParent.isAtTheWall)
                {
                    kb.x += pp.knockback.x / 1.5f;
                }

                OnVelocityImpulseRelativeToOtherFighter(kb);

                HitConnected?.Invoke(this, EventArgs.Empty);
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

            if (currentAttackState == CurrentAttackState.Recovery)
            {
                otherFighterMain.SendNotification("Punish!!");
            }
            else
            {
                otherFighterMain.SendNotification("Counter Hit!!!");
            }
        }

        AutoTurnaround();

        // decide if we blocked
        bool blocked = DidWeBlock(properties);
        
        if (blocked)
        {
            if (properties.blockProperties.damage >= CurrentHealth)
            {
                //die to chip damage
                blocked = false;
            }
        }

        GameAttackPropertiesProperties pp = blocked ? properties.blockProperties : properties.hitProperties;

        //meter
        float meterScale = currentCombo.currentlyGettingComboed ? Mathf.Clamp(currentCombo.damageScale,0.3f,0.7f) : 0.7f;
        float meterGain = meterScale * pp.damage * MeterPerDamage;
        CurrentMeter += meterGain;

        //knockback
        Vector2 kb;
        bool groundBounce;
        if (isGrounded)
        {
            kb = pp.knockback;
            groundBounce = pp.groundBounceOnGroundedHit;
        }
        else
        {
            kb = pp.airKnockback;
            groundBounce = pp.groundBounceOnAirHit;
        }

        timeManager.DoHitStop(pp.hitstopTime);

        if (blocked)
        {
            OnVelocityImpulseJuggle(kb, JuggleMomentumMultiplier);
            CurrentHealth -= pp.damage;


            PlaySound(blockSounds[0]);
            SwitchState(blockstun);
            CalculateFrameAdvantage(pp.stunTime);
            currentCombo.lastBlockType = properties.blockType;


            ((IStunState)currentState).SetStun(pp.stunTime);

            Blocked?.Invoke(this, EventArgs.Empty);


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

        CalculateFrameAdvantage(pp.stunTime);
        currentCombo.lastBlockType = properties.blockType;

        SwitchState(hitstun);
        Hitstun hs = (Hitstun)currentState;
        hs.SetStun(pp.stunTime);
        hs.SetHardKD(pp.hardKD);
        hs.SetWallBounce(pp.wallBounce);
        if (pp.wallBounce && isAtTheWall)
        {
            if (wallDirection != otherFighterMain.ShouldFaceDirection())
            {
                // weird corner overlap crossup wallbounce bug
                // just play the vfx but let the crossup knockback rock
                // instead of doing some double negative stuff that means no knockback
                DoWallBounceFX();
            }
            else
            {
                CheckForWalledness(); // this will check for wallbounce if this is the first frame they are walled
                hs.CheckForWallbounce(); // this will check for wallbounce if theyve been walled. but won't wallbounce if the first one did
            }
        }
        hs.SetGroundBounce(groundBounce);
        if (pp.playGroundBounceParticlesOnGroundedHit && isGrounded)
        {
            DoGroundBounceFX();
        }

        GotHit?.Invoke(this, EventArgs.Empty);

        return HitReport.Hit;
    }

    private HitReport GetThrownWith(GameAttackProperties properties)
    {
        if (isThrowInvulnerable) return HitReport.Whiff;
        
        if (currentState is Hitstun)
        {
            if (properties.canGrabHitstun == false)
            {
                return HitReport.Whiff;
            }
            else // can grab hitstun = true
            {
                if (currentCombo != null)
                {
                    if (currentCombo.hasUsedComboGrab) // have we used a combo grab yet
                    {
                        SendNotification("Combo Grab Escape!!");
                        return HitReport.Parried;
                    }
                    else // now we have, so next time it wont hit
                    {
                        currentCombo.hasUsedComboGrab = true;
                    }
                }
            }
        }

        if (isGrounded && currentState.jumpsEnabled && hasJumpInput) return HitReport.Whiff; 
        if ((properties.stanceToBeGrabbed == FighterStance.Air) != (currentStance == FighterStance.Air)) return HitReport.Whiff;

        if (isCurrentlyAttacking && currentAttack is ThrowAttack currentThrow && currentAttackState != CurrentAttackState.Recovery)
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

    /// <summary>
    /// done by defender, displays frame advantage of attacker
    /// </summary>
    /// <param name="stunTime"></param>
    public void CalculateFrameAdvantage(float stunTime)
    {
        // stunTime should be a float in seconds, in increments of 1/60

        var anim = otherFighterMain.fighterAnimator.GetAnimatorStateInfo();
        float remainingAnimTime = anim.length - (anim.length * anim.normalizedTime);
        if (remainingAnimTime < 0)
        {
            // anim must have looped
            return;
        }

        float advantage = stunTime - remainingAnimTime; // in seconds
        float advantageFrames = Mathf.Round(advantage * 60);
        // probably wil need to round / floor / ceil this?

        //string plus = advantageFrames >= 0 ? "+" : "";

        if (currentCombo != null)
        {
            currentCombo.lastHitFrameAdvantage = advantageFrames;
        }
        //Debug.Log($"Attacker is {plus}{advantageFrames}");
    }

    public void DoWallBounce(Vector2 lastVelocity)
    {
        
        //fighterRigidbody.velocity = new Vector2(-fighterRigidbody.velocity.x, fighterRigidbody.velocity.y);

        // use velocity from the frame before, because current velocity might already be 0 from colliding with wall
        if (Mathf.Abs(lastVelocity.x) > Mathf.Abs(fighterRigidbody.velocity.x))
        {
            fighterRigidbody.velocity = new Vector2(-lastVelocity.x, lastVelocity.y); 
        }
        else
        {
            fighterRigidbody.velocity = new Vector2(-fighterRigidbody.velocity.x, fighterRigidbody.velocity.y);
        }

        DoWallBounceFX();

    }

    public void DoWallBounceFX()
    {
        timeManager.DoHitStop(bounceHitstop);
        PlaySound(bounceSound);
        WallBounced?.Invoke(this, new WallBounceEventArgs() { position = (Vector2)transform.position + centerOffset, wallDirection = this.wallDirection });

    }

    /// <summary>
    /// Hit stop, sound, and particles of ground bouce. actual bounce logic is in Hitstun
    /// </summary>
    public void DoGroundBounceFX()
    {
        timeManager.DoHitStop(bounceHitstop);
        PlaySound(bounceSound);
        GroundBounced?.Invoke(this, transform.position);
    }

    public void ExitHitstun()
    {
        currentCombo.currentlyGettingComboed = false;
        LeftHitstun?.Invoke(this, EventArgs.Empty);
    }

    public void ExitBlockstun()
    {
        LeftBlockstun?.Invoke(this, EventArgs.Empty);
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

    /// <summary>
    /// Returns true if the fighter actually did turn around
    /// </summary>
    /// <returns></returns>
    public bool AutoTurnaround()
    {
        if (otherFighter == null) return false;

        Directions.FacingDirection shouldFaceDirection = ShouldFaceDirection();


        if (facingDirection != shouldFaceDirection)
        {
            FaceDirection(shouldFaceDirection);
            return true;
        }
        return false;
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
        SendNotification("Throw Tech!!");
        throwTechParticles.Play();
        ThrowTeched?.Invoke(this, EventArgs.Empty);
    }

    public void ThrowFlipPlayers()
    {
        Vector3 originalPos = transform.position;
        transform.position = throwPivot.position;
        otherFighterMain.transform.position = originalPos;
        AutoTurnaround();
        otherFighterMain.AutoTurnaround();
    }

    public void DoParry()
    {
        canAct = true;
        timeManager.DoHitStop(parryHitstop);
        PlaySound(parrySound);
        PlayParryVFX();
        SendNotification("Parry!!!");
        CurrentMeter += parryMeterGain;
        Parried?.Invoke(this, EventArgs.Empty);
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

    private bool lastGrounded = false;
    private void AdjustCollisionInAir()
    {
        if (otherFighterMain == null)
        {
            return;
        }

        if (isGrounded != lastGrounded) // only do this block when isGrounded changes, not every update
        {
            //if both fighters are on the ground, don't ignore collsion. if either fighter is in the air, ignore collision
            Physics2D.IgnoreCollision(fighterCollider, otherFighterMain.fighterCollider, !(isGrounded && otherFighterMain.isGrounded));
            lastGrounded = isGrounded;
        }
    }

    public void PlayHitVFX(Vector3 hitLocation, GameAttackProperties attackProperties)
    {
        ParticleSystem particles;

        //hack: build a better way to set particle type for each attack
        switch (attackProperties.attackType)
        {
            case GameAttackProperties.AttackType.Light:
            case GameAttackProperties.AttackType.Medium:
            default:
                particles = smallHitParticles;
                break;
            case GameAttackProperties.AttackType.Heavy:
                particles = midHitParticles;
                break;
            case GameAttackProperties.AttackType.Special:
            case GameAttackProperties.AttackType.Super:
                particles = heavyHitParticles;
                break;
        }

        if (attackProperties.blockType == GameAttackProperties.BlockType.Throw)
        {
            particles = grabHitParticles;
        }

        particles.transform.position = hitLocation;
        particles.Play();
    }

    public void PlayBlockVFX(Vector3 hitLocation, GameAttackProperties attackProperties)
    {
        ParticleSystem particles;

        particles = blockParticles;

        particles.transform.position = hitLocation;
        particles.Play();
    }

    public void PlayKillHitVFX(Vector3 hitLocation, GameAttackProperties attackProperties)
    {
        ParticleSystem particles;

        particles = deathHitParticles;

        particles.transform.position = hitLocation;
        particles.Play();
    }

    public void PlayParryVFX()
    {
        parryParticles.Play();
    }

    public void PlayWavedashVFX()
    {
        wavedashParticles.Play();
    }

    public void PlayGriddyVFX()
    {
        SendNotification("Griddy Enhanced!");
        griddyParticles.Play();
        wavedashParticles.Play();
        PlaySound(griddyEnhancedSound);
    }

    public int GetStocks()
    {
        return currentStocks;
    }

    public void SetStocks(int newStocks)
    {
        currentStocks = newStocks;
        StocksUpdated?.Invoke(this, newStocks);
    }

    public void SendNotification(string notification)
    {
        SentNotification?.Invoke(this, notification);
    }
}