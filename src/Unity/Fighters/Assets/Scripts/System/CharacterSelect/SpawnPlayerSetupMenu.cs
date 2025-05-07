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
    public Cursor cursorPrefab;
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
        /*/

        var rootMenu = GameObject.Find("UICanvas");
        if (rootMenu != null)
        {
            Cursor cursor = Instantiate<Cursor>(cursorPrefab, Vector3.zero, Quaternion.identity);
            //cursor.transform.SetParent(rootMenu.transform);
            cursor.canvas = rootMenu.GetComponent<Canvas>();
            cursor.SetGraphicRaycaster(rootMenu.GetComponent<GraphicRaycaster>());
            cursor.SetPlayerInput(input);
            cursor.uiManager = rootMenu.GetComponent<CharacterSelectUIManager>();
            
        }
        //*/
    }
}
