// <copyright file="TextTrimmerConverter.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Converters
{
    using System;
    using Duo.Converters;

    /// <summary>
    /// A converter that trims a string to a specified length and adds an ellipsis ("...") if the string exceeds that length.
    /// Defaults to trimming at 23 characters if no custom length is provided via the parameter.
    /// </summary>
    public partial class TextTrimmerConverter : IAppValueConverter
    {
        private const int DefaultTrimLength = 23;
        private const string Ellipsis = "...";

        /// <summary>
        /// Converts a string by trimming it and appending an ellipsis ("...") if it's longer than the allowed length.
        /// </summary>
        /// <param name="value">The value to convert, expected to be a string.</param>
        /// <param name="targetType">The target type of the binding (typically string).</param>
        /// <param name="parameter">Optional parameter specifying the maximum length before trimming, as a string.</param>
        /// <param name="language">The culture info (not used).</param>
        /// <returns>The trimmed string with ellipsis if needed, or the original string.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
            => this.ConvertSafe(value, targetType, parameter, language);

        /// <summary>
        /// ConvertBack is not supported for this converter.
        /// </summary>
        /// <param name="value">The value to convert back (not used).</param>
        /// <param name="targetType">The type to convert back to (not used).</param>
        /// <param name="parameter">Any optional parameter (not used).</param>
        /// <param name="language">The culture info (not used).</param>
        /// <returns>Throws <see cref="NotImplementedException"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => this.ConvertBackSafe(value, targetType, parameter, language);

        /// <summary>
        /// Trims the input string to a specified or default length, appending an ellipsis if the text is too long.
        /// </summary>
        /// <param name="value">The string to be trimmed.</param>
        /// <param name="targetType">The target type (expected to be a string).</param>
        /// <param name="parameter">An optional string specifying a custom trim length.</param>
        /// <param name="language">The culture info (not used).</param>
        /// <returns>A trimmed string with ellipsis if it exceeds the trim length; otherwise, the original string.</returns>
        public object ConvertSafe(object value, Type targetType, object parameter, string language)
        {
            if (value is string inputText)
            {
                int trimLength = DefaultTrimLength;

                if (parameter is string paramText && int.TryParse(paramText, out int customLength))
                {
                    trimLength = customLength;
                }

                return inputText.Length > trimLength
                    ? string.Concat(inputText.AsSpan(0, trimLength), Ellipsis)
                    : inputText;
            }

            return value;
        }

        /// <summary>
        /// Reverse conversion is not supported and will throw an exception if called.
        /// </summary>
        /// <param name="value">The value to convert back (not used).</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">An optional parameter (not used).</param>
        /// <param name="language">The culture info (not used).</param>
        /// <returns>Throws <see cref="NotImplementedException"/>.</returns>
        public object ConvertBackSafe(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException("Reverse conversion is not supported.");
        }
    }
}
