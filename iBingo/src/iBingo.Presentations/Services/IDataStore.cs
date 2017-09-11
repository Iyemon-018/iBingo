namespace iBingo.Presentations.Services
{
    using System.Collections.ObjectModel;
    using iBingo.Domains.Configurations;
    using iBingo.Presentations.Models;

    public interface IDataStore
    {
        ApplicationConfig Config { get; }

        HistoryConfig HistoryConfig { get; }

        ObservableCollection<NumberData> HitNumbers { get; }
    }
}