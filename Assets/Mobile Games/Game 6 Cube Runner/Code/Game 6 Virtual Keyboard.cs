using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Game6VirtualKeyboard : MonoBehaviour
{
    public InputField targetInputField;
    public GameObject keyButtonPrefab;
    public Transform keysParent;
    public GameObject okButtonPrefab;  // New reference for the OK button

    private string[] keys = new string[] {
        "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P",
        "A", "S", "D", "F", "G", "H", "J", "K", "L",
        "Z", "X", "C", "V", "B", "N", "M", "Space", "Backspace"
    };

    void Start()
    {
        // Instantiate key buttons
        foreach (string key in keys)
        {
            GameObject keyObj = Instantiate(keyButtonPrefab, keysParent);
            keyObj.GetComponentInChildren<Text>().text = key;
            keyObj.GetComponent<Button>().onClick.AddListener(() => OnKeyPressed(key));
        }

        // Instantiate the OK button
        GameObject okButtonObj = Instantiate(okButtonPrefab, keysParent);
        okButtonObj.GetComponentInChildren<Text>().text = "OK";  // Set the text to "OK"
        okButtonObj.GetComponent<Button>().onClick.AddListener(OnOKPressed);
    }

    void OnKeyPressed(string key)
    {
        if (targetInputField == null) return;

        if (key == "Backspace")
        {
            if (targetInputField.text.Length > 0)
                targetInputField.text = targetInputField.text.Substring(0, targetInputField.text.Length - 1);
        }
        else if (key == "Space")
        {
            targetInputField.text += " ";
        }
        else
        {
            targetInputField.text += key;
        }
    }

    void OnOKPressed()
    {
        if (targetInputField != null)
        {
            string playerName = targetInputField.text;
            Debug.Log("OK button pressed! Name entered: " + playerName);

            // Call GameManager and pass the name
            Game6GameManager.instance.OnNameEntered(playerName);
        }
    }
}
