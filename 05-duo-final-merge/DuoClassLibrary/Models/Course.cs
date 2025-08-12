using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace DuoClassLibrary.Models
{
    /// <summary>
    /// Represents a course with its details such as title, description, difficulty, and pricing information.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class Course
    {
        #region Fields and Properties

        /// <summary>
        /// Gets or sets the unique identifier of the course.
        /// </summary>
        [Key]
        public int CourseId { get; set; }

        /// <summary>
        /// Gets or sets the title of the course.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the course.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the course is premium.
        /// </summary>
        public bool IsPremium { get; set; }

        /// <summary>
        /// Gets or sets the cost of the course in coins.
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// Gets or sets the image URL representing the course.
        /// </summary>
        public required string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the estimated time to complete the course, in seconds.
        /// </summary>
        public int TimeToComplete { get; set; }

        /// <summary>
        /// Gets or sets the difficulty level of the course (e.g., Beginner, Intermediate, Advanced).
        /// </summary>
        public required string Difficulty { get; set; }

        public List<Enrollment>? Enrollments { get; set; }

        public List<Tag> Tags { get; set; } = new();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Course"/> class.
        /// </summary>
        public Course()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string representation of the course, including its title and difficulty level.
        /// </summary>
        /// <returns>A string describing the course.</returns>
        public override string ToString()
        {
            return $"Course: {Title}, Difficulty: {Difficulty}, Cost: {Cost} coins";
        }

        #endregion
    }
}
