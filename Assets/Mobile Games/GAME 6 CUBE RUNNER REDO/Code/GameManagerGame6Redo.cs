using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerGame6Redo : MonoBehaviour
{
    public static GameManagerGame6Redo instance;
    public GameObject enemy;
    public Transform spawnPoint;
    public float maxSpawnPointX;
    public Text scoreText;
    public Text highScoreText;
    public GameObject menuPanel;
    
    // Speed variables - will be shown in Unity Inspector
    public float initialEnemySpeed = 5f;
    public float speedIncreaseAmount = 2f;
    public float speedIncreaseInterval = 1f;
    
    // Current speed used by all enemies
    [HideInInspector] public float currentEnemySpeed;
    
    int score = 0;
    int highScore = 0;
    bool gameStarted = false;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    
    void Start()
    {
        currentEnemySpeed = initialEnemySpeed;
        
        if(PlayerPrefs.HasKey("highScoreGame6Redo"))
        {
            highScore = PlayerPrefs.GetInt("highScoreGame6Redo");
            highScoreText.text = "High Score: " + highScore.ToString();
        }
    }
    
    void Update()
    {
        if(Input.anyKeyDown && !gameStarted)
        {
            menuPanel.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(true);
            StartCoroutine("SpawnEnemies");
            StartCoroutine("IncreaseSpeed");
            gameStarted = true;
        }
    }
    
    IEnumerator SpawnEnemies()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.8f);
            Spawn();
        }
    }
    
    IEnumerator IncreaseSpeed()
    {
        while(true)
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
        // No need to set speed on the enemy - it will use the global speed
    }
    
    public void Restart()
    {
        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highScoreGame6Redo", highScore);
        }
        SceneManager.LoadScene("CUBE RUNNER REDO");
    }
    
    public void ScoreUp()
    {
        score++;
        scoreText.text = score.ToString();
    }
}