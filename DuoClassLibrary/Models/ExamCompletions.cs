using DuoClassLibrary.Models.Quizzes;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuoClassLibrary.Models
{
    public class ExamCompletions
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user who completed the exam.
        /// </summary>
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the completed exam.
        /// </summary>
        [ForeignKey(nameof(Exam))]
        public int ExamId { get; set; }

        /// <summary>
        /// Navigation property to the related user.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Navigation property to the related exam.
        /// </summary>
        public Exam Exam { get; set; }
    }
}
