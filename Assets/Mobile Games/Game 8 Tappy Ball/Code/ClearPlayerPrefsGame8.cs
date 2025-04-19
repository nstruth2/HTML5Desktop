using UnityEngine;

public class ClearPlayerPrefsGame8 : MonoBehaviour
{
    // Method to clear all PlayerPrefs
    public void ClearAllPlayerPrefs()
    {
            PlayerPrefs.DeleteAll(); // Clears all PlayerPrefs data
            PlayerPrefs.Save(); // Save the changes
    }
}