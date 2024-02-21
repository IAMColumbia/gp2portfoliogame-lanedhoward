using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSelectUIManager : MonoBehaviour
{
    public List<GamePlayerSlot> playerSlots;

    public GameObject startButton;

    private void Awake()
    {
        startButton.SetActive(false);
    }

    public void SetUpCursor(Cursor cursor)
    {
        cursor.gamePlayerSlot = playerSlots[GamePlayerManager.Instance.GetGamePlayerIndex(cursor.humanPlayerConfig)];
    }

    private void OnEnable()
    {
        GamePlayerSlot.GamePlayerUpdated += GamePlayerSlot_CharacterUpdated;
    }

    private void OnDisable()
    {
        GamePlayerSlot.GamePlayerUpdated -= GamePlayerSlot_CharacterUpdated;
    }

    private void GamePlayerSlot_CharacterUpdated(object sender, System.EventArgs e)
    {
        // if both players have a character , start button is enabled
        startButton.SetActive(playerSlots.All(s => s.gamePlayerConfig.Character != null 
        && (s.gamePlayerConfig.playerType != PlayerType.Human || s.humanPlayerConfig != null)));
        
    }
}
