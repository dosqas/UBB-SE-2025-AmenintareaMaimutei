// <copyright file="PercentageConverter.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Converters
{
    using System;
    using Microsoft.UI.Xaml.Data;

    /// <summary>
    /// Converts a double value (e.g., 0.25) to a percentage string (e.g., "25%").
    /// </summary>
    public partial class PercentageConverter : IValueConverter
    {
        /// <summary>
        /// Converts a double value (e.g., 0.25) to a percentage string (e.g., "25%").
        /// </summary>
        /// <param name="value">The value to convert (expected to be a double).</param>
        /// <param name="targetType">The target type of the binding (not used).</param>
        /// <param name="parameter">An optional parameter (not used).</param>
        /// <param name="language">The culture info (not used).</param>
        /// <returns>A formatted percentage string (e.g., "25%").</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double percentage)
            {
                return (percentage * 100).ToString("F0") + "%";
            }

            return "0%";
        }

        /// <summary>
        /// Not implemented. Throws a NotImplementedException.
        /// </summary>
        /// <param name="value">The value to convert back.</param>
        /// <param name="targetType">The type to convert back to.</param>
        /// <param name="parameter">An optional parameter.</param>
        /// <param name="language">The culture info.</param>
        /// <returns>Throws NotImplementedException.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
