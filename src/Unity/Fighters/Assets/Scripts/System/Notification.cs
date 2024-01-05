using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notification : MonoBehaviour
{
    public Color32 exitColor;

    private float fadeTime;

    private TextMeshProUGUI text;
    private float timer;

    bool fadeStarted;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (timer < 0)
        {
            timer = -1;
            DisableText();
        }
        else
        {
            timer -= Time.deltaTime;
            if (!fadeStarted)
            {
                if (timer < fadeTime)
                {
                    text.CrossFadeColor(exitColor, fadeTime / 1.5f, false, false);
                    fadeStarted = true;
                }
            }
        }
    }

    public void EnableText(string newText, float time, Color32 activeColor, Color32 exitColor, float fadeTime)
    {
        transform.SetAsFirstSibling();
        timer = time;
        text.text = newText;
        gameObject.SetActive(true);
        //this.activeColor = activeColor;
        this.exitColor = exitColor;

        text.CrossFadeColor(activeColor, 0f, false, false);

        fadeStarted = false;
        this.fadeTime = fadeTime;
        //text.CrossFadeColor(exitColor, time / 1.5f, false, false);
    }

    public void DisableText()
    {
        //transform.SetAsLastSibling();
        gameObject.SetActive(false);
    }
}
