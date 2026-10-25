using UnityEngine;

public class LoggerTest : MonoBehaviour
{
    #if UNITY_ANDROID

AndroidJavaObject loggerManagerInstance;

void Start()
{
using var loggerClass = new AndroidJavaClass("com.example.proj_logger.LoggerManager");
loggerManagerInstance = loggerClass.CallStatic<AndroidJavaObject>("GetInstance");

loggerManagerInstance.Call("SendLog", "Hola mundo");
long t = loggerManagerInstance.Call<long>("GetTime");
Debug.Log($"Hora: {t}");
}
    #endif
}
