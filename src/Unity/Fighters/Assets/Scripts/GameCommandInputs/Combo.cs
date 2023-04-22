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

    private const float SCALING_PER_HIT = 0.825f;
    private const float MIN_SCALING = 0.3f;
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
    }

    public void AddHit()
    {
        hitCount += 1;

        UpdateDamageScaling();
    }

    private void UpdateDamageScaling()
    {
        if (hitCount > MAX_UNSCALED_HITS)
        {
            float newScale = damageScale * SCALING_PER_HIT;
            damageScale = MathF.Max(newScale, MIN_SCALING);
        }
    }

}