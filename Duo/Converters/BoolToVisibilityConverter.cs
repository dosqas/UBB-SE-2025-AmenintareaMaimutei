// <copyright file="BoolToVisibilityConverter.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Converters
{
    using System;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Data;

    /// <summary>
    /// Converts a boolean value to a <see cref="Visibility"/> value and back.
    /// - true → Visible
    /// - false → Collapsed.
    /// </summary>
    public partial class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to a <see cref="Visibility"/> value.
        /// </summary>
        /// <param name="value">The source boolean value.</param>
        /// <param name="targetType">The target type (unused).</param>
        /// <param name="parameter">Optional parameter (unused).</param>
        /// <param name="language">The culture or language info (unused).</param>
        /// <returns>
        /// <see cref="Visibility.Visible"/> if the input is true; otherwise, <see cref="Visibility.Collapsed"/>.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is bool b && b ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Converts a <see cref="Visibility"/> value back to a boolean.
        /// </summary>
        /// <param name="value">The <see cref="Visibility"/> value to convert back.</param>
        /// <param name="targetType">The target type (expected to be boolean).</param>
        /// <param name="parameter">Optional parameter (unused).</param>
        /// <param name="language">The culture or language info (unused).</param>
        /// <returns>
        /// true if the input is <see cref="Visibility.Visible"/>; otherwise, false.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is Visibility v && v == Visibility.Visible;
        }
    }
}
