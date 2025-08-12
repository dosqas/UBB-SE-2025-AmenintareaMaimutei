using System;
using Microsoft.UI.Xaml;

namespace Duo.Converters
{
    /// <summary>
    /// Converts a boolean value to a <see cref="Visibility"/> value.
    /// If the boolean is true, it returns <see cref="Visibility.Collapsed"/>.
    /// If the boolean is false, it returns <see cref="Visibility.Visible"/>.
    /// </summary>
    public partial class InverseBooleanToVisibilityConverter : IAppValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => ConvertSafe(value, targetType, parameter, language);

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => ConvertBackSafe(value, targetType, parameter, language);

        /// <summary>
        /// Converts a boolean value to a <see cref="Visibility"/> value.
        /// Inverts the boolean value: returns Collapsed if true, and Visible if false.
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <param name="targetType">The target type (expected to be <see cref="Visibility"/>).</param>
        /// <param name="parameter">Any optional parameters passed in XAML.</param>
        /// <param name="language">The language for localization.</param>
        /// <returns><see cref="Visibility.Collapsed"/> if true, <see cref="Visibility.Visible"/> if false,
        /// or <see cref="Visibility.Collapsed"/> if the value is not boolean.</returns>
        public object ConvertSafe(object value, Type targetType, object parameter, string language)
        {
            // Check if the value is a boolean and apply the inverse logic for visibility
            if (value is bool booleanValue)
            {
                // Return Collapsed when true, and Visible when false
                return booleanValue ? Visibility.Collapsed : Visibility.Visible;
            }
            // Return a default visibility state (Collapsed) if the value isn't a boolean
            return Visibility.Collapsed;
        }

        /// <summary>
        /// Reverse conversion is not supported for this converter as it's one-way.
        /// </summary>
        /// <param name="value">The value to convert back.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">Any optional parameters passed.</param>
        /// <param name="language">The language for localization.</param>
        /// <returns>Throws NotImplementedException because reverse conversion is not needed.</returns>
        public object ConvertBackSafe(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException("Reverse conversion is not supported.");
        }
    }
}