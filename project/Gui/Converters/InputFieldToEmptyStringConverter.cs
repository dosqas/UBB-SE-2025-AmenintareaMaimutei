namespace Project.Utils
{
    using System;
    using Microsoft.UI.Xaml.Data;

    /// <summary>
    /// A converter from zero to empty string.
    /// </summary>
    public class InputFieldToEmptyStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts the value to the target type.
        /// </summary>
        /// <param name="value">The given value.</param>
        /// <param name="targetType">The given target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>The converted value.</returns>
        public object? Convert(object value, Type targetType, object parameter, string language)
        {
            if ((value is float floatValue && floatValue == 0) ||
                (value is int intValue && intValue == 0) ||
                (value is DateOnly dateValue && dateValue == DateOnly.MinValue) ||
                (value is TimeSpan timeValue && timeValue == TimeSpan.Zero))
            {
                return string.Empty;
            }

            return value.ToString();
        }

        /// <summary>
        /// Converts the value to the target type.
        /// </summary>
        /// <param name="value">The given value.</param>
        /// <param name="targetType">The given target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>The converted value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string stringValue && string.IsNullOrEmpty(stringValue))
            {
                if (targetType == typeof(float) || targetType == typeof(int))
                {
                    return 0;
                }

                if (targetType == typeof(DateOnly))
                {
                    return DateOnly.MinValue;
                }

                if (targetType == typeof(TimeSpan))
                {
                    return TimeSpan.Zero;
                }
            }

            if (targetType == typeof(float) && float.TryParse(value.ToString(), out float floatResult))
            {
                return floatResult;
            }

            if (targetType == typeof(int) && int.TryParse(value.ToString(), out int intResult))
            {
                return intResult;
            }

            if (targetType == typeof(DateOnly) && DateOnly.TryParse(value.ToString(), out DateOnly dateResult))
            {
                return dateResult;
            }

            if (targetType == typeof(TimeSpan) && TimeSpan.TryParse(value.ToString(), out TimeSpan timeResult))
            {
                return timeResult;
            }

            return value;
        }
    }
}