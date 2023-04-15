using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float hitstopTimeScale = 0.1f;

    public float baseHitstopDuration = 0.1f;

    public float realTimeUntilTimeScaleReset = 0f;
    public bool isInSlowmo = false;

    private void Start()
    {
        
    }

    public void SetTimeScale(float newTimeScale)
    {
        Time.timeScale = newTimeScale;
    }

    public void ResetTimeScale()
    {
        SetTimeScale(1f);
    }


    public void DoHitStop()
    {
        DoHitStop(baseHitstopDuration);
    }

    public void DoHitStop(float hitstopDuration)
    {
        isInSlowmo = true;
        realTimeUntilTimeScaleReset = hitstopDuration;
        SetTimeScale(hitstopTimeScale);
    }

    private void FixedUpdate()
    {
        if (isInSlowmo)
        {
            if (realTimeUntilTimeScaleReset > 0)
            {
                realTimeUntilTimeScaleReset -= Time.unscaledDeltaTime;
            }
            else
            {
                // first frame out of slowmo
                ResetTimeScale();
                realTimeUntilTimeScaleReset = 0;
                isInSlowmo = false;
            }
        }
    }
}
