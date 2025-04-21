using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GameManagerGame4 : MonoBehaviour
{
    public GameObject menuUI;           // Main Menu UI
    public GameObject gamePlayUI;       // Gameplay UI
    public GameObject spawner;
    public GameObject backgroundParticle;
    public static GameManagerGame4 instance;
    public bool gameStarted = false;
    Vector3 originalCamPos;
    public GameObject player;

    private int lives = 3;
    private int score = 0;
    private int currentHighScore = 0;
    private string currentHighScorePlayer = ""; // Store the player name with the highest score

    public Text scoreText;
    public Text livesText;
    public Text highScoreText;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        originalCamPos = Camera.main.transform.position;

        // Disable the main menu during gameplay
        menuUI.SetActive(true);  // Always show the menu UI at the start
        gamePlayUI.SetActive(true);  // Hide the gameplay UI initially
        spawner.SetActive(false);     // Hide spawner initially
        backgroundParticle.SetActive(false);  // Hide background particles initially
        highScoreText.text = "";
        StartCoroutine(FetchTopScorers());
        StartGame();
        
    }

    public void StartGame()
    {
        gameStarted = true;
        lives = 3;  // Reset lives
        UpdateLivesUI();  // Update UI
        Debug.Log("StartGame: Lives reset to " + lives);
        menuUI.SetActive(false); // Ensure the main menu is hidden when the game starts
        gamePlayUI.SetActive(true);
        spawner.SetActive(true);
        backgroundParticle.SetActive(true);
        player.SetActive(true);
        score = 0;
        scoreText.text = "Score: " + score;

        // Keep the high score on the screen with player name
        if (!string.IsNullOrEmpty(currentHighScorePlayer) && currentHighScore > 0)
        {
            highScoreText.text = $"Top Score: {currentHighScorePlayer}: {currentHighScore}";
        }
        else
        {
            highScoreText.text = "";
        }
        // Ensure game is running at normal speed
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        player.SetActive(false);

        // Save the final score for reference
        PlayerPrefs.SetInt("Game4_SubmitScore", score);
        PlayerPrefs.Save();

        if (score >= currentHighScore)
        {
            // Player beat the high score, go to the submit scene
            SceneManager.LoadScene("SubmitScoreAndNameGame4");
        }
        else
        {
            // Player did not beat the high score, just reload the game or show game over UI
            Debug.Log("Game over - did not beat high score.");
            SceneManager.LoadScene("MainMenuGame4");
            menuUI.SetActive(true);
            gamePlayUI.SetActive(false);
            spawner.SetActive(false);
            backgroundParticle.SetActive(false);
            // Optionally reset the game or show a retry button
        }
    }


    private IEnumerator FreezeGameAfterDelay(float delay)
    {
        // Allow input field and button to process during the delay
        yield return new WaitForSecondsRealtime(delay);  // Wait for 1.5 seconds, using real time

        // Freeze the game after the delay
        Time.timeScale = 0;  // Pause the game

        // If the score is lower than the high score, reload the scene
        if (score < currentHighScore)
        {
            ReloadLevel();
        }
    }

    public void UpdateLives()
    {
        Debug.Log("Before Decrement: Lives = " + lives);
        lives--;
        Debug.Log("After Decrement: Lives = " + lives);
        
        UpdateLivesUI(); // Ensure UI reflects the correct value

        if (lives <= 0)
        {
            Debug.Log("Game Over Triggered");
            GameOver();
        }
    }

    private void UpdateLivesUI()
    {
        Debug.Log("Updating UI: Lives = " + lives);
        livesText.text = "Lives: " + lives;
    }

    public void UpdateScore()
    {
        score++;
        scoreText.text = "Score: " + score;

        // Update the high score display only if the score exceeds current high score
        if (score > currentHighScore)
        {
            currentHighScore = score;
            currentHighScorePlayer = ""; // Set a default value here (since player name input is removed)

            // Ensure no references to directionsText, submitButton, or playerNameInput
        }

        string highScoreDisplayText = $"Top Score: {currentHighScorePlayer}";

        // Only add the colon and the score if the current score is *less than* the high score.
        if (score <= currentHighScore)
        {
            highScoreDisplayText += $": {currentHighScore}";
        }
        else
        {
            // If the score surpasses the current high score, just display the score without a colon.
            highScoreDisplayText += $" {currentHighScore}";
        }

        highScoreText.text = highScoreDisplayText;
    }

    public void ExitGame()
    {
        // Only run this on Android platform
        #if UNITY_ANDROID
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        // Call finishAndRemoveTask() method to close the app and remove it from recent apps
        currentActivity.Call("finishAndRemoveTask");
        #else
        // For other platforms, fallback to the standard quit behavior
        Application.Quit();
        #endif
    }

    public void Shake()
    {
        StartCoroutine(CameraShake());
    }

    private IEnumerator CameraShake()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector2 randomPos = Random.insideUnitCircle * 0.5f;
            Camera.main.transform.position = new Vector3(randomPos.x, randomPos.y, originalCamPos.z);
            yield return null;
        }
        Camera.main.transform.position = originalCamPos;
    }

    private void ReloadLevel()
    {
        // Immediately reload the scene without delay
        Time.timeScale = 1;
        SceneManager.LoadScene("Game 4 Line Runner");
    }

    IEnumerator FetchTopScorers()
    {
        string url = "https://ourgoodguide.com/MobileProject/get_top_scorers.php"; // Change to your actual PHP file URL
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;
                Debug.Log("Received JSON: " + json);

                if (!string.IsNullOrEmpty(json))
                {
                    HighScoreArray topScorers = JsonUtility.FromJson<HighScoreArray>("{\"scores\":" + json + "}");

                    if (topScorers.scores.Length > 0)
                    {
                        // Find the highest score
                        int highestScore = topScorers.scores[0].score;
                        List<string> topPlayers = new List<string>();

                        foreach (HighScoreData scorer in topScorers.scores)
                        {
                            if (scorer.score == highestScore)
                            {
                                topPlayers.Add(scorer.player_name);
                            }
                        }

                        // Display based on number of top scorers
                        if (topPlayers.Count == 1)
                        {
                            // If there is only one top player, use the format with the colon
                            if (score <= currentHighScore)
                            {
                                // Show the player and their high score
                                highScoreText.text = $"Top Score: {topPlayers[0]}: {highestScore}";
                            }
                            else
                            {
                                // If the score surpasses the high score, avoid double colon and just show the score
                                highScoreText.text = $"Top Score: {topPlayers[0]} {highestScore}";
                            }
                            currentHighScorePlayer = topPlayers[0]; // Store the top player's name
                        }
                        else
                        {
                            // If there are multiple top players, show the format without a colon after players' names
                            highScoreText.text = $"Top Scorers: {string.Join(", ", topPlayers)} {highestScore}";
                            currentHighScorePlayer = topPlayers[0]; // Store the top player's name (or choose one from the list)
                        }
                        // Store the highest score in currentHighScore
                        currentHighScore = highestScore;
                    }
                    else
                    {
                        highScoreText.text = "No high scores yet.";
                    }
                }
                else
                {
                    highScoreText.text = "No high scores yet.";
                }
            }
            else
            {
                Debug.LogError("Error fetching top scorers: " + www.error);
                highScoreText.text = "Failed to load scores.";
            }
        }
    }

    [System.Serializable]
    public class HighScoreData
    {
        public string player_name;
        public int score;
    }

    [System.Serializable]
    public class HighScoreArray
    {
        public HighScoreData[] scores;
    }
}
