using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSelectUIManager : MonoBehaviour
{
    public List<GamePlayerSlot> playerSlots;

    public List<MenuPanels> menuPanels;

    public GameObject startButton;

    private void Awake()
    {
        startButton.SetActive(false);
    }

    public void SetUpCursor(Cursor cursor)
    {
        var index = GamePlayerManager.Instance.GetGamePlayerIndex(cursor.humanPlayerConfig);
        cursor.gamePlayerSlot = playerSlots[index];
        cursor.panels = menuPanels[index];
        cursor.panels.Setup(cursor);
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

    public void OpenControlsPanel(Cursor cursor)
    {
        cursor.panels.OpenControlsMenu(cursor);
    }

    public void SwapPlayers()
    {
        GamePlayerManager.Instance.SwapPlayers();
    }
}
