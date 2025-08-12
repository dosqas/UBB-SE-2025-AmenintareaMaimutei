using System.Diagnostics.CodeAnalysis;

namespace CourseApp.Models
{
    /// <summary>
    /// Represents a user within the application, including their identifier, username, and email.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public required string Email { get; set; }
    }
}