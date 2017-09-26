namespace iBingo.Presentations.ComponentModels
{
    using System;
    using System.Windows.Input;
    using iBingo.Presentations.Services;
    using Prism.Commands;

    public abstract class WindowViewModelBase : ViewModelBase
    {
        private readonly IDialogService _dialogService;

        private readonly IServicesProvider _servicesProvider;

        private readonly IDataStore _dataStore;

        protected WindowViewModelBase(IServicesProvider servicesProvider, IDialogService dialogService, IDataStore dataStore)
        {
            _dialogService = dialogService;
            _dataStore = dataStore;
            _servicesProvider = servicesProvider;
            CloseCommand = new DelegateCommand(ExecuteCloseCommand);
        }

        protected virtual void ExecuteCloseCommand()
        {
            var close = DialogService.Question("終了オプション", $"終了します。{Environment.NewLine}よろしいですか？", false);
            if (close)
            {
                _servicesProvider.SystemControlService.Shutdown();
            }
        }

        public abstract string Title { get; }

        protected IDialogService DialogService => _dialogService;

        protected IDataStore DataStore => _dataStore;

        public ICommand CloseCommand { get; private set; }
    }
}