namespace Project.Utils
{
    using System;
    using Microsoft.UI.Xaml.Data;

    /// <summary>
    /// Converts DateOnly to string and vice versa.
    /// </summary>
    public class DateOnlyToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts a DateOnly value to a string.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>The converted value as a string.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateOnly date)
            {
                return date.ToString("yyyy-MM-dd"); // Format it as a string (you can adjust the format)
            }

            return string.Empty;
        }

        /// <summary>
        /// Converts a string value back to a DateOnly.
        /// </summary>
        /// <param name="value">The value to convert back.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>The converted value as a DateOnly.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string valueString && DateOnly.TryParse(valueString, out DateOnly date))
            {
                return date;
            }

            return new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day); // Default value if the string is not valid
        }
    }
}