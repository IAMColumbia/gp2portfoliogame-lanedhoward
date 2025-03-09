using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : SoundPlayer
{
    public AudioClip defaultMusic;

    public void PlayMusic(AudioClip music)
    {
        audioSource.loop = true;
        if (audioSource.isPlaying) audioSource.Stop();
        audioSource.clip = music;
        audioSource.Play();
    }
}
