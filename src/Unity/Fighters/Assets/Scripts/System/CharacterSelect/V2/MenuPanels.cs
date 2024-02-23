using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.Windows;

public class MenuPanels : MonoBehaviour
{
    public ControlsManager controlsManager;

    [SerializeField]
    private MultiplayerEventSystem eventSystem;

    [SerializeField]
    private InputSystemUIInputModule uiInputModule;

    public void Setup(Cursor cursor)
    {
        controlsManager.human = cursor.humanPlayerConfig;

        // original version of the actions asset
        // gotta save this, because player 2's actions asset will be a copy of this one
        // and when the uiInputModule sets to player 2's asset, it will disable the controls of the original
        // so player 1 doesn't have controls anymore
        // unless we re-enable the original assets (that player 1 uses)
        var originalActionsAsset = uiInputModule.actionsAsset;

        cursor.humanPlayerConfig.Input.uiInputModule = uiInputModule;

        originalActionsAsset.Enable();
    }

    public void OpenControlsMenu(Cursor cursor)
    {
        
        //gameObject.SetActive(true);

        //eventSystem.enabled = true;
        controlsManager.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        ControlsPanel.ControlsClosed += ControlsPanel_ControlsClosed;
    }

    private void OnDisable()
    {
        ControlsPanel.ControlsClosed -= ControlsPanel_ControlsClosed;
    }

    private void ControlsPanel_ControlsClosed(object sender, int e)
    {
        //controlsManager.human.Input.uiInputModule = null;
        //eventSystem.SetSelectedGameObject(null);
        //eventSystem.enabled = false;
        //gameObject.SetActive(false);
    }
}
