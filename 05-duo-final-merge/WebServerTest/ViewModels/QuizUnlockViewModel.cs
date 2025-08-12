// File: ViewModels/SectionUnlockViewModel.cs
using DuoClassLibrary.Models.Quizzes;

namespace DuoWebApp.ViewModels
{
    public class QuizUnlockViewModel
    {
        public Quiz Quiz { get; set; }
        public bool IsUnlocked { get; set; }
        public bool IsCompleted { get; set; }
    }
}
