using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderMainMenu : MonoBehaviour
{
    public void LoadDodgingDog()
    {
        SceneManager.LoadScene("Main Menu Game 2");
    }
    public void LoadTappyBall()
    {
        SceneManager.LoadScene("StartMenuGame8");
    }
    public void LoadJumper()
    {
        SceneManager.LoadScene("Game 1 Simple Endless 3D Runner");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
