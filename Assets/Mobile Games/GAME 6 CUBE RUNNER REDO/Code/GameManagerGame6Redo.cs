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

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("highScoreGame6Redo"))
        {
            highScore = PlayerPrefs.GetInt("highScoreGame6Redo");
            highScoreText.text = "High SCore: " + highScore.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown && !gameStarted)
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
        float randomSpawnX = Random.Range(-maxSpawnPointX, maxSpawnPointX);

        Vector3 enemySpawnPos = spawnPoint.position;
        enemySpawnPos.x = randomSpawnX;
        Instantiate(enemy, enemySpawnPos, Quaternion.identity);
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