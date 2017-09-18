namespace iBingo.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using iBingo.Commands;
    using iBingo.Presentations.ComponentModels;
    using iBingo.Presentations.Models;
    using iBingo.Presentations.Services;
    using iBingo.Presentations.Utility;

    public class ShellViewModel : WindowViewModelBase
    {
        #region Fields

        private readonly ShuffleValues _shuffleValues;

        private int _currentNumber;

        private bool _shuffling;

        #endregion

        #region Ctor

        public ShellViewModel(IServicesProvider servicesProvider, IDialogService dialogService, IDataStore dataStore)
            : base(servicesProvider,
                dialogService,
                dataStore)
        {
            ShuffleCommand = new RelayCommand(ExecuteShuffleCommand, CanExecuteShuffleCommand);
            StopCommand = new RelayCommand(ExecuteStopCommand, CanExecuteStopCommand);
            HitNumbers = dataStore.HitNumbers;
            _shuffleValues = new ShuffleValues(dataStore.Config.Shuffle.Minimum, dataStore.Config.Shuffle.Maximum, OnShufflingValue);
        }

        #endregion

        #region Properties

        public override string Title => "iBingo";

        public ObservableCollection<NumberData> HitNumbers { get; private set; }

        public ICommand ShuffleCommand { get; private set; }

        public ICommand StopCommand { get; private set; }

        public bool Shuffling
        {
            get => _shuffling;
            private set => SetProperty(ref _shuffling, value);
        }

        public int CurrentNumber
        {
            get => _currentNumber;
            private set => SetProperty(ref _currentNumber, value);
        }

        #endregion

        #region Methods

        private bool CanExecuteShuffleCommand()
        {
            return !Shuffling;
        }

        private bool CanExecuteStopCommand()
        {
            return Shuffling;
        }

        private async void ExecuteShuffleCommand()
        {
            Shuffling = true;
            var hitNumber = await _shuffleValues.Shuffle(DataStore.HitNumbers);
            CurrentNumber = hitNumber.Number;
            DataStore.HitNumbers.Add(hitNumber);
        }

        private void ExecuteStopCommand()
        {
            _shuffleValues.Stop();
            Shuffling = false;
        }

        private void OnShufflingValue(int value)
        {
            CurrentNumber = value;
        }

        #endregion
    }
}