namespace DuoClassLibrary.Constants
{
    public static class MockDataConstants
    {
        /// <summary>
        /// Milliseconds delay for simulating asynchronous operations in mock services
        /// </summary>
        public const int MockAsyncOperationDelayMilliseconds = 100;

        /// <summary>
        /// Minimum number of random quizzes to generate
        /// </summary>
        public const int MinimumRandomQuizCount = 10;

        /// <summary>
        /// Maximum number of random quizzes to generate
        /// </summary>
        public const int MaximumRandomQuizCount = 21;

        /// <summary>
        /// Maximum days in the past for generated completion dates
        /// </summary>
        public const int MaximumCompletionDaysInPast = 365;

        /// <summary>
        /// Minimum random quiz index
        /// </summary>
        public const int MinimumRandomIndex = 1;

        /// <summary>
        /// Decimal places for rounding accuracy percentages
        /// </summary>
        public const int AccuracyDecimalPlaces = 2;

    }
} 