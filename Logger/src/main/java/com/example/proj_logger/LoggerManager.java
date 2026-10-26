package com.example.proj_logger;

import android.util.Log;
import android.content.Context;
import com.unity3d.player.UnityPlayer;

import android.app.AlertDialog;
import android.content.DialogInterface;

import java.util.ArrayList;
import java.util.List;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;

public class LoggerManager
{
    private static final String fileName = "logs.txt";
    private static final LoggerManager instance = new LoggerManager();

    private List<String> logs = new ArrayList<>();

    public static LoggerManager GetInstance()
    {
        return instance;
    }

    public long GetTime()
    {
        return System.currentTimeMillis();
    }

    public void SendLog(String msg)
    {
        Log.i( "Log", msg);
        logs.add(msg);

        try
        {
        Context context = UnityPlayer.currentActivity.getApplicationContext();

        File file = new File(context.getFilesDir(), fileName);
        FileWriter writer = new FileWriter(file, true);
        writer.append(msg).append("\n");
        writer.flush();
        writer.close();
        }
        catch(IOException e)
        {
            Log.e("Log", "Failed to write log: " + e.getMessage());
        }
    }

    public void DeleteLogs()
    {
        UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
            @Override
            public void run()
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(UnityPlayer.currentActivity);
                builder.setTitle("Delete logs");
                builder.setMessage("Are you sure you want to delete all logs?");

                builder.setPositiveButton("Yes", new DialogInterface.OnClickListener()
                {
                    @Override
                    public void onClick(DialogInterface dialog, int which)
                    {
                        logs.clear();

                        try
                        {
                            Context context = UnityPlayer.currentActivity.getApplicationContext();
                            File file = new File(context.getFilesDir(), fileName);

                            if (file.exists())
                            {
                                FileWriter writer = new FileWriter(file, false);
                                writer.write("");
                                writer.close();
                            }
                        }
                        catch (IOException e)
                        {
                            Log.e("Log", "Failed to clear log file: " + e.getMessage());
                        }

                        UnityPlayer.UnitySendMessage("LoggerVisual", "HandleClearButtonPress", "");
                    }
                });

                builder.setNegativeButton("No", new DialogInterface.OnClickListener()
                {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        dialog.dismiss();
                    }
                });

                builder.show();
            }
        });
    }

    public String[] GetLogs()
    {
        return logs.toArray(new String[0]);
    }

}
