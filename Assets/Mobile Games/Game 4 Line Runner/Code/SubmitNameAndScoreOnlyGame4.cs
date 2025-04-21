using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SubmitNameAndScoreOnlyGame4 : MonoBehaviour
{
    public InputField playerNameInput;
    public Button submitButton;

    private string submitScoreURL = "https://ourgoodguide.com/MobileProject/submit_score.php";
    private int score = 0;

    private void Start()
    {
        // Get the score stored in PlayerPrefs from previous scene
        score = PlayerPrefs.GetInt("Game4_SubmitScore", 0);
        submitButton.onClick.AddListener(OnSubmitClicked);
    }

    private void OnSubmitClicked()
    {
        string playerName = playerNameInput.text;
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("Player name is required.");
            return;
        }

        submitButton.interactable = false;
        StartCoroutine(SubmitScoreAndLoadMainMenu(playerName, score));
    }

    private IEnumerator SubmitScoreAndLoadMainMenu(string playerName, int score)
    {
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
            }
        }

        // Optionally clear the stored score
        PlayerPrefs.DeleteKey("Game4_SubmitScore");

        // Load the main menu scene
        SceneManager.LoadScene("Main Menu Game 4");
    }
}
