using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlRebinder : MonoBehaviour
{
    public PlayerSetupController playerSetupController;

    PlayerInput playerInput;

    public string ActionPath;

    public TextMeshProUGUI bindingText;

    /// <summary>
    /// A control was rebound. Argument: playerIndex
    /// </summary>
    public static event EventHandler<int> ControlRebound;

    private void OnEnable()
    {
        playerInput = PlayerConfigurationManager.Instance.GetPlayerInput(playerSetupController.PlayerIndex);
        UpdateBindingText();
        ControlsManager.ControlsUpdated += (s,e) => UpdateBindingText();
    }

    private void OnDisable()
    {
        ControlsManager.ControlsUpdated -= (s, e) => UpdateBindingText();
    }

    public void PerformRebinding()
    {
        InputAction action = playerInput.actions[ActionPath];

        action.Disable();

        bindingText.text = "<Waiting...>";

        var bindingIndex = GetBindingIndex(action);

        action.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(operation => {
                UpdateBindingText();
                action.Enable();
                ControlRebound?.Invoke(this, playerSetupController.PlayerIndex);
                operation.Dispose();
            })
            .OnCancel(operation =>
            {
                UpdateBindingText();
                action.Enable();
                operation.Dispose();
            })
            .WithMatchingEventsBeingSuppressed(true)
            .WithTimeout(5)
            .OnMatchWaitForAnother(0.1f)
            .Start();
    }

    public void UpdateBindingText()
    {
        if (playerInput == null)
        {
            return;
        }
        InputAction action = playerInput.actions[ActionPath];
        int bindingIndex = GetBindingIndex(action);

        bindingText.text = InputControlPath.ToHumanReadableString(action.bindings[bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.UseShortNames);
        //action.bindings[action.GetBindingIndex(InputBinding.MaskByGroup(playerInput.currentControlScheme))].name;


    }

    private int GetBindingIndex(InputAction action)
    {
        return action.GetBindingIndex(InputBinding.MaskByGroup(playerInput.currentControlScheme));
    }
}
