using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    InputAction pressAnyKey;

    public GameObject titleScreen;
    public GameObject menuRoot;

    public GameObject firstSelected;

    // Start is called before the first frame update
    void Start()
    {
        pressAnyKey = new InputAction(binding: "/*/<button>");
        pressAnyKey.performed += PressAnyKey_performed;
        pressAnyKey.Enable();
    }

    private void PressAnyKey_performed(InputAction.CallbackContext obj)
    {
        pressAnyKey.performed -= PressAnyKey_performed;
        pressAnyKey.Disable();

        titleScreen.SetActive(false);
        menuRoot.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public void PressStart()
    {
        if (GamePlayerManager.Instance != null)
        {
            GamePlayerManager.Instance.Clear();
        }
        SceneManager.LoadScene("PlayerSetupVersion2");
    }

    public void PressOptions()
    {
        // nothing yet
    }

    public void PressQuit()
    {
        Application.Quit();
    }
}
