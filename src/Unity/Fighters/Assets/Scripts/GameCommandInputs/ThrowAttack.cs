using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ThrowAttack : GameAttack
{
    public ThrowAttackSuccess success;

    public ThrowAttack(ThrowAttackSuccess _success) : base()
    {
        success = _success;

        properties.attackType = GameAttackProperties.AttackType.Throw;
        properties.blockType = GameAttackProperties.BlockType.Throw;
    }

    public override void OnHit(FighterMain fighter, FighterMain otherFighter)
    {
        base.OnHit(fighter, otherFighter);

        fighter.SetCurrentAttack(success);


    }
}

public class ThrowAttackSuccess : GameAttack
{
    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);

        fighter.SwitchState(fighter.grabbing);
        fighter.otherFighterMain.SwitchState(fighter.otherFighterMain.getGrabbed);
    }
    public override void OnActive(FighterMain fighter)
    {
        base.OnActive(fighter);

        fighter.otherFighterMain.isStrikeInvulnerable = false;
        fighter.otherFighterMain.canBlock = false;

        Grabbing grabbing = (Grabbing)fighter.currentState;
        if (grabbing != null)
        {
            grabbing.grabbing = false;
        }

        fighter.otherFighterMain.GetHitWith(this.properties);
    }
}