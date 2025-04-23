using UnityEditor;
using UnityEngine;

public class ClearPlayerPrefsEditor : EditorWindow
{
    [MenuItem("Tools/Clear PlayerPrefs")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("All PlayerPrefs deleted!");
    }
}