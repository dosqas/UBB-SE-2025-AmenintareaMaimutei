// File: ViewModels/SectionUnlockViewModel.cs
using DuoClassLibrary.Models.Quizzes;
using DuoClassLibrary.Models.Sections;

namespace DuoWebApp.ViewModels
{
    public class SectionUnlockViewModel
    {
        public Section Section { get; set; }
        public bool IsUnlocked { get; set; }
        public List<QuizUnlockViewModel> Quizzes { get; set; }
        public Exam? Exam { get; set; }
        public bool IsExamUnlocked { get; set; }
        public bool IsExamCompleted { get; set; }
    }
}
