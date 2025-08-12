// <copyright file="StatusColorConverter.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Converters
{
    using System;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Media;

    /// <summary>
    /// Converts a boolean status to a color brush: green for true, gray for false.
    /// </summary>
    public partial class StatusColorConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to a <see cref="SolidColorBrush"/>:
        /// Green if <c>true</c>, Gray if <c>false</c>.
        /// </summary>
        /// <param name="value">The value to convert, expected to be a <see cref="bool"/>.</param>
        /// <param name="targetType">The target type of the binding (not used).</param>
        /// <param name="parameter">An optional parameter (not used).</param>
        /// <param name="language">The culture info (not used).</param>
        /// <returns>A green brush if true, a gray brush if false or if input is invalid.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool status)
            {
                return new SolidColorBrush(status ? Microsoft.UI.Colors.Green : Microsoft.UI.Colors.Gray);
            }

            return new SolidColorBrush(Microsoft.UI.Colors.Gray); // Default color
        }

        /// <summary>
        /// Not implemented. Throws a <see cref="NotImplementedException"/>.
        /// </summary>
        /// <param name="value">The value to convert back.</param>
        /// <param name="targetType">The type to convert back to.</param>
        /// <param name="parameter">An optional parameter.</param>
        /// <param name="language">The culture info.</param>
        /// <returns>Throws <see cref="NotImplementedException"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
