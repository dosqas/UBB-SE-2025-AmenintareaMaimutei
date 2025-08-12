// File: ViewModels/SectionUnlockViewModel.cs
using DuoClassLibrary.Models.Quizzes;

namespace WebServerTest.Models
{
    public class QuizUnlockViewModel
    {
        public Quiz Quiz { get; set; }
        public bool IsUnlocked { get; set; }
        public bool IsCompleted { get; set; }
    }
}
