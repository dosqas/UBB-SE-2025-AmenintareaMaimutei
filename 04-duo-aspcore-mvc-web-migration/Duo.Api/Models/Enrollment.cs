using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Duo.Api.Models
{
    /// <summary>
    /// Represents a user's enrollment in a course, including progress and completion status.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class Enrollment
    {
        #region Fields and Properties

        /// <summary>
        /// Gets or sets the unique identifier of the enrolled user.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the course the user is enrolled in.
        /// </summary>
        public int? CourseId { get; set; }

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

        /// <summary>
        /// Navigation property to the related user.
        /// </summary>
        public User? User { get; set; } = null!;

        /// <summary>
        /// Navigation property to the related course.
        /// </summary>
        public Course? Course { get; set; } = null!;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Enrollment"/> class.
        /// </summary>
        public Enrollment()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string representation of the enrollment, including the user ID, course ID, and enrollment status.
        /// </summary>
        /// <returns>A string describing the enrollment.</returns>
        public override string ToString()
        {
            return $"User ID: {UserId}, Course ID: {CourseId}, Enrolled At: {EnrolledAt}, " +
                   $"Time Spent: {TimeSpent} seconds, Completed: {IsCompleted}";
        }

        #endregion
    }
}
