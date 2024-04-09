using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelpDialogue : MonoBehaviour
{
    [TextArea(3, 20)]
    public string[] dialogue;


    public GameObject dialogueRoot;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI dialogueButtonText;

    private int index;

    private void Awake()
    {
        //Debug.Log("show tutorial: " + GameSettings.Instance.ShowTutorial);
        if (GameSettings.Instance.ShowTutorial)
        {
            ShowDialogue();
        }
        else
        {
            HideDialogue();
        }
    }

    public void ShowDialogue()
    {
        index = 0;
        UpdateDialogue();
        dialogueRoot.SetActive(true);
    }

    public void HideDialogue()
    {
        index = 0;
        dialogueRoot.SetActive(false);
    }

    public void PressNext()
    {
        index += 1;
        if (index >= dialogue.Length)
        {
            GameSettings.Instance.ShowTutorial = false;
            GameSettings.Instance.SaveSettings();
            HideDialogue();
        }
        else
        {
            UpdateDialogue();
        }
    }

    public void UpdateDialogue()
    {
        dialogueText.text = dialogue[index];

        dialogueButtonText.text = (index == dialogue.Length - 1) ? "Close" : "Next";
    }
}
