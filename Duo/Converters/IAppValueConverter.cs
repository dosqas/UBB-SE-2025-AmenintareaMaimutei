// <copyright file="IAppValueConverter.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Converters
{
    using System;
    using Microsoft.UI.Xaml.Data;

    /// <summary>
    /// Provides a base interface for value converters with type-safe method variants.
    /// Implements <see cref="IValueConverter"/> and adds <c>ConvertSafe</c> and <c>ConvertBackSafe</c>
    /// for clearer separation of safe logic versus the general interface implementation.
    /// </summary>
    public interface IAppValueConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value to a different type in a type-safe way.
        /// Typically called internally by the standard <see cref="IValueConverter.Convert"/> implementation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The target type of the conversion.</param>
        /// <param name="parameter">An optional parameter to assist in the conversion logic.</param>
        /// <param name="language">The language or culture info used in the conversion.</param>
        /// <returns>The converted value.</returns>
        object ConvertSafe(object value, Type targetType, object parameter, string language);

        /// <summary>
        /// Converts a value back to its source type in a type-safe way.
        /// Typically called internally by the standard <see cref="IValueConverter.ConvertBack"/> implementation.
        /// </summary>
        /// <param name="value">The value to convert back.</param>
        /// <param name="targetType">The target type to convert to.</param>
        /// <param name="parameter">An optional parameter to assist in the conversion logic.</param>
        /// <param name="language">The language or culture info used in the conversion.</param>
        /// <returns>The converted back value.</returns>
        object ConvertBackSafe(object value, Type targetType, object parameter, string language);
    }
}
