using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameAttack
{
    //gonna need conditions like

    // can this be done in the air, ground, or while crouching?

    // is this a light, medium, heavy, dash, or special move?


    // then after its exectuted, itll need proprties like

    // was this a light, medium, heavy, dash, or special move?

    // what cancels can you do from this move? and when?

    // also,
    
    // is this a strike or a throw?

    // damage, knockback, hitstun, blockstun

    // strike / throw invuln

    // counterhit state (and maybe more things that come with that ?)

    public virtual bool CanExecute()
    {
        return false;
    }


}
