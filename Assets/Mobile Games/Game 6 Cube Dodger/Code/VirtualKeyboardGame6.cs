using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VirtualKeyboardGame6 : MonoBehaviour
{
    public InputField targetInputField;
    public GameObject keyButtonPrefab;
    public Transform keysParent;
    public SubmitNameAndScoreOnlyGame6 submitHandler;

    private bool showingSymbols = false;
    private bool capsLock = true;

    private string[] letterKeysBase = new string[] {
        "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P",
        "A", "S", "D", "F", "G", "H", "J", "K", "L",
        "Z", "X", "C", "V", "B", "N", "M",
        "Caps", "123", "Space", "Backspace", "OK"
    };

    private string[] symbolKeys = new string[] {
        "1", "2", "3", "4", "5", "6", "7", "8", "9", "0",
        "!", "@", "#", "$", "%", "^", "&", "*", "(",
        ")", "-", "_", "=", "+", "/", "?",
        "ABC", "Space", "Backspace", "OK"
    };

    private List<GameObject> instantiatedKeys = new List<GameObject>();

    void Start()
    {
        BuildKeyboard();
    }

    void BuildKeyboard()
    {
        // Clear existing keys
        foreach (GameObject key in instantiatedKeys)
        {
            Destroy(key);
        }
        instantiatedKeys.Clear();

        // Pick layout
        string[] layout = showingSymbols ? symbolKeys : letterKeysBase;

        foreach (string key in layout)
        {
            string displayKey = key;

            if (!showingSymbols && key.Length == 1 && char.IsLetter(key[0]))
            {
                // Adjust case for letters
                displayKey = capsLock ? key.ToUpper() : key.ToLower();
            }
            else if (key == "Caps")
            {
                displayKey = capsLock ? "Caps (ON)" : "Caps (off)";
            }

            string capturedKey = key;
            GameObject keyObj = Instantiate(keyButtonPrefab, keysParent);
            keyObj.GetComponentInChildren<Text>().text = displayKey;
            keyObj.GetComponent<Button>().onClick.AddListener(() => OnKeyPressed(capturedKey));
            instantiatedKeys.Add(keyObj);
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
            if (submitHandler != null)
            {
                string playerName = targetInputField.text;
                if (!string.IsNullOrEmpty(playerName))
                {
                    int score = PlayerPrefs.GetInt("Game6_SubmitScore", 0);
                    submitHandler.StartCoroutine(submitHandler.SubmitScoreAndCheckRank(playerName, score));
                }
                else
                {
                    Debug.Log("Player name is required.");
                }
            }
        }
        else if (key == "123")
        {
            showingSymbols = true;
            BuildKeyboard();
        }
        else if (key == "ABC")
        {
            showingSymbols = false;
            BuildKeyboard();
        }
        else if (key == "Caps")
        {
            capsLock = !capsLock;
            BuildKeyboard();
        }
        else
        {
            string inputChar = key;
            if (!showingSymbols && key.Length == 1 && char.IsLetter(key[0]))
            {
                inputChar = capsLock ? key.ToUpper() : key.ToLower();
            }

            if (targetInputField != null)
            {
                targetInputField.text += inputChar;
                targetInputField.MoveTextEnd(false);
                targetInputField.ActivateInputField();
            }
        }
    }

}
