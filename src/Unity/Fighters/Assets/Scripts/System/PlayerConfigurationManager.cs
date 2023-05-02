using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;

    public int MaxPlayers = 2;

    public static PlayerConfigurationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {

        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
            playerConfigs = new List<PlayerConfiguration>();
        }
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        if(!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            pi.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfiguration(pi));
        }
    }

    public void SetCharacter(int index, CharacterModule character)
    {
        playerConfigs[index].Character = character;
    }

    public void SetReady(int index)
    {
        playerConfigs[index].IsReady = true;
        if (playerConfigs.All(p => p.IsReady == true))
        {
            SceneManager.LoadScene("Fight");
        }
    }
}

public class PlayerConfiguration
{
    public PlayerInput Input;

    public int PlayerIndex;

    public bool IsReady;

    public CharacterModule Character;

    public int CharacterMaterialIndex;

    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }
}