using DuoClassLibrary.Models.Quizzes;
using System.Collections.Generic;

namespace Duo.Web.ViewModels
{
    public class QuizPreviewViewModel
    {
        public BaseQuiz Quiz { get; set; }

        public List<int> ExerciseIds { get; set; }
        public string SectionTitle { get; set; }
    }
}
