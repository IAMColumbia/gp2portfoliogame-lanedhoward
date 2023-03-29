using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neutral : FighterState
{
    public Neutral(FighterMain fighterMain) : base(fighterMain)
    {
    }

    public override void DoState()
    {
        AllowHorizontalMovement();
        /*
        playerMovement.UpdateGravity();
        AllowFalling();
        AllowGroundToResetJumps();
        */
    }
}
