using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SoundOption : SoundPlayer
{
    /// <summary>
    /// 0-1
    /// </summary>
    public float value = 1f;

    public float increment = 0.05f;

    public string optionKey;

    public AudioMixer mixer;

    public TextMeshProUGUI valueText;

    public bool playTestSound;
    public AudioClip[] testSounds;

    public void Increment()
    {
        value = Mathf.Clamp(value + increment, 0, 1);
        SetVolume();
        if (playTestSound) PlaySound(testSounds);
    }

    public void Decrement()
    {
        value = Mathf.Clamp(value - increment, 0, 1);
        SetVolume();
        if (playTestSound) PlaySound(testSounds);
    }

    public void SetVolume()
    {
        mixer.SetFloat(optionKey, Mathf.Log10(value + 0.00001f)*20);
        PlayerPrefs.SetFloat(optionKey, value);
        valueText.text = $"{Mathf.Round(value * 100)}%";
    }

    public void LoadVolume()
    {
        value = PlayerPrefs.GetFloat(optionKey, 1);
        SetVolume();
    }

    private void OnEnable()
    {
        LoadVolume();
    }
}
