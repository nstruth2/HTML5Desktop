using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject obstacle;
    public Transform spawnPoint;
    public Transform playerStartPoint; // Reference to the player's starting point

    int score = 0;
    int highScore = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject playButton;
    public GameObject player;
    public GameObject mainMenu; // Reference to the Main Menu UI container
    public GameObject clearHighScoreButton; // Reference to the Clear High Score button

    void Start()
    {
        // Load the saved high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HighScoreGame1", 0);
        highScoreText.text = "High Score: " + highScore.ToString();

        // Initially show the score as 0
        scoreText.text = "Score: 0";

        // Ensure the main menu and clear button are active and player is inactive at the start
        mainMenu.SetActive(true);
        clearHighScoreButton.SetActive(true);
        player.SetActive(false);
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            float waitTime = Random.Range(0.5f, 2f);
            yield return new WaitForSeconds(waitTime);
            Instantiate(obstacle, spawnPoint.position, Quaternion.identity);
        }
    }

    void ScoreUp()
    {
        score++;
        scoreText.text = "Score: " + score.ToString();

        // Check and update the high score in real-time
        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = "High Score: " + highScore.ToString();

            // Save the new high score in PlayerPrefs
            PlayerPrefs.SetInt("HighScoreGame1", highScore);
            PlayerPrefs.Save();
        }
    }

    // This method is called when the Play button is pressed
    public void OnPlayButtonPressed()
    {
        GameStart();
    }

    public void GameStart()
    {
        // Hide the main menu and clear high score button when the game starts
        mainMenu.SetActive(false);
        clearHighScoreButton.SetActive(false);

        // Reset the score and display it
        score = 0;
        scoreText.text = "Score: " + score.ToString();

        // Reset player position to the starting point
        player.transform.position = playerStartPoint.position;

        // Activate player and start game logic
        player.SetActive(true);
        playButton.SetActive(false); // Hide play button after starting the game

        StartCoroutine("SpawnObstacles");
        InvokeRepeating("ScoreUp", 2f, 1f); // Start scoring after 2 seconds
    }

    public void GameOver()
    {
        // Stop scoring and spawning obstacles
        CancelInvoke("ScoreUp");
        StopCoroutine("SpawnObstacles");

        // Reset the score to 0 and update the UI
        score = 0;
        scoreText.text = "Score: 0"; // Ensure score text is reset when game ends

        // Reset UI and game state
        player.SetActive(false);
        playButton.SetActive(true); // Show the play button for a new game

        // Reactivate the main menu and clear high score button when the game is over
        mainMenu.SetActive(true);
        clearHighScoreButton.SetActive(true);
    }

    public void ClearHighScore()
    {
        // Delete the high score from PlayerPrefs
        PlayerPrefs.DeleteKey("HighScoreGame1");
        PlayerPrefs.Save(); // Ensure the deletion is saved

        // Set highScore to 0 and update the UI
        highScore = 0;
        highScoreText.text = "High Score: 0"; // Update the high score text
    }
}
