using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuoClassLibrary.Models.Quizzes;

namespace Duo.Web.ViewModels
{
    public class ExamPreviewViewModel
    {
        public required Exam Exam { get; set; }
        public required List<int> ExerciseIds { get; set; }
        public required string SectionTitle { get; set; }

    }
}
