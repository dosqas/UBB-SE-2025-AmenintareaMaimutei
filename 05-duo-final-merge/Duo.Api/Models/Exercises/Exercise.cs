using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Duo.Api.Models.Quizzes;
using Microsoft.EntityFrameworkCore;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1010 // Opening square brackets should be spaced correctly

namespace Duo.Api.Models.Exercises
{
    /// <summary>
    /// Represents the base class for all exercise types in the system.
    /// Configured for Table-Per-Hierarchy (TPH) inheritance to allow different exercise types
    /// to share a single database table with a discriminator column.
    /// </summary>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(FlashcardExercise), "Flashcard")]
    [JsonDerivedType(typeof(MultipleChoiceExercise), "MultipleChoice")]
    [JsonDerivedType(typeof(FillInTheBlankExercise), "FillInTheBlank")]
    [JsonDerivedType(typeof(AssociationExercise), "Association")]
    [Index(nameof(Question))] // Optimizes search queries on the Question field
    [ExcludeFromCodeCoverage]
    [Serializable]
    public abstract class Exercise
    {
        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the exercise.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExerciseId { get; set; }

        /// <summary>
        /// Gets or sets the question or prompt for the exercise.
        /// This field is required and indexed for efficient querying.
        /// </summary>
        [Required]
        public string? Question { get; set; }

        /// <summary>
        /// Gets or sets the difficulty level of the exercise.
        /// </summary>
        public Difficulty Difficulty { get; set; }

        /// <summary>
        /// Navigation property to the exams that include this exercise.
        /// This establishes a many-to-many relationship between exercises and exams.
        /// </summary>
        public ICollection<Exam> Exams { get; set; } = [];

        /// <summary>
        /// Navigation property to the quizzes that include this exercise.
        /// This establishes a many-to-many relationship between exercises and quizzes.
        /// </summary>
        public ICollection<Quiz> Quizzes { get; set; } = [];

        /// <summary>
        /// The discriminator column for TPH inheritance.
        /// </summary>
        public string? Type { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Exercise"/> class.
        /// This parameterless constructor is required for Entity Framework.
        /// </summary>
        public Exercise()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string representation of the exercise, including its ID, question, and difficulty level.
        /// </summary>
        /// <returns>A string describing the exercise.</returns>
        public override string ToString()
        {
            return $"Exercise {ExerciseId}: {Question} (Difficulty: {Difficulty})";
        }

        #endregion
    }
}