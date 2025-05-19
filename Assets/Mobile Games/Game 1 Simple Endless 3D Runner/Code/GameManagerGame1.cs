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

    public GameObject player;

    public HighScoreFetcherGame1 highScoreFetcher;

    void Start()
    {
        highScoreText.text = "Fetching top score...";
        player.SetActive(false);

        highScoreFetcher = FindObjectOfType<HighScoreFetcherGame1>();

        if (highScoreFetcher == null)
        {
            Debug.LogError("HighScoreFetcherGame1 not found in scene.");
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

        cachedHighScore = highScoreFetcher.currentHighScore;
        highScoreText.text = $"High Score: {highScoreFetcher.currentHighScorePlayer}: {cachedHighScore}";
        highScoreFetched = true;

        GameStart(); // Start game immediately after fetching score
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
            Instantiate(obstacle, spawnPoint.position, Quaternion.identity);
        }
    }

    public void ScoreUp()
    {
        Debug.Log("ScoreUp called. highScoreFetched = " + highScoreFetched);

        if (!highScoreFetched)
        {
            Debug.Log("ScoreUp blocked: highScore not fetched yet.");
            return;
        }

        score++;
        scoreText.text = $"Score: {score}";
        Debug.Log("Score incremented: " + score);

        if (score > cachedHighScore)
        {
            cachedHighScore = score;
            highScoreText.text = $"High Score: You: {score}";
            PlayerPrefs.SetInt("HighScoreGame1", cachedHighScore);
            PlayerPrefs.Save();
        }
    }


    void GameStart()
    {
        score = 0;
        scoreText.text = $"Score: {score}";

        player.transform.position = playerStartPoint.position;
        player.SetActive(true);

        StartCoroutine(SpawnObstacles());
    }

    public void GameOver()
    {
        StopAllCoroutines();

        PlayerPrefs.SetInt("Game1_SubmitScore", score);
        PlayerPrefs.Save();

        player.SetActive(false);

        if (Application.platform == RuntimePlatform.WebGLPlayer && Application.isMobilePlatform)
        {
            SceneManager.LoadScene("Submit Score and Name Game 1 Landscape");
        }
        else
        {
            SceneManager.LoadScene("Game Over Game 1");
        }
    }
}
