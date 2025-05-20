using System;

namespace DuoClassLibrary.Exceptions
{
    public class QuizServiceException : Exception
    {
        public QuizServiceException()
        {
        }

        public QuizServiceException(string message) : base(message)
        {
        }

        public QuizServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}