namespace Project.Utils
{
    using System;
    using Microsoft.UI.Xaml.Data;

    /// <summary>
    /// A color converter, based on the message.
    /// </summary>
    public class MessageToColorConverter : IValueConverter
    {
        /// <summary>
        /// Converts the message to a corresponding color.
        /// </summary>
        /// <param name="value">The message.</param>
        /// <param name="targetType">The type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>The color corresponding to the message.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string message)
            {
                if (message.Contains("successfully"))
                {
                    return "Green";
                }
                else
                {
                    return "Red";
                }
            }

            return "Red";
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value">The color.</param>
        /// <param name="targetType">The type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>The message corresponding to the color.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
