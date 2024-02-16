using LogicUI.FancyTextRendering;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovesListPanel : MonoBehaviour
{
    public PlayerSetupController parent;

    public TextMeshProUGUI SystemText;
    public MarkdownRenderer CharacterText;

    public enum State
    {
        ShowingSystem,
        ShowingCharacter
    }

    public State CurrentState = State.ShowingSystem;

    public Button buttonToSelectOnOpen;
    public Button buttonToSelectOnClose;

    public EventSystem eventSystem;

    private void OnEnable()
    {
        ShowCharacter();
        eventSystem.SetSelectedGameObject(buttonToSelectOnOpen.gameObject);

    }

    private void OnDisable()
    {
        eventSystem.SetSelectedGameObject(buttonToSelectOnClose.gameObject);
    }

    public void ShowSystem()
    {
        SystemText.gameObject.SetActive(true);
        CharacterText.gameObject.SetActive(false);
        CurrentState = State.ShowingSystem;
    }

    public void ShowCharacter()
    {
        SystemText.gameObject.SetActive(false);
        CharacterText.gameObject.SetActive(true);
        CurrentState = State.ShowingCharacter;

        CharacterModule character = PlayerConfigurationManager.Instance.GetCharacter(parent.PlayerIndex);

        if (character != null)
        {
            CharacterText.Source = character.MovesList;
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
