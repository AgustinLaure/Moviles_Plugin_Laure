using System;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LoggerTest : MonoBehaviour
{
#if UNITY_ANDROID

    [SerializeField] private Button showButton;
    [SerializeField] private Button clearButton;

    public AndroidJavaObject loggerManagerInstance;

    private readonly ConcurrentQueue<string> pendingLogs = new();

    private int logCount = 0;

    private void Start()
    {
        Application.logMessageReceived += HandleLogReceive;

        using var loggerClass = new AndroidJavaClass("com.example.proj_logger.LoggerManager");
        loggerManagerInstance = loggerClass.CallStatic<AndroidJavaObject>("GetInstance");

        showButton.onClick.AddListener(HandleShowLogsPress);
        clearButton.onClick.AddListener(HandleClearButtonPressed);
    }

    private void Update()
    {
        while (pendingLogs.TryDequeue(out var msg))
        {
            loggerManagerInstance?.Call("SendLog", msg);
        }
    }

    private void HandleShowLogsPress()
    {
        Debug.Log("Log: " + logCount);
        logCount++;
    }

    private void HandleClearButtonPressed()
    {
        loggerManagerInstance.Call("DeleteLogs");
    }

    private void HandleLogReceive(string logString, string stackTrace, LogType type)
    {
        pendingLogs.Enqueue(logString);
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= HandleLogReceive;
        showButton.onClick.RemoveListener(HandleShowLogsPress);
        clearButton.onClick.RemoveListener(HandleClearButtonPressed);
    }
#endif
}