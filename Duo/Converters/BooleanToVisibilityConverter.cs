using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace Duo.Converters
{
    /// <summary>
    /// Converts boolean values to and from Visibility enum values
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to a Visibility enum value
        /// </summary>
        /// <param name="value">The source boolean value</param>
        /// <param name="targetType">The target type</param>
        /// <param name="parameter">Optional parameter to invert the conversion</param>
        /// <param name="language">The language</param>
        /// <returns>Visibility.Visible if true, Visibility.Collapsed if false (or the inverse if parameter specifies)</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool invert = parameter != null && parameter.ToString() == "Invert";
            bool visibility = value is bool boolValue ? boolValue : false;
            
            if (invert)
            {
                visibility = !visibility;
            }
            
            return visibility ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Converts a Visibility enum value back to a boolean value
        /// </summary>
        /// <param name="value">The source Visibility enum value</param>
        /// <param name="targetType">The target type</param>
        /// <param name="parameter">Optional parameter to invert the conversion</param>
        /// <param name="language">The language</param>
        /// <returns>True if Visibility.Visible, false otherwise (or the inverse if parameter specifies)</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            bool invert = parameter != null && parameter.ToString() == "Invert";
            bool visibility = value is Visibility visibilityValue ? visibilityValue == Visibility.Visible : false;
            
            if (invert)
            {
                visibility = !visibility;
            }
            
            return visibility;
        }
    }
} 