package com.example.proj_logger;
import android.util.Log;

public class LoggerManager
{
    private static final LoggerManager instance = new LoggerManager();
    public static LoggerManager GetInstance() {return instance;}

    public long GetTime()
    {
        return System.currentTimeMillis();
    }

    public void SendLog(String msg)
    {
        Log.i( "Log", msg);
    }

}
