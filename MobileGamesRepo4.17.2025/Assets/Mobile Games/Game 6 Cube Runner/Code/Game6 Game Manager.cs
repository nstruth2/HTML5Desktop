using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game6GameManager : MonoBehaviour
{
    public static Game6GameManager instance;

    [Header("Enemy Spawning")]
    public GameObject enemy;
    public Transform spawnPoint;
    public float maxSpawnPointX = 5f;

    [Header("UI")]
    public GameObject menuPanel;
    public Text scoreText;
    public Text highScoreText;

    [Header("Enemy Speed")]
    public float currentEnemySpeed = 3f;
    public float speedIncreaseRate = 0.5f;
    public float maxEnemySpeed = 10f;

    private int score = 0;
    private int highScore = 0;
    private bool gameStarted = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("highScore"))
        {
            highScore = PlayerPrefs.GetInt("highScore");
        }

        highScoreText.text = "HIGH SCORE: " + highScore;
        scoreText.gameObject.SetActive(false);
        StartCoroutine(IncreaseEnemySpeed());
    }

    private void Update()
    {
        if (!gameStarted && Input.anyKeyDown)
        {
            StartGame();
        }
    }

    void StartGame()
    {
        menuPanel.SetActive(false);
        scoreText.gameObject.SetActive(true);
        gameStarted = true;
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.8f);
            SpawnEnemy();
        }
    }

    IEnumerator IncreaseEnemySpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            currentEnemySpeed = Mathf.Min(currentEnemySpeed + speedIncreaseRate, maxEnemySpeed);
        }
    }

    void SpawnEnemy()
    {
        if (enemy == null || spawnPoint == null) return;

        float x = Random.Range(-maxSpawnPointX, maxSpawnPointX);
        Vector3 spawnPos = new Vector3(x, spawnPoint.position.y, spawnPoint.position.z);

        GameObject newEnemy = Instantiate(enemy, spawnPos, Quaternion.identity);

        Game6Enemy enemyScript = newEnemy.GetComponent<Game6Enemy>();
        if (enemyScript != null)
        {
            enemyScript.speed = currentEnemySpeed;
        }
    }

    public void ScoreUp()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void Restart()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highScore", highScore);
        }

        SceneManager.LoadScene("Cube Runner");
    }
}
