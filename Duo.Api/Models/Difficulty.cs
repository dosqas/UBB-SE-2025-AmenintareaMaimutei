using System.Diagnostics.CodeAnalysis;

namespace Duo.Api.Models
{
    /// <summary>
    /// Represents the difficulty levels for exercises or courses.
    /// </summary>
    public enum Difficulty
    {
        /// <summary>
        /// Represents an easy difficulty level.
        /// </summary>
        Easy = 1,

        /// <summary>
        /// Represents a normal difficulty level.
        /// </summary>
        Normal = 2,

        /// <summary>
        /// Represents a hard difficulty level.
        /// </summary>
        Hard = 3
    }

    /// <summary>
    /// Provides a static list of difficulty levels as strings.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DifficultyList
    {
        #region Fields

        /// <summary>
        /// A list of difficulty levels represented as strings.
        /// </summary>
        public static readonly List<string> Difficulties =
        [
            "Easy",
            "Normal",
            "Hard"
        ];

        #endregion
    }
}
