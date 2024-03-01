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
    public TextMeshProUGUI swapButtonText;

    public enum State
    {
        ShowingSystem,
        ShowingCharacter
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
        swapButtonText.text = "Show Character Moves";
        CurrentState = State.ShowingSystem;
    }

    public void ShowCharacter()
    {
        SystemText.gameObject.SetActive(false);
        CharacterText.gameObject.SetActive(true);
        swapButtonText.text = "Show Universal Moves";
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

    public void OnSwap()
    {
        if (CurrentState == State.ShowingCharacter)
        {
            ShowSystem();
        }
        else
        {
            ShowCharacter();
        }
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }
}
