namespace iBingo.Services
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using iBingo.Presentations.Services;
    using Microsoft.Win32;

    public class DialogService : IDialogService
    {
        private static readonly Dictionary<WarningChoice, MessageBoxResult> WarningButtonResultMap
            = new Dictionary<WarningChoice, MessageBoxResult>
              {
                  {WarningChoice.Positive, MessageBoxResult.Yes},
                  {WarningChoice.Negative, MessageBoxResult.No},
                  {WarningChoice.Retry, MessageBoxResult.None},
              };

        public void Information(string caption, string message)
        {
            MessageBox.Show(Application.Current.MainWindow, message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool Question(string caption, string message, bool defaultSelectButton)
        {
            var defaultButtonType = defaultSelectButton ? MessageBoxResult.Yes : MessageBoxResult.No;
            var result = MessageBox.Show(Application.Current.MainWindow,
                message,
                caption,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                defaultButtonType);
            return result == MessageBoxResult.Yes;
        }

        public WarningChoice Warning(string caption, string message, WarningChoice defaultChoice)
        {
            throw new System.NotImplementedException();
        }

        public void Error(string caption, string message)
        {
            MessageBox.Show(Application.Current.MainWindow, message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public string OpenFile(string filter)
        {
            var dialog = new OpenFileDialog
            {
                Title = "ファイルを選択",
                InitialDirectory = Environment.CurrentDirectory,
                Filter = filter,
            };
            var result = dialog.ShowDialog();
            return result.HasValue && result.Value ? dialog.FileName : null;
        }
    }
}