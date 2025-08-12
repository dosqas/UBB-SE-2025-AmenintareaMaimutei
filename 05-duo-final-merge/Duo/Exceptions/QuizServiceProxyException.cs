// <copyright file="QuizServiceProxyException.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Exceptions
{
    using System;

    /// <summary>
    /// Represents errors that occur during proxy operations in the quiz service,
    /// such as remote communication failures or service unavailability.
    /// </summary>
    public class QuizServiceProxyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuizServiceProxyException"/> class.
        /// </summary>
        public QuizServiceProxyException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuizServiceProxyException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public QuizServiceProxyException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuizServiceProxyException"/> class
        /// with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public QuizServiceProxyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
