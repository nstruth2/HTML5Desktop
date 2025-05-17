using UnityEngine;

public class OrientationLockLandscape : MonoBehaviour
{
    void Start()
    {
        // Step 1: Force landscape orientation
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // Step 2: Optionally delay a frame to ensure it applies properly
        StartCoroutine(LockToLandscapeAfterFrame());
    }

    private System.Collections.IEnumerator LockToLandscapeAfterFrame()
    {
        // Wait one frame so Unity can complete orientation change
        yield return null;

        // Step 3: Lock orientation by disabling auto-rotation
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        Screen.orientation = ScreenOrientation.LandscapeLeft; // Re-set to enforce lock
    }
}
