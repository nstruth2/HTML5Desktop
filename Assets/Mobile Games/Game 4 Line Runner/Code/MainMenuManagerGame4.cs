using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenumanagerGame4: MonoBehaviour
{
    private bool hasTapped = false;

    void Update()
    {
        // Check for touch or mouse click
        if (!hasTapped && (Input.GetMouseButtonDown(0) || Input.touchCount > 0))
        {
            hasTapped = true;
            LoadGameplayScene();
        }
    }

    private void LoadGameplayScene()
    {
        SceneManager.LoadScene("GameplayGame4");
    }
}
