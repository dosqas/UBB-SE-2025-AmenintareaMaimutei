namespace DuoClassLibrary.Models.Quizzes.API
{
    public class AnswerSubmission
    {
        public int QuestionId { get; set; }
        public int? SelectedOptionIndex { get; set; }
        public string? WrittenAnswer { get; set; }
        public int? AssociatedPairId { get; set; }
    }
}
