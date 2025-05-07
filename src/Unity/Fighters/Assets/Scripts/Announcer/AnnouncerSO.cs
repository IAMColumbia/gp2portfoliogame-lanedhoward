using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnnouncerSounds")]
public class AnnouncerSO : ScriptableObject
{
    public AudioClip[] RoundIntro;
    public AudioClip[] RoundStart;
    public AudioClip[] RoundEnd;
    public AudioClip[] Perfect;
    public AudioClip DoubleKO;
    public AudioClip[] TrainWhistles;
}
