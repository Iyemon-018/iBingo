namespace iBingo.Presentations.ComponentModels
{
    using iBingo.Presentations.Services;

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
        }

        public abstract string Title { get; }

        protected IDialogService DialogService => _dialogService;

        protected IDataStore DataStore => _dataStore;
    }
}