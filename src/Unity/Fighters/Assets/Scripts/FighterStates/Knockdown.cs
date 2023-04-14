using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockdown : FighterState
{

    float knockdownTime;

    public Knockdown(FighterMain fighterMain) : base(fighterMain)
    {
        jumpsEnabled = false;

        knockdownTime = 2f;
    }

    public override void EnterState()
    {
        base.EnterState();

        fighter.canAct = false;
        fighter.canBlock = false;

        fighter.isStrikeInvulnerable = true;
        fighter.isThrowInvulnerable = true;

        fighter.fighterAnimator.StartAnimation("Knockdown");

    }

    public override void DoState()
    {
        base.DoState();

        DoFriction(fighter.groundFriction);

        TimeTransitionToNextState(knockdownTime, fighter.getup);
    }
}
