using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowCamera : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera vcam;

    private void OnEnable()
    {
        vcam.Priority = 0;
        ThrowAttackSuccess.ThrowStarted += ThrowStarted;
        ThrowAttackSuccess.ThrowEnded += ThrowEnded;
    }

    private void OnDisable()
    {
        ThrowAttackSuccess.ThrowStarted -= ThrowStarted;
        ThrowAttackSuccess.ThrowEnded -= ThrowEnded;
    }

    public void ThrowStarted(object sender, EventArgs e)
    {
        vcam.Priority = 150;
    }

    public void ThrowEnded(object sender, EventArgs e)
    {
        vcam.Priority = 0;
    }
}
