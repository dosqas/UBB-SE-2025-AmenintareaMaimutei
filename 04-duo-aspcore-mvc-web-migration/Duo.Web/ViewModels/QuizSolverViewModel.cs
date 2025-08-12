using DuoClassLibrary.Models.Exercises;
using System.Collections.Generic;

namespace Duo.Web.ViewModels
{
    public class QuizSolverViewModel
    {
        public int QuizId { get; set; }
        public string QuizTitle { get; set; } = "";
        public List<Exercise> AllExercises { get; set; } = new();
        public int CurrentExerciseIndex { get; set; }
        public Exercise CurrentExercise { get; set; }
        public string CurrentExerciseType { get; set; } = "";
        public bool IsLastExercise { get; set; }
    }
}
