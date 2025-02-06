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
using System.Linq;

public enum StageChoice
{
    California,
    StateLake,
    Random
}

public class GameManager : SoundPlayer
{
    public GameObject fighterPrefab;

    public int round;

    public GameMode gameMode;

    public GameObject[] AllStages;
    public GameObject[] CaliforniaStages;
    public GameObject[] StateLakeStages;

    public GameObject[] Walls;

    public CharacterModule[] allCharacters;

    [Header("Player 1")]
    public Transform player1spawn;
    public FighterMain player1;
    public Healthbar player1Healthbar;
    public Healthbar player1Superbar;
    public ComboUI player1ComboUI;
    public StocksDisplay player1StocksDisplay;
    public NotificationManager player1NotificationManager;
    public int player1lives;
    public int player1GameWins;

    [Header("Player 2")]
    public Transform player2spawn;
    public FighterMain player2;
    public Healthbar player2Healthbar;
    public Healthbar player2Superbar;
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
    public PostGameUIPanels[] postGameUIs;
    public TrainingInfo trainingInfo;

    public GameObject Announcer;
    public TMPro.TextMeshProUGUI AnnouncerText;

    public EventSystem eventSystem;

    public PlayableDirector introTimeline;
    public PlayableDirector creditsTimeline;

    public AnnouncerSO announcerVoiceLines;

    public MusicPlayer musicPlayer;

    private bool gameActive;

    public CinemachineTargetGroup targetGroup;

    public StartButtonClicker pauseClicker;

    private event Action CycleBlockSetting;

    public enum RoundEndTypes
    {
        Normal,
        Perfect,
        DoubleKO
    }

    public override void Awake()
    {
        base.Awake();
        if (GamePlayerManager.Instance == null)
        {
            SceneManager.LoadScene("PlayerSetupVersion2");
            return;
        }
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
        


        // test
        //Application.targetFrameRate = 30;

        // any human controller can press pause, regardless of if they are controlling a fighter
        // so that we can pause during cpu battles
        foreach (HumanPlayerConfig human in GamePlayerManager.Instance.humanPlayerConfigs)
        {
            human.Input.actions["Pause"].performed += PausePressed;
        }

        // decide gamemode
        if (GamePlayerManager.Instance.gamePlayerConfigs.All(c => c.playerType == PlayerType.Human))
        {
            gameMode = GameMode.TwoPlayer;
        }
        else if (GamePlayerManager.Instance.gamePlayerConfigs.All(c => (c.playerType == PlayerType.CPU) 
        || (c.playerType == PlayerType.Training))) // if you have two training dummys or a dummy and a cpu, treat it as cpu v cpu
        {
            gameMode = GameMode.CPUvsCPU;
        }
        else if (GamePlayerManager.Instance.gamePlayerConfigs.Any(c => c.playerType == PlayerType.Training))
        {
            gameMode = GameMode.Training;
        }
        else
        {
            gameMode = GameMode.SinglePlayer;
        }

        player1GameWins = 0;
        player2GameWins = 0;

        SetupPostGameUI();

        NewGame();

    }

    private void SetupPostGameUI()
    {
        foreach (var pg in postGameUIs)
        {
            //pg.Hide();
            pg.UIActive = false;
        }
        if (gameMode == GameMode.CPUvsCPU)
        {
            if (GamePlayerManager.Instance.humanPlayerConfigs.Count > 0)
            {
                postGameUIs[0].Setup(GamePlayerManager.Instance.humanPlayerConfigs[0].Input);
                postGameUIs[0].UIActive = true;

                GamePlayerManager.Instance.humanPlayerConfigs[0].Input.onDeviceLost += Input_onDeviceLost;

                if (GamePlayerManager.Instance.humanPlayerConfigs.Count > 1)
                {
                    postGameUIs[1].Setup(GamePlayerManager.Instance.humanPlayerConfigs[1].Input);
                    postGameUIs[1].UIActive = true;

                    GamePlayerManager.Instance.humanPlayerConfigs[1].Input.onDeviceLost += Input_onDeviceLost;
                }
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                if (GamePlayerManager.Instance.gamePlayerConfigs[i].playerType == PlayerType.Human)
                {
                    postGameUIs[i].Setup(GamePlayerManager.Instance.gamePlayerConfigs[i].humanPlayerConfig.Input);
                    //postGameUIs[i].Show();
                    postGameUIs[i].UIActive = true;

                    GamePlayerManager.Instance.gamePlayerConfigs[i].humanPlayerConfig.Input.onDeviceLost += Input_onDeviceLost;
                }
            }
        }
    }

    private void Input_onDeviceLost(PlayerInput obj)
    {
        if (gameActive == false && postGameUIs.Any(ui => ui.panel.activeInHierarchy))
        {
            StartCoroutine(DelayReturnToMenu());
        }
    }

