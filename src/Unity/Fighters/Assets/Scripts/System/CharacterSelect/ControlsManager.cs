using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsManager : MonoBehaviour
{
    //public PlayerSetupController playerSetupController;

    public HumanPlayerConfig human;

    PlayerInput playerInput;

    public static event EventHandler ControlsUpdated;

    private void OnEnable()
    {
        //playerInput = PlayerConfigurationManager.Instance.GetPlayerInput(playerSetupController.PlayerIndex);
        playerInput = human.Input;

        ControlRebinder.ControlRebound += ControlRebinder_ControlRebound;
    }
    private void OnDisable()
    {
        ControlRebinder.ControlRebound -= ControlRebinder_ControlRebound;
    }

    private void ControlRebinder_ControlRebound(object sender, int playerIndex)
    {
        if (playerIndex == human.PlayerIndex)
        {
            SaveControls();
        }
    }

    public void ResetAllBindings()
    {
        ResetAllBindings(true);
    }

    public void ResetAllBindings(bool save)
    {
        foreach (var map in playerInput.actions.actionMaps)
        {
            map.RemoveAllBindingOverrides();
        }
        if (save)
        {
            SaveControls();
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
        if (human == null) return;
        playerInput = human.Input;
        if (playerInput == null) return;
        Debug.Log("Loading Controls for " + GetRebindsKey());
        ResetAllBindings(false);
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
        Debug.Log("Saving Controls for " + GetRebindsKey());
        string rebinds = playerInput.actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(GetRebindsKey(), rebinds);
    }
}
