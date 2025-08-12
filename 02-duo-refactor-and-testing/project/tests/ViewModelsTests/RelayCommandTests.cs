// <copyright file="RelayCommandTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Tests.ViewModelsTests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Input;
    using CourseApp.ViewModels;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="RelayCommand"/> class.
    /// Validates construction, execution behavior, and event invocation logic.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RelayCommandTests
    {
        /// <summary>
        /// Tests that the constructor throws an <see cref="ArgumentNullException"/>
        /// if the execute delegate is null.
        /// </summary>
        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenExecuteIsNull()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => new RelayCommand(null!));
        }

        /// <summary>
        /// Tests that <see cref="RelayCommand.CanExecute"/> returns true
        /// if no canExecute predicate is provided.
        /// </summary>
        [Fact]
        public void CanExecute_ShouldReturnTrue_WhenNoPredicateProvided()
        {
            // Arrange
            var command = new RelayCommand(_ => { });

            // Act
            var result = command.CanExecute(null);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Tests that <see cref="RelayCommand.CanExecute"/> returns false
        /// when a predicate is provided that returns false.
        /// </summary>
        [Fact]
        public void CanExecute_ShouldReturnPredicateResult_WhenPredicateProvided()
        {
            // Arrange
            var command = new RelayCommand(_ => { }, _ => false);

            // Act
            var result = command.CanExecute(null);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests that the <see cref="RelayCommand.Execute"/> method
        /// invokes the provided execute action.
        /// </summary>
        [Fact]
        public void Execute_ShouldInvokeExecuteAction()
        {
            // Arrange
            bool executed = false;
            var command = new RelayCommand(_ => executed = true);

            // Act
            command.Execute(null);

            // Assert
            Assert.True(executed);
        }

        /// <summary>
        /// Tests that calling <see cref="RelayCommand.RaiseCanExecuteChanged"/>
        /// raises the <see cref="ICommand.CanExecuteChanged"/> event.
        /// </summary>
        [Fact]
        public void RaiseCanExecuteChanged_ShouldTriggerCanExecuteChangedEvent()
        {
            // Arrange
            var command = new RelayCommand(_ => { });
            bool eventRaised = false;

            command.CanExecuteChanged += (_, _) => eventRaised = true;

            // Act
            command.RaiseCanExecuteChanged();

            // Assert
            Assert.True(eventRaised);
        }

        /// <summary>
        /// Tests that calling <see cref="RelayCommand.RaiseCanExecuteChanged"/>
        /// does not throw an exception when there are no subscribers.
        /// </summary>
        [Fact]
        public void RaiseCanExecuteChanged_ShouldNotThrow_WhenNoSubscribers()
        {
            // Arrange
            var command = new RelayCommand(_ => { });

            // Act & Assert
            var exception = Record.Exception(() => command.RaiseCanExecuteChanged());

            Assert.Null(exception);
        }
    }
}
