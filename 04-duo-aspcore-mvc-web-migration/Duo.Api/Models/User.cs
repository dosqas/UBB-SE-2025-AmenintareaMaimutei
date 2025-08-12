using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Duo.Api.Models
{
    /// <summary>
    /// Represents a user of the platform.
    /// A user can complete sections, quizzes, and interact with various platform features.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class User
    {
        #region Fields and Properties

        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        [Key]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the number of completed sections by the user.
        /// </summary>
        public int NumberOfCompletedSections { get; set; }

        /// <summary>
        /// Gets or sets the number of completed quizzes in the current section.
        /// </summary>
        public int NumberOfCompletedQuizzesInSection { get; set; }

        /// <summary>
        /// Gets or sets the email of the user (optional).
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the coin balance of the user.
        /// </summary>
        public int CoinBalance { get; set; }

        /// <summary>
        /// Gets or sets the last login time of the user.
        /// </summary>
        public DateTime LastLoginTime { get; set; }
        public List<Enrollment>? Enrollments { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string representation of the user, including their username and coin balance.
        /// </summary>
        /// <returns>A string describing the user.</returns>
        public override string ToString()
        {
            return $"User: {Username}, Coin Balance: {CoinBalance}, Last Login: {LastLoginTime}";
        }

        #endregion
    }
}