using UnityEngine;
using System.Runtime.InteropServices;

public class CanvasResizer : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ResizeCanvasForUnitySceneReload();

    void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        ResizeCanvasForUnitySceneReload();
#endif
    }
}
