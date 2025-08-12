// <copyright file="InverseBooleanToVisibilityConverter.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Converters
{
    using System;
    using Duo.Converters;
    using Microsoft.UI.Xaml;

    /// <summary>
    /// Converts a boolean value to a <see cref="Visibility"/> value, with inversion logic.
    /// Returns <see cref="Visibility.Collapsed"/> if the value is <c>true</c>,
    /// and <see cref="Visibility.Visible"/> if <c>false</c>.
    /// </summary>
    public partial class InverseBooleanToVisibilityConverter : IAppValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, string language)
            => this.ConvertSafe(value, targetType, parameter, language);

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => this.ConvertBackSafe(value, targetType, parameter, language);

        /// <summary>
        /// Converts a boolean value to an inverted <see cref="Visibility"/> value.
        /// </summary>
        /// <param name="value">The value to convert (expected to be of type <see cref="bool"/>).</param>
        /// <param name="targetType">The target type (should be <see cref="Visibility"/>).</param>
        /// <param name="parameter">An optional parameter (unused).</param>
        /// <param name="language">The language or culture info (unused).</param>
        /// <returns>
        /// <see cref="Visibility.Collapsed"/> if <paramref name="value"/> is <c>true</c>,
        /// <see cref="Visibility.Visible"/> if <paramref name="value"/> is <c>false</c>,
        /// otherwise returns <see cref="Visibility.Collapsed"/>.
        /// </returns>
        public object ConvertSafe(object value, Type targetType, object parameter, string language)
        {
            return value is bool booleanValue
                ? (booleanValue ? Visibility.Collapsed : Visibility.Visible)
                : Visibility.Collapsed;
        }

        /// <summary>
        /// Reverse conversion is not supported and will throw a <see cref="NotImplementedException"/>.
        /// </summary>
        /// <param name="value">The value to convert back (ignored).</param>
        /// <param name="targetType">The target type (ignored).</param>
        /// <param name="parameter">An optional parameter (ignored).</param>
        /// <param name="language">The language or culture info (ignored).</param>
        /// <returns>This method does not return; it always throws.</returns>
        /// <exception cref="NotImplementedException">Always thrown since reverse conversion is not supported.</exception>
        public object ConvertBackSafe(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException("Reverse conversion is not supported.");
        }
    }
}
