using CommandInputReaderLibrary;
using CommandInputReaderLibrary.Gestures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu]
public class CharacterModule : ScriptableObject
{
    [Header("Cosmetics")]
    public string CharacterName;
    public string CharacterDescription;
    public Sprite Portrait;
    public Sprite Headshot;
    public Material[] materials;
    public AudioClip song;
    public AudioClip nameAnnouncement;

    [Header("Health Values")]
    public float MaxHealth = 4000;

    [Header("Combo Values")]
    public float JuggleMomentumMultiplier = 0.2f;

    [Header("KD Values")]
    public float softKnockdownTime = 0.1f;
    public float hardKnockdownTime = 0.5f;

    [Header("Movement Values")]
    public float walkAccel = 100;
    public float walkMaxSpeed = 5;
    public float groundFriction = 50;
    public float velocityToStopMoveAnimation = 1;

    [Header("Dashing Values")]
    public float forwardDashTime = 0.3f;
    public float forwardDashActionableDelay = 0.1f;
    public Vector2 forwardDashVelocity = new Vector2(6, 13);

    public float backDashTime = 0.3f;
    public float backDashActionableDelay = 0.1f;
    public float backDashStrikeInvulnTime = 0f;
    public Vector2 backDashVelocity = new Vector2(-6, 13);
    
    public float neutralDashTime = 0.3f;
    public float neutralDashActionableDelay = 0.1f;
    public Vector2 neutralDashVelocity = new Vector2(0, 13);

    public float dashJumpCancelWindow = 0f;
    public float dashMomentumMultiplier = 0.5f;


    [Header("Jump Values")]
    public float jumpVelocityHorizontal = 5f;
    public float jumpVelocityVertical = 19f;
    [Tooltip("Multiplier applied to current x velocity before the jump.")]
    public float jumpMomentumMultiplier = 0.5f;

    [Header("Fireball")]
    public GameObject fireballPrefab;

    [Header("Stocks")]
    public bool displayStocks;
    public int startingStocks;
    public Sprite stocksIcon;

    [Header("Moves List")]
    [TextArea(3,20)]
    public string MovesList;

    [TextArea(3, 20)]
    public string Bio;

    public virtual List<GameAttack> GetGameAttacks()
    {
        return null;
    }
    public virtual List<IReadableGesture> GetPossibleGestures()
    {
        return null;
    }

    public virtual List<CpuCombo> GetCpuCombos()
    {
        List<CpuCombo> combos = new List<CpuCombo>()
        {
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackA(), true),
                    new CpuMove(new CrouchGesture(), new AttackB(), false)
                }
            },
            // throws
            new CpuCombo()
            {
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackD(), false)
                }
            },
            new CpuCombo()
            {
                moves = new List<CpuMove>()
                {
                    new CpuMove(new BackGesture(), new AttackD(), false)
                }
            },
            // wavedash
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new DownForwardGesture(), new DashMacro(), false)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new DownBackGesture(), new DashMacro(), false)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new DownForwardGesture(), new DashMacro(), false),
                    new CpuMove(new CrouchGesture(), new AttackB(), false)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, true)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new DownBackGesture(), new DashMacro(), false),
                    new CpuMove(new CrouchGesture(), new AttackB(), false)
                }
            },
            // parry
            new CpuCombo()
            {
                moves = new List<CpuMove>()
                {
                    new CpuMove(new CrouchGesture(), new AttackD(), true)
                }
            },
            // air normals
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, false)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackA(), true)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, false)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackB(), true)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, false)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackC(), true)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, false)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackA(), true),
                    new CpuMove(new NoGesture(), new AttackB(), true)
                }
            },
            new CpuCombo()
            {
                conditions = new List<GameAttackCondition>()
                {
                    new GroundedCondition(null, false)
                },
                moves = new List<CpuMove>()
                {
                    new CpuMove(new NoGesture(), new AttackA(), true),
                    new CpuMove(new NoGesture(), new AttackC(), true)
                }
            }

        };
        return combos;
    }
}
