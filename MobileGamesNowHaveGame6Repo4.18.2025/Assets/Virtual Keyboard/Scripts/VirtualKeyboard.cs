using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VirtualKeyboard : MonoBehaviour
{
    public InputField targetInputField;
    public GameObject keyButtonPrefab;
    public Transform keysParent;

    private string[] keys = new string[] {
        "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P",
        "A", "S", "D", "F", "G", "H", "J", "K", "L",
        "Z", "X", "C", "V", "B", "N", "M", "Space", "Backspace", "OK"
    };

    void Start()
    {
        foreach (string key in keys)
        {
            string capturedKey = key; // capture key for closure
            GameObject keyObj = Instantiate(keyButtonPrefab, keysParent);
            keyObj.GetComponentInChildren<Text>().text = key;
            keyObj.GetComponent<Button>().onClick.AddListener(() => OnKeyPressed(capturedKey));
        }
    }

    void OnKeyPressed(string key)
    {
        if (key == "Backspace")
        {
            if (targetInputField != null && targetInputField.text.Length > 0)
                targetInputField.text = targetInputField.text.Substring(0, targetInputField.text.Length - 1);
        }
        else if (key == "Space")
        {
            if (targetInputField != null)
                targetInputField.text += " ";
        }
        else if (key == "OK")
        {
            // Handle OK pressed (for example, submit or hide keyboard)
            Debug.Log("OK pressed. Input submitted: " + targetInputField?.text);
            gameObject.SetActive(false); // hide the keyboard
        }
        else
        {
            if (targetInputField != null)
                targetInputField.text += key;
        }
    }
}
