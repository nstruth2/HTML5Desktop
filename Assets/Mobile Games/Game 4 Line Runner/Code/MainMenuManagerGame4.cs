using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenumanagerGame4 : MonoBehaviour
{
    public Button startButton;       // Reference to the Start button
    public Button mainMenuButton;    // Reference to the Main Menu button

    private bool isSceneLoading = false;  // Flag to prevent double-clicks for any button

    void Start()
    {
        // Ensure the Start button and Main Menu button are hooked up to their respective methods
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        }
    }

    // This method will be called when the Start button is clicked
    public void OnStartButtonClicked()
    {
        if (isSceneLoading) return;  // Prevent double-clicks

        isSceneLoading = true; // Set flag to prevent further clicks
        Debug.Log("Start Button Clicked!");
        LoadGameplayScene();
    }

    // Load the gameplay scene
    public void LoadGameplayScene()
    {
        Debug.Log("Loading GameplayGame4...");
        SceneManager.LoadScene("GameplayGame4");
    }

    // This method will be called when the Main Menu button is clicked
    public void OnMainMenuButtonClicked()
    {
        if (isSceneLoading) return;  // Prevent double-clicks

        isSceneLoading = true; // Set flag to prevent further clicks
        Debug.Log("Loading Global Main Menu...");
        SceneManager.LoadScene("Global Main Menu");
    }
}
