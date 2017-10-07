namespace iBingo.Presentations.Services
{
    public enum WarningChoice
    {
        Positive,
        Negative,
        Retry,
    }

    public interface IDialogService
    {
        void Information(string caption, string message);

        bool Question(string caption, string message, bool defaultSelectButton);

        WarningChoice Warning(string caption, string message, WarningChoice defaultChoice);

        void Error(string caption, string message);

        string OpenFile(string filter, string initialDirectory);
    }
}