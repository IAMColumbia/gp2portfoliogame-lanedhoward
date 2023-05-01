using CommandInputReaderLibrary.Gestures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class BlackhandAttacks
{
    public static List<GameAttack> GetBlackhandAttacks()
    {
        List<GameAttack> attacks = new List<GameAttack>()
            {
                new TwoA(),
                new FiveA(),
                new JumpA(),
                new TwoB(),
                new FiveB(),
                new JumpB(),
                new TwoC(),
                new FiveC(),
                new JumpC(),
                new CannonGrabWhiff(new CannonGrabSuccess()),
                new BackThrowWhiff(new GrabSuccess()),
                new GrabWhiff(new GrabSuccess()),
                new AirBackThrowWhiff(new AirGrabSuccess()),
                new AirGrabWhiff(new AirGrabSuccess()),
                new BackDash(),
                new ForwardDash(),
                new NeutralDash()
            };
        return attacks;
    }
}

// blackhand
public class CannonGrabWhiff : ThrowAttack
{
    public CannonGrabWhiff(ThrowAttackSuccess _success) : base(_success)
    {
        conditions.Add(new GestureCondition(this, new DragonPunch()));
        conditions.Add(new ButtonCondition(this, new AttackD()));
        conditions.Add(new GroundedCondition(this, true));
        conditions.Add(new NoGatlingCondition(this));

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[0];
        whiffSoundIndex = 0;
        hitSoundIndex = 1;

        properties.AnimationName = "CannonGrab";

        canBeTeched = false;
        canTech = false;

    }
}

public class CannonGrabSuccess : ThrowAttackSuccess
{
    public CannonGrabSuccess() : base()
    {
        properties.AnimationName = "CannonGrabSuccess";

        //whiffSound = fighter.whiffSounds[0];
        //hitSound = fighter.hitSounds[2];
        whiffSoundIndex = -1;
        hitSoundIndex = 3;

        properties.hitProperties.knockback.Set(-20f, 7.5f);
        properties.hitProperties.selfKnockback.Set(0f, 0);
        properties.hitProperties.damage = 1600f;
        properties.hitProperties.hitstopTime = FighterAttacks.attackLevel4_hithitstop;
        properties.hitProperties.stunTime = FighterAttacks.attackLevel4_hitstun;
        properties.hitProperties.hardKD = true;

        properties.hitProperties.wallBounce = true;
    }
}