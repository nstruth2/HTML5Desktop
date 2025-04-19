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
    public GameObject keyboardPanel; // assign in Inspector
    public InputField nameInputField; // assign in Inspector
    public static Game6GameManager instance;

    public float initialSpeed = -12f;
    public float maxSpeed = -400f;
    private float currentSpeed;

    public Text scoreText;
    public Text highScoreText;

    private int score = 0;
    private int highScore = 0;
    private bool gameStarted = false;

    private enum GameState { Menu, Playing, GameOver, WaitingForName }
    private GameState gameState = GameState.Menu;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        scoreText.gameObject.SetActive(false);
        menuPanel.SetActive(true);
        keyboardPanel.SetActive(false);
        Time.timeScale = 1f;

        currentSpeed = initialSpeed;

        if (PlayerPrefs.HasKey("highScore"))
        {
            highScore = PlayerPrefs.GetInt("highScore");
            highScoreText.text = "HIGH SCORE: " + highScore.ToString();
        }
    }

    void Update()
    {
        if (Input.anyKeyDown && gameState == GameState.Menu)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        menuPanel.SetActive(false);
        scoreText.gameObject.SetActive(true);
        StartCoroutine(SpawnEnemies());
        StartCoroutine(IncreaseSpeedOverTime());
        gameState = GameState.Playing;
        gameStarted = true;
        score = 0;
        currentSpeed = initialSpeed;
    }

    public void GameOver()
    {
        StopAllCoroutines();
        gameState = GameState.GameOver;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highScore", highScore);
            highScoreText.text = "HIGH SCORE: " + highScore.ToString();

            // Wait for player to enter name
            keyboardPanel.SetActive(true);
            Time.timeScale = 0f;
            gameState = GameState.WaitingForName;
        }
        else
        {
            BackToMenu();
        }
    }

    public void OnNameEntered(string playerName)
    {
        Debug.Log("Player name: " + playerName);
        PlayerPrefs.SetString("playerName", playerName);

        keyboardPanel.SetActive(false);
        Time.timeScale = 1f;
        BackToMenu();
    }

    private void BackToMenu()
    {
        menuPanel.SetActive(true);
        scoreText.gameObject.SetActive(false);
        gameState = GameState.Menu;
        gameStarted = false;
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

    public void ScoreUp()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public int GetScore()
    {
        return score;
    }
    
    public void Restart()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highScore", highScore);
        }

        // Restart the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
