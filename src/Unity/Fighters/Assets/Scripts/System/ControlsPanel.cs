using LogicUI.FancyTextRendering;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class ControlsPanel : MonoBehaviour
{
    public PlayerSetupController parent;

    public Button buttonToSelectOnOpen;
    public Button buttonToSelectOnClose;

    public EventSystem eventSystem;

    //public InputActionAsset inputActions;

    private void OnEnable()
    {
        //inputActions.FindActionMap("Fighter").Disable();
        eventSystem.SetSelectedGameObject(buttonToSelectOnOpen.gameObject);
    }

    private void OnDisable()
    {
        //inputActions.FindActionMap("Fighter").Enable();
        eventSystem.SetSelectedGameObject(buttonToSelectOnClose.gameObject);
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }
}
