using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayerSlot : MonoBehaviour
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

    private void Awake()
    {
        token = Instantiate(tokenPrefab, transform.position, Quaternion.identity, transform.parent);
        token.SetUpToken(this);
        portraitImage.gameObject.SetActive(false);
        nameText.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        gamePlayerConfig = GamePlayerManager.Instance.gamePlayerConfigs[PlayerSlotIndex];
    }

    public void SetHumanPlayerConfig(HumanPlayerConfig human)
    {
        humanPlayerConfig = human;
        
        if (human == null)
        {
            // need to set the control type to CPU probably
        }
        // set controller label
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
        nameText.gameObject.SetActive(true);
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
}
