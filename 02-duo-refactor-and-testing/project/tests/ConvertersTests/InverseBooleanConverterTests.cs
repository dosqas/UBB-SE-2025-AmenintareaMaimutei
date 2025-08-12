// <copyright file="InverseBooleanConverterTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Tests.ConvertersTests
{
    using System.Diagnostics.CodeAnalysis;
    using CourseApp.Converters;
    using Microsoft.UI;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Media;
    using Windows.UI;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="InverseBooleanConverter"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class InverseBooleanConverterTests
    {
        private readonly InverseBooleanConverter testConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="InverseBooleanConverterTests"/> class.
        /// </summary>
        public InverseBooleanConverterTests()
        {
            // Initialize the converter before each test
            this.testConverter = new InverseBooleanConverter();
        }

        /// <summary>
        /// Tests that Convert returns false when the input is true.
        /// </summary>
        [Fact]
        public void ConvertShouldReturnFalseWhenTrue()
        {
            // Arrange
            var input = true;

            // Act
            var result = this.testConverter.Convert(input, typeof(bool), null!, string.Empty);

            // Assert
            Assert.False((bool)result);
        }

        /// <summary>
        /// Tests that Convert returns true when the input is false.
        /// </summary>
        [Fact]
        public void ConvertShouldReturnTrueWhenFalse()
        {
            // Arrange
            var input = false;

            // Act
            var result = this.testConverter.Convert(input, typeof(bool), null!, string.Empty);

            // Assert
            Assert.True((bool)result);
        }

        /// <summary>
        /// Tests that Convert returns the original value when the input is not a boolean.
        /// </summary>
        [Fact]
        public void ConvertShouldReturnOriginalValueWhenInputIsNotBoolean()
        {
            // Arrange
            var input = "string";

            // Act
            var result = this.testConverter.Convert(input, typeof(bool), null!, string.Empty);

            // Assert
            Assert.Equal(input, result);
        }

        /// <summary>
        /// Tests that ConvertBack returns false when the input is true.
        /// </summary>
        [Fact]
        public void ConvertBackShouldReturnFalseWhenTrue()
        {
            // Arrange
            var input = true;

            // Act
            var result = this.testConverter.ConvertBack(input, typeof(bool), null!, string.Empty);

            // Assert
            Assert.False((bool)result);
        }

        /// <summary>
        /// Tests that ConvertBack returns true when the input is false.
        /// </summary>
        [Fact]
        public void ConvertBackShouldReturnTrueWhenFalse()
        {
            // Arrange
            var input = false;

            // Act
            var result = this.testConverter.ConvertBack(input, typeof(bool), null!, string.Empty);

            // Assert
            Assert.True((bool)result);
        }

        /// <summary>
        /// Tests that ConvertBack returns the original value when the input is not a boolean.
        /// </summary>
        [Fact]
        public void ConvertBackShouldReturnOriginalValueWhenInputIsNotBoolean()
        {
            // Arrange
            var input = "string";

            // Act
            var result = this.testConverter.ConvertBack(input, typeof(bool), null!, string.Empty);

            // Assert
            Assert.Equal(input, result);
        }

        /// <summary>
        /// Tests that ConvertSafe returns false when the input is true.
        /// </summary>
        [Fact]
        public void ConvertSafeShouldReturnFalseWhenTrue()
        {
            // Arrange
            var input = true;

            // Act
            var result = this.testConverter.ConvertSafe(input, typeof(bool), null!, string.Empty);

            // Assert
            Assert.False((bool)result);
        }

        /// <summary>
        /// Tests that ConvertSafe returns true when the input is false.
        /// </summary>
        [Fact]
        public void ConvertSafeShouldReturnTrueWhenFalse()
        {
            // Arrange
            var input = false;

            // Act
            var result = this.testConverter.ConvertSafe(input, typeof(bool), null!, string.Empty);

            // Assert
            Assert.True((bool)result);
        }

        /// <summary>
        /// Tests that ConvertSafe returns the original value when the input is not a boolean.
        /// </summary>
        [Fact]
        public void ConvertSafeShouldReturnOriginalValueWhenInputIsNotBoolean()
        {
            // Arrange
            var input = "string";

            // Act
            var result = this.testConverter.ConvertSafe(input, typeof(bool), null!, string.Empty);

            // Assert
            Assert.Equal(input, result);
        }

        /// <summary>
        /// Tests that ConvertBackSafe returns false when the input is true.
        /// </summary>
        [Fact]
        public void ConvertBackSafeShouldReturnFalseWhenTrue()
        {
            // Arrange
            var input = true;

            // Act
            var result = this.testConverter.ConvertBackSafe(input, typeof(bool), null!, string.Empty);

            // Assert
            Assert.False((bool)result);
        }

        /// <summary>
        /// Tests that ConvertBackSafe returns true when the input is false.
        /// </summary>
        [Fact]
        public void ConvertBackSafeShouldReturnTrueWhenFalse()
        {
            // Arrange
            var input = false;

            // Act
            var result = this.testConverter.ConvertBackSafe(input, typeof(bool), null!, string.Empty);

            // Assert
            Assert.True((bool)result);
        }

        /// <summary>
        /// Tests that ConvertBackSafe returns the original value when the input is not a boolean.
        /// </summary>
        [Fact]
        public void ConvertBackSafeShouldReturnOriginalValueWhenInputIsNotBoolean()
        {
            // Arrange
            var input = "string";

            // Act
            var result = this.testConverter.ConvertBackSafe(input, typeof(bool), null!, string.Empty);

            // Assert
            Assert.Equal(input, result);
        }
    }
}
