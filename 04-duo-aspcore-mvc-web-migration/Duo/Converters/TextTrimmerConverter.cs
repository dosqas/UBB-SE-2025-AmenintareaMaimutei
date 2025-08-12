using System;

namespace Duo.Converters
{
    /// <summary>
    /// A converter that trims a string to a specified length and adds ellipsis ("...") at the end.
    /// If no custom length is provided, the string is trimmed to a default length of 23 characters.
    /// </summary>
    public partial class TextTrimmerConverter : IAppValueConverter
    {
        private const int DefaultTrimLength = 23;
        private const string Ellipsis = "...";

        public object Convert(object value, Type targetType, object parameter, string language)
            => ConvertSafe(value, targetType, parameter, language);

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => ConvertBackSafe(value, targetType, parameter, language);

        /// <summary>
        /// Converts a string by trimming it to a specified length and adding an ellipsis ("...") if the string is too long.
        /// </summary>
        /// <param name="value">The string to be converted.</param>
        /// <param name="targetType">The target type (expected to be a string).</param>
        /// <param name="parameter">Optional parameter specifying a custom trim length as a string (can be null).</param>
        /// <param name="language">The language for localization.</param>
        /// <returns>A trimmed string with ellipsis if it exceeds the trim length; otherwise, the original string.</returns>
        public object ConvertSafe(object value, Type targetType, object parameter, string language)
        {
            // Ensure the value is a string before processing
            if (value is string inputText)
            {
                // Default to the predefined trim length
                int trimLength = DefaultTrimLength;
                // If a parameter is provided, attempt to parse it as a custom trim length
                if (parameter is string paramText && int.TryParse(paramText, out int customLength))
                {
                    trimLength = customLength;
                }
                // If the string is longer than the trim length, truncate it and append ellipsis
                return inputText.Length > trimLength
                    ? string.Concat(inputText.AsSpan(0, trimLength), Ellipsis)
                    : inputText;
            }
            // If the value is not a string, return the original value.
            return value;
        }

        /// <summary>
        /// Reverse conversion is not supported.
        /// </summary>
        /// <param name="value">The value to convert back (not used in this case).</param>
        /// <param name="targetType">The target type (not used).</param>
        /// <param name="parameter">Any optional parameters passed (not used in this case).</param>
        /// <param name="language">The language for localization (not used in this case).</param>
        /// <returns>Throws NotImplementedException because reverse conversion is not needed.</returns>
        public object ConvertBackSafe(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException("Reverse conversion is not supported.");
        }
    }
}
