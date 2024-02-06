using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsManager : MonoBehaviour
{
    public PlayerSetupController playerSetupController;

    PlayerInput playerInput;

    public static event EventHandler ControlsUpdated;

    private void OnEnable()
    {
        playerInput = PlayerConfigurationManager.Instance.GetPlayerInput(playerSetupController.PlayerIndex);

        ControlRebinder.ControlRebound += ControlRebinder_ControlRebound;
    }
    private void OnDisable()
    {
        ControlRebinder.ControlRebound -= ControlRebinder_ControlRebound;
    }

    private void ControlRebinder_ControlRebound(object sender, int playerIndex)
    {
        if (playerIndex == playerSetupController.PlayerIndex)
        {
            SaveControls();
        }
    }

    public void ResetAllBindings()
    {
        foreach (var map in playerInput.actions.actionMaps)
        {
            map.RemoveAllBindingOverrides();
        }

        ControlsUpdated?.Invoke(this, EventArgs.Empty);
    }

    public string GetRebindsKey()
    {
        if (playerInput.devices.Count < 1)
        {
            return null;
        }
        return $"Rebinds-{playerInput.devices[0].name}";
    }

    public void LoadControls()
    {
        if (PlayerConfigurationManager.Instance.playerConfigs[playerSetupController.PlayerIndex].IsRealPlayer == false) return;
        playerInput = PlayerConfigurationManager.Instance.GetPlayerInput(playerSetupController.PlayerIndex);
        if (playerInput == null) return;
        ResetAllBindings();
        string rebinds = PlayerPrefs.GetString(GetRebindsKey());
        if (!string.IsNullOrEmpty(rebinds))
        {
            playerInput.actions.LoadBindingOverridesFromJson(rebinds);
        }
        ControlsUpdated?.Invoke(this, EventArgs.Empty);
    }

    public void SaveControls()
    {
        if (playerInput == null) return;
        string rebinds = playerInput.actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(GetRebindsKey(), rebinds);
    }
}
