using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Duo.Commands
{
    /// <summary>
    /// A flexible ICommand implementation that supports both parameterless and parameterized execution logic.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Func<object?, Task> executeAsync;
        private readonly Func<object?, Task<bool>> canExecuteAsync;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Func<object?, Task> executeAsync, Func<object?, Task<bool>> canExecuteAsync = null)
        {
            this.executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            this.canExecuteAsync = canExecuteAsync;
        }

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            this.executeAsync = parameter =>
            {
                execute(parameter);
                return Task.CompletedTask;
            };
            this.canExecuteAsync = canExecute != null
                ? parameter => Task.FromResult(canExecute(parameter))
                : null;
        }

        public bool CanExecute(object? parameter)
        {
            return canExecuteAsync == null || canExecuteAsync(parameter).Result;
        }

        public async void Execute(object? parameter)
        {
            await executeAsync(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
