using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSetupController : MonoBehaviour
{
    public int PlayerIndex;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI controllerText;
    public Image previewImage;

    public float ignoreInputTime = 0.25f;
    public bool inputEnabled;

    private int forceStart = 5;

    [SerializeField]
    private EventSystem eventSystem;

    public void SetPlayer(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        titleText.SetText("Player " + (PlayerIndex + 1).ToString());
        controllerText.SetText(pi.devices[0].displayName);
        ignoreInputTime = Time.time + ignoreInputTime;
    }

    private void Update()
    {
        if (inputEnabled == false && Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    private void OnEnable()
    {
        PlayerConfigurationManager.PlayerJoined += PlayerConfigurationManager_PlayerJoined;
    }

    private void OnDisable()
    {
        PlayerConfigurationManager.PlayerJoined -= PlayerConfigurationManager_PlayerJoined;
    }

    private void PlayerConfigurationManager_PlayerJoined(object sender, System.EventArgs e)
    {
        //eventSystem.enabled = false;
        //eventSystem.enabled = true;
        //eventSystem.gameObject.SetActive(false);
        //eventSystem.gameObject.SetActive(true);
        StartCoroutine(ResetEventSystem());
    }

    private IEnumerator ResetEventSystem()
    {
        //eventSystem.enabled = false;
        eventSystem.gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(0.1f);

        //eventSystem.enabled = true;
        eventSystem.gameObject.SetActive(true);
    }

    public void SetCharacter(CharacterModule character)
    {
        if (!inputEnabled) return;

        PlayerConfigurationManager.Instance.SetCharacter(PlayerIndex, character);
        UpdatePreviewImage();
    }

    public void SetColor(int modifier)
    {
        if (!inputEnabled) return;

        PlayerConfigurationManager.Instance.SetMaterialIndex(PlayerIndex, modifier);

        UpdatePreviewImage();
    }

    public void SetReady()
    {
        PlayerConfigurationManager.Instance.SetReady(PlayerIndex);

        forceStart -= 1;
        if (forceStart <= 0)
        {
            PlayerConfigurationManager.Instance.ForceStart();
        }

        UpdatePreviewImage();
    }

    public void UpdatePreviewImage()
    {
        Material newMat = PlayerConfigurationManager.Instance.GetMaterial(PlayerIndex);

        if (newMat == null) return;

        previewImage.color = Color.white;

        previewImage.material = newMat;
    }
}
