namespace iBingo.Presentations.Extensions
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public static class CollectionExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> self)
        {
            return new ObservableCollection<T>(self);
        }

        public static void AddRange<T>(this ObservableCollection<T> self, IEnumerable<T> values)
        {
            foreach (var value in values) self.Add(value);
        }
    }
}