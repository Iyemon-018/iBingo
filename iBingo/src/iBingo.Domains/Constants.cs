namespace iBingo.Domains
{
    using System;
    using System.IO;

    public static class Constants
    {
        public static readonly string HistoryDirectoryName = "Histories";

        public static string GetHistoryFileName() => Path.Combine(HistoryDirectoryName, $"{DateTime.Now:yyyy-MM-dd_HHmmss}.csv");
    }
}