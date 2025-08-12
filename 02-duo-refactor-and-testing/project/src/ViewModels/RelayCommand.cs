using System;
using System.Windows.Input;

#pragma warning disable IDE0290

namespace CourseApp.ViewModels
{
    /// <summary>
    /// A command implementation that can be executed and supports conditional execution based on a predicate.
    /// </summary>
    public partial class RelayCommand : ICommand, IRelayCommand
    {
        private readonly Action<object?> executeAction;
        private readonly Predicate<object?>? canExecutePredicate;

        /// <summary>
        /// Event that is triggered when the execution condition changes (i.e., when CanExecute changes).
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class with only an execute action.
        /// </summary>
        /// <param name="execute">The action to be executed when the command is triggered.</param>
        public RelayCommand(Action<object?> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class with both an execute action and a can-execute predicate.
        /// </summary>
        /// <param name="execute">The action to be executed when the command is triggered.</param>
        /// <param name="canExecute">A predicate that determines whether the command can be executed.</param>
        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute)
        {
            executeAction = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecutePredicate = canExecute;
        }

        /// <summary>
        /// Determines whether the command can be executed based on the given parameter and the canExecute predicate.
        /// </summary>
        /// <param name="parameter">The parameter to evaluate against the can-execute predicate.</param>
        /// <returns>True if the command can be executed, otherwise false.</returns>
        public bool CanExecute(object? parameter)
        {
            return canExecutePredicate == null || canExecutePredicate(parameter);
        }

        /// <summary>
        /// Executes the action associated with this command.
        /// </summary>
        /// <param name="parameter">The parameter to pass to the execute action.</param>
        public void Execute(object? parameter)
        {
            executeAction(parameter);
        }

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event to notify that the can-execute state has changed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
