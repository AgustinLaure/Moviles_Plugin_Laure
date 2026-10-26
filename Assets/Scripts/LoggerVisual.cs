using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoggerVisual : MonoBehaviour
{
#if UNITY_ANDROID
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private GameObject savedLogsGameObject;
    [SerializeField] private GameObject thrownLogsGameObject;
    [SerializeField] private LoggerTest loggerTest;

    private List<GameObject> savedLogs = new List<GameObject>();

    int currentLogsLoaded = 0;

    private void Update()
    {
        if (loggerTest == null || loggerTest.loggerManagerInstance == null)
        {
            return;
        }

        string[] logs = loggerTest.loggerManagerInstance.Call<string[]>("GetLogs");

        if (logs.Length != currentLogsLoaded)
        {
            for (int i = currentLogsLoaded; i < logs.Length; i++)
            {
                HandleLogReceive(logs[i]);
            }

            currentLogsLoaded = logs.Length;
        }
    }

    private void HandleLogReceive(string msg)
    {
        GameObject newText;

        if (thrownLogsGameObject != null)
        {
            newText = Instantiate(textPrefab, thrownLogsGameObject.transform, false);

            if (newText.TryGetComponent<TMP_Text>(out TMP_Text text1))
            {
                text1.text = msg;
            }
        }

        newText = Instantiate(textPrefab, savedLogsGameObject.transform, false);

        if (newText.TryGetComponent<TMP_Text>(out TMP_Text text2))
        {
            text2.text = msg;
        }

        Canvas.ForceUpdateCanvases();

        if (thrownLogsGameObject != null)
        {
            if (thrownLogsGameObject.TryGetComponent<RectTransform>(out RectTransform rectThrown))
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectThrown);
            }
        }

        if (savedLogsGameObject.TryGetComponent<RectTransform>(out RectTransform rectSaved))
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectSaved);
        }

        savedLogs.Add(newText);
    }

    public void HandleClearButtonPress(string msg)
    {
        foreach (GameObject log in savedLogs)
        {
            if (log != null)
            {
                Destroy(log);
            }
        }

        savedLogs.Clear();

        Canvas.ForceUpdateCanvases();

        if (savedLogsGameObject.TryGetComponent<RectTransform>(out RectTransform rectTransform))
        {
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }

        currentLogsLoaded = 0;
    }
#endif
}