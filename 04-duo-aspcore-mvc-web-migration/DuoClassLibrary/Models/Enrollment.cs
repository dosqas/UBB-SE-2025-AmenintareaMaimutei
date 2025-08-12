using System;
using System.Diagnostics.CodeAnalysis;

namespace DuoClassLibrary.Models
{
    /// <summary>
    /// Represents a user's enrollment in a course, including progress and completion status.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Enrollment
    {
        /// <summary>
        /// Gets or sets the unique identifier of the enrolled user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the course the user is enrolled in.
        /// </summary>
        public int CourseId { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the user enrolled in the course.
        /// </summary>
        public DateTime EnrolledAt { get; set; }

        /// <summary>
        /// Gets or sets the total time spent by the user in the course, in seconds.
        /// </summary>
        public int TimeSpent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the course has been completed by the user.
        /// </summary>
        public bool IsCompleted { get; set; }
    }
}