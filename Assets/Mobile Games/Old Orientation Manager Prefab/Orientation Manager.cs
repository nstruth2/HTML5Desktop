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

        if (directionText == null)
        {
            Debug.LogWarning("[OrientationOverlay] directionText is not assigned in the Inspector.");
        }
        else
        {
            directionText.gameObject.SetActive(false); // Hide initially
        }

        // Register scene loaded event (Editor or WebGL)
        SceneManager.sceneLoaded += OnSceneLoaded;
        // Call immediately for the first scene
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[OrientationOverlay] Scene Loaded: {scene.name}");
        SetCurrentScene(scene.name);
    }

    // ✅ Called from JS via SendMessage
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

    public void SetCurrentScene(string sceneName)
    {
        Debug.Log($"[OrientationOverlay] SetCurrentScene called with: {sceneName}");

#if UNITY_WEBGL && !UNITY_EDITOR
        SetCurrentSceneJS(sceneName);
#else
        Debug.Log("[OrientationOverlay] Skipping JS call in non-WebGL build.");
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
