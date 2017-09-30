namespace iBingo.Views
{
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using iBingo.Domains;
    using iBingo.Domains.Configurations;
    using iBingo.Presentations.Services;
    using iBingo.Services;
    using iBingo.ViewModels;

    /// <summary>
    /// Shell.xaml の相互作用ロジック
    /// </summary>
    public partial class Shell : Window
    {
        #region Fields

        private ShellViewModel _vm;

        #endregion

        #region Ctor

        public Shell()
        {
            InitializeComponent();

            // TODO ここはあとでConfig を読み込んで設定する。
            var servicesProvider = new ServicesProvider(new SystemControlService(() => Application.Current.Shutdown()));
            var dialogService = new DialogService();
            _vm = new ShellViewModel(servicesProvider,
                dialogService,
                new DataStore(new ApplicationConfig
                {
                    Shuffle = new ShuffleConfig
                    { Minimum = 1, Maximum = 75 }
                }, null));
            DataContext = _vm;
        }

        #endregion

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            _vm.SaveData();
        }
    }
}