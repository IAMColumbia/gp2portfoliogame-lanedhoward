using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrainingDetails : MonoBehaviour
{
    bool value => GameSettings.Instance.ShowTrainingInfo;
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
        GameSettings.Instance.ShowTrainingInfo = !value;
        GameSettings.Instance.SaveSettings();
        SetButtonText();
    }
}
