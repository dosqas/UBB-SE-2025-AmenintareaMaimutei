namespace Project.Gui.Converters
{
    using System;
    using Microsoft.UI.Xaml.Data;

    /// <summary>
    /// Converts the string to a double percentage.
    /// </summary>
    public class PercentageConverter : IValueConverter
    {
        /// <summary>
        /// Converts the string to a double percentage.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>The double value of the percentage corresponding to the string.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double width && parameter is string percentageString)
            {
                if (double.TryParse(percentageString, out double percentage))
                {
                    return width * percentage;
                }
            }

            return value; // Fallback to original value
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
