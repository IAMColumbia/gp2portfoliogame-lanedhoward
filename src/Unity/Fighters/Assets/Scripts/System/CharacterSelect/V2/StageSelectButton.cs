using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageSelectButton : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void Start()
    {
        SetLabel();
    }

    public void ClickButton()
    {
        switch (GamePlayerManager.Instance.stageChoice)
        {
            case StageChoice.California:
                GamePlayerManager.Instance.stageChoice = StageChoice.StateLake;
                break;
            case StageChoice.StateLake:
                GamePlayerManager.Instance.stageChoice = StageChoice.Random;
                break;
            default:
            case StageChoice.Random:
                GamePlayerManager.Instance.stageChoice = StageChoice.California;
                break;
        }
        SetLabel();
    }

    public void SetLabel()
    {
        switch (GamePlayerManager.Instance.stageChoice)
        {
            case StageChoice.California:
                text.text = "NEXT STOP: CALIFORNIA";
                break;
            case StageChoice.StateLake:
                text.text = "NEXT STOP: STATE & LAKE";
                break;
            default:
            case StageChoice.Random:
                text.text = "NEXT STOP: ????";
                break;
        }
    }
}
