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
    /// Represents an exam, a specialized type of quiz with additional constraints or behaviors.
    /// Inherits from BaseQuiz to share common quiz properties and functionality.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class Exam
    {
        #region Fields and Properties

        /// <summary>
        /// Gets or sets the unique identifier for the exam.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the section this exam belongs to.
        /// This is a foreign key relationship.
        /// </summary>
        public int? SectionId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the section this exam belongs to.
        /// </summary>
        public Section? Section { get; set; }

        /// <summary>
        /// Gets or sets the collection of exercises included in this exam.
        /// </summary>
        public ICollection<Exercise> Exercises { get; set; } = [];

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Exam"/> class.
        /// </summary>
        public Exam()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string representation of the exam, including its ID and the number of exercises.
        /// </summary>
        /// <returns>A string describing the exam.</returns>
        public override string ToString()
        {
            return $"Exam ID: {Id}, Exercises Count: {Exercises.Count}";
        }

        #endregion
    }
}
