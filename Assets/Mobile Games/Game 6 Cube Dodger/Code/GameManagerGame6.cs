using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerGame6 : MonoBehaviour
{
    public static GameManagerGame6 instance;
    public GameObject enemy;
    public Transform spawnPoint;
    public float maxSpawnPointX;
    public Text scoreText;
    public Text highScoreText;
    public GameObject menuPanel;

    public float initialEnemySpeed = 5f;
    public float speedIncreaseAmount = 2f;
    public float speedIncreaseInterval = 1f;

    [HideInInspector] public float currentEnemySpeed;

    private int score = 0;
    private int localHighScore = 0;
    private int fetchedHighScore = 0;
    private string fetchedHighScorePlayer = "???";

    private bool gameStarted = false;
    private bool isGameOver = false;
    private bool hasFetchedHighScore = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        currentEnemySpeed = initialEnemySpeed;

        localHighScore = PlayerPrefs.GetInt("Game6_SubmitScore", 0);
        UpdateUI();
    }

    void Update()
    {
        if (Input.anyKeyDown && !gameStarted && !isGameOver)
        {
            menuPanel.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(true);
            StartCoroutine(SpawnEnemies());
            StartCoroutine(IncreaseSpeed());
            gameStarted = true;
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.8f);
            Spawn();
        }
    }

    IEnumerator IncreaseSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(speedIncreaseInterval);
            currentEnemySpeed += speedIncreaseAmount;
            Debug.Log("Enemy speed increased to: " + currentEnemySpeed);
        }
    }

    public void Spawn()
    {
        float randomSpawnX = Random.Range(-maxSpawnPointX, maxSpawnPointX);
        Vector3 enemySpawnPos = spawnPoint.position;
        enemySpawnPos.x = randomSpawnX;
        Instantiate(enemy, enemySpawnPos, Quaternion.identity);
    }

    public void ScoreUp()
    {
        score++;

        if (score > localHighScore)
        {
            localHighScore = score;
            PlayerPrefs.SetInt("Game6_SubmitScore", localHighScore);
            PlayerPrefs.Save();
        }

        UpdateUI(); // move this to the end so it reflects the new state
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        StopAllCoroutines(); // Stop spawning and speed increase
        Debug.Log("Game Over");

        scoreText.gameObject.SetActive(false);
        SceneManager.LoadScene("Submit Score and Name Game 6");
    }

    public void SetFetchedHighScore(int fetchedScore, string fetchedPlayer = "???")
    {
        fetchedHighScore = fetchedScore;
        fetchedHighScorePlayer = fetchedPlayer;
        hasFetchedHighScore = true;

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }

        if (highScoreText != null)
        {
            if (hasFetchedHighScore)
            {
                if (localHighScore > fetchedHighScore)
                {
                    highScoreText.text = $"Top Score: You: {localHighScore}";
                }
                else
                {
                    highScoreText.text = $"Top Score: {fetchedHighScorePlayer}: {fetchedHighScore}";
                }
            }
            else
            {
                highScoreText.text = $"High Score: {localHighScore}";
            }
        }
    }

    public int GetCurrentScore() => score;
    public int GetHighScore() => localHighScore;
}
