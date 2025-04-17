using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScoresandTimesScript : MonoBehaviour
{
    public void ResetScoresandTimes()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs cleared");
    }
}
