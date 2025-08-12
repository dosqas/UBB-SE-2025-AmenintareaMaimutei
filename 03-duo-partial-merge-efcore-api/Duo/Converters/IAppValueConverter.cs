using System;
using Microsoft.UI.Xaml.Data;

namespace Duo.Converters
{
    public interface IAppValueConverter : IValueConverter
    {
        /// <summary>
        /// Type-safe version of Convert with default implementation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">Optional parameter.</param>
        /// <param name="language">The language for localization.</param>
        /// <returns>The converted value.</returns>
        object ConvertSafe(object value, Type targetType, object parameter, string language);

        /// <summary>
        /// Type-safe version of ConvertBack with default implementation.
        /// </summary>
        /// <param name="value">The value to convert back.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">Optional parameter.</param>
        /// <param name="language">The language for localization.</param>
        /// <returns>The converted back value.</returns>
        object ConvertBackSafe(object value, Type targetType, object parameter, string language);
    }
}
