using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ThrowAttack : GameAttack
{
    public bool canTech;
    public bool canBeTeched;
    public ThrowAttackSuccess success;

    public ThrowAttack(ThrowAttackSuccess _success) : base()
    {
        success = _success;

        canTech = true;
        canBeTeched = true;

        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.blockType = GameAttackProperties.BlockType.Throw;
    }

    public override void OnHit(FighterMain fighter, FighterMain otherFighter)
    {
        base.OnHit(fighter, otherFighter);

        success.canBeTeched = canBeTeched;
        fighter.SetCurrentAttack(success);


    }
}

public class ThrowAttackSuccess : GameAttack
{
    public bool flipOnTech;
    public bool canBeTeched;

    public ThrowAttackSuccess()
    {
        flipOnTech = false;
        canBeTeched = false;
    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);

        fighter.SwitchState(fighter.grabbing);

        bool otherFighterCanTech = fighter.otherFighterMain.canAct 
            && !fighter.otherFighterMain.isCurrentlyAttacking 
            && canBeTeched;

        fighter.otherFighterMain.SwitchState(fighter.otherFighterMain.getGrabbed);

        if (!otherFighterCanTech)
        {
            if (fighter.otherFighterMain.currentState is GetGrabbed gg)
            {
                gg.canTech = false;
            }
        }
    }
    public override void OnActive(FighterMain fighter)
    {
        // throw success on active is basically base onhit

        base.OnHit(fighter, fighter.otherFighterMain);


        fighter.otherFighterMain.isStrikeInvulnerable = false;
        fighter.otherFighterMain.canBlock = false;

        Grabbing grabbing = (Grabbing)fighter.currentState;
        if (grabbing != null)
        {
            grabbing.grabbing = false;
        }

        fighter.otherFighterMain.GetHitWith(this.properties);
    }

    public virtual void OnThrowTeched(FighterMain fighter, FighterMain otherFighter)
    {
        if (flipOnTech)
        {
            fighter.ThrowFlipPlayers();
        }
    }
}

public class BackThrowAttack : ThrowAttack
{
    public BackThrowAttack(ThrowAttackSuccess _success) : base(_success)
    {
        success.flipOnTech = true;
    }

    public override void OnHit(FighterMain fighter, FighterMain otherFighter)
    {
        fighter.ThrowFlipPlayers();

        base.OnHit(fighter, otherFighter);
    }
}
