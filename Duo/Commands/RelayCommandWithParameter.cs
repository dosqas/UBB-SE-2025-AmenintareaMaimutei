// <copyright file="RelayCommandWithParameter.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Commands
{
    using System;
    using System.Windows.Input;

    #pragma warning disable IDE0079 // Remove unnecessary suppression
    #pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

    /// <summary>
    /// A generic implementation of ICommand that relays its functionality to delegates.
    /// Useful for commands that require a parameter of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the command parameter.</typeparam>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RelayCommandWithParameter{T}"/> class.
    /// </remarks>
    /// <param name="execute">The action to execute when the command is invoked.</param>
    /// <param name="canExecute">Optional predicate to determine whether the command can execute.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="execute"/> is null.</exception>
    public partial class RelayCommandWithParameter<T>(Action<T> execute, Predicate<T>? canExecute = null) : ICommand
    {
        private readonly Action<T> execute = execute ?? throw new ArgumentNullException(nameof(execute));

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Defines whether the command can execute with the given parameter.
        /// </summary>
        /// <param name="parameter">The parameter passed to the command.</param>
        /// <returns><c>true</c> if the command can execute; otherwise, <c>false</c>.</returns>
        public bool CanExecute(object? parameter)
        {
            if (canExecute == null)
            {
                return true;
            }

            return canExecute((T)parameter!);
        }

        /// <summary>
        /// Executes the command with the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter to pass to the execute action.</param>
        public void Execute(object? parameter)
        {
            this.execute((T)parameter!);
        }

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event to indicate that the return value of <see cref="CanExecute"/> might have changed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
