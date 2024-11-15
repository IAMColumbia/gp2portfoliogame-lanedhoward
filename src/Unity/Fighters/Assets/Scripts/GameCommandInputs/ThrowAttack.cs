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
    public bool canChicagoPunish;


    public ThrowAttack(ThrowAttackSuccess _success) : base()
    {
        success = _success;

        canTech = true;
        canBeTeched = true;
        canChicagoPunish = true;

        properties.attackType = GameAttackProperties.AttackType.Special;
        properties.blockType = GameAttackProperties.BlockType.Throw;
    }

    public override void OnHit(FighterMain fighter, FighterMain otherFighter)
    {
        base.OnHit(fighter, otherFighter);

        success.canBeTeched = canBeTeched;
        success.canChicagoPunish = canChicagoPunish;
        fighter.SetCurrentAttack(success);


    }
}

public class ThrowAttackSuccess : GameAttack
{
    public bool flipOnTech;
    public bool canBeTeched;
    public bool canChicagoPunish;

    public static EventHandler ThrowStarted;
    public static EventHandler ThrowEnded;

    public ThrowAttackSuccess()
    {
        flipOnTech = false;
        canBeTeched = false;
        canChicagoPunish = false;
    }

    public override void OnStartup(FighterMain fighter)
    {
        base.OnStartup(fighter);

        ThrowStarted?.Invoke(this, EventArgs.Empty);

        fighter.SwitchState(fighter.grabbing);

        bool otherFighterCanTech = fighter.otherFighterMain.canAct 
            && !fighter.otherFighterMain.isCurrentlyAttacking 
            && canBeTeched;

        bool chicagoPunish = false;

        if (canChicagoPunish)
        {
            if (fighter.otherFighterMain.isCurrentlyAttacking && fighter.otherFighterMain.currentAttackState == CurrentAttackState.Recovery)
            {
                chicagoPunish = true;
            }
        }

        fighter.otherFighterMain.isGettingGrabbed = true;
        fighter.otherFighterMain.SwitchState(fighter.otherFighterMain.getGrabbed);

        if (!otherFighterCanTech)
        {
            if (fighter.otherFighterMain.currentState is GetGrabbed gg)
            {
                gg.canTech = false;

                if (canBeTeched)
                {
                    // if the throw was techable but they can't tech now, it is a punish
                    if (chicagoPunish)
                    {
                        fighter.SendNotification("Chicago Punish!!!!");
                    }
                    else
                    {
                        fighter.SendNotification("Punish!!");
                    }
                }
            }
        }
    }
    public override void OnActive(FighterMain fighter)
    {
        // throw success on active is basically base onhit

        base.OnHit(fighter, fighter.otherFighterMain);

        // meter
        var cc = fighter.otherFighterMain.currentCombo;
        float meterScale = cc.currentlyGettingComboed ? cc.damageScale : 1;
        float meterGain = meterScale * properties.hitProperties.damage * fighter.MeterPerDamage;
        fighter.CurrentMeter += meterGain;

        fighter.otherFighterMain.isStrikeInvulnerable = false;
        fighter.otherFighterMain.canBlock = false;

        Grabbing grabbing = (Grabbing)fighter.currentState;
        if (grabbing != null)
        {
            grabbing.grabbing = false;
        }
        float enemyStartHp = fighter.otherFighterMain.CurrentHealth;
        fighter.otherFighterMain.GetHitWith(this.properties);
        
        Vector3 hitlocation = fighter.otherFighter.transform.position + (Vector3)fighter.otherFighterMain.centerOffset;
        fighter.PlayHitVFX(hitlocation, properties);

        if (enemyStartHp > 0 && fighter.otherFighterMain.CurrentHealth <= 0)
        {
            fighter.PlayKillHitVFX(hitlocation, properties);
        }
    }

    public virtual void OnThrowTeched(FighterMain fighter, FighterMain otherFighter)
    {
        if (flipOnTech)
        {
            fighter.ThrowFlipPlayers();
        }
        ThrowEnded?.Invoke(this, EventArgs.Empty);

    }

    public override void OnRecovery(FighterMain fighter)
    {
        base.OnRecovery(fighter);
        ThrowEnded?.Invoke(this, EventArgs.Empty);
    }

    public override void OnAnimationEnd(FighterMain fighter)
    {
        base.OnAnimationEnd(fighter);
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
