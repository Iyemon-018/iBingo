namespace iBingo.Domains.Logs
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;

    public enum LogLevel
    {
        Trace,

        Debug,

        Info,

        Error,

        Fatal,
    }

    public static class Log
    {
        public static readonly string OutputDirectory = "";

        public static readonly string OutputFileName = "";

        public static void Output(LogLevel level,
                                  string message,
                                  [CallerFilePath] string filePath = null,
                                  [CallerMemberName] string memberName = null,
                                  [CallerLineNumber] int lineNumber = 0)
        {
            // TODO Exchange NLog.
            Console.WriteLine($"{DateTime.Now}|{level.ToString().ToUpper()}|{Path.GetFileName(filePath)}/{memberName}/Line:{lineNumber}|{message}");
        }

        public static void Trace(string message,
                                 [CallerFilePath] string filePath = null,
                                 [CallerMemberName] string memberName = null,
                                 [CallerLineNumber] int lineNumber = 0)
        {
            Output(LogLevel.Trace, message, filePath, memberName, lineNumber);
        }

        public static void Debug(string message,
                                 [CallerFilePath] string filePath = null,
                                 [CallerMemberName] string memberName = null,
                                 [CallerLineNumber] int lineNumber = 0)
        {
            Output(LogLevel.Debug, message, filePath, memberName, lineNumber);
        }

        public static void Info(string message,
                                [CallerFilePath] string filePath = null,
                                [CallerMemberName] string memberName = null,
                                [CallerLineNumber] int lineNumber = 0)
        {
            Output(LogLevel.Info, message, filePath, memberName, lineNumber);
        }

        public static void Error(string message,
                                 [CallerFilePath] string filePath = null,
                                 [CallerMemberName] string memberName = null,
                                 [CallerLineNumber] int lineNumber = 0)
        {
            Output(LogLevel.Error, message, filePath, memberName, lineNumber);
        }

        public static void Error(string message,
                                 Exception exception,
                                 [CallerFilePath] string filePath = null,
                                 [CallerMemberName] string memberName = null,
                                 [CallerLineNumber] int lineNumber = 0)
        {
            Output(LogLevel.Error, message, filePath, memberName, lineNumber);
            Output(LogLevel.Error, $"[Message]{exception.Message}", filePath, memberName, lineNumber);
            Output(LogLevel.Error, $"{exception.StackTrace}", filePath, memberName, lineNumber);
        }

        public static void Fatal(string message,
                                 Exception exception,
                                 [CallerFilePath] string filePath = null,
                                 [CallerMemberName] string memberName = null,
                                 [CallerLineNumber] int lineNumber = 0)
        {
            Output(LogLevel.Fatal, message, filePath, memberName, lineNumber);
            Output(LogLevel.Fatal, $"[Message]{exception.Message}", filePath, memberName, lineNumber);
            Output(LogLevel.Fatal, $"{exception.StackTrace}", filePath, memberName, lineNumber);
        }
    }
}