using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.Windows;

public class PostGameUIPanels : MonoBehaviour
{
    [SerializeField]
    public GameObject panel;

    [SerializeField]
    private MultiplayerEventSystem eventSystem;

    [SerializeField]
    private InputSystemUIInputModule uiInputModule;

    private InputActionReference move;
    private InputActionReference submit;

    public bool UIActive;

    public GameObject rematchButton;

    public GameObject rematchMessage;

    public bool rematchClicked;

    public static event EventHandler RematchClicked, MenuClicked;

    public void Setup(PlayerInput player)
    {
        // original version of the actions asset
        // gotta save this, because player 2's actions asset will be a copy of this one
        // and when the uiInputModule sets to player 2's asset, it will disable the controls of the original
        // so player 1 doesn't have controls anymore
        // unless we re-enable the original assets (that player 1 uses)
        var originalActionsAsset = uiInputModule.actionsAsset;

        //uiInputModule.move = move;
        //uiInputModule.submit = submit;

        //cursor.humanPlayerConfig.Input.uiInputModule = uiInputModule;
        player.uiInputModule = uiInputModule;


        if (originalActionsAsset != null)
        {
         //   originalActionsAsset.Enable();
        }
    }

    private void Start()
    {
        move = uiInputModule.move;
        submit = uiInputModule.submit;

        
    }

    public void Show()
    {
        panel.SetActive(true);
    }
    public void Hide()
    {
        panel.SetActive(false);
    }

    public void SetRematchSelected()
    {
        rematchClicked = false;
        rematchMessage.SetActive(false);
        eventSystem.SetSelectedGameObject(rematchButton);
    }

    public void PressRematch()
    {
        rematchClicked = true;
        rematchMessage.SetActive(true);
        Hide();
        RematchClicked?.Invoke(this, EventArgs.Empty);
    }

    public void PressMenuButton()
    {
        MenuClicked?.Invoke(this, EventArgs.Empty);
    }

    private void OnDisable()
    {
        //var originalActionsAsset = uiInputModule.actionsAsset;

        //if (originalActionsAsset != null)
        //{
        //    originalActionsAsset.Enable();
        //}
    }
}
