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

    private void OnEnable()
    {
        playerInput = PlayerConfigurationManager.Instance.GetPlayerInput(playerSetupController.PlayerIndex);
        UpdateBindingText();
    }

    public void PerformRebinding()
    {
        InputAction action = playerInput.actions[ActionPath];

        action.Disable();

        bindingText.text = "<Waiting...>";

        //action.GetBindingIndex()
        var bindingIndex = GetBindingIndex(action);

        action.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(operation => {
                UpdateBindingText();
                action.Enable();
                operation.Dispose();
                })
            .OnCancel(operation =>
            {
                UpdateBindingText();
                action.Enable();
                operation.Dispose();
            })
            .WithTimeout(5)
            .Start();
    }

    public void UpdateBindingText()
    {
        InputAction action = playerInput.actions[ActionPath];
        int bindingIndex = GetBindingIndex(action);

        bindingText.text = InputControlPath.ToHumanReadableString(action.bindings[bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        //action.bindings[action.GetBindingIndex(InputBinding.MaskByGroup(playerInput.currentControlScheme))].name;


    }

    private int GetBindingIndex(InputAction action)
    {
        return action.GetBindingIndex(InputBinding.MaskByGroup(playerInput.currentControlScheme));
    }
}