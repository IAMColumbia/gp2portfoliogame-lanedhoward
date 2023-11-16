using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Build.Reporting;
using UnityEditor.Build;
using UnityEditor;
using UnityEngine;

public class UpdateVersionNumber : MonoBehaviour
#if UNITY_EDITOR
    , IPreprocessBuildWithReport
#endif
{

#if UNITY_EDITOR
    const string textPath = "C:\\Users\\ldcac\\Desktop\\indie fighters\\LatestVersionNumber.txt";
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        Debug.Log("Updating version number... ", this);

        try
        {
            Debug.Log("Previous version: " + PlayerSettings.bundleVersion);

            int versionNumber = int.Parse(PlayerSettings.bundleVersion);

            versionNumber++;

            PlayerSettings.bundleVersion = versionNumber.ToString();

            System.IO.File.WriteAllText(textPath, PlayerSettings.bundleVersion);

            Debug.Log("New version: " + PlayerSettings.bundleVersion);
        }
        catch (System.Exception e)
        {
            Debug.Log($"Could not update version number: {e.Message}");
        }
    }
#endif
}
