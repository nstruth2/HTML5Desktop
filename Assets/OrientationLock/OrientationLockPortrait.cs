using UnityEngine;

public class OrientationLockPortrait : MonoBehaviour
{
    void Start()
    {
        // Step 1: Force portrait orientation
        Screen.orientation = ScreenOrientation.Portrait;

        // Step 2: Optionally delay a frame to ensure it applies properly
        StartCoroutine(LockToPortraitAfterFrame());
    }

    private System.Collections.IEnumerator LockToPortraitAfterFrame()
    {
        // Wait one frame so Unity can complete orientation change
        yield return null;

        // Step 3: Lock orientation by disabling auto-rotation
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        Screen.orientation = ScreenOrientation.Portrait; // Re-set to enforce lock
    }
}
