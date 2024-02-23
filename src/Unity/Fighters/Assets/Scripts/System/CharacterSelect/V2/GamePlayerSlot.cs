using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayerSlot : SoundPlayer
{
    [Tooltip("Set in insepctor (0 or 1) for which player side this slot is for")]
    public int PlayerSlotIndex;

    /// <summary>
    /// Set during start(), never changes
    /// </summary>
    public GamePlayerConfig gamePlayerConfig;

    /// <summary>
    /// Might be set by GamePlayerManager
    /// </summary>
    public HumanPlayerConfig humanPlayerConfig;

    [SerializeField]
    private Token tokenPrefab;

    public Token token;

    public Color color;

    public Image portraitImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;

    public TextMeshProUGUI controllerText;

    public static event EventHandler GamePlayerUpdated;

    public override void Awake()
    {
        base.Awake();
        token = Instantiate(tokenPrefab, transform.position, Quaternion.identity, transform.parent);
        token.SetUpToken(this);
        portraitImage.gameObject.SetActive(false);
        nameText.transform.parent.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        gamePlayerConfig = GamePlayerManager.Instance.gamePlayerConfigs[PlayerSlotIndex];
        SetControllerSetting(PlayerType.CPU);
    }

    public void SetHumanPlayerConfig(HumanPlayerConfig human)
    {
        humanPlayerConfig = human;
        
        if (human == null)
        {
            // need to set the control type to CPU probably
            SetControllerSetting(PlayerType.CPU);
        }
        else
        {
            // set controller label
            SetControllerSetting(PlayerType.Human);
        }
    }

    public void SetCharacter(CharacterModule characterModule)
    {
        gamePlayerConfig.Character = characterModule;
        gamePlayerConfig.CharacterMaterialIndex = 0;

        portraitImage.sprite = characterModule.Portrait;
        portraitImage.material = characterModule.materials[0];
        portraitImage.gameObject.SetActive(true);

        nameText.text = characterModule.CharacterName;
        descText.text = characterModule.CharacterDescription;
        nameText.transform.parent.gameObject.SetActive(true);

        if (characterModule.nameAnnouncement != null)
        {
            PlaySound(characterModule.nameAnnouncement);
        }

        GamePlayerUpdated?.Invoke(this, EventArgs.Empty);
    }

    public void ClearCharacter()
    {
        gamePlayerConfig.Character = null;

        portraitImage.gameObject.SetActive(false);

        nameText.transform.parent.gameObject.SetActive(false);

        GamePlayerUpdated?.Invoke(this, EventArgs.Empty);
    }


    public void CycleColor()
    {
        if (gamePlayerConfig.Character != null)
        {
            gamePlayerConfig.CharacterMaterialIndex += 1;
            if (gamePlayerConfig.CharacterMaterialIndex >= gamePlayerConfig.Character.materials.Length)
            {
                gamePlayerConfig.CharacterMaterialIndex = 0;
            }

            portraitImage.material = gamePlayerConfig.Character.materials[gamePlayerConfig.CharacterMaterialIndex];
        }
    }

    public void CycleControllerSetting()
    {
        switch (gamePlayerConfig.playerType)
        {
            case PlayerType.Human:
                SetControllerSetting(PlayerType.CPU);
                break;
            case PlayerType.CPU:
                SetControllerSetting(PlayerType.Training);
                break;
            case PlayerType.Training:
                SetControllerSetting(PlayerType.Human);
                break;
        }
        GamePlayerUpdated?.Invoke(this, EventArgs.Empty);
    }

    public void SetControllerSetting(PlayerType playerType)
    {
        gamePlayerConfig.playerType = playerType;

        string text = $"P{PlayerSlotIndex + 1}: ";

        switch (playerType)
        {
            case PlayerType.Human:
                if (humanPlayerConfig != null)
                {
                    text += humanPlayerConfig.Input.devices[0].name;
                }
                else
                {
                    text += "NO CONTROLLER";
                }
                break;
            case PlayerType.CPU:
                text += "CPU";
                break;
            case PlayerType.Training:
                text += "Training Dummy";
                break;
        }

        controllerText.text = text;
    }
}
