using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiCheat : MonoBehaviour
{
    void Awake()
    {
        Application.runInBackground = true;
    }
}
