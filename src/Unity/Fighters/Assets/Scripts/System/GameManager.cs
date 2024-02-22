using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using LaneLibrary;
using Cinemachine;

public class GameManager : SoundPlayer
{
    public GameObject fighterPrefab;

    public int round;

    public GameObject[] stages;

    public GameObject[] Walls;

    [Header("Player 1")]
    public Transform player1spawn;
    public FighterMain player1;
    public Healthbar player1Healthbar;
    public ComboUI player1ComboUI;
    public StocksDisplay player1StocksDisplay;
    public NotificationManager player1NotificationManager;
    public int player1lives;
    public int player1GameWins;

    [Header("Player 2")]
    public Transform player2spawn;
    public FighterMain player2;
    public Healthbar player2Healthbar;
    public ComboUI player2ComboUI;
    public StocksDisplay player2StocksDisplay;
    public NotificationManager player2NotificationManager;
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

    public AnnouncerSO announcerVoiceLines;

    public MusicPlayer musicPlayer;

    private bool gameActive;

    public CinemachineTargetGroup targetGroup;

    public enum RoundEndTypes
    {
        Normal,
        Perfect,
        DoubleKO
    }

    private void Start()
    {
        /*
        if (PlayerConfigurationManager.Instance == null)
        {
            SceneManager.LoadScene("PlayerSetup");
            return;
        }
        */

        if (GamePlayerManager.Instance == null)
        {
            SceneManager.LoadScene("PlayerSetupVersion2");
            return;
        }

        foreach (var s in stages)
        {
            s.SetActive(false);
        }
        LaneLibrary.RandomMethods.Choose(stages).SetActive(true);

        player1GameWins = 0;
        player2GameWins = 0;

        NewGame();

        PlayCharacterMusic();
    }

    private void PlayCharacterMusic()
    {
        AudioClip[] songs = new AudioClip[2];
        songs[0] = player1.characterModule.song;
        songs[1] = player2.characterModule.song;

        musicPlayer.PlayMusic(RandomMethods.Choose(songs));
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

        player1ComboUI.HideText();
        player2ComboUI.HideText();

        player1Healthbar.SetHealthbar(1,1);
        player2Healthbar.SetHealthbar(1,1);

        player1Healthbar.UpdateHearts(player1lives);
        player2Healthbar.UpdateHearts(player2lives);

        player1Healthbar.InitializeEvents(player1);
        player2Healthbar.InitializeEvents(player2);

        targetGroup.AddMember(player1.transform, 1, 1);
        targetGroup.AddMember(player2.transform, 1, 1);

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
            ui = player2ComboUI;
        }
        else
        {
            ui = player1ComboUI;
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
            ui = player2ComboUI;
        }
        else
        {
            ui = player1ComboUI;
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
        deadFighter.isDead = true;
        //deadFighter.SwitchState(deadFighter.knockdown);
        //var hs = (Knockdown)deadFighter.currentState;
        //hs.SetStun(50000);

        gameActive = false;

        deadFighter.timeManager.DoHitStop(1f);
    }

