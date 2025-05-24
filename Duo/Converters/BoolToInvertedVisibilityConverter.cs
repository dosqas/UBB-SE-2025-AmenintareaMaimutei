// <copyright file="BoolToInvertedVisibilityConverter.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Converters
{
    using System;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Data;

    /// <summary>
    /// Converts a boolean or string value to an inverted <see cref="Visibility"/> value.
    /// - For booleans: true → Collapsed, false → Visible.
    /// - For strings: empty/null → Collapsed, non-empty → Visible.
    /// Supports reverse conversion based on target type.
    /// </summary>
    public partial class BoolToInvertedVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean or string value to a <see cref="Visibility"/> value.
        /// - Boolean: true becomes Collapsed, false becomes Visible.
        /// - String: non-empty becomes Visible, null or empty becomes Collapsed.
        /// </summary>
        /// <param name="value">The input value (boolean or string).</param>
        /// <param name="targetType">The target type (unused).</param>
        /// <param name="parameter">Optional parameter (unused).</param>
        /// <param name="language">The culture or language info (unused).</param>
        /// <returns>
        /// <see cref="Visibility.Visible"/> or <see cref="Visibility.Collapsed"/> based on the input value and type.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool b)
            {
                // Invert the boolean: true → Collapsed, false → Visible
                return !b ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (value is string s)
            {
                // Null or empty string → Collapsed, otherwise → Visible
                return string.IsNullOrEmpty(s) ? Visibility.Collapsed : Visibility.Visible;
            }

            // Default to Collapsed for unsupported types
            return Visibility.Collapsed;
        }

        /// <summary>
        /// Converts a <see cref="Visibility"/> value back to a boolean or string.
        /// - If target type is boolean: Collapsed → true, Visible → false.
        /// - If target type is string: Visible → "True", Collapsed → "".
        /// </summary>
        /// <param name="value">The <see cref="Visibility"/> value to convert back.</param>
        /// <param name="targetType">The target type to convert to (bool or string).</param>
        /// <param name="parameter">Optional parameter (unused).</param>
        /// <param name="language">The culture or language info (unused).</param>
        /// <returns>
        /// Converted boolean or string based on the target type. Returns null for unsupported types.
        /// </returns>
        public object? ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (targetType == typeof(bool))
            {
                // Collapsed → true, Visible → false
                return value is Visibility v && v == Visibility.Collapsed;
            }
            else if (targetType == typeof(string))
            {
                // Visible → "True", Collapsed → ""
                return value is Visibility v && v == Visibility.Visible ? "True" : string.Empty;
            }

            // Default return for unsupported types
            return null;
        }
    }
}
