using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAnimationEvents : MonoBehaviour
{
    public Action FighterAttackActiveStarted;
    public Action FighterAttackRecoveryStarted;
    public Action<Vector2> FighterAnimationVelocityImpulse;
    public Action FighterAnimationHaltVerticalVelocity;


    public void CallAttackActiveEvent()
    {
        FighterAttackActiveStarted?.Invoke();
    }

    public void CallAttackRecoveryEvent()
    {
        FighterAttackRecoveryStarted?.Invoke();
    }
    public void CauseVelocityImpulseX(float xVelocity)
    {
        CauseVelocityImpulse(xVelocity, 0);
    }
    public void CauseVelocityImpulseY(float yVelocity)
    {
        CauseVelocityImpulse(0, yVelocity);
    }

    public void CauseVelocityImpulse(float xVelocity, float yVelocity)
    {
        Vector2 v = new Vector2(xVelocity, yVelocity);
        
        FighterAnimationVelocityImpulse?.Invoke(v);
    }

    public void HaltVerticalVelocity()
    {
        FighterAnimationHaltVerticalVelocity?.Invoke();
    }
}