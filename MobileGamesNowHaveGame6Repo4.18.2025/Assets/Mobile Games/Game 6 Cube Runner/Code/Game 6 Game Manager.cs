using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game6GameManager : MonoBehaviour
{
    public GameObject enemy;
    public Transform spawnPoint;
    public float maxSpawnPointX;
    public GameObject menuPanel;
    public static Game6GameManager instance;

    public float initialSpeed = -12f;
    public float maxSpeed = -400f;
    private float currentSpeed;

    public Text scoreText;
    public Text highScoreText;

    private int score = 0;
    private int highScore = 0;
    private bool gameStarted = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        currentSpeed = initialSpeed;

        if (PlayerPrefs.HasKey("highScore"))
        {
            highScore = PlayerPrefs.GetInt("highScore");
            highScoreText.text = "HIGH SCORE: " + highScore.ToString();
        }
    }

    void Update()
    {
        if (Input.anyKeyDown && !gameStarted)
        {
            menuPanel.SetActive(false);
            scoreText.gameObject.SetActive(true);
            StartCoroutine(SpawnEnemies());
            StartCoroutine(IncreaseSpeedOverTime());
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

    IEnumerator IncreaseSpeedOverTime()
    {
        while (currentSpeed > maxSpeed)
        {
            yield return new WaitForSeconds(1f);
            currentSpeed -= 5f;
        }
    }

    public void Spawn()
    {
        float randomSpawnX = Random.Range(-maxSpawnPointX, maxSpawnPointX);
        Vector3 enemySpawnPos = spawnPoint.position;
        enemySpawnPos.x = randomSpawnX;

        GameObject newEnemy = Instantiate(enemy, enemySpawnPos, Quaternion.identity);
        newEnemy.GetComponent<Game6Enemy>().speed = currentSpeed;
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

    public void ScoreUp()
    {
        score++;
        scoreText.text = score.ToString();
    }
}
