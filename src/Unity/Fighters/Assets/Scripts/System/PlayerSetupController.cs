using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSetupController : MonoBehaviour
{
    public int PlayerIndex;

    public TextMeshProUGUI titleText;

    public float ignoreInputTime = 1.0f;
    public bool inputEnabled;

    public void SetPlayerIndex(int pi)
    {
        PlayerIndex = pi;
        titleText.SetText("Player " + (pi + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
    }

    private void Update()
    {
        if (inputEnabled == false && Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    public void SetCharacter(CharacterModule character)
    {
        if (!inputEnabled) return;

        PlayerConfigurationManager.Instance.SetCharacter(PlayerIndex, character);
        PlayerConfigurationManager.Instance.SetReady(PlayerIndex);

    }
}
