// <copyright file="InverseBooleanConverter.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Converters
{
    using System;

    /// <summary>
    /// A converter that inverts a boolean value.
    /// Returns <c>false</c> if the input is <c>true</c>, and <c>true</c> if the input is <c>false</c>.
    /// </summary>
    public partial class InverseBooleanConverter : IAppValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, string language)
            => this.ConvertSafe(value, targetType, parameter, language);

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => this.ConvertBackSafe(value, targetType, parameter, language);

        /// <summary>
        /// Inverts a boolean value.
        /// </summary>
        /// <param name="value">The value to invert (expected to be of type <see cref="bool"/>).</param>
        /// <param name="targetType">The target type (expected to be <see cref="bool"/>).</param>
        /// <param name="parameter">An optional parameter (unused).</param>
        /// <param name="language">The language or culture info (unused).</param>
        /// <returns>
        /// <c>true</c> if <paramref name="value"/> is <c>false</c>,
        /// <c>false</c> if <paramref name="value"/> is <c>true</c>,
        /// otherwise returns the input <paramref name="value"/> unchanged.
        /// </returns>
        public object ConvertSafe(object value, Type targetType, object parameter, string language)
        {
            return value is bool booleanValue ? !booleanValue : value;
        }

        /// <summary>
        /// Inverts a boolean value back to its original.
        /// </summary>
        /// <param name="value">The value to convert back (expected to be of type <see cref="bool"/>).</param>
        /// <param name="targetType">The original type (expected to be <see cref="bool"/>).</param>
        /// <param name="parameter">An optional parameter (unused).</param>
        /// <param name="language">The language or culture info (unused).</param>
        /// <returns>
        /// The inverse of the input boolean value,
        /// or the original value if it is not of type <see cref="bool"/>.
        /// </returns>
        public object ConvertBackSafe(object value, Type targetType, object parameter, string language)
        {
            return value is bool booleanValue ? !booleanValue : value;
        }
    }
}
