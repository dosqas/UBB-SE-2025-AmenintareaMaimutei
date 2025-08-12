using System.Diagnostics.CodeAnalysis;

namespace Duo.Api.Models.Exercises
{
    #region Enums

    /// <summary>
    /// Enum representing the different types of exercises in the system.
    /// This enum helps categorize exercises by their type.
    /// </summary>
    public enum ExerciseType
    {
        /// <summary>
        /// Represents an association exercise where users match items from two lists.
        /// </summary>
        Association,

        /// <summary>
        /// Represents a fill-in-the-blank exercise where users complete missing parts of a sentence or text.
        /// </summary>
        FillInTheBlank,

        /// <summary>
        /// Represents a multiple-choice exercise where users select one or more correct answers from a list of options.
        /// </summary>
        MultipleChoice,

        /// <summary>
        /// Represents a flashcard exercise used for studying and memorization.
        /// </summary>
        Flashcard
    }

    #endregion

    #region Static Classes

    /// <summary>
    /// A static class that contains predefined exercise types as strings.
    /// This is used to quickly access a list of available exercise types in the system.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ExerciseTypes
    {
        #region Fields

        /// <summary>
        /// List of available exercise types represented as strings.
        /// </summary>
        public static readonly List<string> ExerciseTypeList =
        [
            "Association",
            "Fill in the Blank",
            "Multiple Choice",
            "Flashcard"
        ];

        #endregion
    }

    #endregion
}