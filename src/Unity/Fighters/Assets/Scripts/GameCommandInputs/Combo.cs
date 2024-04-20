using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// owned by the fighter getting comboed
/// </summary>
public class Combo
{
    public bool currentlyGettingComboed;
    public int hitCount;
    public float totalDamage;

    public float damageScale;
    public float momentumScale;
    public Vector2 knockbackScale;

    public GameAttackProperties.BlockType lastBlockType;
    public float lastHitFrameAdvantage;
    public bool hasUsedComboGrab;

    private const float DAMAGE_SCALING_PER_HIT = 0.825f;
    private const float DAMAGE_MIN_SCALING = 0.3f;
    private const float MOMENTUM_SCALING_PER_HIT = 1.17f;
    private const float MOMENTUM_MAX_SCALING = 1.0f;
    private const float KNOCKBACK_MAX_SCALING = 10f;
    private const float KNOCKBACK_SCALING_PER_HIT = 1.02f;
    private const int MAX_UNSCALED_HITS = 2;

    public Combo()
    {
        ResetCombo();
    }

    public void ResetCombo()
    {
        currentlyGettingComboed = false;
        hitCount = 0;
        totalDamage = 0;
        damageScale = 1;
        momentumScale = 1;
        knockbackScale = Vector2.one;
        hasUsedComboGrab = false;
    }

    public void AddHit()
    {
        hitCount += 1;

        UpdateComboScaling();
    }

    private void UpdateComboScaling()
    {
        if (hitCount > MAX_UNSCALED_HITS)
        {
            damageScale = MathF.Max(damageScale * DAMAGE_SCALING_PER_HIT, DAMAGE_MIN_SCALING);
            //momentumScale = MathF.Min(momentumScale * MOMENTUM_SCALING_PER_HIT, MOMENTUM_MAX_SCALING);
            knockbackScale.x = MathF.Min(knockbackScale.x * KNOCKBACK_SCALING_PER_HIT, KNOCKBACK_MAX_SCALING);
            knockbackScale.y = MathF.Max(knockbackScale.y / KNOCKBACK_SCALING_PER_HIT, 1/KNOCKBACK_MAX_SCALING);
        }
    }

}