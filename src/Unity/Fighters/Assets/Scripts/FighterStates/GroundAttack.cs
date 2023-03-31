using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAttack : FighterState
{
    public GroundAttack(FighterMain fighterMain) : base(fighterMain)
    {
        jumpsEnabled = false;
    }

    public override void EnterState()
    {
        base.EnterState();

        GameAttack attack = fighter.currentAttack;

        //fighter.currentStance = FighterStance.Crouching;

        fighter.fighterAnimator.StartAnimation(attack.properties.AnimationName);
    }

    public override void DoState()
    {
        base.DoState();


        DoFriction(fighter.groundFriction);


        AnimationEndTransitionToNextState(fighter.neutral);
    }
}
