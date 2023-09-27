using CommandInputReaderLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu]
public class CharacterModule : ScriptableObject
{
    [Header("Cosmetics")]
    public string CharacterName;
    public Material[] materials;
    public AudioClip song;

    [Header("Health Values")]
    public float MaxHealth = 1000;

    [Header("Combo Values")]
    public float JuggleMomentumMultiplier = 0.2f;

    [Header("KD Values")]
    public float softKnockdownTime = 0.25f;
    public float hardKnockdownTime = 1.25f;

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

    [Header("Fireball")]
    public GameObject fireballPrefab;

    [Header("Stocks")]
    public bool displayStocks;
    public int startingStocks;
    public Sprite stocksIcon;

    public virtual List<GameAttack> GetGameAttacks()
    {
        throw new System.NotImplementedException();
    }
    public virtual List<IReadableGesture> GetPossibleGestures()
    {
        throw new System.NotImplementedException();
    }
}
