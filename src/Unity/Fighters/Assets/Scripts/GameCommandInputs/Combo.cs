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



    public Combo()
    {
        ResetCombo();
    }

    public void ResetCombo()
    {
        currentlyGettingComboed = false;
        hitCount = 0;
        totalDamage = 0;
    }

}