using UnityEngine;
using System.Runtime.InteropServices;

public class CanvasResizer : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void ResizeCanvasForUnitySceneReload();
#endif

    void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        ResizeCanvasForUnitySceneReload();
#endif
    }
}
