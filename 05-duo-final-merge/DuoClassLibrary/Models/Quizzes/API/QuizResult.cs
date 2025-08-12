using System;

namespace DuoClassLibrary.Models.Quizzes.API
{
    public class QuizResult
    {
        public int QuizId { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public TimeSpan TimeTaken { get; set; }
    }
}
