#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

using UnityEngine;

public class KeyboardControl : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void DisableKeyboardOnDesktop();
#endif

    void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        DisableKeyboardOnDesktop();
#endif
    }
}
