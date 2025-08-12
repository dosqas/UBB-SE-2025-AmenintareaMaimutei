using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.UI.Xaml.Media;

namespace Duo.Converters
{
    /// <summary>
    /// Converts a boolean value to a color brush indicating availability.
    /// If the value is true, the brush will be blue (indicating available);
    /// if false, the brush will be gray (indicating unavailable).
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class AvailabilityColorConverter : IAppValueConverter
    {
        // Static color values for available and unavailable states.
        private static readonly Windows.UI.Color AvailableColor = Windows.UI.Color.FromArgb(255, 79, 79, 176);
        private static readonly Windows.UI.Color UnavailableColor = Windows.UI.Color.FromArgb(255, 128, 128, 128);

        public object Convert(object value, Type targetType, object parameter, string language)
            => ConvertSafe(value, targetType, parameter, language);

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => ConvertBackSafe(value, targetType, parameter, language);

        /// <summary>
        /// Converts a boolean value indicating availability to a SolidColorBrush.
        /// If true, returns a blue color indicating availability;
        /// if false, returns a gray color indicating unavailability.
        /// </summary>
        /// <param name="value">The boolean value representing availability.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">Any optional parameters passed in XAML.</param>
        /// <param name="language">The language for localization.</param>
        /// <returns>A SolidColorBrush based on the availability state.</returns>
        public object ConvertSafe(object value, Type targetType, object parameter, string language)
        {
            // Check if the input value is a boolean in order to indicate availability.
            if (value is bool isAvailable)
            {
                // Return a brush with the appropriate color based on availability.
                return new SolidColorBrush(isAvailable ? AvailableColor : UnavailableColor);
            }
            // Return a default color (gray) if the value is not a boolean.
            return new SolidColorBrush(UnavailableColor);
        }

        /// <summary>
        /// The ConvertBack method is not implemented as this converter only supports one-way conversion.
        /// </summary>
        /// <param name="value">The value to convert back.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">Any optional parameters passed in XAML.</param>
        /// <param name="language">The language for localization.</param>
        /// <returns>Throws NotImplementedException because reverse conversion is not needed.</returns>
        public object ConvertBackSafe(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException("Reverse conversion is not supported");
        }
    }
}