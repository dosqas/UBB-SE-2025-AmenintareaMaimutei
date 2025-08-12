using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuoClassLibrary.Models.Exercises;
using DuoClassLibrary.Models.Quizzes;

namespace Duo.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseService exerciseServiceProxy;

        public ExerciseService(IExerciseService exerciseServiceProxy)
        {
            this.exerciseServiceProxy = exerciseServiceProxy ?? throw new ArgumentNullException(nameof(exerciseServiceProxy));
        }

        public async Task<List<Exercise>> GetAllExercises()
        {
            return await exerciseServiceProxy.GetAllExercises();
        }

        public async Task<Exercise> GetExerciseById(int exerciseId)
        {
            return await exerciseServiceProxy.GetExerciseById(exerciseId);
        }

        public async Task<List<Exercise>> GetAllExercisesFromQuiz(int quizId)
        {
            return await exerciseServiceProxy.GetAllExercisesFromQuiz(quizId);
        }
        public async Task<List<Exercise>> GetAllExercisesFromExam(int examId)
        {
            return await exerciseServiceProxy.GetAllExercisesFromExam(examId);
        }

        public async Task DeleteExercise(int exerciseId)
        {
            await exerciseServiceProxy.DeleteExercise(exerciseId);
        }

        public async Task CreateExercise(Exercise exercise)
        {
            ValidationHelper.ValidateGenericExercise(exercise);
            await exerciseServiceProxy.CreateExercise(exercise);
        }
    }
}
