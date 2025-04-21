using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenumanagerGame4 : MonoBehaviour
{
    public Button startButton; // Reference to the Start Button

    private bool hasClicked = false;

    void Start()
    {
        // Ensure the start button is clickable
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
    }

    void Update()
    {
        // Check for mouse click or touch input, allowing the scene load on first click/tap
        if (!hasClicked && (Input.GetMouseButtonDown(0) || Input.touchCount > 0))
        {
            if (!IsPointerOverUI())
            {
                hasClicked = true;
                Debug.Log("Detected Click/Tap");
                LoadGameplayScene();
            }
        }
    }

    private bool IsPointerOverUI()
    {
        // Checks if the pointer (mouse or touch) is over a UI element
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    // This method will be called when the Start button is clicked
    public void OnStartButtonClicked()
    {
        if (!hasClicked)
        {
            hasClicked = true;
            Debug.Log("Start Button Clicked!");
            LoadGameplayScene();
        }
    }

    // Load the gameplay scene
    public void LoadGameplayScene()
    {
        Debug.Log("Loading GameplayGame4...");
        SceneManager.LoadScene("GameplayGame4");
    }

    public void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene("Global Main Menu");
    }
}
