using CommandInputReaderLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGrabbed : FighterState
{
    public bool canTech;

    public GetGrabbed(FighterMain fighterMain) : base(fighterMain)
    {
        jumpsEnabled = false;
        canTech = true;
    }

    public override void EnterState()
    {
        base.EnterState();

        fighter.canAct = false;
        fighter.canBlock = false;

        canTech = true;

        UpdateStance();
        fighter.fighterAnimator.StartAnimation("GetGrabbed");

        fighter.OnHaltAllVelocity();

        fighter.fighterRigidbody.simulated = false;
        
    }

    public override void DoState()
    {
        base.DoState();
        if (canTech && stateTimer <= fighter.throwTechWindow)
        {
            if (fighter.inputReceiver.bufferedInput != null)
            {
                UpdateStance();
                var foundAttack = fighter.inputReceiver.ParseAttack(fighter.inputReceiver.bufferedInput);

                if (foundAttack != null)
                {
                    ThrowAttack foundThrow = (ThrowAttack)foundAttack;
                    if (foundThrow != null)
                    {
                        if (foundThrow.canTech)
                        {
                            fighter.inputReceiver.bufferedInput = null;
                            fighter.InitializeThrowTech();
                        }
                    }
                }

            }
        }

    }

    public override void ExitState()
    {
        base.ExitState();
        fighter.fighterRigidbody.simulated = true;
    }
}
