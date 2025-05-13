using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OrientationOverlayManager : MonoBehaviour
{
    public GameObject warningOverlay;
    public Text warningText;

    private string requiredOrientation = "landscape";
    private string currentOrientation = "landscape";

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        requiredOrientation = GetOrientationForScene(scene.name);
        CheckOrientation();
    }

    string GetOrientationForScene(string sceneName)
    {
        // Return "landscape" by default, as the orientation logic is now handled by JS
        return "landscape"; 
    }

    // This method will be called by JavaScript when the orientation changes
    public void OnBrowserOrientationChanged(string orientation, string requiredOrientation)
    {
        currentOrientation = orientation.ToLower();
        this.requiredOrientation = requiredOrientation.ToLower();
        CheckOrientation();
    }

    void CheckOrientation()
    {
        if (currentOrientation != requiredOrientation)
        {
            warningOverlay.SetActive(true);
            warningText.text = $"Please rotate your device to {requiredOrientation}.";
        }
        else
        {
            warningOverlay.SetActive(false);
        }
    }

    public void ShowWarning(string orientation)
    {
        currentOrientation = orientation.ToLower();
        requiredOrientation = orientation.ToLower(); // This is okay because JS already validated
        CheckOrientation();
    }

    public void HideWarning()
    {
        warningOverlay.SetActive(false);
    }
}
