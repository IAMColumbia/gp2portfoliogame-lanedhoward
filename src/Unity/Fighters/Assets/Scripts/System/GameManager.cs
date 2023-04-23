using System;
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
    public ComboUI player1comboUI;

    [Header("Player 2")]
    public FighterMain player2;
    public Healthbar player2Healthbar;
    public ComboUI player2comboUI;

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

        player1.GotHit += Player_GotHit;
        player2.GotHit += Player_GotHit;

        player1.LeftHitstun += Player_LeftHitstun;
        player2.LeftHitstun += Player_LeftHitstun;

        player1comboUI.HideText();
        player2comboUI.HideText();

        StartScreen.SetActive(true);
        Countdown.SetActive(false);
        WinScreen.SetActive(false);

        eventSystem.SetSelectedGameObject(StartButton);
    }

    private void Player_LeftHitstun(object sender, EventArgs e)
    {
        FighterMain s = (FighterMain)sender;
        if (s == null) throw new Exception("Got lefthitstun event from null sender");

        ComboUI ui;
        if (s == player1)
        {
            ui = player2comboUI;
        }
        else
        {
            ui = player1comboUI;
        }

        ui.StartEndComboCount();
    }

    private void Player_GotHit(object sender, EventArgs e)
    {
        FighterMain s = (FighterMain)sender;

        if (s == null) throw new Exception("Got hit event from null sender");

        Combo combo;
        combo = s.currentCombo;

        ComboUI ui;
        if (s == player1)
        {
            ui = player2comboUI;
        }
        else
        {
            ui = player1comboUI;
        }

        if (!ui.isActiveAndEnabled) ui.ShowText();

        ui.UpdateComboText(combo.hitCount, combo.totalDamage);
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

        gameActive = false;

        deadFighter.timeManager.DoHitStop(1f);

        StartCoroutine(DeathEffectCoroutine());
        
    }

    private IEnumerator DeathEffectCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        WinScreen.SetActive(true);

        eventSystem.SetSelectedGameObject(RematchButton);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.25f);
        Countdown.SetActive(true);
        CountdownText.text = "3...";
        yield return new WaitForSeconds(0.75f);
        CountdownText.text = "2...";
        yield return new WaitForSeconds(0.75f);
        CountdownText.text = "1...";
        yield return new WaitForSeconds(0.75f);
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
