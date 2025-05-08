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
        public void LoadJumper()
    {
        SceneManager.LoadScene("Menu Game 1");
    }
    public void LoadDodgingDog()
    {
        SceneManager.LoadScene("Menu Game 2");
    }

    public void LoadCarGame()
    {
        SceneManager.LoadScene("Menu Game 3");
    }
    public void LoadLineRunner()
    {
        SceneManager.LoadScene("Menu Game 4");
    }
    public void LoadBallSmash()
    {
        SceneManager.LoadScene("Menu Game 5");
    }
    public void LoadCubeDodger()
    {
        SceneManager.LoadScene("Menu Game 6");
    }
    public void LoadBallBounce()
    {
        SceneManager.LoadScene("Menu Game 7");
    }
    public void LoadTappyBall()
    {
        SceneManager.LoadScene("Menu Game 8");
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
