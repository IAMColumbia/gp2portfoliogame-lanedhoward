using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Neutraldashing : Dashing
{

    public Neutraldashing(FighterMain fighterMain) : base(fighterMain)
    {
        jumpsEnabled = false;

        invulnTime = 0;
        dashTime = fighterMain.neutralDashTime;
        dashVelocity = fighterMain.neutralDashVelocity;
    }
}
