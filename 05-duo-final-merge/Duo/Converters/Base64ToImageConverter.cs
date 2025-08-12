// <copyright file="Base64ToImageConverter.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Converters
{
    using System;
    using System.IO;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Media.Imaging;

    /// <summary>
    /// Converts a Base64-encoded string to a BitmapImage for use in XAML bindings.
    /// </summary>
    public partial class Base64ToImageConverter : IValueConverter
    {
        /// <summary>
        /// Converts a Base64 string to a BitmapImage.
        /// </summary>
        /// <param name="value">The Base64-encoded image string.</param>
        /// <param name="targetType">The target type of the binding (unused).</param>
        /// <param name="parameter">An optional parameter (unused).</param>
        /// <param name="language">The language of the conversion (unused).</param>
        /// <returns>
        /// A BitmapImage if conversion is successful; otherwise, null.
        /// </returns>
        public object? Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string base64String && !string.IsNullOrEmpty(base64String))
            {
                try
                {
                    byte[] imageBytes = System.Convert.FromBase64String(base64String);
                    using var stream = new MemoryStream(imageBytes);
                    var bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(stream.AsRandomAccessStream());
                    return bitmapImage;
                }
                catch (FormatException)
                {
                    // Handle the exception or log it
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Not implemented. Converts a BitmapImage back to a Base64 string.
        /// </summary>
        /// <param name="value">The value produced by the binding target (unused).</param>
        /// <param name="targetType">The type to convert to (unused).</param>
        /// <param name="parameter">An optional parameter (unused).</param>
        /// <param name="language">The language of the conversion (unused).</param>
        /// <returns>
        /// This method always throws a NotImplementedException.
        /// </returns>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
