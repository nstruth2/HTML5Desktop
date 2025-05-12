using UnityEngine;

public class NativeMobileOrientationManager : MonoBehaviour
{
    public Canvas gameCanvas;  // Reference to the canvas containing the game scene

    void Start()
    {
        // Initially hide the game canvas until the correct orientation is reached
        gameCanvas.gameObject.SetActive(false);

        // Lock the screen orientation to LandscapeLeft for mobile devices (iOS/Android)
        if (Application.isMobilePlatform)
        {
            // Force landscape orientation (LandscapeLeft)
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }

    void Update()
    {
        // Check if the platform is a mobile device (iOS/Android)
        if (Application.isMobilePlatform)
        {
            // If the orientation is LandscapeLeft or LandscapeRight, show the game content
            if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
            {
                gameCanvas.gameObject.SetActive(true); // Show the game content when in landscape
            }
            else
            {
                gameCanvas.gameObject.SetActive(false); // Hide the game content if not in landscape
            }
        }
        else
        {
            // For non-mobile platforms, just show the game content
            gameCanvas.gameObject.SetActive(true);
        }
    }
}
