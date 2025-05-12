using UnityEngine;
using UnityEngine.UI;

public class OrientationWarning : MonoBehaviour
{
    public GameObject warningPanel;
    public Text warningText;

    public void ShowWarning(string requiredOrientation)
    {
        warningPanel.SetActive(true);
        warningText.text = $"Please rotate your device to {requiredOrientation} mode.";
    }

    public void HideWarning()
    {
        warningPanel.SetActive(false);
    }
}
