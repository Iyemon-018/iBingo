namespace iBingo.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using iBingo.Commands;
    using iBingo.Presentations.ComponentModels;
    using iBingo.Presentations.Models;
    using iBingo.Presentations.Services;
    using iBingo.Presentations.Utility;

    public class ShellViewModel : WindowViewModelBase
    {
        private static readonly Dictionary<int, double> NumbersFontSizeMap
            = new Dictionary<int, double>
              {
                  {0, 24.0},
                  {1, 36.0},
                  {2, 48.0},
                  {3, 60.0},
                  {4, 72.0},
                  {5, 96.0},
                  {6, 128.0},
              };

        #region Fields

        private readonly ShuffleValues _shuffleValues;

        private int _currentNumber;

        private double _numbersFontSize;

        private bool _shuffling;

        private bool _visibleHitNumbersView;

        private bool _visibleShuffleView = true;

        private int _numbersFontSizeIndex = 0;

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
            NumbersFontSizeUpCommand = new RelayCommand(ExecuteNumbersFontSizeUpCommand);
            NumbersFontSizeDownCommand = new RelayCommand(ExecuteNumbersFontSizeDownCommand);

            HitNumbers = dataStore.HitNumbers;
            _shuffleValues = new ShuffleValues(dataStore.Config.Shuffle.Minimum, dataStore.Config.Shuffle.Maximum, OnShufflingValue);
            _numbersFontSize = 32.0;
        }

        #endregion

        #region Properties

        public override string Title => "iBingo";

        public ObservableCollection<NumberData> HitNumbers { get; private set; }

        public ICommand ShuffleCommand { get; private set; }

        public ICommand StopCommand { get; private set; }

        public ICommand CheckCommnad { get; private set; }

        public ICommand BackCommand { get; private set; }

        public ICommand NumbersFontSizeUpCommand { get; private set; }

        public ICommand NumbersFontSizeDownCommand { get; private set; }

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

        public bool VisibleShuffleView
        {
            get => _visibleShuffleView;
            private set => SetProperty(ref _visibleShuffleView, value);
        }

        public bool VisibleHitNumbersView
        {
            get => _visibleHitNumbersView;
            private set => SetProperty(ref _visibleHitNumbersView, value);
        }

        public double NumbersFontSize
        {
            get => _numbersFontSize;
            private set => SetProperty(ref _numbersFontSize, value);
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

        private void ExecuteNumbersFontSizeDownCommand()
        {
            if (_numbersFontSizeIndex > 0)
            {
                _numbersFontSizeIndex--;
                NumbersFontSize = NumbersFontSizeMap[_numbersFontSizeIndex];
            }
        }

        private void ExecuteNumbersFontSizeUpCommand()
        {
            if (NumbersFontSizeMap.Count - 1 > _numbersFontSizeIndex)
            {
                _numbersFontSizeIndex++;
                NumbersFontSize = NumbersFontSizeMap[_numbersFontSizeIndex];
            }
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