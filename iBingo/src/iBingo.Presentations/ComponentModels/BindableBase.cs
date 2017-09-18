namespace iBingo.Presentations.ComponentModels
{
    using System.ComponentModel;

    public abstract class BindableBase : Prism.Mvvm.BindableBase
    {
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            OnPropertyChangedCore(args.PropertyName);
        }

        protected virtual void OnPropertyChangedCore(string propertyName)
        {

        }
    }
}