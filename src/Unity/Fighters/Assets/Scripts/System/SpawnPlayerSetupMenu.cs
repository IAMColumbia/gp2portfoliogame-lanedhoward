using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class SpawnPlayerSetupMenu : MonoBehaviour
{
    public GameObject playerSetupMenuPrefab;
    public PlayerInput input;
    public GameObject cursorPrefab;
    private void Awake()
    {
        /*
        var rootMenu = GameObject.Find("Menus");
        if (rootMenu != null)
        {
            var menu = Instantiate(playerSetupMenuPrefab, rootMenu.transform);
            input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
            menu.GetComponent<PlayerSetupController>().SetPlayer(input);
        }
        */

        var rootMenu = GameObject.Find("UICanvas");
        if (rootMenu != null)
        {
            var cursor = Instantiate(cursorPrefab, Vector3.zero, Quaternion.identity);
            cursor.GetComponent<Cursor>().SetGraphicRaycaster(rootMenu.GetComponent<GraphicRaycaster>());
            cursor.GetComponent<Cursor>().SetPlayerInput(input);
            
        }
    }
}
