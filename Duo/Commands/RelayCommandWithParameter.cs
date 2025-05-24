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
    /// A generic implementation of <see cref="ICommand"/> that supports strongly-typed parameters.
    /// </summary>
    /// <typeparam name="T">The type of the parameter passed to the command.</typeparam>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RelayCommandWithParameter{T}"/> class.
    /// </remarks>
    /// <param name="execute">The action to execute when the command is invoked.</param>
    /// <param name="canExecute">The function that determines whether the command can execute. Optional.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="execute"/> is null.</exception>
    public partial class RelayCommandWithParameter<T>(Action<T> execute, Predicate<T>? canExecute = null) : ICommand
    {
        private readonly Action<T> execute = execute ?? throw new ArgumentNullException(nameof(execute));
        private readonly Predicate<T>? canExecute = canExecute;

        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Determines whether the command can execute with the given parameter.
        /// </summary>
        /// <param name="parameter">The parameter passed to the command.</param>
        /// <returns><c>true</c> if the command can execute; otherwise, <c>false</c>.</returns>
        public bool CanExecute(object? parameter)
        {
            if (this.canExecute == null)
            {
                return true;
            }

            if (parameter is T typedParameter)
            {
                return this.canExecute(typedParameter);
            }

            if (parameter == null)
            {
                if (default(T) == null) // T is a reference type or nullable
                {
                    return this.canExecute(default!);
                }

                return false;
            }

            try
            {
                object converted = Convert.ChangeType(parameter, typeof(T));
                if (converted is T typedConverted)
                {
                    return this.canExecute(typedConverted);
                }
            }
            catch
            {
                // Ignore conversion exceptions
            }

            return false;
        }

        /// <summary>
        /// Executes the command with the given parameter.
        /// </summary>
        /// <param name="parameter">The parameter passed to the command.</param>
        /// <exception cref="ArgumentException">Thrown if the parameter is not of the correct type.</exception>
        public void Execute(object? parameter)
        {
            if (parameter is T typedParameter)
            {
                this.execute(typedParameter);
                return;
            }

            if (parameter == null)
            {
                if (default(T) == null) // T is reference type or nullable
                {
                    this.execute(default!);
                    return;
                }

                throw new ArgumentException($"Null parameter not allowed for value type {typeof(T)}", nameof(parameter));
            }

            throw new ArgumentException($"Invalid parameter type. Expected {typeof(T)}.", nameof(parameter));
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
