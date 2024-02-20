using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

}
