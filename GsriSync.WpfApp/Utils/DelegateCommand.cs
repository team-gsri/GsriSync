using System;
using System.Windows.Input;

namespace GsriSync.WpfApp.Utils
{
    internal class DelegateCommand : ICommand
    {
        protected Action<object> execute;
        private bool canExecute;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<object> execute, bool canExecute = true)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => canExecute;

        public void Execute(object parameter)
        {
            execute(parameter);
        }

        public void SetCanExecute(bool value)
        {
            if (value == canExecute) return;
            canExecute = value;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}