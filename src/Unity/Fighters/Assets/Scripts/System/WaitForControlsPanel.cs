using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaitForControlsPanel : MonoBehaviour
{
    public PlayerSetupController parent;
    public GameObject panel;

    // Start is called before the first frame update
    void Awake()
    {
        ControlsPanel.ControlsOpened += ControlsPanel_ControlsOpened;
        ControlsPanel.ControlsClosed += ControlsPanel_ControlsClosed;
    }

    private void ControlsPanel_ControlsOpened(object sender, int e)
    {
        if (parent.PlayerIndex != e)
        {
            panel.SetActive(true);
        }
    }
    private void ControlsPanel_ControlsClosed(object sender, int e)
    {
        if (parent.PlayerIndex != e)
        {
            panel.SetActive(false);
        }
    }
}
