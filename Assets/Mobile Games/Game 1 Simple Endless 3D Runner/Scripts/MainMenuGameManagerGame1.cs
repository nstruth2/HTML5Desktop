using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenumanagerGame1 : MonoBehaviour
{
    public Button startButton; // Reference to the Start Button prefab
    public Button mainMenuButton; // Reference to the Main Menu Button prefab
    private bool isLoadingScene = false; // Flag to prevent multiple scene loads

    void Start()
    {
        // Ensure both buttons are clickable and their actions are assigned
        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners(); // Clear any existing listeners
            startButton.onClick.AddListener(OnStartButtonClicked);
            Debug.Log("Start button listener attached");
        }
        else
        {
            Debug.LogWarning("Start Button prefab not assigned!");
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveAllListeners(); // Clear any existing listeners
            mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
            Debug.Log("Main menu button listener attached");
        }
        else
        {
            Debug.LogWarning("Main Menu Button prefab not assigned!");
        }
    }

    void Update()
    {
        // The input detection from Update() has been removed.
        // Button clicks are now handled solely by the button's onClick event.
    }

    // This method will be called when the Start button is clicked
    public void OnStartButtonClicked()
    {
        // Prevent multiple scene loads
        if (isLoadingScene)
            return;

        Debug.Log("Start Button Clicked!");
        LoadGameplayScene();
    }

    // This method will be called when the Main Menu button is clicked
    public void OnMainMenuButtonClicked()
    {
        // Prevent multiple scene loads
        if (isLoadingScene)
            return;

        Debug.Log("Main Menu Button Clicked!");
        LoadMainMenuScene();
    }

    // Load the gameplay scene
    public void LoadGameplayScene()
    {
        if (isLoadingScene)
            return;

        isLoadingScene = true;
        Debug.Log("Loading GameplayGame4...");
        Time.timeScale = 1f; // Reset time scale
        SceneManager.LoadScene("Gameplay Game 1");
    }

    // Load the main menu scene
    public void LoadMainMenuScene()
    {
        if (isLoadingScene)
            return;

        isLoadingScene = true;
        Debug.Log("Loading Global Main Menu...");
        SceneManager.LoadScene("Global Main Menu");
    }
}