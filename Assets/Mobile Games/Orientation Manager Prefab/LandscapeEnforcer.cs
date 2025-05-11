using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class LandscapeEnforcer : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void EnforceLandscapeMode();

    [DllImport("__Internal")]
    private static extern void ResetOrientationOverride();
#endif

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        bool isLandscapeScene = sceneName == "Submit Score and Name Game 1 Landscape"
                             || sceneName == "Submit Score and Name Game 2 Landscape"
                             || sceneName.StartsWith("Landscape Game");

#if UNITY_WEBGL && !UNITY_EDITOR
        if (isLandscapeScene)
        {
            EnforceLandscapeMode();
        }
        else
        {
            ResetOrientationOverride();
        }
#endif
    }
}
