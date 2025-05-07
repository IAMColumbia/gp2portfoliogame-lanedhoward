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

    public string ControlSchemeRequired;

    /// <summary>
    /// A control was rebound. Argument: playerIndex
    /// </summary>
    public static event EventHandler<int> ControlRebound;

    private void OnEnable()
    {

        playerInput = controls.human.Input;
        ControlsManager.ControlsUpdated += UpdateBindingText;

        if (!string.IsNullOrEmpty(ControlSchemeRequired))
        {
            if (playerInput.currentControlScheme != ControlSchemeRequired)
            {
                gameObject.SetActive(false);
                return;
            }
        }

        UpdateBindingText();
    }

    private void OnDisable()
    {
        ControlsManager.ControlsUpdated -= UpdateBindingText;
    }

    public void PerformRebinding()
    {
        InputAction action = playerInput.actions[ActionPath];

        action.Disable();

        bindingText.text = "<Waiting...>";

        var bindingIndex = GetBindingIndex(action);

        //if (isPartOfComposite)
        //{
        //    var t = action.ChangeCompositeBinding(playerInput.currentControlScheme).NextPartBinding(PartName).bindingIndex;
        //    //t.NextPartBinding("Up").bindingIndex;

        //}


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
            .WithCancelingThrough("an enormous string of absolute gibberish which overrides the default which is escape and causes the above bug")
            .Start();
    }

    public void UpdateBindingText(object sender, EventArgs e)
    {
        UpdateBindingText();
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
        //action.GetBindingIndex()
        if (isPartOfComposite)
        {
            return action.ChangeCompositeBinding(playerInput.currentControlScheme).NextPartBinding(PartName).bindingIndex;
        }
        return action.GetBindingIndex(InputBinding.MaskByGroup(playerInput.currentControlScheme));
    }
}
