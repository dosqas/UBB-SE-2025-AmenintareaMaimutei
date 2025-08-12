using System;

namespace Duo.Models.Quizzes.API
{
    [Serializable]
    public class QuizResult
    {
        public int QuizId { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public TimeSpan TimeTaken { get; set; }
    }
}
