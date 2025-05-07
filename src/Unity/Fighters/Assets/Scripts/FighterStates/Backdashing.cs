using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Backdashing : Dashing
{

    public Backdashing(FighterMain fighterMain) : base(fighterMain)
    {
        jumpsEnabled = false;

        invulnTime = fighterMain.backDashStrikeInvulnTime;
        dashTime = fighterMain.backDashTime;
        dashVelocity = fighterMain.backDashVelocity;

        actionableDelay = fighterMain.backDashActionableDelay;
    }
}
