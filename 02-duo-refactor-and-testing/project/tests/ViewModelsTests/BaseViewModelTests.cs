// <copyright file="BaseViewModelTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Tests.ViewModelsTests
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using CourseApp.ViewModels;
    using Xunit;

    /// <summary>
    /// Contains unit tests for the <see cref="BaseViewModel"/> class,
    /// verifying property change notifications and SetProperty behavior.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BaseViewModelTests
    {
        /// <summary>
        /// OnPropertyChanged_WhenCalled_ShouldRaisePropertyChangedEvent.
        /// </summary>
        [Fact]
        public void OnPropertyChanged_WhenCalled_ShouldRaisePropertyChangedEvent()
        {
            // Arrange
            var viewModel = new TestViewModel();
            bool eventRaised = false;
            string? propertyName = null;

            viewModel.PropertyChanged += (s, e) =>
            {
                eventRaised = true;
                propertyName = e.PropertyName;
            };

            // Act
            viewModel.Name = "NewName";

            // Assert
            Assert.True(eventRaised);
            Assert.Equal(nameof(viewModel.Name), propertyName);
        }

        /// <summary>
        /// SetProperty_WhenValueIsDifferent_ShouldUpdateFieldAndReturnTrue.
        /// </summary>
        [Fact]
        public void SetProperty_WhenValueIsDifferent_ShouldUpdateFieldAndReturnTrue()
        {
            // Arrange
            var viewModel = new TestViewModel();

            // Act
            var result = viewModel.TrySetName("TestName");

            // Assert
            Assert.True(result);
            Assert.Equal("TestName", viewModel.GetRawName());
        }

        /// <summary>
        /// SetProperty_WhenValueIsSame_ShouldReturnFalseAndNotRaisePropertyChanged.
        /// </summary>
        [Fact]
        public void SetProperty_WhenValueIsSame_ShouldReturnFalseAndNotRaisePropertyChanged()
        {
            // Arrange
            var viewModel = new TestViewModel
            {
                Name = "SameValue"
            };

            bool eventRaised = false;
            viewModel.PropertyChanged += (_, _) => eventRaised = true;

            // Act
            var result = viewModel.TrySetName("SameValue");

            // Assert
            Assert.False(result);
            Assert.False(eventRaised);
        }

        /// <summary>
        /// IBaseViewModel_OnPropertyChanged_WhenCalled_ShouldRaisePropertyChangedEvent.
        /// </summary>
        [Fact]
        public void IBaseViewModel_OnPropertyChanged_WhenCalled_ShouldRaisePropertyChangedEvent()
        {
            // Arrange
            var viewModel = new TestViewModel();
            var iViewModel = (IBaseViewModel)viewModel;

            bool eventRaised = false;
            string? propName = null;

            viewModel.PropertyChanged += (_, e) =>
            {
                eventRaised = true;
                propName = e.PropertyName;
            };

            // Act
            iViewModel.OnPropertyChanged("CustomProp");

            // Assert
            Assert.True(eventRaised);
            Assert.Equal("CustomProp", propName);
        }

        /// <summary>
        /// IBaseViewModel_SetProperty_WhenCalled_ShouldThrowNotImplementedException.
        /// </summary>
        [Fact]
        public void IBaseViewModel_SetProperty_WhenCalled_ShouldThrowNotImplementedException()
        {
            // Arrange
            var viewModel = new TestViewModel();
            var iViewModel = (IBaseViewModel)viewModel;
            string testValue = "initial";

            // Act & Assert
            var ex = Assert.Throws<NotImplementedException>(() =>
            {
                iViewModel.SetProperty(ref testValue, "new", "SomeProp");
            });
            Assert.Equal("The method or operation is not implemented.", ex.Message);
        }

        /// <summary>
        /// A test subclass of BaseViewModel for testing purposes.
        /// </summary>
        private partial class TestViewModel : BaseViewModel
        {
            private string name = string.Empty;

            public string Name
            {
                get => this.name;
                set => this.SetProperty(ref this.name, value);
            }

            public bool TrySetName(string value)
            {
                return this.SetProperty(ref this.name, value, nameof(this.Name));
            }

            public string GetRawName() => this.name;
        }
    }
}
