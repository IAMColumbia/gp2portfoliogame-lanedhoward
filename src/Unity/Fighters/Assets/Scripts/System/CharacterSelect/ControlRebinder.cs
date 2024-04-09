using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlRebinder : MonoBehaviour
{
    //public PlayerSetupController playerSetupController;

    public ControlsManager controls;

    PlayerInput playerInput;

    public string ActionPath;

    public bool isPartOfComposite;
    public string PartName;

    public TextMeshProUGUI bindingText;


    /// <summary>
    /// A control was rebound. Argument: playerIndex
    /// </summary>
    public static event EventHandler<int> ControlRebound;

    private void OnEnable()
    {
        //playerInput = PlayerConfigurationManager.Instance.GetPlayerInput(playerSetupController.PlayerIndex);
        playerInput = controls.human.Input;
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

        if (isPartOfComposite)
        {
            var t = action.ChangeCompositeBinding("Keyboard-Space").WithGroup(playerInput.currentControlScheme).NextPartBinding(PartName).bindingIndex;
            //t.NextPartBinding("Up").bindingIndex;

        }


        action.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(operation => {
                UpdateBindingText();
                action.Enable();
                ControlRebound?.Invoke(this, controls.human.PlayerIndex);
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
