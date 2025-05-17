using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SubmitNameAndScoreOnlyGame4 : MonoBehaviour
{
    public InputField playerNameInput;
    public Button submitButton;
    public Button continueButton;
    public Button retryButton; // 🔹 New retry button
    public Text rankText;

    private string submitScoreURL = "https://ourgoodguide.com/MobileProject/ScoreandNameSubmission/submit_score_game_4.php";
    private string checkRankURL = "https://ourgoodguide.com/MobileProject/CheckRank/check_rank_game_4.php";
    private int score = 0;

    private void Start()
    {
        score = PlayerPrefs.GetInt("Game4_SubmitScore", 0);

        submitButton.onClick.AddListener(OnSubmitClicked);
        continueButton.onClick.AddListener(OnContinueClicked);
        retryButton.onClick.AddListener(OnRetryClicked); // 🔹 Attach retry listener
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
        // 🔹 Submit score
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
                submitButton.interactable = true;
                yield break;
            }
            else
            {
                Debug.Log("Score submitted successfully: " + submitRequest.downloadHandler.text);
            }
        }

        // 🔹 Check rank
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

        // 🔹 Show Continue and Retry buttons
        continueButton.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);

        PlayerPrefs.DeleteKey("Game4_SubmitScore");
    }

    private void OnContinueClicked()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private void OnRetryClicked()
    {
        SceneManager.LoadScene("Gameplay Game 4"); // 🔹 Load gameplay scene
    }

    [System.Serializable]
    public class RankResponse
    {
        public int rank;
    }
}
