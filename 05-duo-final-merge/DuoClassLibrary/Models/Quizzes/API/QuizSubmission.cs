using System.Collections.Generic;

namespace DuoClassLibrary.Models.Quizzes.API
{
    public class QuizSubmission
    {
        public int QuizId { get; set; }
        public List<AnswerSubmission> Answers { get; set; }
    }
}
