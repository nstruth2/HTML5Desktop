using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderGameOverGame1 : MonoBehaviour
{
    // Call this after submitting score and name to go to Menu
    public void Submit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Submit Score and Name Game 1"); // Replace with your actual menu scene name
    }

    // Retry the current game scene
    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Gameplay Game 1");
    }

    // Go to main menu directly
    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu"); // Replace with your actual menu scene name
    }
}
