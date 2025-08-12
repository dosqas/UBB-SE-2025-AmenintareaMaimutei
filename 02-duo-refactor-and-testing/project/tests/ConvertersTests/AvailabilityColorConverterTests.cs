// <copyright file="AvailabilityColorConverterTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Tests.ConvertersTests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using CourseApp.Converters;
    using Microsoft.UI.Xaml.Media;
    using Windows.UI;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="AvailabilityColorConverter"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AvailabilityColorConverterTests
    {
        private readonly AvailabilityColorConverter testConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvailabilityColorConverterTests"/> class.
        /// </summary>
        public AvailabilityColorConverterTests()
        {
            this.testConverter = new AvailabilityColorConverter();
        }

        /// <summary>
        /// Tests that the ConvertBackSafe method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void ConvertBackSafeShouldThrowNotImplementedException()
        {
            // Arrange
            var input = true;

            // Act & Assert
            var exception = Assert.Throws<NotImplementedException>(() =>
                this.testConverter.ConvertBackSafe(input, typeof(bool), null!, null!));
            Assert.Equal("Reverse conversion is not supported", exception.Message);
        }

        /// <summary>
        /// Tests that the ConvertBack method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void ConvertBackShouldThrowNotImplementedException()
        {
            // Arrange
            var input = true;

            // Act & Assert
            var exception = Assert.Throws<NotImplementedException>(() =>
                this.testConverter.ConvertBack(input, typeof(bool), null!, null!));
            Assert.Equal("Reverse conversion is not supported", exception.Message);
        }
    }
}
