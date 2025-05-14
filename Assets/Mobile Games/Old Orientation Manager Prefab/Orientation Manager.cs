using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class OrientationOverlayManager : MonoBehaviour
{
    public Text directionText;

    private string requiredOrientation = "landscape";
    private string currentOrientation = "landscape";

    void Start()
    {
        Debug.Log("Hello from Unity");
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (directionText == null)
        {
            Debug.LogWarning($"[OrientationOverlay] Missing directionText in scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}", this);
        }
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        requiredOrientation = GetOrientationForScene(scene.name);

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Application.ExternalCall("SetCurrentScene", scene.name);
        }

        CheckOrientation();
    }

    string GetOrientationForScene(string sceneName)
    {
        return "landscape";
    }

    public void OnBrowserOrientationChanged(string orientation, string requiredOrientation)
    {
        Debug.Log($"[OrientationOverlay] JS called with current: {orientation}, required: {requiredOrientation}");

        currentOrientation = orientation.ToLower();
        this.requiredOrientation = requiredOrientation.ToLower();
        CheckOrientation();
    }

    void CheckOrientation()
    {
        if (directionText == null)
        {
            Debug.LogWarning("[OrientationOverlay] Missing directionText.");
            return;
        }

        Debug.Log($"[OrientationOverlay] Checking: current = {currentOrientation}, required = {requiredOrientation}");

        if (currentOrientation != requiredOrientation)
        {
            directionText.text = $"Please rotate your device to {requiredOrientation}.";
            directionText.enabled = true;
        }
        else
        {
            directionText.enabled = false;
        }
    }

    public void ShowWarning(string orientation)
    {
        currentOrientation = orientation.ToLower();
        requiredOrientation = orientation.ToLower();
        CheckOrientation();
    }

    public void HideWarning()
    {
        if (directionText != null)
        {
            directionText.enabled = false;
        }
    }

    void CreateDirectionText()
    {
        GameObject canvasGO = new GameObject("OrientationCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;

        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        canvasGO.AddComponent<GraphicRaycaster>();

        GameObject textGO = new GameObject("DirectionText");
        textGO.transform.SetParent(canvasGO.transform, false);

        directionText = textGO.AddComponent<Text>();
        directionText.alignment = TextAnchor.MiddleCenter;
        directionText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        directionText.fontSize = 36;
        directionText.color = Color.blue;

        // ✅ Set your initial message here
        directionText.text = $"Please rotate your device to {requiredOrientation}.";
        directionText.enabled = true; // Show it immediately for testing

        // 🔑 Prevent text from clipping
        directionText.horizontalOverflow = HorizontalWrapMode.Overflow;
        directionText.verticalOverflow = VerticalWrapMode.Overflow;

        RectTransform rectTransform = directionText.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }

}
