using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    public virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }

    /// <summary>
    /// plays a random sound from the array
    /// </summary>
    /// <param name="sounds"></param>
    public void PlaySound(AudioClip[] sounds)
    {
        int clipIndex = Random.Range(0, sounds.Length);
        audioSource.PlayOneShot(sounds[clipIndex]);
    }
}
