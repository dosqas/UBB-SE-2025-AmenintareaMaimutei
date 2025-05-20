using DuoClassLibrary.Models.Exercises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoClassLibrary.Services
{
    public interface IExerciseServiceProxy
    {
        Task<List<Exercise>> GetAllExercises();
        Task<Exercise?> GetExerciseById(int exerciseId);
        Task<List<Exercise>> GetAllExercisesFromQuiz(int quizId);
        Task<List<Exercise>> GetAllExercisesFromExam(int examId);
        Task CreateExercise(Exercise exercise);
        Task DeleteExercise(int exerciseId);
    }
}
