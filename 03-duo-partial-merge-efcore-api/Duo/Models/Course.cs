using System.Diagnostics.CodeAnalysis;

namespace CourseApp.Models
{
    /// <summary>
    /// Represents a course with its details such as title, description, difficulty, and pricing information.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Course
    {
        /// <summary>
        /// Gets or sets the unique identifier of the course.
        /// </summary>
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
    }
}
