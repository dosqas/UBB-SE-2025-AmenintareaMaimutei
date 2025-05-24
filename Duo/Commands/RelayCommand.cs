// <copyright file="RelayCommand.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Commands
{
    using System;
    using System.Windows.Input;

    #pragma warning disable IDE0079 // Remove unnecessary suppression
    #pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

    /// <summary>
    /// A basic implementation of ICommand for relaying commands in MVVM architectures.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RelayCommand"/> class.
    /// </remarks>
    /// <param name="execute">The action to execute when the command is invoked.</param>
    /// <param name="canExecute">Optional function that determines whether the command can execute.</param>
    /// <exception cref="ArgumentNullException">Thrown when the execute delegate is null.</exception>
    public partial class RelayCommand(Action execute, Func<bool>? canExecute = null) : ICommand
    {
        private readonly Action execute = execute ?? throw new ArgumentNullException(nameof(execute));
        private readonly Func<bool>? canExecute = canExecute;

        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. Ignored in this implementation.</param>
        /// <returns>true if the command can execute; otherwise, false.</returns>
        public bool CanExecute(object? parameter) => this.canExecute?.Invoke() ?? true;

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">Data used by the command. Ignored in this implementation.</param>
        public void Execute(object? parameter) => this.execute();

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event to indicate that the return value of <see cref="CanExecute"/> might have changed.
        /// </summary>
        public void RaiseCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
