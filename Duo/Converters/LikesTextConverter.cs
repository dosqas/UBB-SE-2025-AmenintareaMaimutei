// <copyright file="LikesTextConverter.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Converters
{
    using System;
    using Microsoft.UI.Xaml.Data;

    /// <summary>
    /// Converts an integer like count into a formatted string (e.g., "12 likes").
    /// Returns a default text ("0 likes") if the value is not a valid integer.
    /// </summary>
    public partial class LikesTextConverter : IValueConverter
    {
        // Constants for text formatting
        private const string LIKESFORMAT = "{0} likes";
        private const string DEFAULTLIKESTEXT = "0 likes";

        /// <summary>
        /// Converts an integer value to a formatted like count string.
        /// </summary>
        /// <param name="value">The value to convert (expected to be an <see cref="int"/>).</param>
        /// <param name="targetType">The target binding type (usually <see cref="string"/>).</param>
        /// <param name="parameter">Optional converter parameter (unused).</param>
        /// <param name="language">The language or culture info (unused).</param>
        /// <returns>
        /// A string in the format "{n} likes" if <paramref name="value"/> is an <see cref="int"/>,
        /// otherwise "0 likes".
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is int likeCount
                ? string.Format(LIKESFORMAT, likeCount)
                : DEFAULTLIKESTEXT;
        }

        /// <summary>
        /// Reverse conversion is not supported and will throw a <see cref="NotImplementedException"/>.
        /// </summary>
        /// <param name="value">The value to convert back (ignored).</param>
        /// <param name="targetType">The target type (ignored).</param>
        /// <param name="parameter">Optional parameter (ignored).</param>
        /// <param name="language">The language or culture info (ignored).</param>
        /// <returns>This method does not return; it always throws.</returns>
        /// <exception cref="NotImplementedException">Always thrown since reverse conversion is not supported.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException("Reverse conversion is not supported.");
        }
    }
}
