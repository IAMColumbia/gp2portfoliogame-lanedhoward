using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using UnityEngine.UI;
using TMPro;
using PrimeTween;
using System;

public class SuperPortrait : MonoBehaviour
{
    public Image portrait;
    public TextMeshProUGUI superNameText;

    public float totalDuration = 0.5f, portraitFadeTime = 0.1f;
    public float textStartX, textEndX;

    private void Awake()
    {
        portrait.gameObject.SetActive(false);
        superNameText.gameObject.SetActive(false);
    }

    public void StartSuperPortrait(object sender, FighterMain.SuperPortraitEventArgs e)
    {
        superNameText.text = e.SuperName;
        StartTween();
    }

    private void StartTween()
    {
        portrait.gameObject.SetActive(true);
        superNameText.gameObject.SetActive(true);

        Tween.Alpha(portrait, 
                startValue: 0f, endValue: 1f, 
                duration: portraitFadeTime, 
                useUnscaledTime: true);
        Tween.Alpha(portrait, startValue: 1f, endValue: 0f, 
                duration: portraitFadeTime, 
                useUnscaledTime: true, 
                startDelay: totalDuration-portraitFadeTime)
            .OnComplete(target: portrait.gameObject, target => target.SetActive(false)); 

        Tween.LocalPositionX(superNameText.transform, 
                startValue: textStartX, endValue: 0f, 
                duration: totalDuration / 2, 
                useUnscaledTime: true, 
                ease: Ease.OutCubic);
        Tween.LocalPositionX(superNameText.transform, 
                startValue: 0f, endValue: textEndX, 
                duration: totalDuration / 2, 
                useUnscaledTime: true, 
                ease: Ease.InCubic, 
                startDelay: totalDuration/2)
            .OnComplete(target: superNameText.gameObject, target => target.SetActive(false));

    }
}
