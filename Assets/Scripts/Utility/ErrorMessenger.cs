using System;
using UnityEngine;

public class ErrorMessenger : MonoBehaviour
{
    public static void DisplayMessage(string message)
    {
        Debug.Log($"{DateTime.Now} >> {message}");
    }
}