using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace iBingo
{
    using System.IO;
    using iBingo.Domains;
    using iBingo.ViewModels;

    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += (sender, args) => { CatchException(args.Exception); };
            AppDomain.CurrentDomain.UnhandledException += (sender, args) => { CatchException(args.ExceptionObject as Exception); };
        }

        private void CatchException(Exception exception)
        {
            if (exception != null)
            {
                var log = $"-------------------------------------------------{Environment.NewLine}"
                          + $"Exception Catch [{DateTime.Now:yyyy-M-d HH:mm:ss.fff}]{Environment.NewLine}"
                          + $"- Message{Environment.NewLine}"
                          + $"{exception.Message}{Environment.NewLine}"
                          + $"- StackTrace{Environment.NewLine}"
                          + $"{exception.StackTrace}{Environment.NewLine}";
                File.WriteAllText($"{DateTime.Now:yyyy-MM-dd_HHmmss}.log", log);
            }

            if (MainWindow != null)
            {
                var vm = MainWindow.DataContext as ShellViewModel;
                vm?.SaveData();
            }
        }
    }
}
