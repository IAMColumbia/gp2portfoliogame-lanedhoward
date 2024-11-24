using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperFlashCamera : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera vcam;

    private void OnEnable()
    {
        vcam.Priority = 0;
        FighterMain.SuperFlashStarted += SuperFlashStarted;
        FighterMain.SuperFlashEnded += SuperFlashEnded;
    }

    private void OnDisable()
    {
        FighterMain.SuperFlashStarted -= SuperFlashStarted;
        FighterMain.SuperFlashEnded -= SuperFlashEnded;
    }

    public void SuperFlashStarted(object sender, GameObject e)
    {
        vcam.Priority = 150;
        vcam.Follow = e.transform;
    }

    public void SuperFlashEnded(object sender, EventArgs e)
    {
        vcam.Priority = 0;
    }
}
