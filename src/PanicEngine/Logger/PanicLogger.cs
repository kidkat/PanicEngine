using System;

namespace PanicEngine.Logger
{
    public static class PanicLogger
    {
        public static Action<string> LogHandler { get; set; } = Console.WriteLine;

        public static void Debug(string message)
        {
            LogHandler?.Invoke($"[PanicEngine] {message}");
        }

        public static void Info(string message)
        {
            LogHandler?.Invoke($"[PanicEngine][INFO] {message}");
        }

        public static void Warning(string message)
        {
            LogHandler?.Invoke($"[PanicEngine][WARN] {message}");
        }

        public static void Error(string message)
        {
            LogHandler?.Invoke($"[PanicEngine][ERROR] {message}");
        }
    }
}