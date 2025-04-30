using System.Runtime.InteropServices;
using UnityEngine;

public class KeyboardPlatformManagerGame2 : MonoBehaviour
{
    public GameObject keysParent;
    public GameObject submitScoreAndNameButton;
    public GameObject nameInstructionText; // ⬅️ This is the new reference

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern int IsDesktopPlatform();
#endif

    void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (IsDesktopPlatform() == 1)
        {
            if (keysParent != null)
                keysParent.SetActive(false);

            if (submitScoreAndNameButton != null)
                submitScoreAndNameButton.SetActive(true);
            if (nameInstructionText != null)
                nameInstructionText.SetActive(false); // ⬅️ Hide instruction text
        }
#endif
    }
}