    public IEnumerator DelayReturnToMenu()
    {
        yield return new WaitForSeconds(0.1f);
        ReturnToCharacterSelect();
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
        if (creditsTimeline.state == PlayState.Playing)
        {
            creditsTimeline.Stop();
        }
        creditsTimeline.gameObject.SetActive(false);

        SetStage();

        trainingInfo.gameObject.SetActive(gameMode == GameMode.Training && GameSettings.Instance.ShowTrainingInfo);

        round = 1;

        player1lives = 2;
        player2lives = 2;

        // handle random select
        foreach (GamePlayerConfig gamePlayer in GamePlayerManager.Instance.gamePlayerConfigs)
        {
            if (gamePlayer.IsRandomSelect == true)
            {
                gamePlayer.Character = LaneLibrary.RandomMethods.Choose(allCharacters);
                gamePlayer.CharacterMaterialIndex = RandomMethods.RANDOM.Next(gamePlayer.Character.materials.Length);
            }
        }

        SetupRound();

        PlayCharacterMusic();
    }

    public void SetupRound()
    {
        SpawnPlayers();

        gameActive = false;

        player1.enabled = false;
        player2.enabled = false;

        player1.GotHit += Player_GotHit;
        player2.GotHit += Player_GotHit;

        player1.Blocked += Player_Blocked;
        player2.Blocked += Player_Blocked;

        player1.LeftHitstun += Player_LeftHitstun;
        player2.LeftHitstun += Player_LeftHitstun;

        player1.LeftBlockstun += Player_LeftBlockstun;
        player2.LeftBlockstun += Player_LeftBlockstun;

        player1ComboUI.HideText();
        player2ComboUI.HideText();

        player1Healthbar.SetHealthbar(1,1);
        player2Healthbar.SetHealthbar(1,1);

        player1Healthbar.UpdateHearts(player1lives);
        player2Healthbar.UpdateHearts(player2lives);

        player1Healthbar.burst.SetActive(true);
        player2Healthbar.burst.SetActive(true);

        player1Healthbar.InitializeEvents(player1);
        player2Healthbar.InitializeEvents(player2);

        player1Superbar.SetHealthbar(player1.CurrentMeter, player1.MaxMeter, false);
        player2Superbar.SetHealthbar(player2.CurrentMeter, player2.MaxMeter, false);

        targetGroup.AddMember(player1.transform, 1, 1);
        targetGroup.AddMember(player2.transform, 1, 1);

        

        //StartScreen.SetActive(true);
        Announcer.SetActive(false);
        //winScreen.gameObject.SetActive(false);
        winScreen.HideWinScreen();

        //eventSystem.SetSelectedGameObject(StartButton);
        StartCoroutine(StartRound());
    }

    private void Player_LeftBlockstun(object sender, EventArgs e)
    {
        if (gameMode != GameMode.Training) return;

        FighterMain s = (FighterMain)sender;
        if (s == null) throw new Exception("Got leftblockstun event from null sender");

        Healthbar h;
        if (s == player1)
        {
            h = player1Healthbar;
        }
        else
        {
            h = player2Healthbar;

        }
        
        s.CurrentHealth = s.MaxHealth;
        h.SetHealthbar(1, 1, true);
        player1.CurrentMeter = player1.MaxMeter;
        player2.CurrentMeter = player2.MaxMeter;

    }

    private void Player_Blocked(object sender, EventArgs e)
    {
        if (gameMode != GameMode.Training) return;

        FighterMain s = (FighterMain)sender;
        if (s == null) throw new Exception("Got blocked event from null sender");

        Combo combo;
        combo = s.currentCombo;

        trainingInfo.SetInfo(combo);
    }

    private void Player_LeftHitstun(object sender, EventArgs e)
    {
        FighterMain s = (FighterMain)sender;
        if (s == null) throw new Exception("Got lefthitstun event from null sender");

        ComboUI ui;
        Healthbar h;
        if (s == player1)
        {
            ui = player2ComboUI;
            h = player1Healthbar;
        }
        else
        {
            ui = player1ComboUI;
            h = player2Healthbar;

        }

        ui.StartEndComboCount();

        if (gameMode == GameMode.Training)
        {
            s.CurrentHealth = s.MaxHealth;
            h.SetHealthbar(1, 1, true);

            player1.CurrentMeter = player1.MaxMeter;
            player2.CurrentMeter = player2.MaxMeter;
        }
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

        if (gameMode == GameMode.Training)
        {
            trainingInfo.SetInfo(combo);
        }
    }

    

    private void Update()
    {
        if (player1 != null)
        {
            player1Healthbar.SetHealthbar(player1.CurrentHealth, player1.MaxHealth);
            player1Superbar.SetHealthbar(player1.CurrentMeter, player1.MaxMeter);
        }

        if (player2 != null)
        {
            player2Healthbar.SetHealthbar(player2.CurrentHealth, player2.MaxHealth);
            player2Superbar.SetHealthbar(player2.CurrentMeter, player2.MaxMeter);
        }

        if (gameActive)
        {

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

            //winScreen.gameObject.SetActive(true);
            winScreen.UpdateRecord(player1GameWins, player2GameWins);
            winScreen.ShowWinScreen();

            //eventSystem.SetSelectedGameObject(RematchButton);
            

            if (gameMode == GameMode.CPUvsCPU && GameSettings.Instance.CPUvCPUAutoRematch)
            {
                yield return new WaitForSeconds(2.5f);
                Rematch();
            }

            // credits easter egg
            yield return new WaitForSeconds(10f);
            creditsTimeline.gameObject.SetActive(true);
            creditsTimeline.Play();

        }
        else
        {
            RestartRound();
        }

    }

