using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MainMenuScriptGame4 : MonoBehaviour
{
    [Header("UI Elements")]
    public Text titleText;
    public Text topScoreText;
    public Text scoreText;
    public Text livesText;
    public GameObject player;  // Player object (could be the player's avatar or character)
    public GameObject line;    // Line object (could be part of the UI or a graphic element)
    public Button startButton;  // Reference to the Start button
    public Button exitButton;   // Reference to the Exit button
    
    private string topScoreURL = "https://ourgoodguide.com/MobileProject/submit_score.php";  // URL to your PHP script for fetching top scores

    private int currentScore = 0;  // Default current score
    private int lives = 3;         // Default lives

    void Start()
    {
        // Set initial UI values
        titleText.text = "Line Runner"; // Replace with your actual game title
        scoreText.text = "Score: " + currentScore.ToString();
        livesText.text = "Lives: " + lives.ToString();
        
        // Start fetching top score from the database
        StartCoroutine(GetTopScore());
        startButton.onClick.AddListener(StartGame);  // Start the game
        exitButton.onClick.AddListener(ExitGame);    // Exit the game
    }

    // Coroutine to fetch the top score from the database via the PHP script
    IEnumerator GetTopScore()
    {
        UnityWebRequest www = UnityWebRequest.Get(topScoreURL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error fetching top score: " + www.error);
        }
        else
        {
            // Parse the JSON response
            string jsonResponse = www.downloadHandler.text;

            // Handle the response
            if (jsonResponse == "[]")
            {
                topScoreText.text = "No top scores available.";
            }
            else
            {
                // Deserialize the JSON response to an array of top scores
                TopScore[] topScores = JsonHelper.FromJson<TopScore>(jsonResponse);

                // Display the top score (you can modify this based on your UI)
                if (topScores.Length > 0)
                {
                    string scoresText = "Top Score: " + topScores[0].score + " - " + topScores[0].player_name;
                    topScoreText.text = scoresText;
                }
                else
                {
                    topScoreText.text = "No top scores available.";
                }
            }
        }
    }
    void StartGame()
    {
        // Assuming the first scene is "GameScene"
        SceneManager.LoadScene("GameplayGame4"); // Replace "GameScene" with your actual scene name
    }

    // Method for exiting the game
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
}

// Helper class to match the structure of the JSON response
[System.Serializable]
public class TopScore
{
    public string player_name;
    public int score;
}

// Utility class to help with parsing JSON arrays
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{ \"items\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.items;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }
}
