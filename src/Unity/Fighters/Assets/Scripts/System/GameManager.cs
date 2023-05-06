using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject fighterPrefab;

    [Header("Player 1")]
    public Transform player1spawn;
    public FighterMain player1;
    public Healthbar player1Healthbar;
    public ComboUI player1comboUI;

    [Header("Player 2")]
    public Transform player2spawn;
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

    public PlayableDirector introTimeline;

    private bool gameActive;

    private void Start()
    {
        
        if (PlayerConfigurationManager.Instance == null)
        {
            SceneManager.LoadScene("PlayerSetup");
            return;
        }
        

        SpawnPlayers();

        

        gameActive = false;

        player1.enabled = false;
        player2.enabled = false;

        player1.GotHit += Player_GotHit;
        player2.GotHit += Player_GotHit;

        player1.LeftHitstun += Player_LeftHitstun;
        player2.LeftHitstun += Player_LeftHitstun;

        player1comboUI.HideText();
        player2comboUI.HideText();

        //StartScreen.SetActive(true);
        Countdown.SetActive(false);
        WinScreen.SetActive(false);

        //eventSystem.SetSelectedGameObject(StartButton);
        StartCoroutine(StartGame());
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
        if (introTimeline != null)
        {
            introTimeline.Play();
        }
        yield return new WaitForSeconds(3f);
        Countdown.SetActive(true);
        CountdownText.text = "All aboard!!!";
        yield return new WaitForSeconds(1f);
        CountdownText.text = "Fight!";
        player1.enabled = true;
        player2.enabled = true;
        gameActive = true;
        yield return new WaitForSeconds(0.75f);
        Countdown.SetActive(false);
    }

    public void StartGameButton()
    {
        StartScreen.SetActive(false);
        StartCoroutine(StartGame());
    }

    public void SpawnPlayers()
    {
        var configManager = PlayerConfigurationManager.Instance;

        for (int i = 0; i < 2; i++)
        {
            if (configManager.playerConfigs[i] != null)
            {

                Transform t = i == 0 ? player1spawn : player2spawn;

                GameObject fighter = Instantiate(fighterPrefab);

                fighter.transform.position = t.position;

                //Debug.Log($"Player created: Player {configManager.playerConfigs[i].PlayerIndex} with {configManager.playerConfigs[i].Input.devices[0].name}");

                var fm = fighter.GetComponent<FighterMain>();

                fm.InitializePlayerInput(configManager.playerConfigs[i].Input);

                fm.characterModule = configManager.playerConfigs[i].Character;
                fm.InitializeCharacterModule();

                fm.SetMaterial(configManager.playerConfigs[i].Character.materials[configManager.playerConfigs[i].CharacterMaterialIndex]);

                if (i==0)
                {
                    player1 = fm;
                }
                else
                {
                    player2 = fm;
                }
            }
        }

        player1.otherFighter = player2.gameObject;
        player1.otherFighterMain = player2;
        player1.AutoTurnaround();
        player1Healthbar.SetNametag(player1.characterModule.Name);
        player1Healthbar.SetMaterial(configManager.playerConfigs[0].Character.materials[configManager.playerConfigs[0].CharacterMaterialIndex]);

        player2.otherFighter = player1.gameObject;
        player2.otherFighterMain = player1;
        player2.AutoTurnaround();
        player2Healthbar.SetNametag(player2.characterModule.Name);
        player2Healthbar.SetMaterial(configManager.playerConfigs[1].Character.materials[configManager.playerConfigs[1].CharacterMaterialIndex]);

    }
}