    public void Rematch()
    {
        StopAllCoroutines();

        Destroy(player1.gameObject);
        Destroy(player2.gameObject);

        NewGame();
    }

    public void ReturnToCharacterSelect()
    {
        // remove references
        foreach (HumanPlayerConfig human in GamePlayerManager.Instance.humanPlayerConfigs)
        {
            human.Input.actions["Pause"].performed -= PausePressed;
        }

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

        yield return new WaitForSeconds(.25f);

        PlaySound(RandomMethods.Choose(announcerVoiceLines.RoundIntro));
        
        yield return new WaitForSeconds(1.75f);
        
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
                        fm.InjectInputReceiver(new HardCpuInputReceiver(fm, null, null));
                        break;
                    case PlayerType.Training:
                        TrainingInputReceiver ti = new TrainingInputReceiver(fm, null, null);

                        CycleBlockSetting += ti.ChangeBlockSetting;

                        fm.InjectInputReceiver(ti);

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
        player1Healthbar.SetCharacterModule(player1.characterModule);
        player1Healthbar.SetMaterial(configManager.gamePlayerConfigs[0].Character.materials[configManager.gamePlayerConfigs[0].CharacterMaterialIndex]);
        player1StocksDisplay.InitializeStocksDisplay(player1);
        player1StocksDisplay.SetMaterial(configManager.gamePlayerConfigs[0].Character.materials[configManager.gamePlayerConfigs[0].CharacterMaterialIndex]);
        player1.Walls = Walls;
        player1NotificationManager.SetupNotificationManager(player1);

        player2.otherFighter = player1.gameObject;
        player2.otherFighterMain = player1;
        player2.AutoTurnaround();
        player2Healthbar.SetCharacterModule(player2.characterModule);
        player2Healthbar.SetMaterial(configManager.gamePlayerConfigs[1].Character.materials[configManager.gamePlayerConfigs[1].CharacterMaterialIndex]);
        player2StocksDisplay.InitializeStocksDisplay(player2);
        player2StocksDisplay.SetMaterial(configManager.gamePlayerConfigs[1].Character.materials[configManager.gamePlayerConfigs[1].CharacterMaterialIndex]);
        player2.Walls = Walls;
        player2NotificationManager.SetupNotificationManager(player2);
    }

    private void PausePressed(InputAction.CallbackContext obj)
    {
        if (gameMode == GameMode.Training)
        {
            FighterMain humanFighter;
            if (GamePlayerManager.Instance.gamePlayerConfigs[0].playerType == PlayerType.Human)
            {
                humanFighter = player1;
            }
            else
            {
                humanFighter = player2;
            }

            if (humanFighter.inputReceiver.LeftRight == 0 && humanFighter.inputReceiver.UpDown == -1)
            {
                // cycle block settings
                CycleBlockSetting?.Invoke();
                return;
            }

        }
        PauseGame();
    }

    public void PauseGame()
    {
        // pause not implemented, just go back to character select
        if (pauseClicker.PressStart())
        {
            ReturnToCharacterSelect();
        }
    }

    private void OnEnable()
    {
        PostGameUIPanels.MenuClicked += PostGameUIPanels_MenuClicked;
        PostGameUIPanels.RematchClicked += PostGameUIPanels_RematchClicked;
    }

    private void OnDisable()
    {
        PostGameUIPanels.MenuClicked -= PostGameUIPanels_MenuClicked;
        PostGameUIPanels.RematchClicked -= PostGameUIPanels_RematchClicked;
    }

    private void PostGameUIPanels_RematchClicked(object sender, EventArgs e)
    {
        if (postGameUIs.Where(p => p.UIActive).All(p => p.rematchClicked))
        {
            Rematch();
        }
    }

    private void PostGameUIPanels_MenuClicked(object sender, EventArgs e)
    {
        // if anyone clicks menu, immediately return to menu
        ReturnToCharacterSelect();
    }

    private void SetStage()
    {
        foreach (var s in AllStages)
        {
            s.SetActive(false);
        }

        switch (GamePlayerManager.Instance.stageChoice)
        {
            case StageChoice.California:
                LaneLibrary.RandomMethods.Choose(CaliforniaStages).SetActive(true);
                break;
            case StageChoice.StateLake:
                LaneLibrary.RandomMethods.Choose(StateLakeStages).SetActive(true);
                break;
            default:
            case StageChoice.Random:
                LaneLibrary.RandomMethods.Choose(AllStages).SetActive(true);
                break;
        }

    }
}
