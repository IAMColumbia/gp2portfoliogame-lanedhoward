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
    public ControlsManager manager;
    //public PlayerSetupController parent;

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
        eventSystem.SetSelectedGameObject(buttonToSelectOnOpen.gameObject);
        ControlsOpened?.Invoke(this, manager.human.PlayerIndex);
    }

    private void OnDisable()
    {
        //eventSystem.SetSelectedGameObject(buttonToSelectOnClose.gameObject);
        eventSystem.SetSelectedGameObject(null);
        ControlsClosed?.Invoke(this, manager.human.PlayerIndex);
    }

    public void OnClose()
    {
        //ControlsClosed?.Invoke(this, 0);
        gameObject.SetActive(false);
    }
}
