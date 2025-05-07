using LogicUI.FancyTextRendering;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovesListPanel : MonoBehaviour
{
    public GamePlayerConfig gamePlayerConfig;

    public TextMeshProUGUI SystemText;
    public MarkdownRenderer CharacterText;
    public MarkdownRenderer BioText;
    public TextMeshProUGUI swapButtonText;

    public enum State
    {
        ShowingSystem,
        ShowingCharacter,
        ShowingBio
    }

    public State CurrentState = State.ShowingSystem;

    public Button buttonToSelectOnOpen;
    public Button buttonToSelectOnClose;

    public EventSystem eventSystem;

    public static event EventHandler MovesListOpened;
    public static event EventHandler MovesListClosed;

    private void OnEnable()
    {
        ShowCharacter();
        eventSystem.SetSelectedGameObject(buttonToSelectOnOpen.gameObject);
        MovesListOpened?.Invoke(this, EventArgs.Empty);

    }

    private void OnDisable()
    {
        //eventSystem.SetSelectedGameObject(buttonToSelectOnClose.gameObject);
        MovesListClosed?.Invoke(this, EventArgs.Empty);

    }

    public void ShowSystem()
    {
        SystemText.gameObject.SetActive(true);
        CharacterText.gameObject.SetActive(false);
        BioText.gameObject.SetActive(false);
        swapButtonText.text = "Show Character Moves";
        CurrentState = State.ShowingSystem;
    }

    public void ShowCharacter()
    {
        SystemText.gameObject.SetActive(false);
        CharacterText.gameObject.SetActive(true);
        BioText.gameObject.SetActive(false);
        swapButtonText.text = "Show Character Bio";
        CurrentState = State.ShowingCharacter;

        CharacterModule character = gamePlayerConfig.Character;

        if (character != null)
        {
            CharacterText.Source = character.MovesList;
        }
        else
        {
            CharacterText.Source = 
@"## Character Moves


### No Character Selected
";
        }
    }

    public void ShowBio()
    {
        SystemText.gameObject.SetActive(false);
        CharacterText.gameObject.SetActive(false);
        BioText.gameObject.SetActive(true);
        swapButtonText.text = "Show System Mechanics";
        CurrentState = State.ShowingBio;

        CharacterModule character = gamePlayerConfig.Character;

        if (character != null)
        {
            BioText.Source = character.Bio;
        }
        else
        {
            BioText.Source =
@"## Character Bio


### No Character Selected
";
        }
    }

    public void OnSwap()
    {
        switch (CurrentState)
        {
            case State.ShowingSystem:
                ShowCharacter();
                break;
            case State.ShowingCharacter:
                ShowBio();
                break;
            case State.ShowingBio:
                ShowSystem();
                break;
        }
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }
}
