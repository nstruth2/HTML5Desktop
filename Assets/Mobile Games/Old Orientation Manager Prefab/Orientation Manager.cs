using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class OrientationWarning : MonoBehaviour
{
    public Text directionText;

    private string requiredOrientation = "";
    private string currentOrientation = "";

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void SetCurrentSceneJS(string sceneName);
#endif

    void Start()
    {
        Debug.Log("[OrientationOverlay] Initialized.");

#if !UNITY_WEBGL || UNITY_EDITOR
        SceneManager.sceneLoaded += OnSceneLoaded;
        // Also handle the current scene immediately
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
#endif

        if (directionText == null)
        {
            Debug.LogWarning("[OrientationOverlay] directionText is not assigned in the Inspector.");
        }
        else
        {
            directionText.gameObject.SetActive(false); // Hide initially
        }
    }

#if !UNITY_WEBGL || UNITY_EDITOR
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[OrientationOverlay] Scene Loaded: {scene.name}");

        // Call JS function to set the current scene
        SetCurrentScene(scene.name);
    }
#endif

    // ✅ Called from JS via SendMessage in WebGL builds
    public void ReceiveOrientationData(string json)
    {
        Debug.Log($"[OrientationOverlay] Received orientation data: {json}");

        OrientationData data = JsonUtility.FromJson<OrientationData>(json);

        if (data != null)
        {
            currentOrientation = data.current.ToLower();
            requiredOrientation = data.required.ToLower();
            CheckOrientation();
        }
        else
        {
            Debug.LogWarning("[OrientationOverlay] Failed to parse orientation JSON.");
        }
    }

    void CheckOrientation()
    {
        if (directionText == null)
        {
            Debug.LogWarning("[OrientationOverlay] directionText reference is missing.");
            return;
        }

        if (string.IsNullOrEmpty(requiredOrientation) || string.IsNullOrEmpty(currentOrientation))
        {
            Debug.LogWarning("[OrientationOverlay] Orientation values are missing.");
            return;
        }

        Debug.Log($"[OrientationOverlay] Current: {currentOrientation}, Required: {requiredOrientation}");

        if (currentOrientation != requiredOrientation)
        {
            directionText.text = $"Please rotate your device to {requiredOrientation}.";
            directionText.gameObject.SetActive(true);
        }
        else
        {
            directionText.gameObject.SetActive(false);
        }
    }

    public void HideWarning()
    {
        directionText?.gameObject.SetActive(false);
    }

    // ✅ One-way JS call from Unity
    public void SetCurrentScene(string sceneName)
    {
        Debug.Log($"[OrientationOverlay] SetCurrentScene called with: {sceneName}");

#if UNITY_WEBGL && !UNITY_EDITOR
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            SetCurrentSceneJS(sceneName); // Call into JS .jslib
        }
#endif
    }

    [System.Serializable]
    public class OrientationData
    {
        public string current;
        public string required;
        public string scene;
    }
}
