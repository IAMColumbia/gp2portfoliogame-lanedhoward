using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class MainMenu : SoundPlayer
{
    InputAction pressAnyKey;
    bool canPressAnyKey;

    InputAction resetButton;

    public GameObject pressAnyKeyText;
    public GameObject titleText;
    public GameObject menuRoot;
    public GameObject optionsRoot;

    public GameObject tutorialRoot;
    bool showingTutorial;

    public GameObject firstSelected;
    public GameObject firstSelectedOptions;

    public SoundOption[] soundOptions;

    public AudioClip nameAnnouncement;

    public InputSystemUIInputModule uiInputModule;

    // Start is called before the first frame update
    void Start()
    {

        // apply settings
        ApplySettings();

        showingTutorial = false;

        pressAnyKey = new InputAction(binding: "/*/<button>");
        pressAnyKey.started += PressAnyKey_performed;
        pressAnyKey.Enable();
        canPressAnyKey = false;

        StartCoroutine(EnablePressAnyKey(1f));

        resetButton = new InputAction(binding: "<Keyboard>/F1");
        resetButton.started += ResetButton_started;
        resetButton.Enable();

    }

    private void ResetButton_started(InputAction.CallbackContext obj)
    {
        Debug.Log("Reset pressed");
        PlayerPrefs.DeleteAll();

        GameSettings.Instance.LoadSettings();

        resetButton.started -= ResetButton_started;
        resetButton.Disable();

        uiInputModule.actionsAsset.RemoveAllBindingOverrides();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator EnablePressAnyKey(float delay)
    {
        yield return new WaitForSeconds(delay);

        canPressAnyKey = true;
    }

    private void PressAnyKey_performed(InputAction.CallbackContext obj)
    {
        if (canPressAnyKey)
        {
            if (GameSettings.Instance.ShowTutorial && !showingTutorial)
            {
                ShowTutorial();
                canPressAnyKey = false;
                StartCoroutine(EnablePressAnyKey(3f));
                return;
            }

            pressAnyKey.performed -= PressAnyKey_performed;
            pressAnyKey.Disable();

            pressAnyKeyText.SetActive(false);
            StartCoroutine(ShowMenu());


            //PlaySound(nameAnnouncement);

            //obj.control.device

            //abridged version from ControlsManager.cs

            foreach (var device in InputSystem.devices)
            {
                if (device.enabled)
                {
                    string rebindsKey = $"Rebinds-{device.name}";

                    string json = PlayerPrefs.GetString(rebindsKey);

                    if (!string.IsNullOrEmpty(json))
                    {
                        try
                        {
                            uiInputModule.actionsAsset.LoadBindingOverridesFromJson(json, false);
                        }
                        catch
                        {
                            // if there is an error, just reset them and save it
                            uiInputModule.actionsAsset.RemoveAllBindingOverrides();
                        }
                    }
                }
            }

            

        }
    }

    public void PressStart()
    {
        if (GamePlayerManager.Instance != null)
        {
            GamePlayerManager.Instance.Clear();
        }

        resetButton.started -= ResetButton_started;
        resetButton.Disable();

        SceneManager.LoadScene("PlayerSetupVersion2");
    }

    public void PressOptions()
    {
        StartCoroutine(ShowOptions());
    }

    public IEnumerator ShowOptions()
    {
        tutorialRoot.SetActive(false);

        menuRoot.SetActive(false);
        titleText.SetActive(false);

        optionsRoot.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);

        yield return new WaitForSeconds(0.1f);

        EventSystem.current.SetSelectedGameObject(firstSelectedOptions);
    }

    public void ShowTutorial()
    {
        menuRoot.SetActive(false);
        titleText.SetActive(false);
        pressAnyKeyText.SetActive(false);
        optionsRoot.SetActive(false);

        showingTutorial = true;

        tutorialRoot.SetActive(true);
    }

    public void PressBack()
    {
        StartCoroutine(ShowMenu());
    }

    public IEnumerator ShowMenu()
    {
        tutorialRoot.SetActive(false);
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
