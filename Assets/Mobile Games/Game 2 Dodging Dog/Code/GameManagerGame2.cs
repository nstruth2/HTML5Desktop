using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerGame2 : MonoBehaviour
{
    public static GameManagerGame2 instance;
    bool gameOver = false;
    int score = 0;
    public Text scoreText;
    public GameObject gameOverPanel;
    public Text highScoreText; // Assign via the Inspector
    private int highScore;

    // Add a reference to the Clear High Score button
    public GameObject clearHighScoreButton;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Load the saved high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HighScoreGame2", 0);
        // Display the high score
        UpdateHighScoreText();

        // Make the Clear High Score button visible when the game first loads
        clearHighScoreButton.SetActive(true); // Show the button initially
    }

    public void GameOver()
    {
        gameOver = true;
        GameObject.Find("ObstacleSpawner").GetComponent<ObstacleSpawnerGame2>().StopSpawning();
        gameOverPanel.SetActive(true);

        // Save the current score for later submission
        PlayerPrefs.SetInt("Game2_SubmitScore", score);
        PlayerPrefs.Save();

        // Make the Clear High Score button visible again after the game ends
        SceneManager.LoadScene("Submit Score And Name Game 2");
    }

    public void IncrementScore()
    {
        if (gameOver) return;
        score++;
        UpdateScoreText();

        // Check and update high score
        if (score > highScore)
        {
            SetHighScore(score);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Gameplay Game 2");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Main Menu Game 2");
    }

    public void ClearHighScore()
    {
        // Delete the high score from PlayerPrefs
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.Save();

        // Reset the high score and update the display
        highScore = 0;
        UpdateHighScoreText();
    }

    private void SetHighScore(int newHighScore)
    {
        highScore = newHighScore;
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
        UpdateHighScoreText();
    }

    // Updates the high score text UI
    private void UpdateHighScoreText()
    {
        highScoreText.text = $"High Score: {highScore}";
    }

    // Updates the current score UI text
    private void UpdateScoreText()
    {
        scoreText.text = $"{score}";
    }
}
