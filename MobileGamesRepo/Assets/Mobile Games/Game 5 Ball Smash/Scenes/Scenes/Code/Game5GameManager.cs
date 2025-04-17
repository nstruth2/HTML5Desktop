using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game5GameManager : MonoBehaviour
{
    public GameObject winText;
    public GameObject restartButton;
    int score = 0;
    // Update is called once per frame
    void Update()
    {
        
    }
    public void ScoreUp()
    {
        score++;
        if(score >= 4)
        {
            Win();
        }
        void Win()
        {
            winText.SetActive(true);
            restartButton.SetActive(true);
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene("Game 5 Ball Smash");
    }
}