using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Combo
{
    public bool currentlyGettingComboed;
    public int hitCount;
    public float totalDamage;

    public float damageScale;
    public float momentumScale;

    private const float DAMAGE_SCALING_PER_HIT = 0.825f;
    private const float DAMAGE_MIN_SCALING = 0.3f;
    private const float MOMENTUM_SCALING_PER_HIT = 1.17f;
    private const float MOMENTUM_MAX_SCALING = 1.0f;
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
            momentumScale = MathF.Min(momentumScale * MOMENTUM_SCALING_PER_HIT, MOMENTUM_MAX_SCALING);

        }
    }

}