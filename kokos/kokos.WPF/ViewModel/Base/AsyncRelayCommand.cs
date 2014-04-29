using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace kokos.WPF.ViewModel.Base
{
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<object, bool> _canExecute;
        private readonly Func<object, Task> _asyncExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public AsyncRelayCommand(Func<object, Task> execute)
            : this(execute, null)
        {
        }

        public AsyncRelayCommand(Func<object, Task> asyncExecute, Func<object, bool> canExecute)
        {
            _asyncExecute = asyncExecute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public async void Execute(object parameter)
        {
            await _asyncExecute(parameter);
        }
    }
}
