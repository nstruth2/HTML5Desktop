using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SubmitNameAndScoreOnlyGame5 : MonoBehaviour
{
    public InputField playerNameInput;
    public Button submitButton;
    public Button continueButton;
    public Button retryButton; // 🔹 New retry button
    public Text rankText;

    private string submitScoreURL = "https://ourgoodguide.com/MobileProject/ScoreandNameSubmission/submit_time_game_5.php";
    private string checkRankURL = "https://ourgoodguide.com/MobileProject/CheckRank/check_rank_game_5.php";
    private string time_raw = "";

    private void Start()
    {
        time_raw = PlayerPrefs.GetString("Game5_SubmitTimeRaw", PlayerPrefs.GetString("Game5_SubmitTime", ""));

        submitButton.onClick.AddListener(OnSubmitClicked);
        continueButton.onClick.AddListener(OnContinueClicked);
        retryButton.onClick.AddListener(OnRetryClicked); // 🔹 Add retry listener

        if (continueButton == null) Debug.LogError("Continue button is NULL!");
        if (retryButton == null) Debug.LogError("Retry button is NULL!");

        // 🔹 Hide buttons initially
        continueButton.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
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
        StartCoroutine(SubmitScoreAndCheckRank(playerName, time_raw));
    }

    public IEnumerator SubmitScoreAndCheckRank(string playerName, string time_raw)
    {
        // Submit time_raw
        WWWForm submitForm = new WWWForm();
        submitForm.AddField("player_name", playerName);
        submitForm.AddField("time_raw", PlayerPrefs.GetString("Game5_SubmitTimeRaw", "0"));

        using (UnityWebRequest submitRequest = UnityWebRequest.Post(submitScoreURL, submitForm))
        {
            yield return submitRequest.SendWebRequest();

            if (submitRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error submitting time_raw: " + submitRequest.error);
                rankText.text = "Error submitting time_raw. Please try again.";
                submitButton.interactable = true;
                yield break;
            }
            else
            {
                Debug.Log("time_raw submitted successfully: " + submitRequest.downloadHandler.text);
            }
        }

        // Check rank
        WWWForm rankForm = new WWWForm();
        rankForm.AddField("time_raw", time_raw);

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

        // 🔹 Show both buttons
        continueButton.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);

        PlayerPrefs.DeleteKey("Game5_SubmitTime");
    }

    public void OnContinueClicked()
    {
        SceneManager.LoadScene("Menu Game 5");
    }

    public void OnRetryClicked()
    {
        SceneManager.LoadScene("Gameplay Game 5"); // 🔹 Correct retry scene
    }
}
