using Duo.Api.Models.Quizzes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Duo.Api.Models
{
    public class QuizCompletions
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user who completed the quiz.
        /// </summary>
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the completed quiz.
        /// </summary>
        [ForeignKey(nameof(Quiz))]
        public int QuizId { get; set; }

        /// <summary>
        /// Navigation property to the related user.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Navigation property to the related quiz.
        /// </summary>
        public Quiz Quiz { get; set; }
    }
}
