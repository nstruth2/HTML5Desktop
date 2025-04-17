using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControllerGame2 : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Game 2 Dodging Dog 2D");
    }
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("Global Main Menu");
    }
}
