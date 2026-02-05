using UnityEngine;
using PanicEngine.Logger;

public static class UnityPanicLogger
{
    public static void Install()
    {
        PanicLogger.LogHandler = message =>
        {
            Debug.Log(message);
        };
    }
}