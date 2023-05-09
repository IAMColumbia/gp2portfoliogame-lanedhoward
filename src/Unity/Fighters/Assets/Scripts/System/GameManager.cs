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

    public int round;

    [Header("Player 1")]
    public Transform player1spawn;
    public FighterMain player1;
    public Healthbar player1Healthbar;
    public ComboUI player1comboUI;
    public int player1lives;
    public int player1GameWins;

    [Header("Player 2")]
    public Transform player2spawn;
    public FighterMain player2;
    public Healthbar player2Healthbar;
    public ComboUI player2comboUI;
    public int player2lives;
    public int player2GameWins;

    [Header("UI")]
    public WinScreen winScreen;
    public GameObject RematchButton;
    public GameObject StartScreen;
    public GameObject StartButton;

    public GameObject Announcer;
    public TMPro.TextMeshProUGUI AnnouncerText;

    public EventSystem eventSystem;

    public PlayableDirector introTimeline;

    private bool gameActive;

    public enum RoundEndTypes
    {
        Normal,
        Perfect,
        DoubleKO
    }

    private void Start()
    {
        
        if (PlayerConfigurationManager.Instance == null)
        {
            SceneManager.LoadScene("PlayerSetup");
            return;
        }

        player1GameWins = 0;
        player2GameWins = 0;

        NewGame();
    }

    public void NewGame()
    {
        round = 1;

        player1lives = 2;
        player2lives = 2;


        SetupRound();
    }

    public void SetupRound()
    {
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

        player1Healthbar.SetHealthbar(1,1);
        player2Healthbar.SetHealthbar(1,1);

        player1Healthbar.UpdateHearts(player1lives);
        player2Healthbar.UpdateHearts(player2lives);

        //StartScreen.SetActive(true);
        Announcer.SetActive(false);
        winScreen.gameObject.SetActive(false);

        //eventSystem.SetSelectedGameObject(StartButton);
        StartCoroutine(StartRound());
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

            bool p1died = false;
            bool p2died = false;
            bool perfect = false;
            if (player1.CurrentHealth <= 0)
            {
                DeathEffect(player1);
                p1died = true;
                if (player2.CurrentHealth == player2.MaxHealth)
                {
                    perfect = true;
                }
            }

            if (player2.CurrentHealth <= 0)
            {
                DeathEffect(player2);
                p2died = true;
                if (player1.CurrentHealth == player1.MaxHealth)
                {
                    perfect = true;
                }
            }

            if (p1died || p2died)
            {
                RoundEndTypes roundEnd = HandleRoundEnd(p1died, p2died, perfect);
                // round is over
                player1Healthbar.UpdateHearts(player1lives);
                player2Healthbar.UpdateHearts(player2lives);


                StartCoroutine(RoundEndCoroutine(roundEnd));
            }
        }
    }

    protected RoundEndTypes HandleRoundEnd(bool p1died, bool p2died, bool perfect)
    {
        if (p1died && p2died)
        {
            // double ko
            player1lives = 1;
            player2lives = 1;
            return RoundEndTypes.DoubleKO;
        }

        if (p1died)
        {
            player1lives -= 1;
            if (perfect) return RoundEndTypes.Perfect;
            return RoundEndTypes.Normal;
        }
        
        player2lives -= 1;
        if (perfect) return RoundEndTypes.Perfect;
        return RoundEndTypes.Normal;
        
    }

    private void DeathEffect(FighterMain deadFighter)
    {
        deadFighter.SwitchState(deadFighter.knockdown);
        var hs = (Knockdown)deadFighter.currentState;
        hs.SetStun(50000);

        gameActive = false;

        deadFighter.timeManager.DoHitStop(1f);
    }

    private IEnumerator RoundEndCoroutine(RoundEndTypes endType)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Announcer.SetActive(true);
        switch (endType)
        {
            case RoundEndTypes.DoubleKO:
                AnnouncerText.text = $"DOUBLE K.O.!!";
                break;
            case RoundEndTypes.Perfect:
                AnnouncerText.text = $"PERFECT!!";
                break;
            default:
                AnnouncerText.text = $"END OF THE LINE!";
                break;
        }
        
        yield return new WaitForSecondsRealtime(0.5f);
        yield return new WaitForSeconds(1.5f);
        Announcer.SetActive(false);

        yield return new WaitForSeconds(2.5f);

        if (player1lives <= 0 || player2lives <= 0)
        {
            // game is over

            Announcer.SetActive(true);
            if (player1lives <= 0)
            {
                AnnouncerText.text = $"{player2.characterModule.Name} wins!!!";
                player2GameWins += 1;
            }
            else
            {
                AnnouncerText.text = $"{player1.characterModule.Name} wins!!!";
                player1GameWins += 1;
            }

            winScreen.gameObject.SetActive(true);
            winScreen.UpdateRecord(player1GameWins, player2GameWins);

            eventSystem.SetSelectedGameObject(RematchButton);
        }
        else
        {
            RestartRound();
        }

    }

    public void Rematch()
    {
        Destroy(player1.gameObject);
        Destroy(player2.gameObject);

        NewGame();
    }



    public void RestartRound()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        round += 1;

        Destroy(player1.gameObject);
        Destroy(player2.gameObject);

        SetupRound();

    }

    private IEnumerator StartRound()
    {
        if (introTimeline != null)
        {
            introTimeline.Play();
        }
        yield return new WaitForSeconds(2f);
        Announcer.SetActive(true);
        AnnouncerText.text = $"Round {round}-";
        yield return new WaitForSeconds(1f);
        Announcer.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        Announcer.SetActive(true);
        AnnouncerText.text = "All aboard!!!";
        player1.enabled = true;
        player2.enabled = true;
        gameActive = true;
        yield return new WaitForSeconds(1f);
        Announcer.SetActive(false);
    }

    public void StartGameButton()
    {
        StartScreen.SetActive(false);
        StartCoroutine(StartRound());
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
