using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class OrientationOverlayManager : MonoBehaviour
{
    public GameObject warningOverlay;
    public Text warningText;

    private string requiredOrientation = "landscape";
    private string currentOrientation = "landscape";

    void Start()
    {
        Debug.Log("Hello from Unity");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        requiredOrientation = GetOrientationForScene(scene.name);

        // Send the scene name to JavaScript using ExternalCall
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            // Use ExternalCall to call SetCurrentScene in JS
            Application.ExternalCall("SetCurrentScene", scene.name);
        }

        // Find even if inactive
        var allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        var panelTransform = allTransforms.FirstOrDefault(t => t.name == "OrientationPanel");

        if (panelTransform != null)
        {
            warningOverlay = panelTransform.gameObject;
            warningText = warningOverlay.GetComponentInChildren<Text>(true);
            Debug.Log("[OrientationOverlay] OrientationPanel and warningText found.");
        }
        else
        {
            Debug.LogWarning("[OrientationOverlay] Could not find 'OrientationPanel' in the scene.");
        }

        CheckOrientation();
    }

    string GetOrientationForScene(string sceneName)
    {
        // JS determines actual orientation needs — default to landscape
        return "landscape"; 
    }

    // Called by JS on orientation change
    public void OnBrowserOrientationChanged(string orientation, string requiredOrientation)
    {
        Debug.Log($"[OrientationOverlay] JS called with current: {orientation}, required: {requiredOrientation}");

        currentOrientation = orientation.ToLower();
        this.requiredOrientation = requiredOrientation.ToLower();
        CheckOrientation();
    }

    void CheckOrientation()
    {
        if (warningOverlay == null || warningText == null)
        {
            Debug.LogWarning("[OrientationOverlay] Missing references — skipping CheckOrientation.");
            return;
        }

        Debug.Log($"[OrientationOverlay] Checking: current = {currentOrientation}, required = {requiredOrientation}");

        if (currentOrientation != requiredOrientation)
        {
            warningOverlay.SetActive(true);
            warningText.text = $"Please rotate your device to {requiredOrientation}.";
            Debug.Log("[OrientationOverlay] Showing warning overlay.");
        }
        else
        {
            warningOverlay.SetActive(false);
            Debug.Log("[OrientationOverlay] Hiding warning overlay.");
        }
    }

    public void ShowWarning(string orientation)
    {
        currentOrientation = orientation.ToLower();
        requiredOrientation = orientation.ToLower(); // JS already validated
        CheckOrientation();
    }

    public void HideWarning()
    {
        if (warningOverlay != null)
        {
            warningOverlay.SetActive(false);
            Debug.Log("[OrientationOverlay] Manually hiding warning overlay.");
        }
    }
}
