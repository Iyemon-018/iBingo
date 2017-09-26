using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace iBingo.Views
{
    using iBingo.Domains.Configurations;
    using iBingo.Presentations.Services;
    using iBingo.Services;
    using iBingo.ViewModels;

    /// <summary>
    /// Shell.xaml の相互作用ロジック
    /// </summary>
    public partial class Shell : Window
    {
        public Shell()
        {
            InitializeComponent();

            // TODO ここはあとでConfig を読み込んで設定する。
            var servicesProvider = new ServicesProvider(new SystemControlService(() => Application.Current.Shutdown()));
            var dialogService = new DialogService();
            DataContext = new ShellViewModel(servicesProvider,
                dialogService,
                new DataStore(new ApplicationConfig { Shuffle = new ShuffleConfig { Minimum = 1, Maximum = 75, } }, null));
        }
    }
}