using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class OrientationReporter : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void SendSceneNameToBrowser(string sceneName);
#endif

    void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SendSceneNameToBrowser(SceneManager.GetActiveScene().name);
#endif
    }
}
