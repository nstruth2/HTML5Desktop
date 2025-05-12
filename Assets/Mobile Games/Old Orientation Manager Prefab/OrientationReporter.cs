using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class OrientationManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SetCurrentScene(string sceneName); // Calling JS function to set the current scene

    void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            // Call JS to set the initial scene
            SetCurrentScene(SceneManager.GetActiveScene().name);
        }

        // Listen for scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Set the current scene whenever a new scene is loaded
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            SetCurrentScene(scene.name);
        }

        // Check and prompt orientation change after the scene is loaded
        CheckOrientationAndPrompt(scene.name);
    }

    public void CheckOrientationAndPrompt(string sceneName)
    {
    #if UNITY_WEBGL && !UNITY_EDITOR
        string requiredOrientation = GetRequiredOrientation(Application.productName, sceneName);
        if (!string.IsNullOrEmpty(requiredOrientation) && 
            requiredOrientation != Screen.orientation.ToString().ToLower())
        {
            PromptOrientationChange(requiredOrientation);
        }
    #endif
    }

    [DllImport("__Internal")]
    private static extern void PromptOrientationChange(string orientation); // Prompt JS to alert the user to change orientation

    // This method is used to get the orientation requirement from JS (already defined in your code)
    [DllImport("__Internal")]
    private static extern string GetRequiredOrientation(string gameName, string sceneName);
}
