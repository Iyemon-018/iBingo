namespace iBingo.Presentations.Services
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Data;
    using iBingo.Domains.Configurations;
    using iBingo.Presentations.Models;
    using iBingo.Presentations.Extensions;

    public sealed class DataStore : IDataStore
    {
        private object _lockHitNumbers = new object();

        public DataStore(ApplicationConfig applicationConfig, HistoryConfig historyConfig)
        {
            Config = applicationConfig;
            HistoryConfig = historyConfig;
            HitNumbers = historyConfig != null
                ? historyConfig.HitNumbers.Select(x => new NumberData(x.Number, true)).ToObservableCollection()
                : new ObservableCollection<NumberData>();

            BindingOperations.EnableCollectionSynchronization(HitNumbers, _lockHitNumbers);
                
        }

        public ApplicationConfig Config { get; }

        public HistoryConfig HistoryConfig { get; }

        public ObservableCollection<NumberData> HitNumbers { get; }
    }
}