using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CpuRematch : MonoBehaviour
{
    bool value => GameSettings.Instance.CPUvCPUAutoRematch;
    public TextMeshProUGUI buttonText;

    private void Start()
    {
        SetButtonText();
    }

    public void SetButtonText()
    {
        buttonText.text = value ? "ON" : "OFF";
    }

    public void Toggle()
    {
        GameSettings.Instance.CPUvCPUAutoRematch = !value;
        GameSettings.Instance.SaveSettings();
        SetButtonText();
    }
}
