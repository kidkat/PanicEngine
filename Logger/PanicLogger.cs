using System;

namespace PanicEngine.Logger
{
    public static class PanicLogger
    {
        public static Action<string> LogHandler { get; set; } = Console.WriteLine;

        public static void Debug(string message)
        {
            if(LogHandler != null)
                LogHandler($"[PanicEngine] {message}");
        }

        public static void Info(string message)
        {
            if(LogHandler != null)
                LogHandler($"[PanicEngine][INFO] {message}");
        }

        public static void Warning(string message)
        {
            if(LogHandler != null)
                LogHandler($"[PanicEngine][WARN] {message}");
        }

        public static void Error(string message)
        {
            if(LogHandler != null)
                LogHandler($"[PanicEngine][ERROR] {message}");
        }
    }
}