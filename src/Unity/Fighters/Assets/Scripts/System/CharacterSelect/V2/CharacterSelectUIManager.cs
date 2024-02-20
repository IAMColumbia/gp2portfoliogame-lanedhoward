using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectUIManager : MonoBehaviour
{
    public List<GamePlayerSlot> playerSlots;

    public void SetUpCursor(Cursor cursor)
    {
        cursor.gamePlayerSlot = playerSlots[GamePlayerManager.Instance.GetGamePlayerIndex(cursor.humanPlayerConfig)];
    }
}
