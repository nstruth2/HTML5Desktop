using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public GameObject player; // Reference to the player object
    public GameObject gameOverPanel; // Reference to the Game Over UI panel

    private void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // Ensure the Game Over UI is hidden at the start
        }

        if (player != null)
        {
            player.SetActive(true); // Make sure the player is active at the start
        }

        Time.timeScale = 1f; // Ensure the game is running from the start
        Debug.Log("Game Started!");
    }

    public void GameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // Display Game Over UI
        }
        Time.timeScale = 0f; // Pause the game
        Debug.Log("Game Over. Displaying Game Over UI.");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Ensure time is normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    public void BackToStartMenu()
    {
        Time.timeScale = 1f; // Ensure time is normal
        SceneManager.LoadScene("StartScene"); // Load the start scene
        Debug.Log("Returning to Start Menu.");
    }
}
