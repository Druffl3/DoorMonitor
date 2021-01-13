using System;
using System.Windows.Input;

namespace DoorMonitor.WPF.Commands
{
    /// <summary>
    /// Templated Command implementation of ICommand
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Command<T> : ICommand
    {
        private readonly Action<T> _callBack;

        public Command(Action<T> callback)
        {
            _callBack = callback;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _callBack?.Invoke((T)parameter);
        }
    }

    /// <summary>
    /// Command implementation of ICommand interface
    /// </summary>
    public class Command : ICommand
    {
        private readonly Action _callBack;

        public Command(Action callback)
        {
            _callBack = callback;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _callBack?.Invoke();
        }
    }
}
