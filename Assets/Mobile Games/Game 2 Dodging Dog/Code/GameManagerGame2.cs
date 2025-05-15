using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerGame2 : MonoBehaviour
{
    public static GameManagerGame2 instance;

    private bool gameOver = false;
    private int score = 0;

    public Text scoreText;
    public Text highScoreText; // Shared with HighScoreFetcher
    public GameObject gameOverPanel;
    public GameObject clearHighScoreButton;

    private HighScoreFetcherGame2 highScoreFetcher;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Fetch reference to HighScoreFetcher in scene
        highScoreFetcher = FindObjectOfType<HighScoreFetcherGame2>();

        if (highScoreFetcher == null)
        {
            Debug.LogError("HighScoreFetcherGame2 not found in scene.");
        }

        UpdateScoreText();
    }

    public void IncrementScore()
    {
        if (gameOver) return;

        score++;
        UpdateScoreText();

        if (highScoreFetcher != null)
        {
            int fetchedHighScore = highScoreFetcher.currentHighScore;
            if (score > fetchedHighScore)
            {
                // Display your own score as new top
                highScoreText.text = $"High Score: You: {score}";
            }
        }
    }

    public void GameOver()
    {
        gameOver = true;

        ObstacleSpawnerGame2 spawner = GameObject.Find("ObstacleSpawner")?.GetComponent<ObstacleSpawnerGame2>();
        if (spawner != null)
            spawner.StopSpawning();

        PlayerPrefs.SetInt("Game2_SubmitScore", score);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Game Over Game 2");
    }

    public void ClearHighScore()
    {
        PlayerPrefs.DeleteKey("HighScoreGame2");
        PlayerPrefs.Save();

        // Reset display to the database score if fetcher is available
        if (highScoreFetcher != null)
        {
            highScoreText.text = $"Top Score: {highScoreFetcher.currentHighScorePlayer}: {highScoreFetcher.currentHighScore}";
        }
        else
        {
            highScoreText.text = "High Score: 0";
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }
}
