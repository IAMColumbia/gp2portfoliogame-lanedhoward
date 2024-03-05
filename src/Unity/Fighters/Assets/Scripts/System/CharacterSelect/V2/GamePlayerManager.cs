using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GamePlayerManager : MonoBehaviour
{
    public List<HumanPlayerConfig> humanPlayerConfigs;
    public List<GamePlayerConfig> gamePlayerConfigs;

    public int MaxPlayers = 2;

    /// <summary>
    /// Game Mode doesn't really matter for setup, so we can parse that when it is time to start
    /// maybe GamePlayerManager doesn't even need that, maybe its just GameManager that can parse and have that
    /// </summary>
    public GameMode gameMode;

    public PlayerInputManager playerInputManager;

    public static GamePlayerManager Instance { get; private set; }

    public static event EventHandler HumanPlayerJoined;
    /// <summary>
    /// Event for the start of player swap process
    /// </summary>
    public static event EventHandler PlayersAboutToSwap;
    /// <summary>
    /// Event for the actual player swap process
    /// </summary>
    public static event EventHandler PlayersSwapped;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
            playerInputManager = GetComponent<PlayerInputManager>();
            ControlsPanel.ControlsOpened += ControlsPanel_ControlsOpened;
            ControlsPanel.ControlsClosed += ControlsPanel_ControlsClosed;
            MovesListPanel.MovesListOpened += MovesListPanel_MovesListOpened;
            MovesListPanel.MovesListClosed += MovesListPanel_MovesListClosed;
            Initialize();
        }
    }

    

    public void Initialize()
    {
        // no humans until some controller joins
        humanPlayerConfigs = new List<HumanPlayerConfig>();

        // two players fighting
        gamePlayerConfigs = new List<GamePlayerConfig>();
        gamePlayerConfigs.Add(new GamePlayerConfig());
        gamePlayerConfigs.Add(new GamePlayerConfig());
        
    }

    /// <summary>
    /// Called by unity event from PlayerInputManager
    /// Human Player Joins, save their playerinput in DDOL and add them to HumanPlayerConfig
    /// </summary>
    /// <param name="pi"></param>
    public void HandlePlayerJoin(PlayerInput pi)
    {
        if (!humanPlayerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            // make PI DDOL
            pi.transform.SetParent(transform);

            // add human config
            HumanPlayerConfig newHuman = new HumanPlayerConfig(pi);

            humanPlayerConfigs.Add(newHuman);
            HumanPlayerJoined?.Invoke(this, EventArgs.Empty);

            // give human config to a gameplayerconfig
            if (gamePlayerConfigs[0].humanPlayerConfig == null)
            {
                gamePlayerConfigs[0].humanPlayerConfig = newHuman;
            }
            else
            {
                gamePlayerConfigs[1].humanPlayerConfig = newHuman;
            }
        }
    }

    public int GetGamePlayerIndex(HumanPlayerConfig human)
    {
        if (gamePlayerConfigs[0].humanPlayerConfig == human)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    /// <summary>
    /// called after HandlePlayerJoin
    /// </summary>
    /// <param name="cursor"></param>
    public void SetUpCursor(Cursor cursor)
    {
        cursor.humanPlayerConfig = humanPlayerConfigs[cursor.playerInput.playerIndex];
        Debug.Log($"Cursor {cursor.playerInput.playerIndex} now has human {cursor.humanPlayerConfig.PlayerIndex}");
    }



    public void StartGame()
    {
        if (gamePlayerConfigs.All(p => p.IsReady == true))
        {
            SceneManager.LoadScene("Fight");
        }
    }

    public void BackToCharacterSelect()
    {
        humanPlayerConfigs.Clear();
        gamePlayerConfigs.Clear();
        foreach (PlayerInput pi in GetComponentsInChildren<PlayerInput>())
        {
            Destroy(pi.gameObject);
        }
        Initialize();
        SceneManager.LoadScene("PlayerSetupVersion2");
    }



    private void ControlsPanel_ControlsOpened(object sender, int e)
    {
        if (humanPlayerConfigs.Count >= 2)
        {
            foreach (var config in humanPlayerConfigs)
            {
                if (config.PlayerIndex != e)
                {
                    InputSystem.DisableDevice(config.Input.devices[0]);
                }
            }
        }

        playerInputManager.DisableJoining();
    }
    private void ControlsPanel_ControlsClosed(object sender, int e)
    {
        if (humanPlayerConfigs.Count >= 2)
        {
            foreach (var config in humanPlayerConfigs)
            {
                if (config.PlayerIndex != e)
                {
                    InputSystem.EnableDevice(config.Input.devices[0]);
                }
            }
        }

        playerInputManager.EnableJoining();
    }

    private void MovesListPanel_MovesListOpened(object sender, EventArgs e)
    {
        playerInputManager.DisableJoining();
    }

    private void MovesListPanel_MovesListClosed(object sender, EventArgs e)
    {
        playerInputManager.EnableJoining();
    }

    public void SwapPlayers()
    {
        PlayersAboutToSwap?.Invoke(this, EventArgs.Empty);

        //swap the humans between the gameplayerconfigs
        // (could be null)
        var human1 = gamePlayerConfigs[0].humanPlayerConfig;

        gamePlayerConfigs[0].humanPlayerConfig = gamePlayerConfigs[1].humanPlayerConfig;
        gamePlayerConfigs[1].humanPlayerConfig = human1;

        PlayersSwapped?.Invoke(this, EventArgs.Empty);
        

    }
}

/// <summary>
/// Config of a real human player -
/// Houses their PlayerInput, device, and controls settings.
/// </summary>
public class HumanPlayerConfig
{
    public PlayerInput Input;

    /// <summary>
    /// PlayerInput index, not necessarily game player index
    /// </summary>
    public int PlayerIndex;

    public HumanPlayerConfig(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }
}

public enum PlayerType
{
    Human,
    CPU,
    Training
}

/// <summary>
/// Config of a game player slot (player 1, player 2)
/// Can be human, cpu, or training dummy.
/// Holds character information etc.
/// </summary>
public class GamePlayerConfig
{
    public bool IsReady 
    {
        get
        {
            return Character != null;
        }
    }

    public CharacterModule Character;

    public int CharacterMaterialIndex;

    public bool IsRandomSelect = false;

    public int GameWins;

    public PlayerType playerType;

    public HumanPlayerConfig humanPlayerConfig;

    public GamePlayerConfig()
    {
    }
}