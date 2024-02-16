using LogicUI.FancyTextRendering;
using System;
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

    /// <summary>
    /// int: player index
    /// </summary>
    public static event EventHandler<int> ControlsOpened;
    public static event EventHandler<int> ControlsClosed;

    private void OnEnable()
    {
        //inputActions.FindActionMap("Fighter").Disable();
        eventSystem.SetSelectedGameObject(buttonToSelectOnOpen.gameObject);
        ControlsOpened?.Invoke(this, parent.PlayerIndex);
    }

    private void OnDisable()
    {
        //inputActions.FindActionMap("Fighter").Enable();
        eventSystem.SetSelectedGameObject(buttonToSelectOnClose.gameObject);
        ControlsClosed?.Invoke(this, parent.PlayerIndex);
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }
}
