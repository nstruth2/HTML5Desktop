using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerGame8 : MonoBehaviour
{
    public static GameManagerGame8 instance;

    public Text scoreText;
    public Text highScoreText;

    private int score = 0;
    private int highScore = 0;

    private bool isGameOver = false;

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        score = 0;
        highScore = PlayerPrefs.GetInt("Game8_SubmitScore", 0);
        UpdateUI();
    }

    public void ScoreUp()
    {
        if (isGameOver) return;

        score++;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("Game8_SubmitScore", highScore);
        }

        UpdateUI();
    }

    public void GameOver()
    {
        isGameOver = true;

        // Save final score for submission
        PlayerPrefs.SetInt("Game8_SubmitScore", score);
        PlayerPrefs.Save();

        Debug.Log($"Game Over. Final Score: {score}, High Score: {highScore}");

        // Load the score submission scene
        SceneManager.LoadScene("Submit Score and Name Game 8"); // Replace with your actual submission scene name
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }

        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore;
        }
    }

    public int GetCurrentScore() => score;
    public int GetHighScore() => highScore;
}
