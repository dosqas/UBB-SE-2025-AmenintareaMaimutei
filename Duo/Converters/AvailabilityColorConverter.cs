// <copyright file="AvailabilityColorConverter.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Converters
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.UI.Xaml.Media;

    /// <summary>
    /// Converts a boolean value to a color brush indicating availability status.
    /// - <c>true</c> → Blue brush (available)
    /// - <c>false</c> or invalid input → Gray brush (unavailable).
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class AvailabilityColorConverter : IAppValueConverter
    {
        /// <summary>
        /// Color used when the item is available (blue).
        /// </summary>
        private static readonly Windows.UI.Color AvailableColor = Windows.UI.Color.FromArgb(255, 79, 79, 176);

        /// <summary>
        /// Color used when the item is unavailable (gray).
        /// </summary>
        private static readonly Windows.UI.Color UnavailableColor = Windows.UI.Color.FromArgb(255, 128, 128, 128);

        /// <summary>
        /// Converts a boolean value to a corresponding <see cref="SolidColorBrush"/>.
        /// This is the method that will be called from XAML bindings.
        /// </summary>
        /// <param name="value">The source data, expected to be a boolean.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">Optional parameter to use in the converter logic.</param>
        /// <param name="language">The culture to use in the converter.</param>
        /// <returns>A <see cref="SolidColorBrush"/> based on availability.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
            => this.ConvertSafe(value, targetType, parameter, language);

        /// <summary>
        /// ConvertBack is not implemented as this converter is intended for one-way binding only.
        /// </summary>
        /// <param name="value">The value produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="language">The culture to use in the converter.</param>
        /// <returns>Throws a <see cref="NotImplementedException"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => this.ConvertBackSafe(value, targetType, parameter, language);

        /// <summary>
        /// Safely converts a boolean value to a <see cref="SolidColorBrush"/> based on availability.
        /// </summary>
        /// <param name="value">Expected to be a <see cref="bool"/> indicating availability.</param>
        /// <param name="targetType">The type of the binding target.</param>
        /// <param name="parameter">Optional parameter (not used).</param>
        /// <param name="language">The culture for localization (not used).</param>
        /// <returns>
        /// A <see cref="SolidColorBrush"/>: Blue if available, Gray if not available or input is invalid.
        /// </returns>
        public object ConvertSafe(object value, Type targetType, object parameter, string language)
        {
            if (value is bool isAvailable)
            {
                return new SolidColorBrush(isAvailable ? AvailableColor : UnavailableColor);
            }

            // If the input is not a bool, default to unavailable color
            return new SolidColorBrush(UnavailableColor);
        }

        /// <summary>
        /// Not implemented: This converter is not intended to support back conversion.
        /// </summary>
        /// <param name="value">The value to convert back (unused).</param>
        /// <param name="targetType">The target type to convert to (unused).</param>
        /// <param name="parameter">Optional parameter (unused).</param>
        /// <param name="language">The culture for localization (unused).</param>
        /// <returns>Never returns; always throws.</returns>
        /// <exception cref="NotImplementedException">Always thrown to indicate back conversion is unsupported.</exception>
        public object ConvertBackSafe(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException("Reverse conversion is not supported");
        }
    }
}
