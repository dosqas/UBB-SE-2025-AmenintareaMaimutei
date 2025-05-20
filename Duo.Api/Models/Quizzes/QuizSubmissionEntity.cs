using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Duo.Api.Models.Quizzes
{
    [Serializable]
    public class QuizSubmissionEntity
    {
        [Key]
        public int Id { get; set; }

        public int QuizId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<AnswerSubmissionEntity> Answers { get; set; } =
            [];
    }

    [Serializable]
    public class AnswerSubmissionEntity
    {
        [Key]
        public int Id { get; set; }

        public int QuestionId { get; set; }
        public int SelectedOptionIndex { get; set; }
        public bool IsCorrect { get; set; }
    }
}
