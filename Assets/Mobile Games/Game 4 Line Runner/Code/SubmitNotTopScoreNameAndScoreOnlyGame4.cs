using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SubmitNotTopScoreNameAndScoreOnlyGame4 : MonoBehaviour
{
    public InputField playerNameInput;
    public Button submitButton;
    public Text rankText;  // A Text UI element to display the rank

    private string submitScoreURL = "https://ourgoodguide.com/MobileProject/ScoreandNameSubmission/submit_score_not_top_game_4.php";
    private string checkRankURL = "https://ourgoodguide.com/MobileProject/check_rank_game_4.php"; // URL to check the player's rank
    private int score = 0;

    private void Start()
    {
        // Get the score stored in PlayerPrefs from previous scene
        score = PlayerPrefs.GetInt("Game4_SubmitScore", 0);
        submitButton.onClick.AddListener(OnSubmitClicked);
    }

    public void OnSubmitClicked()
    {
        string playerName = playerNameInput.text;
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("Player name is required.");
            return;
        }

        submitButton.interactable = false;
        StartCoroutine(SubmitScoreAndCheckRank(playerName, score));
    }

    public IEnumerator SubmitScoreAndCheckRank(string playerName, int score)
    {
        // Submit the score to the server
        WWWForm form = new WWWForm();
        form.AddField("player_name", playerName);
        form.AddField("score", score);

        using (UnityWebRequest www = UnityWebRequest.Post(submitScoreURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Score submitted successfully: " + www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Error submitting score: " + www.error);
                rankText.text = "Error submitting score. Please try again.";
                yield break;
            }
        }

        // Optionally clear the stored score
        PlayerPrefs.DeleteKey("Game4_SubmitScore");

        // Now, check the player's rank after submission
        yield return StartCoroutine(CheckPlayerRank(playerName, score));

        // Load the main menu scene after submission and rank check
        SceneManager.LoadScene("Main Menu Game 4");
    }

    public IEnumerator CheckPlayerRank(string playerName, int score)
    {
        // Send the player name and score to check the rank
        WWWForm form = new WWWForm();
        form.AddField("player_name", playerName);
        form.AddField("score", score);

        using (UnityWebRequest www = UnityWebRequest.Post(checkRankURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string response = www.downloadHandler.text;
                Debug.Log("Rank check response: " + response);

                // Assuming the response is a simple integer rank or a message
                if (int.TryParse(response, out int rank))
                {
                    rankText.text = $"Your rank: {rank}";
                }
                else
                {
                    rankText.text = "Error retrieving rank.";
                }
            }
            else
            {
                Debug.Log("Error checking rank: " + www.error);
                rankText.text = "Error retrieving rank. Please try again.";
            }
        }
    }
}
