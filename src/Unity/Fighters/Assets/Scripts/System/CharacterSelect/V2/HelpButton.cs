using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelpButton : MonoBehaviour
{
    public string showText = "Show Help";
    public string hideText = "Hide Help";

    public TextMeshProUGUI buttonText;

    public GameObject helpMenu;

    private void Start()
    {
        helpMenu.SetActive(false);
        buttonText.text = showText;
    }

    public void PressHelpButton()
    {
        if (helpMenu.activeInHierarchy)
        {
            helpMenu.SetActive(false);
            buttonText.text = showText;
        }
        else
        {
            helpMenu.SetActive(true);
            buttonText.text = hideText;
        }

    }
}
