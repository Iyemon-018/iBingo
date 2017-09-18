namespace iBingo.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using iBingo.Presentations.ComponentModels;
    using iBingo.Presentations.Extensions;
    using iBingo.Presentations.Models;
    using iBingo.Presentations.Services;
    using iBingo.Presentations.Utility;
    using Prism.Commands;

    public class ShellViewModel : WindowViewModelBase
    {
        private readonly ShuffleValues _shuffleValues;

        public ObservableCollection<NumberData> HitNumbers { get; private set; }

        public ShellViewModel(IServicesProvider servicesProvider, IDialogService dialogService, IDataStore dataStore)
            : base(servicesProvider,
                dialogService,
                dataStore)
        {
            ShuffleCommand = new DelegateCommand(ExecuteShuffleCommand);
            StopCommand = new DelegateCommand(ExecuteStopCommand);
            HitNumbers = dataStore.HitNumbers;
            _shuffleValues = new ShuffleValues(dataStore.Config.Shuffle.Minimum, dataStore.Config.Shuffle.Maximum, OnShufflingValue);
        }

        private void OnShufflingValue(int value)
        {
            CurrentNumber = value;
        }

        private void ExecuteStopCommand()
        {
            _shuffleValues.Stop();
        }

        private async void ExecuteShuffleCommand()
        {
            var hitNumber = await _shuffleValues.Shuffle(DataStore.HitNumbers);
            CurrentNumber = hitNumber.Number;
            DataStore.HitNumbers.Add(hitNumber);
        }

        public override string Title => "iBingo";

        public ICommand ShuffleCommand { get; private set; }

        public ICommand StopCommand { get; private set; }

        private int _currentNumber;

        public int CurrentNumber
        {
            get => _currentNumber;
            private set => SetProperty(ref _currentNumber, value);
        }
    }
}