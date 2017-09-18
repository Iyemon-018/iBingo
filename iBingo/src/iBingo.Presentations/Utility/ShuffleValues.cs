namespace iBingo.Presentations.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using iBingo.Presentations.Models;

    public class ShuffleValues
    {
        private CancellationTokenSource _tokenSource;

        public int MinimumValue { get; private set; }

        public int MaximumValue { get; private set; }

        internal IEnumerable<int> ShufflingValues => _shufflingValues;

        internal int ShufflingValuesLength => _shufflingValuesLength;

        private readonly Action<int> _shufflingAction;

        private readonly IEnumerable<int> _shufflingValues;

        private readonly int _shufflingValuesLength;

        private CancellationToken _token;

        public ShuffleValues(int minimumValue, int maximumValue, Action<int> shufflingAction)
        {
            if (minimumValue <= 0) throw new ArgumentOutOfRangeException(nameof(minimumValue));
            if (maximumValue <= minimumValue) throw new ArgumentOutOfRangeException(nameof(maximumValue));

            MinimumValue = minimumValue;
            MaximumValue = maximumValue;
            _shufflingAction = shufflingAction ?? throw new ArgumentNullException(nameof(shufflingAction));

            _shufflingValues = Enumerable.Range(minimumValue, (maximumValue - minimumValue) + 1).ToArray();
            _shufflingValuesLength = _shufflingValues.Count() - 1;
        }

        public Task<NumberData> Shuffle(IEnumerable<NumberData> hitNumbers)
        {
            if (_tokenSource != null) return null;
            else _tokenSource = new CancellationTokenSource();

            _token = _tokenSource.Token;
            return Task.Run(() => ShuffleCore(hitNumbers), _token)
                       .ContinueWith(t =>
                                     {
                                         _tokenSource.Dispose();
                                         _tokenSource = null;
                                         return t.Result;
                                     });
        }

        private bool Stopped()
        {
            return _token.IsCancellationRequested;
        }

        internal NumberData ShuffleCore(IEnumerable<NumberData> hitNumbers)
        {
            NumberData result = null;
            var r = new Random();
            while (!Stopped())
            {
                var v = _shufflingValues.ElementAt(r.Next(_shufflingValuesLength));
                _shufflingAction(v);
                if (result == null && hitNumbers.FirstOrDefault(x => x.Number == v) == null)
                {
                    result = new NumberData(v, true);
                }
                Thread.Sleep(TimeSpan.FromMilliseconds(30));
            }

            if (result == null)
            {
                var unhitValues = _shufflingValues.Where(x => !hitNumbers.Any(y => y.Number == x)).ToArray();
                var v = unhitValues.ElementAt(r.Next(unhitValues.Length));
                result = new NumberData(v, true);
            }

            return result;
        }

        public void Stop()
        {
            _tokenSource.Cancel();
        }
    }
}