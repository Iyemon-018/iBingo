namespace iBingo.Commands
{
    using System;
    using System.Windows.Input;
    using Prism.Commands;
    public class RelayCommand : DelegateCommand
    {
        public RelayCommand(Action executeMethod) : base(executeMethod)
        {
        }

        public RelayCommand(Action executeMethod, Func<bool> canExecuteMethod) : base(executeMethod, canExecuteMethod)
        {
        }

        public override event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        protected override void Execute(object parameter)
        {
            CommandManager.InvalidateRequerySuggested();
            base.Execute(parameter);
            CommandManager.InvalidateRequerySuggested();
        }
    }
}