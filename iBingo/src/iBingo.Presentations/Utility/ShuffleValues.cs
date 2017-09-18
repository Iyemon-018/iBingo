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

        private readonly Action<int> _shufflingAction;

        private readonly IEnumerable<int> _shufflingValues;

        private readonly int _shufflingValuesLength;

        public ShuffleValues(int minimumValue, int maximumValue, Action<int> shufflingAction)
        {
            MinimumValue = minimumValue;
            MaximumValue = maximumValue;
            _shufflingAction = shufflingAction;

            _shufflingValues = Enumerable.Range(minimumValue, (maximumValue - minimumValue) + 1).ToArray();
            _shufflingValuesLength = _shufflingValues.Count() - 1;
        }

        public Task<NumberData> Shuffle(IEnumerable<NumberData> hitNumbers)
        {
            if (_tokenSource != null) return null;
            else _tokenSource = new CancellationTokenSource();

            var token = _tokenSource.Token;
            return Task.Run(() =>
                            {
                                NumberData result = null;
                                var r = new Random();
                                while (!token.IsCancellationRequested)
                                {
                                    var v = _shufflingValues.ElementAt(r.Next(_shufflingValuesLength));
                                    _shufflingAction(v);
                                    if (result == null && hitNumbers.Any(x => x.Number != v))
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
                            }, token)
                       .ContinueWith(t =>
                                     {
                                         _tokenSource.Dispose();
                                         _tokenSource = null;
                                         return t.Result;
                                     });
        }

        public void Stop()
        {
            _tokenSource.Cancel();
        }
    }
}