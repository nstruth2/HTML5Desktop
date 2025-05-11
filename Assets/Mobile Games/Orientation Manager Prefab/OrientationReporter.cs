using UnityEngine;
using System.Runtime.InteropServices;

public class OrientationManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void InitializeOrientationMap(); // Calling JS function to initialize orientation map
    [DllImport("__Internal")]
    private static extern void PromptOrientationChange(string orientation); // Prompting JS to alert the user to change orientation

    private bool isOrientationMapInitialized = false;

    void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            // Initialize orientation map from C#
            InitializeOrientationMap(); // This triggers the JS function to initialize the map
            isOrientationMapInitialized = true;
        }
    }

    public string GetOrientation(string gameName, string sceneName)
    {
        if (isOrientationMapInitialized)
        {
            // Call the JS function that gets the required orientation
            return GetRequiredOrientation(gameName, sceneName);
        }
        else
        {
            Debug.LogWarning("Orientation map is not initialized.");
            return "portrait"; // Default if map is not initialized
        }
    }

    // This will check if the user needs to change orientation based on the scene and game
    public void CheckOrientationAndPrompt(string gameName, string sceneName)
    {
        string requiredOrientation = GetOrientation(gameName, sceneName);

        if (requiredOrientation != Screen.orientation.ToString().ToLower())
        {
            // Prompt JS to alert the user about switching orientation
            PromptOrientationChange(requiredOrientation);
        }
    }

    [DllImport("__Internal")]
    private static extern string GetRequiredOrientation(string gameName, string sceneName); // JS function to get orientation
}
