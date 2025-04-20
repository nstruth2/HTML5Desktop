using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenumanagerGame4 : MonoBehaviour
{
    private bool hasTapped = false;

    void Update()
    {
        if (hasTapped)
            return;

        // Check for mouse click or touch input, ignoring UI elements
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && !IsPointerOverUI())
        {
            hasTapped = true;
            LoadGameplayScene();
        }
    }

    private bool IsPointerOverUI()
    {
        // Checks if the pointer (mouse or touch) is over a UI element
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    private void LoadGameplayScene()
    {
        SceneManager.LoadScene("GameplayGame4");
    }
}
