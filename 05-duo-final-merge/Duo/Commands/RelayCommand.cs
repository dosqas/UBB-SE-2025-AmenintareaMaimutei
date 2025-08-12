// <copyright file="RelayCommand.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Commands
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    /// <summary>
    /// A flexible <see cref="ICommand"/> implementation that supports both synchronous and asynchronous execution,
    /// with or without command parameters.
    /// </summary>
    public partial class RelayCommand : ICommand
    {
        private readonly Func<object?, Task> executeAsync;
        private readonly Func<object?, Task<bool>>? canExecuteAsync;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class with asynchronous delegates.
        /// </summary>
        /// <param name="executeAsync">The asynchronous method to execute.</param>
        /// <param name="canExecuteAsync">The asynchronous predicate to determine if the command can execute. Optional.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="executeAsync"/> is null.</exception>
        public RelayCommand(Func<object?, Task> executeAsync, Func<object?, Task<bool>>? canExecuteAsync = null)
        {
            this.executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            this.canExecuteAsync = canExecuteAsync;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class with synchronous delegates using command parameters.
        /// </summary>
        /// <param name="execute">The method to execute.</param>
        /// <param name="canExecute">The predicate to determine if the command can execute. Optional.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="execute"/> is null.</exception>
        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            ArgumentNullException.ThrowIfNull(execute);

            this.executeAsync = parameter =>
            {
                execute(parameter);
                return Task.CompletedTask;
            };

            this.canExecuteAsync = canExecute != null
                ? parameter => Task.FromResult(canExecute(parameter))
                : null;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Determines whether the command can execute with the given parameter.
        /// </summary>
        /// <param name="parameter">The parameter used by the command.</param>
        /// <returns><c>true</c> if the command can execute; otherwise, <c>false</c>.</returns>
        public bool CanExecute(object? parameter)
        {
            return this.canExecuteAsync == null || this.canExecuteAsync(parameter).Result;
        }

        /// <summary>
        /// Executes the command with the given parameter.
        /// </summary>
        /// <param name="parameter">The parameter used by the command.</param>
        public async void Execute(object? parameter)
        {
            await this.executeAsync(parameter);
        }

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event to indicate that the return value of <see cref="CanExecute"/>
        /// might have changed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
