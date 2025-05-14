using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OrientationOverlayManager : MonoBehaviour
{
    public Text directionText;

    private string requiredOrientation = "";
    private string currentOrientation = "";

#if UNITY_WEBGL && !UNITY_EDITOR
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void SetCurrentScene(string sceneName);
#endif

    void Start()
    {
        Debug.Log("[OrientationOverlay] Initialized.");
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (directionText == null)
        {
            Debug.LogWarning("[OrientationOverlay] directionText is not assigned in the Inspector.");
        }
        else
        {
            directionText.gameObject.SetActive(false); // Hide it at start
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[OrientationOverlay] Scene Loaded: {scene.name}");

#if UNITY_WEBGL && !UNITY_EDITOR
        SetCurrentScene(scene.name);
#endif
    }

    // Called from JS via SendMessage
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
        if (directionText != null)
        {
            directionText.gameObject.SetActive(false);
        }
    }

    [System.Serializable]
    public class OrientationData
    {
        public string current;
        public string required;
        public string scene;
    }
}
