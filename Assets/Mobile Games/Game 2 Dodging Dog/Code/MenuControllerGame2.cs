using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControllerGame2 : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Gameplay Game 2");
    }
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
