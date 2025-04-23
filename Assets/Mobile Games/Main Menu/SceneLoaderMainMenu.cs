using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class SceneLoaderMainMenu : MonoBehaviour
{
    #if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void RedirectToPage(string url);
    #endif
    public void LoadDodgingDog()
    {
        SceneManager.LoadScene("Main Menu Game 2");
    }
    public void LoadTappyBall()
    {
        SceneManager.LoadScene("Start Menu Game 8");
    }
    public void LoadJumper()
    {
        SceneManager.LoadScene("Game 1 Simple Endless 3D Runner");
    }
    public void LoadLineRunner()
    {
        SceneManager.LoadScene("Main Menu Game 4");
    }
    public void Quit()
    {
    #if UNITY_WEBGL && !UNITY_EDITOR
            RedirectToPage("https://ourgoodguide.com/HTML5/MobileGamesRedirectPage.html"); // <--- Replace this URL
    #else
            Application.Quit();
    #endif
    }
}
