using System.Collections.Generic;

namespace Duo.Models.Quizzes.API
{
    [Serializable]
    public class QuizSubmission
    {
        public int QuizId { get; set; }
        public List<AnswerSubmission> Answers { get; set; }
    }
}
