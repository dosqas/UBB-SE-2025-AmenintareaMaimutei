using System;
using DuoClassLibrary.Models.Exercises;

namespace Duo.Web.ViewModels
{
    public class ExamSolverViewModel
    {
        public int ExamId { get; set; }
        public string ExamTitle { get; set; } = "";
        public List<Exercise> AllExercises { get; set; } = new();
        public int CurrentExerciseIndex { get; set; }
        public Exercise CurrentExercise { get; set; }
        public string CurrentExerciseType { get; set; } = "";
        public bool IsLastExercise { get; set; }
    }
}
