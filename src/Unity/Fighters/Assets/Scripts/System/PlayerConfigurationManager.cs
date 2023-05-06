using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class PlayerConfigurationManager : MonoBehaviour
{
    public List<PlayerConfiguration> playerConfigs;

    public int MaxPlayers = 2;

    public GameObject configPrefab;
    public CharacterModule defaultCharacter;

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
            //Debug.Log($"Player joined: Player {pi.playerIndex} with {pi.devices[0].name}");
            playerConfigs.Add(new PlayerConfiguration(pi));
        }
    }

    public void SetCharacter(int index, CharacterModule character)
    {
        playerConfigs[index].Character = character;
    }

    public void SetReady(int index)
    {
        if (playerConfigs[index].Character == null) return;

        playerConfigs[index].IsReady = true;
        if (playerConfigs.Count >= MaxPlayers && playerConfigs.All(p => p.IsReady == true))
        {
            SceneManager.LoadScene("Fight");
        }
    }

    public void SetMaterialIndex(int index, int modifier)
    {
        if (playerConfigs[index].Character == null) return;

        int currentColorIndex = playerConfigs[index].CharacterMaterialIndex;
        int maxColorIndex = playerConfigs[index].Character.materials.Length-1;

        int newIndex = currentColorIndex + modifier;
        if (newIndex < 0)
        {
            newIndex = maxColorIndex;
        }
        if (newIndex > maxColorIndex)
        {
            newIndex = 0;
        }

        playerConfigs[index].CharacterMaterialIndex = newIndex;
    }

    public Material GetMaterial(int index)
    {
        if (playerConfigs[index].Character == null) return null;

        return playerConfigs[index].Character.materials[playerConfigs[index].CharacterMaterialIndex];
    }

    public void ForceStart()
    {
        if (playerConfigs.Count < MaxPlayers)
        {
            var pi = PlayerInput.Instantiate(configPrefab);
            HandlePlayerJoin(pi);
            SetCharacter(1, defaultCharacter);
            SetReady(1);
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
        CharacterMaterialIndex = PlayerIndex;
    }
}