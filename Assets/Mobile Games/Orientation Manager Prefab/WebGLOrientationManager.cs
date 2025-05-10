using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WebGLOrientationManager : MonoBehaviour
{
    public Text rotateText;  // Reference to the UI Text component (instructions to rotate)
    public Canvas gameCanvas;  // Reference to the canvas containing the game scene

    void Start()
    {
        // Initially show the rotate message
        rotateText.gameObject.SetActive(true);

        // Make sure the game canvas is hidden until the correct orientation
        gameCanvas.gameObject.SetActive(false);

        // Force landscape orientation only for mobile WebGL
        if (Application.platform == RuntimePlatform.WebGLPlayer && IsMobile())
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }

    void Update()
    {
        // Check if the platform is WebGL and it's a mobile device
        if (Application.platform == RuntimePlatform.WebGLPlayer && IsMobile())
        {
            // If not in landscape orientation, prompt user to rotate
            if (Screen.orientation != ScreenOrientation.LandscapeLeft && Screen.orientation != ScreenOrientation.LandscapeRight)
            {
                rotateText.gameObject.SetActive(true); // Show the rotate message
                gameCanvas.gameObject.SetActive(false); // Hide the game content
            }
            else
            {
                // Once in landscape, hide the message and show the game content
                rotateText.gameObject.SetActive(false);
                gameCanvas.gameObject.SetActive(true);

                // Lock the orientation to landscape once in the correct mode
                Screen.orientation = ScreenOrientation.LandscapeLeft;  // Lock the screen to landscape mode
            }
        }
        else
        {
            // For non-WebGL platforms or WebGL on non-mobile devices, don't change orientation
            rotateText.gameObject.SetActive(false); // Hide the rotate message for non-mobile WebGL or other platforms
            gameCanvas.gameObject.SetActive(true);  // Show the game content
        }
    }

    // Helper function to check if it's a mobile device
    bool IsMobile()
    {
        // A basic check for mobile devices
        return (Application.isMobilePlatform || (Input.touchSupported && Screen.width < 1000));
    }
}
