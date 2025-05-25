using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuoClassLibrary;
using DuoClassLibrary.Models.Exercises;

namespace WebServerTest.Models
{
    public class QuizSolverViewModel
    {
        public int QuizId { get; set; }
        public string QuizTitle { get; set; }
        public List<Exercise> AllExercises { get; set; }
        public Exercise CurrentExercise { get; set; }
        public int CurrentExerciseIndex { get; set; }
        public bool IsLastExercise { get; set; }

        public string CurrentExerciseType { get; set; }

        public QuizSolverViewModel()
        {
            AllExercises = new List<Exercise>();
        }
    }
}