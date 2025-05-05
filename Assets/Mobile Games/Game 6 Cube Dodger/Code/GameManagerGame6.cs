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

    int score = 0;
    int highScore = 0;
    bool gameStarted = false;
    bool isGameOver = false;

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

        highScore = PlayerPrefs.GetInt("Game6_SubmitScore", 0);
        highScoreText.text = "High Score: " + highScore.ToString();
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
        scoreText.text = "Score: " + score.ToString();

        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = "Top Score: You: " + highScore.ToString();
            PlayerPrefs.SetInt("Game6_SubmitScore", highScore);
            PlayerPrefs.Save();
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        StopAllCoroutines(); // Stop spawning and speed increase
        Debug.Log("Game Over");

        // Optional: Hide score UI
        scoreText.gameObject.SetActive(false);

        // Load the submit scene
        SceneManager.LoadScene("Submit Score and Name Game 6");
    }
}
