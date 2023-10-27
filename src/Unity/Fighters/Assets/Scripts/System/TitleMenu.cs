using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    public GameObject titles;

    public GameObject player2JoinPrefab;

    private GameObject p2JoinInstance;

    private void OnEnable()
    {
        PlayerConfigurationManager.PlayerJoined += OnPlayerJoined;
    }
    private void OnDisable()
    {
        PlayerConfigurationManager.PlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(object sender, System.EventArgs e)
    {
        StartCoroutine(PlayerConfigurationManager_PlayerJoined());
    }


    private IEnumerator PlayerConfigurationManager_PlayerJoined()
    {
        if (titles.activeInHierarchy)
        {
            titles.SetActive(false);
        }

        int playerCount = PlayerConfigurationManager.Instance.playerConfigs.Count;

        if (playerCount == 1)
        {
            yield return new WaitForEndOfFrame();
            p2JoinInstance = Instantiate(player2JoinPrefab, transform);
            p2JoinInstance.transform.SetAsLastSibling();
        }
        else
        {
            if (p2JoinInstance != null)
            {
                Destroy(p2JoinInstance);
            }
        }
    }

}
