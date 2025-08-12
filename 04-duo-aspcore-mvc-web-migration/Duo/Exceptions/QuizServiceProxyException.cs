using System;

namespace Duo.Exceptions
{
    public class QuizServiceProxyException : Exception
    {
        public QuizServiceProxyException()
        {
        }

        public QuizServiceProxyException(string message) : base(message)
        {
        }

        public QuizServiceProxyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
