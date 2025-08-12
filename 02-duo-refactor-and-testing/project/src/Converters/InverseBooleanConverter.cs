using System;

namespace CourseApp.Converters
{
    /// <summary>
    /// A converter that inverts a boolean value.
    /// If the value is true, it returns false.
    /// if the value is false, it returns true.
    /// </summary>
    public partial class InverseBooleanConverter : IAppValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => ConvertSafe(value, targetType, parameter, language);

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => ConvertBackSafe(value, targetType, parameter, language);

        /// <summary>
        /// Converts a boolean value to its inverse.
        /// </summary>
        /// <param name="value">The boolean value to invert (expected to be a boolean).</param>
        /// <param name="targetType">The target type (expected to be a boolean).</param>
        /// <param name="parameter">Any optional parameters passed in XAML.</param>
        /// <param name="language">The language for localization.</param>
        /// <returns>The inverse of the input boolean value, or the original value if it's not boolean.</returns>
        public object ConvertSafe(object value, Type targetType, object parameter, string language)
        {
            // Ensure the value is a boolean before inverting it.
            if (value is bool booleanValue)
            {
                // Return the inverse of the boolean value.
                return !booleanValue;
            }
            // Return the original value if it's not a boolean.
            return value;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Converts the inverted boolean value back to its original state.
        /// </summary>
        /// <param name="value">The value to convert back (expected to be a boolean).</param>
        /// <param name="targetType">The target type (expected to be a boolean).</param>
        /// <param name="parameter">Any optional parameters passed in XAML.</param>
        /// <param name="language">The language for localization.</param>
        /// <returns>The inverse of the input boolean value, or the original value if it's not boolean.</returns>
        public object ConvertBackSafe(object value, Type targetType, object parameter, string language)
        {
            // Ensure the value is a boolean before inverting it.
            if (value is bool booleanValue)
            {
                // Return the inverse of the boolean value.
                return !booleanValue;
            }
            // Return the original value if it's not a boolean.
            return value;
        }
    }
}
