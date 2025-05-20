using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Duo.Api.Models.Exercises
{
    /// <summary>
    /// Represents an answer option for a multiple-choice exercise.
    /// This model includes the answer text and whether it is correct.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class MultipleChoiceAnswerModel
    {
        #region Fields and Properties

        /// <summary>
        /// Gets or sets the unique identifier for the answer model.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerModelId { get; set; }

        /// <summary>
        /// Gets or sets the text of the answer.
        /// </summary>
        public string? Answer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this answer is correct.
        /// </summary>
        public bool IsCorrect { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the related <see cref="MultipleChoiceExercise"/>.
        /// </summary>
        [Required]
        public int ExerciseId { get; set; }

        /// <summary>
        /// Navigation property to the related <see cref="MultipleChoiceExercise"/>.
        /// </summary>
        [ForeignKey(nameof(ExerciseId))]
        public MultipleChoiceExercise Exercise { get; set; } = null!;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleChoiceAnswerModel"/> class.
        /// This parameterless constructor is required for Entity Framework.
        /// </summary>
        public MultipleChoiceAnswerModel()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string representation of the answer model, including whether it is correct.
        /// </summary>
        /// <returns>A string describing the answer model.</returns>
        public override string ToString()
        {
            return $"{Answer}{(IsCorrect ? " (Correct)" : string.Empty)}";
        }

        #endregion
    }
}