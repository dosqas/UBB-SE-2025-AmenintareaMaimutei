// <copyright file="QuizServiceException.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Exceptions
{
    using System;

    /// <summary>
    /// Represents errors that occur within the quiz service layer.
    /// </summary>
    public class QuizServiceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuizServiceException"/> class.
        /// </summary>
        public QuizServiceException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuizServiceException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public QuizServiceException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuizServiceException"/> class with a specified
        /// error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public QuizServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
