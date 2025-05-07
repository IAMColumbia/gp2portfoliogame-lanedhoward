using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float hitstopTimeScale = 0.1f;

    public float baseHitstopDuration = 0.1f;

    public float realTimeUntilTimeScaleReset = 0f;
    public bool isInSlowmo = false;
    public bool isInSuperFlash = false;

    private void Start()
    {
        ResetTimeScale();
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
        realTimeUntilTimeScaleReset += hitstopDuration;
        SetTimeScale(hitstopTimeScale);
    }

    private void Update()
    {
        if (isInSuperFlash) return;
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

    private void OnDisable()
    {
        ResetTimeScale();
        isInSlowmo = false;
    }

    public void StartSuperFlash()
    {
        SetTimeScale(0f);
        isInSuperFlash = true;
    }

    public void EndSuperFlash()
    {
        ResetTimeScale();
        isInSuperFlash = false;
    }
}
