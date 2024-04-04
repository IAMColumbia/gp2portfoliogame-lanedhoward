using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : SoundPlayer
{
    InputAction pressAnyKey;
    bool canPressAnyKey;

    public GameObject pressAnyKeyText;
    public GameObject titleText;
    public GameObject menuRoot;
    public GameObject optionsRoot;

    public GameObject firstSelected;
    public GameObject firstSelectedOptions;

    public SoundOption[] soundOptions;

    public AudioClip nameAnnouncement;

    // Start is called before the first frame update
    void Start()
    {
        // apply settings
        ApplySettings();


        pressAnyKey = new InputAction(binding: "/*/<button>");
        pressAnyKey.started += PressAnyKey_performed;
        pressAnyKey.Enable();
        canPressAnyKey = false;

        StartCoroutine(EnablePressAnyKey());

    }

    public IEnumerator EnablePressAnyKey()
    {
        yield return new WaitForSeconds(0.1f);

        canPressAnyKey = true;
    }

    private void PressAnyKey_performed(InputAction.CallbackContext obj)
    {
        if (canPressAnyKey)
        {
            pressAnyKey.performed -= PressAnyKey_performed;
            pressAnyKey.Disable();

            pressAnyKeyText.SetActive(false);
            StartCoroutine(ShowMenu());

            //PlaySound(nameAnnouncement);
        }
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
        StartCoroutine(ShowOptions());
    }

    public IEnumerator ShowOptions()
    {
        menuRoot.SetActive(false);
        titleText.SetActive(false);

        optionsRoot.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);

        yield return new WaitForSeconds(0.1f);

        EventSystem.current.SetSelectedGameObject(firstSelectedOptions);
    }

    public void PressBack()
    {
        StartCoroutine(ShowMenu());
    }

    public IEnumerator ShowMenu()
    {
        optionsRoot.SetActive(false);
        titleText.SetActive(true);
        menuRoot.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);

        yield return new WaitForSeconds(0.1f);

        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public void PressQuit()
    {
        Application.Quit();
    }

    public void ApplySettings()
    {
        foreach (var s in soundOptions)
        {
            s.LoadVolume();
        }
    }
}
