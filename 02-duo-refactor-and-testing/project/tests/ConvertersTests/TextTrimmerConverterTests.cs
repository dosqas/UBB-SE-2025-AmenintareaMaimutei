// <copyright file="TextTrimmerConverterTests.cs" company="PlaceholderCompany">
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

    /// <summary>
    /// Unit tests for the <see cref="TextTrimmerConverter"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TextTrimmerConverterTests
    {
        private readonly TextTrimmerConverter testConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextTrimmerConverterTests"/> class.
        /// </summary>
        public TextTrimmerConverterTests()
        {
            // Initialize the converter before each test
            this.testConverter = new TextTrimmerConverter();
        }

        /// <summary>
        /// Tests that the ConvertSafe method trims the string when its length exceeds the default length.
        /// </summary>
        [Fact]
        public void ConvertSafeShouldTrimStringWhenLengthExceedsDefault()
        {
            // Arrange
            var input = "This is a test string that is too long.";
            var expected = "This is a test string t...";

            // Act
            var result = this.testConverter.ConvertSafe(input, typeof(string), null!, null!);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests that the ConvertSafe method does not trim the string when its length is within the default length.
        /// </summary>
        [Fact]
        public void ConvertSafeShouldNotTrimStringWhenLengthIsWithinDefault()
        {
            // Arrange
            var input = "Short string";
            var expected = "Short string";

            // Act
            var result = this.testConverter.ConvertSafe(input, typeof(string), null!, null!);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests that the ConvertSafe method trims the string when its length exceeds a custom length.
        /// </summary>
        [Fact]
        public void ConvertSafeShouldTrimStringWhenLengthExceedsCustomLength()
        {
            // Arrange
            var input = "This is a test string that is too long.";
            var expected = "This is a test st...";

            // Act
            var result = this.testConverter.ConvertSafe(input, typeof(string), "17", null!);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests that the ConvertSafe method does not trim the string when its length is within a custom length.
        /// </summary>
        [Fact]
        public void ConvertSafeShouldNotTrimStringWhenLengthIsWithinCustomLength()
        {
            // Arrange
            var input = "Short string";
            var expected = "Short string";

            // Act
            var result = this.testConverter.ConvertSafe(input, typeof(string), "20", null!);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests that the ConvertSafe method returns the original value when the input is not a string.
        /// </summary>
        [Fact]
        public void ConvertSafeShouldReturnOriginalValueWhenInputIsNotString()
        {
            // Arrange
            var input = 12345;

            // Act
            var result = this.testConverter.ConvertSafe(input, typeof(string), null!, null!);

            // Assert
            Assert.Equal(input, result);
        }

        /// <summary>
        /// Tests that the ConvertBackSafe method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void ConvertBackSafeShouldThrowNotImplementedException()
        {
            // Arrange
            var input = "Any string";

            // Act & Assert
            var exception = Assert.Throws<NotImplementedException>(() => this.testConverter.ConvertBackSafe(input, typeof(string), null!, null!));
            Assert.Equal("Reverse conversion is not supported.", exception.Message);
        }

        /// <summary>
        /// Tests that the Convert method trims the string when its length exceeds the default length.
        /// </summary>
        [Fact]
        public void ConvertShouldTrimStringWhenLengthExceedsDefault()
        {
            // Arrange
            var input = "This is a test string that is too long.";
            var expected = "This is a test string t...";

            // Act
            var result = this.testConverter.Convert(input, typeof(string), null!, null!);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests that the Convert method does not trim the string when its length is within the default length.
        /// </summary>
        [Fact]
        public void ConvertShouldNotTrimStringWhenLengthIsWithinDefault()
        {
            // Arrange
            var input = "Short string";
            var expected = "Short string";

            // Act
            var result = this.testConverter.Convert(input, typeof(string), null!, null!);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests that the Convert method trims the string when its length exceeds a custom length.
        /// </summary>
        [Fact]
        public void ConvertShouldTrimStringWhenLengthExceedsCustomLength()
        {
            // Arrange
            var input = "This is a test string that is too long.";
            var expected = "This is a test st...";

            // Act
            var result = this.testConverter.Convert(input, typeof(string), "17", null!);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests that the Convert method does not trim the string when its length is within a custom length.
        /// </summary>
        [Fact]
        public void ConvertShouldNotTrimStringWhenLengthIsWithinCustomLength()
        {
            // Arrange
            var input = "Short string";
            var expected = "Short string";

            // Act
            var result = this.testConverter.Convert(input, typeof(string), "20", null!);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests that the Convert method returns the original value when the input is not a string.
        /// </summary>
        [Fact]
        public void ConvertShouldReturnOriginalValueWhenInputIsNotString()
        {
            // Arrange
            var input = 12345;

            // Act
            var result = this.testConverter.Convert(input, typeof(string), null!, null!);

            // Assert
            Assert.Equal(input, result);
        }

        /// <summary>
        /// Tests that the ConvertBack method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void ConvertBackShouldThrowNotImplementedException()
        {
            // Arrange
            var input = "Any string";

            // Act & Assert
            var exception = Assert.Throws<NotImplementedException>(() => this.testConverter.ConvertBack(input, typeof(string), null!, null!));
            Assert.Equal("Reverse conversion is not supported.", exception.Message);
        }
    }
}
