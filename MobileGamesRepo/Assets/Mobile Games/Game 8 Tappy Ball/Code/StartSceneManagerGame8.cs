using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public void StartGame()
    {
        // Load the game scene
        SceneManager.LoadScene("Game 8 Tappy Ball");
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("Global Main Menu");
    }
}