    private IEnumerator RoundEndCoroutine(RoundEndTypes endType)
    {
        yield return new WaitForSecondsRealtime(0.25f);

        PlaySound(RandomMethods.Choose(announcerVoiceLines.TrainWhistles));
        
        yield return new WaitForSecondsRealtime(0.25f);

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

        switch (endType)
        {
            case RoundEndTypes.DoubleKO:
                PlaySound(announcerVoiceLines.DoubleKO);
                break;
            case RoundEndTypes.Perfect:
                PlaySound(RandomMethods.Choose(announcerVoiceLines.Perfect));
                break;
            default:
                PlaySound(RandomMethods.Choose(announcerVoiceLines.RoundEnd));
                break;
        }

        Announcer.SetActive(false);

        yield return new WaitForSeconds(2.5f);

        if (player1lives <= 0 || player2lives <= 0)
        {
            // game is over

            Announcer.SetActive(true);
            if (player1lives <= 0)
            {
                AnnouncerText.text = $"{player2.characterModule.CharacterName} wins!!!";
                player2GameWins += 1;
            }
            else
            {
                AnnouncerText.text = $"{player1.characterModule.CharacterName} wins!!!";
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

    public void ReturnToCharacterSelect()
    {
        //PlayerConfigurationManager.Instance.BackToCharacterSelect();
        GamePlayerManager.Instance.BackToCharacterSelect();
    }



    public void RestartRound()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        round += 1;

        targetGroup.RemoveMember(player1.transform);
        targetGroup.RemoveMember(player2.transform);

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
        
        PlaySound(RandomMethods.Choose(announcerVoiceLines.RoundIntro));
        
        yield return new WaitForSeconds(2f);
        
        Announcer.SetActive(true);
        AnnouncerText.text = $"Round {round}-";
        
        yield return new WaitForSeconds(1f);
        
        Announcer.SetActive(false);
        
        yield return new WaitForSeconds(0.25f);
        
        PlaySound(RandomMethods.Choose(announcerVoiceLines.RoundStart));
        
        Announcer.SetActive(true);
        AnnouncerText.text = "All aboard!!!";
        
        yield return new WaitForSeconds(0.5f);
        
        Announcer.SetActive(false);

        player1.enabled = true;
        player2.enabled = true;
        gameActive = true;
    }

    public void StartGameButton()
    {
        StartScreen.SetActive(false);
        StartCoroutine(StartRound());
    }

    public void SpawnPlayers()
    {
        /*
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

                switch (configManager.gameMode)
                {
                    case GameMode.TwoPlayer:
                        fm.InitializePlayerInput(configManager.playerConfigs[i].Input);
                        break;
                    case GameMode.Training:
                        if (i == 0)
                        {
                            fm.InitializePlayerInput(configManager.playerConfigs[i].Input);
                        }
                        else
                        {
                            fm.InjectInputReceiver(new TrainingInputReceiver(fm, null, null));
                        }
                        break;
                    case GameMode.SinglePlayer:
                        // todo
                        break;
                    case GameMode.AIvsAI:
                        fm.InjectInputReceiver(new CpuInputReceiver(fm, null, null));
                        break;
                }

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
        */

        var configManager = GamePlayerManager.Instance;

        for (int i = 0; i < 2; i++)
        {
            if (configManager.gamePlayerConfigs[i] != null)
            {
                GamePlayerConfig config = configManager.gamePlayerConfigs[i];

                Transform t = i == 0 ? player1spawn : player2spawn;

                GameObject fighter = Instantiate(fighterPrefab);

                fighter.transform.position = t.position;

                var fm = fighter.GetComponent<FighterMain>();

                switch (config.playerType)
                {
                    case PlayerType.Human:
                        fm.InitializePlayerInput(config.humanPlayerConfig.Input);
                        break;
                    case PlayerType.CPU:
                        fm.InjectInputReceiver(new CpuInputReceiver(fm, null, null));
                        break;
                    case PlayerType.Training:
                        fm.InjectInputReceiver(new TrainingInputReceiver(fm, null, null));
                        break;
                }

                fm.characterModule = config.Character;
                fm.InitializeCharacterModule();

                fm.SetMaterial(config.Character.materials[config.CharacterMaterialIndex]);

                if (i == 0)
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
        player1Healthbar.SetNametag(player1.characterModule.CharacterName);
        player1Healthbar.SetMaterial(configManager.gamePlayerConfigs[0].Character.materials[configManager.gamePlayerConfigs[0].CharacterMaterialIndex]);
        player1StocksDisplay.InitializeStocksDisplay(player1);
        player1StocksDisplay.SetMaterial(configManager.gamePlayerConfigs[0].Character.materials[configManager.gamePlayerConfigs[0].CharacterMaterialIndex]);
        player1.PausePressed += (sender, e) => PauseGame();
        player1.Walls = Walls;
        player1NotificationManager.SetupNotificationManager(player1);

        player2.otherFighter = player1.gameObject;
        player2.otherFighterMain = player1;
        player2.AutoTurnaround();
        player2Healthbar.SetNametag(player2.characterModule.CharacterName);
        player2Healthbar.SetMaterial(configManager.gamePlayerConfigs[1].Character.materials[configManager.gamePlayerConfigs[1].CharacterMaterialIndex]);
        player2StocksDisplay.InitializeStocksDisplay(player2);
        player2StocksDisplay.SetMaterial(configManager.gamePlayerConfigs[1].Character.materials[configManager.gamePlayerConfigs[1].CharacterMaterialIndex]);
        player2.PausePressed += (sender, e) => PauseGame();
        player2.Walls = Walls;
        player2NotificationManager.SetupNotificationManager(player2);
    }

    public void PauseGame()
    {
        // pause not implemented, just go back to character select
        ReturnToCharacterSelect();
    }

}
