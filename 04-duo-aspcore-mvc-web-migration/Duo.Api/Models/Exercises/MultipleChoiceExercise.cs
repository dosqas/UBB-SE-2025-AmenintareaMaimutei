using System.Diagnostics.CodeAnalysis;

namespace Duo.Api.Models.Exercises
{
    /// <summary>
    /// Represents a multiple-choice exercise where users select one or more correct answers from a list of options.
    /// Inherits from the <see cref="Exercise"/> base class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class MultipleChoiceExercise : Exercise
    {
        #region Fields and Properties

        /// <summary>
        /// Gets or sets the list of answer choices for the multiple-choice exercise.
        /// Each choice includes the answer text and whether it is correct.
        /// </summary>
        public List<MultipleChoiceAnswerModel>? Choices { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleChoiceExercise"/> class.
        /// This parameterless constructor is required for Entity Framework.
        /// </summary>
        public MultipleChoiceExercise()
        {
            //Type = "MultipleChoice";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates the user's answers against the correct answers for the exercise.
        /// </summary>
        /// <param name="userAnswers">A list of answers provided by the user.</param>
        /// <returns>
        /// <c>true</c> if the user's answers match the correct answers; otherwise, <c>false</c>.
        /// </returns>
        public bool ValidateAnswer(List<string> userAnswers)
        {
            if (userAnswers == null || userAnswers.Count == 0)
            {
                return false;
            }

            var correctAnswers = Choices!
                .Where(a => a.IsCorrect)
                .Select(a => a.Answer)
                .OrderBy(a => a)
                .ToList();

            var userSelection = userAnswers
                .OrderBy(a => a)
                .ToList();

            return correctAnswers.SequenceEqual(userSelection);
        }

        /// <summary>
        /// Returns a string representation of the multiple-choice exercise, including its question, difficulty, and answer choices.
        /// </summary>
        /// <returns>A string describing the multiple-choice exercise.</returns>
        public override string ToString()
        {
            var choices = string.Join(", ", Choices!.Select(c => $"{c.Answer}{(c.IsCorrect ? " (Correct)" : string.Empty)}"));
            return $"{base.ToString()} [Multiple Choice] Choices: {choices}";
        }

        #endregion
    }
}
