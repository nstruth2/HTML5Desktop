using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerGame1 : MonoBehaviour
{
    public GameObject obstacle;
    public Transform spawnPoint;
    public Transform playerStartPoint;

    int score = 0;
    int cachedHighScore = 0;
    bool highScoreFetched = false;

    public Text scoreText;
    public Text highScoreText;

    public GameObject playButton;
    public GameObject player;
    public GameObject mainMenu;

    public HighScoreFetcherGame1 highScoreFetcher;

    void Start()
    {
        highScoreText.text = "Fetching top score...";
        mainMenu.SetActive(true);
        player.SetActive(false);
        playButton.SetActive(false);

        // Find and cache the high score fetcher
        highScoreFetcher = FindObjectOfType<HighScoreFetcherGame1>();

        if (highScoreFetcher == null)
        {
            Debug.LogError("HighScoreFetcherGame1 not found in scene.");
            return;
        }

        StartCoroutine(CacheHighScoreWhenReady());
    }

    IEnumerator CacheHighScoreWhenReady()
    {
        while (highScoreFetcher.currentHighScore == 0 && !highScoreFetcher.hasFetched)
        {
            yield return null;
        }

        cachedHighScore = highScoreFetcher.currentHighScore;
        highScoreText.text = $"High Score: {highScoreFetcher.currentHighScorePlayer}: {cachedHighScore}";
        highScoreFetched = true;
        playButton.SetActive(true);
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
            Instantiate(obstacle, spawnPoint.position, Quaternion.identity);
        }
    }

    void ScoreUp()
    {
        if (!highScoreFetched) return;

        score++;
        scoreText.text = $"Score: {score}";

        if (score > cachedHighScore)
        {
            cachedHighScore = score;
            highScoreText.text = $"High Score: You: {score}";
            PlayerPrefs.SetInt("HighScoreGame1", cachedHighScore);
            PlayerPrefs.Save();
        }
    }

    public void OnPlayButtonPressed()
    {
        if (highScoreFetched)
            GameStart();
    }

    void GameStart()
    {
        score = 0;
        scoreText.text = $"Score: {score}";

        player.transform.position = playerStartPoint.position;
        player.SetActive(true);
        mainMenu.SetActive(false);
        playButton.SetActive(false);

        StartCoroutine(SpawnObstacles());
        InvokeRepeating(nameof(ScoreUp), 2f, 1f);
    }

    public void GameOver()
    {
        CancelInvoke(nameof(ScoreUp));
        StopAllCoroutines();

        PlayerPrefs.SetInt("Game1_SubmitScore", score);
        PlayerPrefs.Save();

        player.SetActive(false);

        SceneManager.LoadScene("Submit Score and Name Game 1");
    }
}
