namespace iBingo.Presentations.Models
{
    using iBingo.Presentations.ComponentModels;

    public class NumberData : BindableBase
    {
        private int _number;
        
        public int Number
        {
            get => _number;
            private set => SetProperty(ref _number, value);
        }

        public NumberData(int number)
        {
            _number = number;
        }

        public NumberData(int number, bool hit) : this(number)
        {
            _hit = hit;
        }
        
        private bool _hit;
        
        public bool Hit
        {
            get => _hit;
            set => SetProperty(ref _hit, value);
        }
    }
}