using JetBrains.Annotations;
using JimmysUnityUtilities;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class CheckForUpdates : MonoBehaviour
{
    string url = "https://itch.io/api/1/x/wharf/latest?target=squigglez/traintracks-combat&channel_name=windows";

    public string InitialText = "Checking for updates... ";
    public string UpdateFoundText = "Update found! Download it from itch.io";
    public string NoUpdateFoundText = "You are on the latest version!";
    public string ErrorText = "Error checking for updates.";

    public TextMeshProUGUI UpdateText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetVersion());
    }

    IEnumerator GetVersion()
    {
        UpdateText.text = InitialText;
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
            VersionResponse v = JsonConvert.DeserializeObject<VersionResponse>(www.downloadHandler.text);
            
            if (v == null)
            {
                Error();
                yield break;
            }

            if (v.errors != null)
            {
                if (v.errors.Length > 0)
                {
                    Error();
                    yield break;
                }
            }

            int versionNumber;

            if (int.TryParse(PlayerSettings.bundleVersion, out versionNumber))
            {
                if (v.latest > versionNumber)
                {
                    UpdateFound();
                    yield break;
                }
                else
                {
                    NoUpdateFound();
                    yield break;
                }
            }
            else
            {
                Error();
                yield break;
            }

        }
    }

    public void UpdateFound()
    {
        UpdateText.text = InitialText + UpdateFoundText;
        FadeOutText();
    }

    public void NoUpdateFound()
    {
        UpdateText.text = InitialText + NoUpdateFoundText;
        FadeOutText();
    }

    public void Error()
    {
        UpdateText.text = InitialText + ErrorText;
        FadeOutText();
    }

    public void FadeOutText()
    {
        UpdateText.CrossFadeColor(new Color(1, 1, 1, 0), 10f, true, true);
    }

    public class VersionResponse
    {
        public int latest;
        public string[] errors;
    }
}
