using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ConsoleToUI : MonoBehaviour
{
    public TextMeshProUGUI logText; 
    private string currentLog = "";
    private Queue<string> logQueue = new Queue<string>();
    public int maxLogs = 30;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string log = $"[{type}] {logString}";
        logQueue.Enqueue(log);

        if (logQueue.Count > maxLogs)
            logQueue.Dequeue();

        currentLog = string.Join("\n", logQueue.ToArray());
        logText.text = currentLog;
    }
}
