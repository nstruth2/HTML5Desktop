using System.Collections;
using System.Collections.Generic;
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
    int score = 0;
    int highScore = 0;
    public Text scoreText;
    public Text highScoreText;
    bool gameStarted = false;

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
    }
    void Start()
    {
        if(PlayerPrefs.HasKey("highScore"))
        {
            highScore = PlayerPrefs.GetInt("highScore");
            highScoreText.text = "HIGH SCORE: " + highScore.ToString();
        }
    }
    void Update()
    {
        if(Input.anyKeyDown&&!gameStarted)
        {
            menuPanel.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(true);
            StartCoroutine("SpawnEnemies");
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
    public void Spawn()
    {
        float randomSpawnX = Random.Range(-maxSpawnPointX,maxSpawnPointX);
        Vector3 enemySpawnPos = spawnPoint.position;
        enemySpawnPos.x = randomSpawnX;
        Instantiate(enemy,enemySpawnPos,Quaternion.identity);
    }
    public void Restart()
    {
        if(score>highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highScore",highScore);
        }
        SceneManager.LoadScene("Cube Runner");
    }
    public void ScoreUp()
    {
        score++;
        scoreText.text = score.ToString();
    }
}