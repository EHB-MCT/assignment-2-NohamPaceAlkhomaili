using System.IO;
using UnityEngine;

public static class DataStorageHandler
{
    private static string filePath = Application.persistentDataPath + "/InputDataLog.txt";

    // Save input data to a file
    public static void SaveInputData(string inputType)
    {
        string logEntry = $"{System.DateTime.Now}: {inputType} pressed\n";
        File.AppendAllText(filePath, logEntry);
        Debug.Log("Data saved: " + logEntry);
    }
}
