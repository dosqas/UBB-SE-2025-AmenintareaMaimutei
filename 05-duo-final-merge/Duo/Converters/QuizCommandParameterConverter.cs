// <copyright file="QuizCommandParameterConverter.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Converters
{
    using System;
    using Microsoft.UI.Xaml.Data;

    /// <summary>
    /// Converts a quiz ID and a boolean exam flag into a tuple (int, bool) used as a command parameter.
    /// </summary>
    public partial class QuizCommandParameterConverter : IValueConverter
    {
        /// <summary>
        /// Converts the input value (quiz ID) and a parameter (exam flag) into a tuple (quizId, isExam).
        /// </summary>
        /// <param name="value">The value to convert, expected to be an <see cref="int"/> representing the quiz ID.</param>
        /// <param name="targetType">The target type of the binding (not used).</param>
        /// <param name="parameter">An optional parameter, expected to be a <see cref="bool"/> indicating if the quiz is an exam.</param>
        /// <param name="language">The culture info (not used).</param>
        /// <returns>A tuple of (quizId, isExam) if input types match; otherwise, <c>null</c>.</returns>
        public object? Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int quizId && parameter is bool isExam)
            {
                return (quizId, isExam);
            }

            return null;
        }

        /// <summary>
        /// Not implemented. Throws a <see cref="NotImplementedException"/>.
        /// </summary>
        /// <param name="value">The value to convert back.</param>
        /// <param name="targetType">The type to convert back to.</param>
        /// <param name="parameter">An optional parameter.</param>
        /// <param name="language">The culture info.</param>
        /// <returns>Throws <see cref="NotImplementedException"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
