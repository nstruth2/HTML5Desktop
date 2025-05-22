using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManagerGame7 : MonoBehaviour
{
    public static GameManagerGame7 instance;

    [Header("UI References")]
    public Text scoreText;
    public Text highScoreText;

    [Header("Ball Reference")]
    public BallGame7 ballScript;
    public HighScoreFetcherGame7 highScoreFetcher;

    private int score = 0;
    private int localHighScore = 0;
    private int fetchedHighScore = 0;
    private string fetchedHighScorePlayer = "???";

    private bool isGameOver = true;
    private bool hasFetchedHighScore = false;
    private bool playerSurpassedFetchedHighScore = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        scoreText.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(true);
        highScoreText.text = "Fetching top score...";

        highScoreFetcher = FindObjectOfType<HighScoreFetcherGame7>();

        if (highScoreFetcher == null)
        {
            Debug.LogError("HighScoreFetcherGame7 not found in scene.");
            return;
        }

        StartCoroutine(CacheHighScoreAndStartGame());
    }

    IEnumerator CacheHighScoreAndStartGame()
    {
        while (highScoreFetcher.currentHighScore == 0 && !highScoreFetcher.hasFetched)
        {
            yield return null;
        }

        SetFetchedHighScore(highScoreFetcher.currentHighScore, highScoreFetcher.currentHighScorePlayer);
        StartGame();
    }

    public void StartGame()
    {
        score = 0;
        isGameOver = false;
        playerSurpassedFetchedHighScore = false;

        scoreText.text = "Score: 0";
        scoreText.gameObject.SetActive(true);
        highScoreText.gameObject.SetActive(true);

        Time.timeScale = 1;

        if (ballScript != null)
        {
            ballScript.BeginGame();
        }
        else
        {
            Debug.LogWarning("BallGame7 script not assigned!");
        }

        UpdateUI();
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;

        // Save local high score if needed
        if (score > localHighScore)
        {
            localHighScore = score;
            PlayerPrefs.SetInt("Game7_SubmitScore", localHighScore);
            PlayerPrefs.Save();
        }

        SceneManager.LoadScene("Game Over Game 7");
    }

    public void ScoreUp()
    {
        if (isGameOver) return;

        score++;

        if (score > localHighScore)
        {
            localHighScore = score;
            PlayerPrefs.SetInt("Game7_SubmitScore", localHighScore);
            PlayerPrefs.Save();
        }

        if (hasFetchedHighScore && !playerSurpassedFetchedHighScore && score > fetchedHighScore)
        {
            playerSurpassedFetchedHighScore = true;
        }

        UpdateUI();
    }

    public void SetFetchedHighScore(int fetchedScore, string fetchedPlayer = "???")
    {
        fetchedHighScore = fetchedScore;
        fetchedHighScorePlayer = fetchedPlayer;
        hasFetchedHighScore = true;

        if (!isGameOver && score > fetchedHighScore)
        {
            playerSurpassedFetchedHighScore = true;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = $"Score: {score}";

        if (hasFetchedHighScore)
        {
            if (playerSurpassedFetchedHighScore)
                highScoreText.text = $"Top Score: You: {score}";
            else
                highScoreText.text = $"Top Score: {fetchedHighScorePlayer}: {fetchedHighScore}";
        }
        else
        {
            highScoreText.text = $"Top Score: You: {localHighScore}";
        }
    }

    // Getters
    public int GetCurrentScore() => score;
    public int GetHighScore() => localHighScore;
}
