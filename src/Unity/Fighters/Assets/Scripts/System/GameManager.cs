using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player 1")]
    public FighterMain player1;
    public Healthbar player1Healthbar;

    [Header("Player 2")]
    public FighterMain player2;
    public Healthbar player2Healthbar;

    [Header("UI")]
    public GameObject WinScreen;
    public GameObject RematchButton;
    public GameObject StartScreen;
    public GameObject StartButton;
    public GameObject Countdown;
    public TMPro.TextMeshProUGUI CountdownText;
    public EventSystem eventSystem;

    private bool gameActive;

    private void Start()
    {
        gameActive = true;

        player1.enabled = false;
        player2.enabled = false;

        StartScreen.SetActive(true);
        Countdown.SetActive(false);
        WinScreen.SetActive(false);

        eventSystem.SetSelectedGameObject(StartButton);
    }

    private void Update()
    {
        if (gameActive)
        {

            player1Healthbar.SetHealthbar(player1.CurrentHealth, player1.MaxHealth);
            player2Healthbar.SetHealthbar(player2.CurrentHealth, player2.MaxHealth);

            if (player1.CurrentHealth <= 0)
            {
                DeathEffect(player1);
            }

            if (player2.CurrentHealth <= 0)
            {
                DeathEffect(player2);
            }
        }
    }

    private void DeathEffect(FighterMain deadFighter)
    {
        deadFighter.SwitchState(deadFighter.knockdown);
        var hs = (Knockdown)deadFighter.currentState;
        hs.SetStun(50000);


        WinScreen.SetActive(true);

        eventSystem.SetSelectedGameObject(RematchButton);
        
        gameActive = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.5f);
        Countdown.SetActive(true);
        CountdownText.text = "3...";
        yield return new WaitForSeconds(1f);
        CountdownText.text = "2...";
        yield return new WaitForSeconds(1f);
        CountdownText.text = "1...";
        yield return new WaitForSeconds(1f);
        CountdownText.text = "All aboard!!!";
        player1.enabled = true;
        player2.enabled = true;
        yield return new WaitForSeconds(0.75f);
        Countdown.SetActive(false);
    }

    public void StartGameButton()
    {
        StartScreen.SetActive(false);
        StartCoroutine(StartGame());
    }
}
