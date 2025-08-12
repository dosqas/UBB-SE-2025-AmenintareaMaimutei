using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Duo.Api.Models.Exercises;
using Duo.Api.Models.Sections;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1010 // Opening square brackets should be spaced correctly

namespace Duo.Api.Models.Quizzes
{
    /// <summary>
    /// Represents a concrete quiz implementation with ordering capability.
    /// Inherits from BaseQuiz to share common quiz properties and behavior.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class Quiz
    {
        #region Fields and Properties

        /// <summary>
        /// Gets or sets the unique identifier for the quiz.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the section this quiz belongs to.
        /// This is a foreign key relationship.
        /// </summary>
        public int? SectionId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the section this quiz belongs to.
        /// </summary>
        public Section? Section { get; set; }

        /// <summary>
        /// Gets or sets the order number of the quiz within its section.
        /// This determines the sequence in which quizzes appear.
        /// </summary>
        public int? OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the collection of exercises included in this quiz.
        /// </summary>
        public ICollection<Exercise> Exercises { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of completions associated with this quiz.
        /// </summary>
        public ICollection<QuizCompletions> QuizCompletions { get; set; } = [];

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Quiz"/> class.
        /// </summary>
        public Quiz()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string representation of the quiz, including its ID and the number of exercises.
        /// </summary>
        /// <returns>A string describing the quiz.</returns>
        public override string ToString()
        {
            return $"Quiz ID: {Id}, Exercises Count: {Exercises.Count}";
        }

        #endregion
    }
}
