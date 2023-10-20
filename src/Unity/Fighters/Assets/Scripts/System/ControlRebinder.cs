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
        var bindingIndex = action.GetBindingIndex(InputBinding.MaskByGroup(playerInput.currentControlScheme));

        action.PerformInteractiveRebinding(bindingIndex)
            .OnApplyBinding((a,b) =>
            {
            })
            .OnComplete(operation => {
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

        bindingText.text = action.bindings[action.GetBindingIndex(InputBinding.MaskByGroup(playerInput.currentControlScheme))].name;


    }
}
