// <copyright file="BooleanToVisibilityConverter.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Converters
{
    using System;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Data;

    /// <summary>
    /// Converts boolean values to <see cref="Visibility"/> values and vice versa.
    /// Supports an optional "Invert" parameter to reverse the conversion logic.
    /// </summary>
    public partial class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to a <see cref="Visibility"/> enum value.
        /// </summary>
        /// <param name="value">The source value, expected to be of type <see cref="bool"/>.</param>
        /// <param name="targetType">The target type of the binding (unused).</param>
        /// <param name="parameter">
        /// Optional parameter; if set to "Invert", the boolean logic is reversed (true → Collapsed, false → Visible).
        /// </param>
        /// <param name="language">The culture or language info (unused).</param>
        /// <returns>
        /// <see cref="Visibility.Visible"/> if <paramref name="value"/> is true;
        /// <see cref="Visibility.Collapsed"/> if false (inverted if "Invert" parameter is specified).
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool invert = parameter != null && parameter.ToString() == "Invert";
            bool isVisible = value is bool boolValue && boolValue;

            if (invert)
            {
                isVisible = !isVisible;
            }

            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Converts a <see cref="Visibility"/> value back to a boolean.
        /// </summary>
        /// <param name="value">The value to convert back, expected to be <see cref="Visibility"/>.</param>
        /// <param name="targetType">The type to convert to (unused).</param>
        /// <param name="parameter">
        /// Optional parameter; if set to "Invert", the conversion result is reversed
        /// (<see cref="Visibility.Visible"/> → false, <see cref="Visibility.Collapsed"/> → true).
        /// </param>
        /// <param name="language">The culture or language info (unused).</param>
        /// <returns>
        /// True if <paramref name="value"/> is <see cref="Visibility.Visible"/>, false otherwise.
        /// Result is inverted if the "Invert" parameter is specified.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            bool invert = parameter != null && parameter.ToString() == "Invert";
            bool isVisible = value is Visibility visibility && visibility == Visibility.Visible;

            if (invert)
            {
                isVisible = !isVisible;
            }

            return isVisible;
        }
    }
}
