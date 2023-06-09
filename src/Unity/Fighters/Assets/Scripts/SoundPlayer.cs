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

    public void PlaySound(AudioClip[] sounds)
    {
        int clipIndex = Random.Range(0, sounds.Length - 1);
        audioSource.PlayOneShot(sounds[clipIndex]);
    }
}
