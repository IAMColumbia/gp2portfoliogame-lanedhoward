using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GameSettings
{
    private static GameSettings instance;
    public static GameSettings Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameSettings();
            }
            return instance;
        }
    }
    
    // sound settings handled in SoundOption.cs
    
    public bool CPUvCPUAutoRematch;
    public bool ShowTutorial;

    protected GameSettings()
    {
        LoadSettings();
    }

    public void LoadSettings()
    {
        CPUvCPUAutoRematch = PlayerPrefs.GetInt("CPUvCPUAutoRematch", 0) == 1;
        ShowTutorial = PlayerPrefs.GetInt("ShowTutorial", 1) == 1;
        Debug.Log("show tutorial: " + ShowTutorial);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("CPUvCPUAutoRematch", CPUvCPUAutoRematch ? 1 : 0);
        PlayerPrefs.SetInt("ShowTutorial", ShowTutorial ? 1 : 0);
    }
}
