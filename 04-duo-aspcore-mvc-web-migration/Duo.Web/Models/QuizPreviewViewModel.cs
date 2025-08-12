using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuoClassLibrary.Models.Quizzes;

namespace Duo.Web.Models
{
    public class QuizPreviewViewModel
    {
        public required Quiz Quiz { get; set; }
        public required List<int> ExerciseIds { get; set; }
        public required string SectionTitle { get; set; }
    }
}