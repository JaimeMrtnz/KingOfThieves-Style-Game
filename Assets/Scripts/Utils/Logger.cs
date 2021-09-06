
using UnityEngine;

/// <summary>
/// Helps to log messages
/// </summary>
public class Logger
{
    private string ownerName;

    public Logger(GameObject owner)
    {
        ownerName = owner.name;
    }

    public Logger(string ownerName)
    {
        this.ownerName = ownerName;
    }

    public void Log(string message)
    {
        Debug.LogFormat("[{0}] - {1}", ownerName, message);
    }

    public void Log(object message)
    {
        Debug.LogFormat("[{0}] - {1}", ownerName, message);
    }

    public void LogWarning(string message)
    {
        Debug.LogWarningFormat("[{0}] - {1}", ownerName, message);
    }

    public void LogWarning(object message)
    {
        Debug.LogWarningFormat("[{0}] - {1}", ownerName, message);
    }

    public void LogError(string message)
    {
        Debug.LogErrorFormat("[{0}] - {1}", ownerName, message);
    }

    public void LogError(object message)
    {
        Debug.LogErrorFormat("[{0}] - {1}", ownerName, message);
    }
}
