using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SubmitNameAndScoreOnlyGame2 : MonoBehaviour
{
    public InputField playerNameInput;
    public Button submitButton;
    public Button continueButton; // New continue button
    public Text rankText;  

    private string submitScoreURL = "https://ourgoodguide.com/MobileProject/ScoreandNameSubmission/submit_score_game_2.php";
    private string checkRankURL = "https://ourgoodguide.com/MobileProject/CheckRank/check_rank_game_2.php"; 
    private int score = 0;

    private void Start()
    {
        score = PlayerPrefs.GetInt("Game2_SubmitScore", 0);
        submitButton.onClick.AddListener(OnSubmitClicked);
        continueButton.onClick.AddListener(OnContinueClicked);
        if (continueButton == null) Debug.LogError("Continue button is NULL!");

        // Hide Continue button at start
        continueButton.gameObject.SetActive(false);
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

        // First submit the score, then check the rank
        StartCoroutine(SubmitScoreAndCheckRank(playerName, score));
    }

    public IEnumerator SubmitScoreAndCheckRank(string playerName, int score)
    {
        // Submit score first
        WWWForm submitForm = new WWWForm();
        submitForm.AddField("player_name", playerName);
        submitForm.AddField("score", score);

        using (UnityWebRequest submitRequest = UnityWebRequest.Post(submitScoreURL, submitForm))
        {
            yield return submitRequest.SendWebRequest();

            if (submitRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error submitting score: " + submitRequest.error);
                rankText.text = "Error submitting score. Please try again.";
                submitButton.interactable = true; // Allow retry
                yield break;
            }
            else
            {
                Debug.Log("Score submitted successfully: " + submitRequest.downloadHandler.text);
            }
        }

        // After submitting, check the rank
        WWWForm rankForm = new WWWForm();
        rankForm.AddField("score", score);

        using (UnityWebRequest rankRequest = UnityWebRequest.Post(checkRankURL, rankForm))
        {
            yield return rankRequest.SendWebRequest();

            if (rankRequest.result == UnityWebRequest.Result.Success)
            {
                string response = rankRequest.downloadHandler.text;
                Debug.Log("Rank check response: " + response);

                try
                {
                    RankResponse rankResponse = JsonUtility.FromJson<RankResponse>(response);
                    rankText.text = $"Your rank: {rankResponse.rank}";
                }
                catch (System.Exception ex)
                {
                    rankText.text = "Error retrieving rank.";
                    Debug.LogError("Error parsing rank response: " + ex.Message);
                }

            }
            else
            {
                Debug.Log("Error checking rank: " + rankRequest.error);
                rankText.text = "Error retrieving rank. Please try again.";
            }
        }

        // After everything, show the Continue button
        continueButton.gameObject.SetActive(true);

        // Optionally clear stored score
        PlayerPrefs.DeleteKey("Game2_SubmitScore");
    }

    public void OnContinueClicked()
    {
        // Load the Main Menu when Continue button is clicked
        Debug.Log("Continue button clicked. Attempting to load scene...");
        SceneManager.LoadScene("Main Menu Game 2");
    }
}
