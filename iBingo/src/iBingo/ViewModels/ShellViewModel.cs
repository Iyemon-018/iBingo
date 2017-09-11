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
    using Prism.Commands;

    public class ShellViewModel : WindowViewModelBase
    {
        private object _lockNumbers = new object();

        public ShellViewModel(IServicesProvider servicesProvider, IDialogService dialogService, IDataStore dataStore)
            : base(servicesProvider,
                dialogService,
                dataStore)
        {
            ShuffleCommand = new DelegateCommand(ExecuteShuffleCommand);
            StopCommand = new DelegateCommand(ExecuteStopCommand);
            Numbers =
                Enumerable.Range(DataStore.Config.Shuffle.Minimum, DataStore.Config.Shuffle.Maximum + 1)
                          .Select(x => new NumberData(x, dataStore.HistoryConfig?.HitNumbers.Any(y => y.Number == x) ?? false))
                          .ToObservableCollection();
        }

        public ObservableCollection<NumberData> Numbers { get; private set; }

        private void ExecuteStopCommand()
        {
            _tokenSource.Cancel();
        }

        private async void ExecuteShuffleCommand()
        {
            if (_tokenSource != null)
            {
                return;
            }
            else
            {
                _tokenSource = new CancellationTokenSource();
            }
            var allNumbers = Enumerable.Range(DataStore.Config.Shuffle.Minimum, DataStore.Config.Shuffle.Maximum + 1);
            var notHits = allNumbers.Where(x => DataStore.HitNumbers.FirstOrDefault(y => y.Number == x) == null).ToArray();

            var token = _tokenSource.Token;
            await Task.Run(() =>
                           {
                               var r = new Random();
                               while (!token.IsCancellationRequested)
                               {
                                   CurrentNumber = allNumbers.ElementAt(r.Next(allNumbers.Count()));
                                   Thread.Sleep(TimeSpan.FromMilliseconds(30));
                               }

                               CurrentNumber = notHits.ElementAt(r.Next(notHits.Length));
                               var newNumber = new NumberData(CurrentNumber, true);
                               DataStore.HitNumbers.Add(newNumber);
                               Numbers.FirstOrDefault(x => x.Number == CurrentNumber).Hit = true;

                           }, token)
                           .ContinueWith(t =>
                                         {
                                             _tokenSource.Dispose();
                                             _tokenSource = null;
                                         });
        }

        public override string Title => "iBingo";

        public ICommand ShuffleCommand { get; private set; }

        public ICommand StopCommand { get; private set; }
        
        private int _currentNumber;

        private CancellationTokenSource _tokenSource;

        public int CurrentNumber
        {
            get => _currentNumber;
            private set => SetProperty(ref _currentNumber, value);
        }
    }
}