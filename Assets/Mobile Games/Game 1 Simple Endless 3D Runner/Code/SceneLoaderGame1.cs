using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderGame1 : MonoBehaviour
{
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("Main Menu Game 1");
    }
}
