// <copyright file="InverseBooleanToVisibilityConverterTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Tests.ConvertersTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CourseApp.Converters;
    using Microsoft.UI.Xaml;

    /// <summary>
    /// Unit tests for the InverseBooleanToVisibilityConverter class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class InverseBooleanToVisibilityConverterTests
    {
        private readonly InverseBooleanToVisibilityConverter testConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="InverseBooleanToVisibilityConverterTests"/> class.
        /// </summary>
        public InverseBooleanToVisibilityConverterTests()
        {
            // Initialize the converter before each test
            this.testConverter = new InverseBooleanToVisibilityConverter();
        }

        /// <summary>
        /// Tests that ConvertSafe method returns Visibility.Collapsed when input is true.
        /// </summary>
        [Fact]
        public void ConvertSafeShouldReturnCollapsedWhenTrue()
        {
            // Arrange
            var input = true;

            // Act
            var result = this.testConverter.ConvertSafe(input, typeof(Visibility), null!, null!);

            // Assert
            Assert.Equal(Visibility.Collapsed, result);
        }

        /// <summary>
        /// Tests that ConvertSafe method returns Visibility.Visible when input is false.
        /// </summary>
        [Fact]
        public void ConvertSafeShouldReturnVisibleWhenFalse()
        {
            // Arrange
            var input = false;

            // Act
            var result = this.testConverter.ConvertSafe(input, typeof(Visibility), null!, null!);

            // Assert
            Assert.Equal(Visibility.Visible, result);
        }

        /// <summary>
        /// Tests that ConvertSafe method returns Visibility.Collapsed when input is not a boolean.
        /// </summary>
        [Fact]
        public void ConvertSafeShouldReturnCollapsedWhenInputIsNotBoolean()
        {
            // Arrange
            var input = "Not a boolean";

            // Act
            var result = this.testConverter.ConvertSafe(input, typeof(Visibility), null!, null!);

            // Assert
            Assert.Equal(Visibility.Collapsed, result);
        }

        /// <summary>
        /// Tests that ConvertBackSafe method throws NotImplementedException.
        /// </summary>
        [Fact]
        public void ConvertBackSafeShouldThrowNotImplementedException()
        {
            // Arrange
            var input = Visibility.Visible;

            // Act & Assert
            var exception = Assert.Throws<NotImplementedException>(() => this.testConverter.ConvertBackSafe(input, typeof(bool), null!, null!));
            Assert.Equal("Reverse conversion is not supported.", exception.Message);
        }

        /// <summary>
        /// Tests that Convert method returns Visibility.Collapsed when input is true.
        /// </summary>
        [Fact]
        public void ConvertShouldReturnCollapsedWhenTrue()
        {
            // Arrange
            var input = true;

            // Act
            var result = this.testConverter.Convert(input, typeof(Visibility), null!, null!);

            // Assert
            Assert.Equal(Visibility.Collapsed, result);
        }

        /// <summary>
        /// Tests that Convert method returns Visibility.Visible when input is false.
        /// </summary>
        [Fact]
        public void ConvertShouldReturnVisibleWhenFalse()
        {
            // Arrange
            var input = false;

            // Act
            var result = this.testConverter.Convert(input, typeof(Visibility), null!, null!);

            // Assert
            Assert.Equal(Visibility.Visible, result);
        }

        /// <summary>
        /// Tests that Convert method returns Visibility.Collapsed when input is not a boolean.
        /// </summary>
        [Fact]
        public void ConvertShouldReturnCollapsedWhenInputIsNotBoolean()
        {
            // Arrange
            var input = "Not a boolean";

            // Act
            var result = this.testConverter.Convert(input, typeof(Visibility), null!, null!);

            // Assert
            Assert.Equal(Visibility.Collapsed, result);
        }

        /// <summary>
        /// Tests that ConvertBack method throws NotImplementedException.
        /// </summary>
        [Fact]
        public void ConvertBackShouldThrowNotImplementedException()
        {
            // Arrange
            var input = Visibility.Visible;

            // Act & Assert
            var exception = Assert.Throws<NotImplementedException>(() => this.testConverter.ConvertBack(input, typeof(bool), null!, null!));
            Assert.Equal("Reverse conversion is not supported.", exception.Message);
        }
    }
}
