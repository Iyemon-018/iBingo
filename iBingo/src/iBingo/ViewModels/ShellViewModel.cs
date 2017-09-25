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
            CheckCommnad = new RelayCommand(ExecuteCheckCommand);
            BackCommand = new RelayCommand(ExecuteBackCommand);
            HitNumbers = dataStore.HitNumbers;
            _shuffleValues = new ShuffleValues(dataStore.Config.Shuffle.Minimum, dataStore.Config.Shuffle.Maximum, OnShufflingValue);
        }

        private void ExecuteBackCommand()
        {
            VisibleShuffleView = true;
            VisibleHitNumbersView = false;
        }

        private void ExecuteCheckCommand()
        {
            VisibleShuffleView = false;
            VisibleHitNumbersView = true;
        }

        #endregion

        #region Properties

        public override string Title => "iBingo";

        public ObservableCollection<NumberData> HitNumbers { get; private set; }

        public ICommand ShuffleCommand { get; private set; }

        public ICommand StopCommand { get; private set; }

        public ICommand CheckCommnad { get; private set; }

        public ICommand BackCommand { get; private set; }

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
        
        private bool _visibleShuffleView = true;
        
        public bool VisibleShuffleView
        {
            get => _visibleShuffleView;
            set => SetProperty(ref _visibleShuffleView, value);
        }



        private bool _visibleHitNumbersView;
        
        public bool VisibleHitNumbersView
        {
            get => _visibleHitNumbersView;
            set => SetProperty(ref _visibleHitNumbersView, value);
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