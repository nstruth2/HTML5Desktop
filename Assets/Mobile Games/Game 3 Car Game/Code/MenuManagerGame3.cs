using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class MenuFrom3GamePackGame2 : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Gameplay Game 3");
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}